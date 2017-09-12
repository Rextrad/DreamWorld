﻿

Imports System
Imports System.IO



Public Class RegionMaker

#Region "Declarations"
    ' hold a copy of the Main region data on a per-form basis
    Private Class Region_data
        Public ProcessID As Integer
        Public RegionName As String
        Public UUID As String
        Public CoordX As Integer
        Public CoordY As String
        Public RegionPort As Integer
        Public SizeX As Integer
        Public SizeY As Integer
    End Class

    Public Shared RegionList As New ArrayList
    Private gCurCurRegionNum As UInteger
#End Region

#Region "Properties"
    Public Shared Property RegionCount() As Integer
        Get
            Try
                Return RegionList.Count
            Catch
                Return 0
            End Try
        End Get
        Set(value As Integer)
        End Set
    End Property

    Public Property ProcessID() As Integer
        Get
            Return RegionList(CurRegionNum()).ProcessID
        End Get
        Set(ByVal Value As Integer)
            RegionList(CurRegionNum()).ProcessID = Value
        End Set
    End Property
    Public Property CurRegionNum() As Integer
        Get
            Return gCurCurRegionNum
        End Get
        Set(ByVal Value As Integer)
            gCurCurRegionNum = Value
        End Set
    End Property
    Public Property RegionName() As String
        Get
            Return RegionList(CurRegionNum()).RegionName
        End Get
        Set(ByVal Value As String)
            RegionList(CurRegionNum()).RegionName = Value
        End Set
    End Property
    Public Property UUID() As String
        Get
            Return RegionList(CurRegionNum()).UUID
        End Get
        Set(ByVal Value As String)
            RegionList(CurRegionNum()).UUID = Value
        End Set
    End Property
    Public Property SizeX() As Integer
        Get
            Return RegionList(CurRegionNum()).SizeX
        End Get
        Set(ByVal Value As Integer)
            RegionList(CurRegionNum()).SizeX = Value
        End Set
    End Property
    Public Property SizeY() As Integer
        Get
            Return RegionList(CurRegionNum()).SizeY
        End Get
        Set(ByVal Value As Integer)
            RegionList(CurRegionNum()).SizeY = Value
        End Set
    End Property
    Public Property RegionPort() As Integer
        Get
            If RegionList(CurRegionNum()).RegionPort <= My.Settings.PrivatePort Then
                RegionList(CurRegionNum()).RegionPort = My.Settings.PrivatePort + 1 ' 8004, by default
            End If
            Return RegionList(CurRegionNum()).RegionPort
        End Get
        Set(ByVal Value As Integer)
            RegionList(CurRegionNum()).RegionPort = Value
        End Set
    End Property
    Public Property CoordX() As Integer
        Get
            Return RegionList(CurRegionNum()).CoordX
        End Get
        Set(ByVal Value As Integer)
            RegionList(CurRegionNum()).CoordX = Value
        End Set
    End Property
    Public Property CoordY() As Integer
        Get
            Return RegionList(CurRegionNum()).CoordY
        End Get
        Set(ByVal Value As Integer)
            RegionList(CurRegionNum()).CoordY = Value
        End Set
    End Property

#End Region

#Region "Code"

    Public Sub New()

        If GetAllRegions() Then
            MsgBox("Failed to load all regions")
        End If

    End Sub
    Public Function CurrentRegionName() As String

        Dim id = CurRegionNum
        Return FindRegionidByName(id)

    End Function

    Public Function RegionListCount() As Integer

        Return RegionCount()

    End Function
    Public Function FindRegionidByName(Name As String) As Integer

        Dim index = RegionListCount() - 1

        While index > -1
            If Name = RegionList(index).RegionName Then
                Return index
            End If
            index = index - 1
        End While
        Return -1

    End Function

    Public Sub CreateRegion()

        Dim r As New Region_data
        r.RegionName = ""
        r.UUID = Guid.NewGuid().ToString
        r.SizeX = 256
        r.SizeY = 256

        RegionList.Add(r)
        Dim index = RegionListCount() - 1
        ' default data
        RegionList(index).RegionPort = LargestPort() + 1 '8004 + 1

        ' form a line acrss the X Axis 4 to the right
        RegionList(index).CoordX = LargestX() + 4
        RegionList(index).CoordY = LargestY() + 0
        CurRegionNum() = index

    End Sub

    Public Function GetAllRegions() As Boolean

        Dim folders() As String
        Dim regionfolders() As String

        folders = Directory.GetDirectories(Form1.prefix + "Regions\")
        For Each FolderName As String In folders
            Form1.Log("Info:Region Path:" + FolderName)
            regionfolders = Directory.GetDirectories(FolderName)
            For Each FileName As String In regionfolders
                Try
                    Form1.Log("Info:Loading region from " + FolderName)
                    Dim inis = Directory.GetFiles(FileName, "*.ini", SearchOption.TopDirectoryOnly)
                    For Each ini As String In inis
                        ' remove the ini
                        Dim fName = Path.GetFileName(ini)
                        fName = Mid(fName, 1, Len(fName) - 4)

                        ' make a slot to hold the region data 
                        CreateRegion()
                        Form1.Log("Info:Reading Region " + ini)

                        ' populate from disk
                        RegionName() = fName
                        UUID() = Form1.GetIni(ini, fName, "RegionUUID", ";")
                        SizeX() = Convert.ToInt16(Form1.GetIni(ini, fName, "SizeX", ";"))
                        SizeY() = Convert.ToInt16(Form1.GetIni(ini, fName, "SizeY", ";"))
                        RegionPort() = Convert.ToInt16(Form1.GetIni(ini, fName, "InternalPort", ";"))
                        ' Location is int,int format.
                        Dim C = Form1.GetIni(ini, fName, "Location", ";")
                        Dim parts As String() = C.Split(New Char() {","c}) ' split at the comma
                        CoordX() = parts(0)
                        CoordY() = parts(1)
                    Next

                Catch ex As Exception
                    Form1.Log("Err:Parse file " + FileName + ":" + ex.Message)
                    Return True
                End Try
            Next
        Next
        Return False
    End Function

#End Region

#Region "Private"

    Private Function LargestX() As Integer

        ' locate largest global coords
        Dim Max As Integer
        Dim counter As Integer = 1
        Dim L = RegionListCount()
        While counter < L
            Dim val = RegionList(counter).CoordX
            If val > Max Then Max = val
            counter += 1
        End While
        If Max = 0 Then Max = 1000
        Return Max

    End Function

    Private Function LargestY() As Integer

        ' locate largest global coords
        Dim Max As Integer
        Dim counter As Integer = 1
        Dim L = RegionListCount()
        While counter < L
            Dim val = RegionList(counter).CoordY
            If val > Max Then Max = val
            counter += 1
        End While
        If Max = 0 Then Max = 1000
        Return Max

    End Function

    Private Function LargestPort() As Integer

        ' locate largest global coords
        Dim Max As Integer
        Dim counter As Integer = 0
        Dim L = RegionListCount()
        While counter < L
            Dim val = RegionList(counter).RegionPort
            If val > Max Then Max = val
            counter += 1
        End While
        If Max = 0 Then Max = 8004
        Return Max

    End Function

#End Region

End Class
