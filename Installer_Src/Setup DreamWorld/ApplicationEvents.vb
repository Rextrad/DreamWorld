﻿#Region "Copyright"

' Copyright 2014 Fred Beckhusen for outworldz.com https://opensource.org/licenses/AGPL

'Permission Is hereby granted, free Of charge, to any person obtaining a copy of this software
' And associated documentation files (the "Software"), to deal in the Software without restriction,
'including without limitation the rights To use, copy, modify, merge, publish, distribute, sublicense,
'And/Or sell copies Of the Software, And To permit persons To whom the Software Is furnished To
'Do so, subject To the following conditions:

'The above copyright notice And this permission notice shall be included In all copies Or '
'substantial portions Of the Software.

'THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or IMPLIED,
' INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY, FITNESS For A PARTICULAR
'PURPOSE And NONINFRINGEMENT.In NO Event SHALL THE AUTHORS Or COPYRIGHT HOLDERS BE LIABLE
'For ANY CLAIM, DAMAGES Or OTHER LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or
'OTHERWISE, ARISING FROM, OUT Of Or In CONNECTION With THE SOFTWARE Or THE USE Or OTHER
'DEALINGS IN THE SOFTWARE.Imports System

#End Region

Namespace My

    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed. This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

#Region "Private Methods"

        Private Sub MyApplication_UnhandledException(
                    ByVal sender As Object,
                    ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs
         ) Handles Me.UnhandledException

            Dim ex = e.Exception

            Dim Result As String
            Dim hr As Integer = Runtime.InteropServices.Marshal.GetHRForException(ex)
            Result = ex.GetType.ToString() & "(0x" & hr.ToString("X8", Globalization.CultureInfo.InvariantCulture) & "): " & ex.Message & Environment.NewLine & ex.StackTrace & Environment.NewLine
            Dim st As StackTrace = New StackTrace(ex, True)
            For Each sf As StackFrame In st.GetFrames
                If sf.GetFileLineNumber() > 0 Then
                    Result &= "Line:" & sf.GetFileLineNumber() & " Filename: " & IO.Path.GetFileName(sf.GetFileName) & Environment.NewLine
                End If
            Next
            Form1.ErrorLog(Result)
            Form1.ErrorLog(DisplayObjectInfo(sender))

        End Sub

        Private Sub AppStart(ByVal sender As Object,
      ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf ResolveAssemblies
        End Sub

        ''' <summary>
        ''' Sample of embedded DLL

        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <returns></returns>
        Private Function ResolveAssemblies(sender As Object, e As System.ResolveEventArgs) As Reflection.Assembly
            Dim desiredAssembly = New Reflection.AssemblyName(e.Name)
            Diagnostics.Debug.Print("Loading Assembly " & desiredAssembly.Name)
            If desiredAssembly.Name = "Ionic.Zip" Then
                Return Reflection.Assembly.Load("") 'replace with your assembly's resource name
            Else
                Return Nothing
            End If
        End Function

    End Class

#End Region

End Namespace
