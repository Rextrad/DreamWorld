﻿#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.IO
Imports System.Net
Imports System.Threading

Public Class NetServer

#Region "Private Fields"

    Private Shared blnFlag As Boolean
    Private Shared singleWebserver As NetServer
    Dim listen As Boolean = True
    Private MyPort As String
    Private WebThread As Thread

#End Region

#Region "Callback"

    Public Sub StartServer(pathinfo As String, settings As MySettings)

        If settings Is Nothing Then Return
        ' stash some globs

        MyPort = CStr(settings.DiagnosticPort)
        settings.CurrentDirectory = pathinfo


        Log(My.Resources.Info_word, Global.Outworldz.My.Resources.Starting_DiagPort_Webserver)
        WebThread = New Thread(AddressOf Looper)
        Try
            WebThread.SetApartmentState(ApartmentState.STA)
        Catch ex As Exception
            BreakPoint.Dump(ex)
            Log(My.Resources.Error_word, ex.Message)
        End Try
        WebThread.Priority = ThreadPriority.Highest
        WebThread.Start()

    End Sub

    Public Sub StopWebserver()

        Log(My.Resources.Info_word, Global.Outworldz.My.Resources.Stopping_Webserver)
        listen = False
        WebThread.Abort()

    End Sub

    Private Sub ListenerCallback(ByVal result As IAsyncResult)
        If result Is Nothing Then Return
        Try
            Dim listener As HttpListener = CType(result.AsyncState, HttpListener)
            ' Call EndGetContext to signal the completion of the asynchronous operation.
            Dim context As HttpListenerContext = listener.EndGetContext(result)

            ' The data sent by client
            Dim request As HttpListenerRequest = context.Request

            Dim body As Stream = request.InputStream
            Dim responseString As String = ""
            Using reader = New System.IO.StreamReader(body, request.ContentEncoding)

                Dim original = request.Url.OriginalString

                If original.ToUpperInvariant.Contains("TELEPORTS.HTM") Then
                    responseString = RegionListHTML(original)
                Else
                    If (request.HasEntityBody) Then
                        Dim POST As String = reader.ReadToEnd()
                        responseString = ParsePost(POST)
                    Else
                        responseString = ParsePost(original)
                    End If
                End If

            End Using

            ' Get the response object to send our confirmation.
            Using response As HttpListenerResponse = context.Response
                ' Construct a minimal response string.
                Dim buffer As Byte() = System.Text.Encoding.UTF8.GetBytes(responseString)
                ' Get the response OutputStream and write the response to it.
                response.ContentLength64 = buffer.Length
                ' Identify the content type.
                response.ContentType = "text/html"
                Using output As System.IO.Stream = response.OutputStream
                    output.Write(buffer, 0, buffer.Length)
                    output.Flush()
                End Using

            End Using
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Internal Methods"

    Friend Shared Function GetWebServer() As NetServer

        If Not blnFlag Then
            singleWebserver = New NetServer
            blnFlag = True
            Return singleWebserver
        Else
            Return singleWebserver
        End If

    End Function

#End Region

#Region "Private Methods"

    Private Shared Sub Log(category As String, message As String)
        Debug.Print(message)
        Try
            Using outputFile As New StreamWriter(IO.Path.Combine(Settings.CurrentDirectory, "OutworldzFiles\Logs\Http.log"), False)
                outputFile.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", Globalization.CultureInfo.InvariantCulture) & ":" & category & ":" & message)
            End Using
        Catch ex As Exception
            ' none to prevent looping
        End Try
    End Sub

    Private Sub Looper()

        listen = True

        Using listener As New System.Net.HttpListener()
            listener.Prefixes.Clear()
            listener.Prefixes.Add("http://*:" & MyPort & "/")

            Try
                listener.Start() ' Throws Exception
            Catch ex As Exception
                BreakPoint.Dump(ex)
                Log(My.Resources.Error_word, ex.Message)
                Return
            End Try

            Log(My.Resources.Info_word, "Webserver is running")
            Dim result As IAsyncResult
            While listen
                result = listener.BeginGetContext((AddressOf ListenerCallback), listener)
                Try
                    result.AsyncWaitHandle.WaitOne()
                Catch
                End Try
                Application.DoEvents()
            End While

        End Using



    End Sub

#End Region

End Class
