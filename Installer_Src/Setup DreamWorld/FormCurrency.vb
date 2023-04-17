#Region "Copyright AGPL3.0"

' Copyright Outworldz, LLC.
' AGPL3.0  https://opensource.org/licenses/AGPL

#End Region

Imports System.Text.RegularExpressions

Public Class FormCurrency

    Private initted As Boolean
    Private isChanged As Boolean

#Region "ScreenSize"

    Private ReadOnly Handler As New EventHandler(AddressOf Resize_page)
    Private _screenPosition As ClassScreenpos

    Public Property ScreenPosition As ClassScreenpos
        Get
            Return _screenPosition
        End Get
        Set(value As ClassScreenpos)
            _screenPosition = value
        End Set
    End Property

    'The following detects  the location of the form in screen coordinates
    Private Sub Resize_page(ByVal sender As Object, ByVal e As System.EventArgs)

        ScreenPosition.SaveXY(Me.Left, Me.Top)
    End Sub

    Private Sub SetScreen()

        ScreenPosition = New ClassScreenpos(Me.Name)
        AddHandler ResizeEnd, Handler
        Dim xy As List(Of Integer) = ScreenPosition.GetXY()
        Me.Left = xy.Item(0)
        Me.Top = xy.Item(1)
    End Sub

#End Region

#Region "Load/Quit"

    Private Sub FormisClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        If isChanged Then
            Dim v = MsgBox(My.Resources.Save_changes_word, MsgBoxStyle.YesNo Or MsgBoxStyle.MsgBoxSetForeground, Global.Outworldz.My.Resources.Save_changes_word)
            If v = vbYes Then
                DoCurrency()
                Settings.SaveSettings()
            End If
        End If

    End Sub

    Private Sub Loaded(sender As Object, e As EventArgs) Handles Me.Load

        My.Application.ChangeUICulture(Settings.Language)
        My.Application.ChangeCulture(Settings.Language)

        SetScreen()

        ' suppress two tab pages as they do not yet work
        TabControl1.TabPages.Remove(TabPage1)
        'TabControl1.TabPages.Insert(0, TabPage1)
        TabControl1.TabPages.Remove(DtlTab)
        'TabControl1.TabPages.Insert(0, DTLTab)

        Button4.Text = Global.Outworldz.My.Resources.Free_Account
        GLBShowNewSessionAuthIMCheckBox.Text = Global.Outworldz.My.Resources.GLBShowNewSessionAuthIM_text
        GLBShowNewSessionPurchaseIMCheckBox.Text = Global.Outworldz.My.Resources.GLBShowNewSessionPurchaseIM_text
        GLBShowWelcomeMessageCheckBox.Text = Global.Outworldz.My.Resources.GLBShowWelcomeMessage_text
        GloebitsEnabled.Text = Global.Outworldz.My.Resources.EnableGloebit_word
        HelpToolStripMenuItem.Image = Global.Outworldz.My.Resources.question_and_answer
        HelpToolStripMenuItem.Text = Global.Outworldz.My.Resources.Help_word
        ProductionCreateAppButton.Text = Global.Outworldz.My.Resources.CreateApp
        ProductionCreateButton.Text = Global.Outworldz.My.Resources.Create_Account
        ProductionReqAppButton.Text = Global.Outworldz.My.Resources.Request_App

        Text = My.Resources.Currency_word
        ContactEmailTextBox.Text = Settings.GlbOwnerEmail
        OwnerNameTextbox.Text = Settings.GlbOwnerName

        ProdKeyTextBox.Text = Settings.GLProdKey
        ProdSecretTextBox.UseSystemPasswordChar = True
        ProdSecretTextBox.Text = Settings.GLProdSecret

        GLBShowNewSessionAuthIMCheckBox.Checked = Settings.GlbShowNewSessionAuthIM
        GLBShowNewSessionPurchaseIMCheckBox.Checked = Settings.GlbShowNewSessionPurchaseIM
        GLBShowWelcomeMessageCheckBox.Checked = Settings.GlbShowWelcomeMessage

        If Settings.DTLEnable Then
            GloebitsEnabled.Checked = False
            EnableDTLcheckbox.Checked = True
        ElseIf Settings.GloebitsEnable Then
            GloebitsEnabled.Checked = Settings.GloebitsEnable
            EnableDTLcheckbox.Checked = False
        End If

        EnableDTLcheckbox.Checked = Settings.DTLEnable
        MoneyPortTextbox.Text = CStr(Settings.DTLMoneyPort)
        InitialBalance.Text = CStr(Settings.DTLInitialBalance)

        If Settings.Banker.Length = 0 Then
            RadioButton1.Checked = True
            Banker.Visible = False
        ElseIf Settings.Banker = "00000000-0000-0000-0000-000000000000" Then
            RadioButton2.Checked = True
            Banker.Visible = False
        Else
            RadioButton3.Checked = True
            Banker.Text = Settings.Banker
            Banker.Visible = True
        End If

        StartMySQL()

        Fillbox()
        initted = True

    End Sub

#End Region

#Region "FillBox"

    Private Sub Fillbox()

        If Banker.Text.Length = 0 Then
            Banker.BackColor = Color.Red
        End If
        With Banker
            .AutoCompleteCustomSource = MysqlInterface.GetAvatarList()
            .AutoCompleteMode = AutoCompleteMode.Suggest
            .AutoCompleteSource = AutoCompleteSource.CustomSource
        End With

    End Sub

#End Region

#Region "Sandbox"

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Dim webAddress As String = "https://sandbox.gloebit.com/signup/"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        Dim webAddress As String = "https://sandbox.gloebit.com/merchant-signup/"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub CreateAppButton2_Click(sender As Object, e As EventArgs)
        Dim webAddress As String = "https://www.gloebit.com"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

#End Region

#Region "Production"

    Private Sub ProdKeyTextBox_Click(sender As Object, e As EventArgs) Handles ProdKeyTextBox.Click

        ProdKeyTextBox.UseSystemPasswordChar = False

    End Sub

    Private Sub ProdKeyTextBox_TextChanged(sender As Object, e As EventArgs) Handles ProdKeyTextBox.TextChanged

        If Not initted Then Return
        isChanged = True
        Settings.GLProdKey = ProdKeyTextBox.Text

    End Sub

    Private Sub ProdSecretTextBox_Click(sender As Object, e As EventArgs) Handles ProdSecretTextBox.Click

        ProdSecretTextBox.UseSystemPasswordChar = False

    End Sub

    Private Sub ProdSecretTextBox_TextChanged(sender As Object, e As EventArgs) Handles ProdSecretTextBox.TextChanged

        If Not initted Then Return
        isChanged = True
        Settings.GLProdSecret = ProdSecretTextBox.Text

    End Sub

    Private Sub ProductionCreateAppButton_Click(sender As Object, e As EventArgs) Handles ProductionCreateAppButton.Click
        Dim webAddress As String = "https://www.gloebit.com/merchant-tools/"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub ProductionCreateButton_Click(sender As Object, e As EventArgs) Handles ProductionCreateButton.Click
        Dim webAddress As String = "https://www.gloebit.com/signup/"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

    Private Sub ProductionReqAppButton_Click(sender As Object, e As EventArgs) Handles ProductionReqAppButton.Click
        Dim webAddress As String = "https://www.gloebit.com/merchant-signup/"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try
    End Sub

#End Region

#Region "OwnerInfo"

    Private Sub Banker_TextChanged(sender As Object, e As EventArgs) Handles Banker.TextChanged

        If Not initted Then Return
        If Banker.Text.Length = 0 Then
            Banker.BackColor = Color.Red
        Else
            Banker.BackColor = Color.FromName("Window")
        End If
        Settings.Banker = Banker.Text
        isChanged = True

    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles Button1.Click

        For Each RegionUUID In RegionUuids()
            DisableGloebits(RegionUUID) = CStr(False)
        Next
        TextPrint(My.Resources.AllGloebitOn)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        For Each RegionUUID In RegionUuids()
            DisableGloebits(RegionUUID) = CStr(True)
        Next
        TextPrint(My.Resources.AllGloebitOff)

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dim webAddress As String = "http://dev.gloebit.com/opensim/"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub ContactEmailTextBox_TextChanged(sender As Object, e As EventArgs) Handles ContactEmailTextBox.TextChanged

        If Not initted Then Return
        isChanged = True
        Settings.GlbOwnerEmail = ContactEmailTextBox.Text

    End Sub

    Private Sub DTLWebSiteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DTLWebSiteToolStripMenuItem.Click

        Dim webAddress As String = "http://www.nsl.tuis.ac.jp/xoops/modules/xpwiki/?OpenSim%2FMoneyServer"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub EnableDTLcheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles EnableDTLcheckbox.CheckedChanged

        If Not initted Then Return
        If EnableDTLcheckbox.Checked Then
            Settings.GloebitsEnable = False
            Settings.DTLEnable = True
            GloebitsEnabled.Checked = False
            isChanged = True
        End If

    End Sub

    Private Sub GLBShowNewSessionAuthIMCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles GLBShowNewSessionAuthIMCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.GlbShowNewSessionAuthIM = GLBShowNewSessionAuthIMCheckBox.Checked
        isChanged = True

    End Sub

    Private Sub GLBShowNewSessionPurchaseIMCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles GLBShowNewSessionPurchaseIMCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.GlbShowNewSessionPurchaseIM = GLBShowNewSessionPurchaseIMCheckBox.Checked
        isChanged = True

    End Sub

    Private Sub GLBShowWelcomeMessageCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles GLBShowWelcomeMessageCheckBox.CheckedChanged

        If Not initted Then Return
        Settings.GlbShowWelcomeMessage = GLBShowWelcomeMessageCheckBox.Checked
        isChanged = True

    End Sub

    Private Sub GloebitsEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles GloebitsEnabled.CheckedChanged

        If Not initted Then Return
        If GloebitsEnabled.Checked Then
            Settings.GloebitsEnable = True
            Settings.DTLEnable = False
            EnableDTLcheckbox.Checked = False
            isChanged = True
        Else
            Settings.GloebitsEnable = False
            isChanged = True
        End If

    End Sub

    Private Sub GloebitWebSiteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GloebitWebSiteToolStripMenuItem.Click

        Dim webAddress As String = "http://dev.gloebit.com"
        Try
            Process.Start(webAddress)
        Catch ex As Exception
            BreakPoint.Dump(ex)
        End Try

    End Sub

    Private Sub HelpGloebitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpGloebitToolStripMenuItem.Click

        HelpManual("Gloebit")

    End Sub

    Private Sub HelptDTLCurrencyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelptDTLCurrencyToolStripMenuItem.Click

        HelpManual("DTL_Currency")

    End Sub

    Private Sub InitialBalance_TextChanged(sender As Object, e As EventArgs) Handles InitialBalance.TextChanged

        If Not initted Then Return

        Dim digitsOnly = New Regex("[^\d]")
        InitialBalance.Text = digitsOnly.Replace(InitialBalance.Text, "")

        Settings.DTLInitialBalance = CInt("0" & InitialBalance.Text)
        isChanged = True

    End Sub

    Private Sub OwnerNameTextbox_TextChanged(sender As Object, e As EventArgs) Handles OwnerNameTextbox.TextChanged

        If Not initted Then Return
        Settings.GlbOwnerName = OwnerNameTextbox.Text
        isChanged = True

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged

        If Not initted Then Return
        Banker.Visible = False
        Settings.Banker = ""
        isChanged = True

    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged

        If Not initted Then Return
        Banker.Visible = False
        Settings.Banker = "00000000-0000-0000-0000-000000000000"
        isChanged = True

    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged

        If Not initted Then Return
        Banker.Visible = True
        isChanged = True

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles MoneyPortTextbox.TextChanged

        If Not initted Then Return

        Dim digitsOnly = New Regex("[^\d]")
        MoneyPortTextbox.Text = digitsOnly.Replace(MoneyPortTextbox.Text, "")

        Settings.DTLMoneyPort = CInt("0" & MoneyPortTextbox.Text)
        isChanged = True
        CheckDefaultPorts()

    End Sub

#End Region

End Class
