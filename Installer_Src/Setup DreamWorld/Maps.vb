﻿Imports System.Threading
Module Maps

#Region "MapMaking"

    Public Sub MakeMaps()

        Dim Mapthread As Thread
        Mapthread = New Thread(AddressOf BuildMap)
        Mapthread.SetApartmentState(ApartmentState.STA)
        Mapthread.Priority = ThreadPriority.BelowNormal
        Mapthread.Start()

    End Sub

    Private Sub BuildMap()

        Dim SavePath = IO.Path.Combine(Settings.CurrentDirectory, "Outworldzfiles\Apache\htdocs\Stats\Maps")
        Try
            FileIO.FileSystem.CreateDirectory(SavePath)
        Catch ex As Exception
            BreakPoint.Show(ex.Message)
        End Try

        For Each RegionUUID In PropRegionClass.RegionUuids
            Application.DoEvents()
            Dim MapPath = IO.Path.Combine(Settings.OpensimBinPath, "maptiles\00000000-0000-0000-0000-000000000000")

            Dim Name = PropRegionClass.RegionName(RegionUUID)
            Dim SimSize As Integer = CInt(PropRegionClass.SizeX(RegionUUID))
            Using bmp As New Bitmap(SimSize, SimSize)
                Dim X = 0
                Dim Y = 0

                ' Loop through the images pixels to reset color.
                For X = 0 To bmp.Width - 1
                    For Y = 0 To bmp.Height - 1
                        Dim newColor = Color.FromArgb(230, 230, 230)
                        bmp.SetPixel(X, Y, newColor)
                    Next
                Next

                Dim Out As Image = bmp
                Dim Src As Image = bmp
                Dim XS = SimSize / 256


                X = 0
                For Xstep = 0 To XS - 1
                    Y = CInt(SimSize - (SimSize / XS))
                    For Ystep = 0 To XS - 1
                        Dim MapImage = $"map-1-{PropRegionClass.CoordX(RegionUUID) + Xstep }-{PropRegionClass.CoordY(RegionUUID) + Ystep  }-objects.jpg"
                        Diagnostics.Debug.Print(Name)
                        Diagnostics.Debug.Print(MapImage)

                        ' images plot at up[per left, Opensim is lower left
                        ' for a 2X2 this is the value
                        'X:Y = 0:256
                        'X:Y = 0:0
                        'X:Y = 256:256
                        'X:Y = 256:0

                        Dim RegionSrc = IO.Path.Combine(MapPath, MapImage)
                        If IO.File.Exists(RegionSrc) Then
                            Src = Image.FromFile(RegionSrc)
                            Using g As Graphics = Graphics.FromImage(Out)
                                Diagnostics.Debug.Print(CStr(X) & ":" & CStr(Y))
                                g.DrawImage(Src, New System.Drawing.Rectangle(X, Y, 256, 256))
                                Out.Save(IO.Path.Combine(SavePath, $"{Name}.jpg"))
                            End Using
                        End If
                        Y -= 256
                    Next
                    X += 256
                Next

            End Using

        Next

    End Sub

#End Region

    Public Sub DeleteMaps(RegionUUID As String)

        Dim path = IO.Path.Combine(Settings.OpensimBinPath(), "maptiles\00000000-0000-0000-0000-000000000000")

        For i = 1 To 8
            For XX = 0 To PropRegionClass.SizeX(RegionUUID) / 256
                For YY = 0 To PropRegionClass.SizeY(RegionUUID) / 256
                    Dim x1 = PropRegionClass.CoordX(RegionUUID) + XX
                    Dim y1 = PropRegionClass.CoordY(RegionUUID) + YY
                    Dim file = IO.Path.Combine(path, $"map-{i}-{x1}-{y1}-objects.jpg")
                    Debug.Print($"Delete map-{i}-{x1}-{y1}-objects.jpg")
                    DeleteFile(file)
                Next
            Next
        Next

    End Sub

End Module
