﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormSpeech
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
                Synth.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.GroupBoxSpeech = New System.Windows.Forms.GroupBox()
        Me.ViewWebLabel = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.CacheSizeLabel = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.APILabel = New System.Windows.Forms.Label()
        Me.APIKeyTextBox = New System.Windows.Forms.TextBox()
        Me.SpeakButton = New System.Windows.Forms.Button()
        Me.CacheFolderLabel = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.MakeSpeechButton = New System.Windows.Forms.Button()
        Me.SpeechBox = New System.Windows.Forms.ComboBox()
        Me.MenuStrip2 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem30 = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBoxSpeech.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBoxSpeech
        '
        Me.GroupBoxSpeech.Controls.Add(Me.ViewWebLabel)
        Me.GroupBoxSpeech.Controls.Add(Me.PictureBox2)
        Me.GroupBoxSpeech.Controls.Add(Me.CacheSizeLabel)
        Me.GroupBoxSpeech.Controls.Add(Me.TextBox2)
        Me.GroupBoxSpeech.Controls.Add(Me.APILabel)
        Me.GroupBoxSpeech.Controls.Add(Me.APIKeyTextBox)
        Me.GroupBoxSpeech.Controls.Add(Me.SpeakButton)
        Me.GroupBoxSpeech.Controls.Add(Me.CacheFolderLabel)
        Me.GroupBoxSpeech.Controls.Add(Me.PictureBox1)
        Me.GroupBoxSpeech.Controls.Add(Me.TextBox1)
        Me.GroupBoxSpeech.Controls.Add(Me.MakeSpeechButton)
        Me.GroupBoxSpeech.Controls.Add(Me.SpeechBox)
        Me.GroupBoxSpeech.Location = New System.Drawing.Point(12, 37)
        Me.GroupBoxSpeech.Name = "GroupBoxSpeech"
        Me.GroupBoxSpeech.Size = New System.Drawing.Size(422, 360)
        Me.GroupBoxSpeech.TabIndex = 1890
        Me.GroupBoxSpeech.TabStop = False
        Me.GroupBoxSpeech.Text = "Speech"
        '
        'ViewWebLabel
        '
        Me.ViewWebLabel.AutoSize = True
        Me.ViewWebLabel.Location = New System.Drawing.Point(289, 109)
        Me.ViewWebLabel.Name = "ViewWebLabel"
        Me.ViewWebLabel.Size = New System.Drawing.Size(56, 13)
        Me.ViewWebLabel.TabIndex = 1901
        Me.ViewWebLabel.Text = "View Web"
        '
        'PictureBox2
        '
        Me.PictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.PictureBox2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox2.Image = Global.Outworldz.My.Resources.Resources.edge
        Me.PictureBox2.Location = New System.Drawing.Point(252, 103)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(29, 30)
        Me.PictureBox2.TabIndex = 1900
        Me.PictureBox2.TabStop = False
        '
        'CacheSizeLabel
        '
        Me.CacheSizeLabel.AutoSize = True
        Me.CacheSizeLabel.Location = New System.Drawing.Point(299, 76)
        Me.CacheSizeLabel.Name = "CacheSizeLabel"
        Me.CacheSizeLabel.Size = New System.Drawing.Size(103, 13)
        Me.CacheSizeLabel.TabIndex = 1899
        Me.CacheSizeLabel.Text = "Cache Size in Hours"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(251, 69)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(33, 20)
        Me.TextBox2.TabIndex = 1898
        '
        'APILabel
        '
        Me.APILabel.AutoSize = True
        Me.APILabel.Location = New System.Drawing.Point(259, 14)
        Me.APILabel.Name = "APILabel"
        Me.APILabel.Size = New System.Drawing.Size(24, 13)
        Me.APILabel.TabIndex = 1897
        Me.APILabel.Text = "API"
        '
        'APIKeyTextBox
        '
        Me.APIKeyTextBox.Location = New System.Drawing.Point(250, 30)
        Me.APIKeyTextBox.Name = "APIKeyTextBox"
        Me.APIKeyTextBox.Size = New System.Drawing.Size(141, 20)
        Me.APIKeyTextBox.TabIndex = 1896
        '
        'SpeakButton
        '
        Me.SpeakButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.SpeakButton.Image = Global.Outworldz.My.Resources.Resources.loudspeaker
        Me.SpeakButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.SpeakButton.Location = New System.Drawing.Point(17, 109)
        Me.SpeakButton.Margin = New System.Windows.Forms.Padding(1)
        Me.SpeakButton.Name = "SpeakButton"
        Me.SpeakButton.Size = New System.Drawing.Size(141, 35)
        Me.SpeakButton.TabIndex = 1895
        Me.SpeakButton.Text = "Speak"
        Me.SpeakButton.UseVisualStyleBackColor = True
        '
        'CacheFolderLabel
        '
        Me.CacheFolderLabel.AutoSize = True
        Me.CacheFolderLabel.Location = New System.Drawing.Point(289, 145)
        Me.CacheFolderLabel.Name = "CacheFolderLabel"
        Me.CacheFolderLabel.Size = New System.Drawing.Size(64, 13)
        Me.CacheFolderLabel.TabIndex = 1893
        Me.CacheFolderLabel.Text = "View Cache"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox1.Image = Global.Outworldz.My.Resources.Resources.document_view
        Me.PictureBox1.Location = New System.Drawing.Point(252, 139)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(31, 30)
        Me.PictureBox1.TabIndex = 1892
        Me.PictureBox1.TabStop = False
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(17, 175)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(374, 169)
        Me.TextBox1.TabIndex = 1891
        Me.TextBox1.Text = "M: Speaks with a Male Voice"
        '
        'MakeSpeechButton
        '
        Me.MakeSpeechButton.Cursor = System.Windows.Forms.Cursors.Hand
        Me.MakeSpeechButton.Image = Global.Outworldz.My.Resources.Resources.loudspeaker
        Me.MakeSpeechButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.MakeSpeechButton.Location = New System.Drawing.Point(17, 65)
        Me.MakeSpeechButton.Margin = New System.Windows.Forms.Padding(1)
        Me.MakeSpeechButton.Name = "MakeSpeechButton"
        Me.MakeSpeechButton.Size = New System.Drawing.Size(208, 35)
        Me.MakeSpeechButton.TabIndex = 1890
        Me.MakeSpeechButton.Text = "Make Speech (Wav + Mp3)"
        Me.MakeSpeechButton.UseVisualStyleBackColor = True
        '
        'SpeechBox
        '
        Me.SpeechBox.AutoCompleteCustomSource.AddRange(New String() {"1 Hour", "4 Hour", "12 Hour", "Daily", "Weekly"})
        Me.SpeechBox.FormattingEnabled = True
        Me.SpeechBox.Location = New System.Drawing.Point(17, 30)
        Me.SpeechBox.Margin = New System.Windows.Forms.Padding(1)
        Me.SpeechBox.MaxDropDownItems = 15
        Me.SpeechBox.Name = "SpeechBox"
        Me.SpeechBox.Size = New System.Drawing.Size(208, 21)
        Me.SpeechBox.Sorted = True
        Me.SpeechBox.TabIndex = 1889
        '
        'MenuStrip2
        '
        Me.MenuStrip2.ImageScalingSize = New System.Drawing.Size(28, 28)
        Me.MenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem30})
        Me.MenuStrip2.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip2.Name = "MenuStrip2"
        Me.MenuStrip2.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.MenuStrip2.Size = New System.Drawing.Size(446, 34)
        Me.MenuStrip2.TabIndex = 1889
        Me.MenuStrip2.Text = "0"
        '
        'ToolStripMenuItem30
        '
        Me.ToolStripMenuItem30.Image = Global.Outworldz.My.Resources.Resources.question_and_answer
        Me.ToolStripMenuItem30.Name = "ToolStripMenuItem30"
        Me.ToolStripMenuItem30.Size = New System.Drawing.Size(72, 32)
        Me.ToolStripMenuItem30.Text = Global.Outworldz.My.Resources.Resources.Help_word
        '
        'FormSpeech
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(446, 408)
        Me.Controls.Add(Me.GroupBoxSpeech)
        Me.Controls.Add(Me.MenuStrip2)
        Me.Name = "FormSpeech"
        Me.Text = "FormSpeech"
        Me.GroupBoxSpeech.ResumeLayout(False)
        Me.GroupBoxSpeech.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip2.ResumeLayout(False)
        Me.MenuStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBoxSpeech As GroupBox
    Friend WithEvents CacheFolderLabel As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents MakeSpeechButton As Button
    Friend WithEvents SpeechBox As ComboBox
    Friend WithEvents MenuStrip2 As MenuStrip
    Friend WithEvents ToolStripMenuItem30 As ToolStripMenuItem
    Friend WithEvents SpeakButton As Button
    Friend WithEvents APILabel As Label
    Friend WithEvents APIKeyTextBox As TextBox
    Friend WithEvents CacheSizeLabel As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents ViewWebLabel As Label
    Friend WithEvents PictureBox2 As PictureBox
End Class
