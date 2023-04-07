﻿#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading
Imports IniParser
Imports IniParser.Model

Public Class LoadIni

    Private ReadOnly _parser As FileIniDataParser
    Private ReadOnly _SettingsData As IniParser.Model.IniData
    Private ReadOnly Readlock As New Object
    Private ReadOnly SaveTheINI As New Object
    Private _encoding As System.Text.Encoding
    Private _filename As String
    Private _sep As String

    Public Sub New(File As String, arg As String, encoding As System.Text.Encoding)

        If File Is Nothing Then Return
        If Not IO.File.Exists(File) Then
            ErrorLog($"Missing file {File}")
            Return
        End If
        FileName = File
        If arg Is Nothing Then Return
        If FileName.Length = 0 Then Return
        If File.Contains("\Region\") Then CheckINI()
        Sep = arg

        Me.Encoding = encoding

        _parser = New FileIniDataParser()
        _parser.Parser.Configuration.SkipInvalidLines = False
        _parser.Parser.Configuration.AssigmentSpacer = ""
        _parser.Parser.Configuration.CommentString = Sep ' Opensim uses semicolons
        _SettingsData = ReadINIFile(File)
        If _SettingsData Is Nothing Then
            ErrorLog($"No Data in {File}")
            MsgBox("Fatal Error! Cannot Read INI File!", MsgBoxStyle.Critical)
            End
        End If

        If _SettingsData.ToString.Length = 0 Then
            ErrorLog($"No Data in {File}")
            MsgBox("Cannot Read INI File!", MsgBoxStyle.Critical)
            End
        End If

    End Sub

    Public Property Encoding As System.Text.Encoding
        Get
            Return _encoding
        End Get
        Set(value As System.Text.Encoding)
            _encoding = value
        End Set
    End Property

    Public Property Sep As String
        Get
            Return _sep
        End Get
        Set(value As String)
            _sep = value
        End Set
    End Property

    Private Property FileName As String
        Get
            Return _filename
        End Get
        Set(value As String)
            _filename = value
        End Set
    End Property

    Public Function GetIni(section As String, key As String, Value As String, Optional V As String = Nothing) As Object

        If _SettingsData Is Nothing Then
            ErrorLog($"No Settings for {section} {key}")
            Return Nothing
        End If

        Dim Variable = Stripqq(_SettingsData(section)(key))

        If Variable = Nothing Then Variable = Value
        If Variable Is Nothing Then Return Value

        Dim bool As Boolean
        If V = "Boolean" Then
            If Not Boolean.TryParse(Variable, bool) Then
                Return False
            End If
            Return bool
        ElseIf V = "String" Then
            Return Variable.Trim
        ElseIf V = "Double" Then
            Dim DBL As Double
            If Not Double.TryParse(Variable, DBL) Then
                Return 0
            End If
            Return DBL
        ElseIf V = "Single" Then
            Dim SNG As Single
            If Not Single.TryParse(Variable, SNG) Then
                Return 0
            End If
            Return SNG
        ElseIf V = "Integer" Then
            Dim I As Integer
            If Not Integer.TryParse(Variable, I) Then
                Return 0
            End If
            Return I
        End If

        Return Variable

    End Function

    Public Function SaveIni() As Boolean

        SyncLock SaveTheINI
            CopyFileFast(Settings.CurrentDirectory + "\OutworldzFiles\Settings.ini", Settings.CurrentDirectory + "\OutworldzFiles\Settings.bak")
            Dim Retry As Integer = 100 ' 1 sec
            While Retry > 0
                Try
                    _parser.WriteFile(FileName, _SettingsData, Encoding)
                    Retry = 0
                Catch ex As Exception
                    Retry -= 1
                    Thread.Sleep(10)
                End Try

                If Retry < 0 Then
                    ErrorLog($"Region INI filed to save: {FileName}")
                    If Not RunningInServiceMode() Then
                        Dim result = MsgBox($"Region INI filed to save: {FileName}", MsgBoxStyle.Critical Or MsgBoxStyle.MsgBoxSetForeground Or MsgBoxStyle.Exclamation, My.Resources.Quit_Now_Word)
                    Else
                        ErrorLog($"Region INI filed to save: {FileName}")
                        Return True
                    End If
                End If
            End While
        End SyncLock
        Return False

    End Function

    ''' <summary>Save to the ini the name value pair.</summary>
    ''' <param name="section"></param>
    ''' <param name="key"></param>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Public Function SetIni(section As String, key As String, value As String) As Boolean

        ' sets values into any INI file Form1.Log(My.Resources.Info, "Writing section [" + section + "] " + key + "=" + value)
        Try
            _SettingsData(section)(key) = value
        Catch ex As Exception
            ErrorLog($"Section {section} Key {key} Value {value} {ex.Message}")
            Return True
        End Try
        Return False

    End Function

    ''' <summary>
    ''' Repair INI files with extra [sections]
    ''' </summary>
    ''' <param name="file">Path to region ini file</param>
    Private Sub CheckINI()

        Dim c As Integer
        Dim RepairedLine As String = ""
        If Not File.Exists(FileName) Then Return ' bug 39914812

        Using Reader As New System.IO.StreamReader(FileName)
            While Not Reader.EndOfStream
                Dim line As String = Reader.ReadLine
                Dim pattern = New Regex("^\[.*?\]")
                Dim match As Match = pattern.Match(line)
                If match.Success Then
                    c += 1
                End If

                ' more than one [Section], skip lines
                If c > 1 Then
                    Continue While
                End If

                RepairedLine += line & vbCrLf
            End While
        End Using
        Try
            If c > 1 Then
                Sleep(100)
                FileStuff.DeleteFile(FileName)
                Sleep(100)
                Using Writer As New StreamWriter(FileName, False)
                    Writer.Write(RepairedLine)
                End Using
            End If
        Catch
        End Try

    End Sub

    Private Function ReadINIFile(FileName As String) As IniData

        SyncLock Readlock

            Dim waiting As Integer = 0
            While waiting < 100
                Try
                    Dim Data = _parser.ReadFile(FileName, Encoding)
                    Return Data
                Catch ex As Exception
                    waiting += 1 ' 10 sec
                    Sleep(100)
                End Try
            End While

            If waiting = 100 Then
                ErrorLog($" Timeout loading INI file: {FileName}")
            End If
            Return Nothing

        End SyncLock
    End Function

End Class
