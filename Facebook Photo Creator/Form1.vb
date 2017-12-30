Imports System.Drawing.Imaging

Public Class Form1


    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ToolStripButton3.BackColor = PicContainer1.Backcolor2
        KeyPreview = True
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim ofd As New OpenFileDialog
        ofd.Filter = "Image Files (*.*)|*.*"
        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                PicContainer1.ProfileImage = Bitmap.FromFile(ofd.FileName)
                Button4.Enabled = True
                CheckBox3.Enabled = True
                GroupBox2.Enabled = True
                RadioButton2.Enabled = True
                RadioButton2.Checked = True
            Catch ex As Exception
                MsgBox("Failed to load image", MsgBoxStyle.Critical, "Error")
            End Try
        End If
        ofd.Dispose()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim ofd As New OpenFileDialog
        ofd.Filter = "Image Files (*.*)|*.*"
        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                PicContainer1.CoverImage = Bitmap.FromFile(ofd.FileName)
                Button3.Enabled = True
                CheckBox1.Enabled = True
                RadioButton1.Enabled = True
                RadioButton1.Checked = True
            Catch ex As Exception
                MsgBox("Failed to load image", MsgBoxStyle.Critical, "Error")
            End Try
        End If
        ofd.Dispose()
    End Sub

    Private Sub PictureBox1_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox1.Click
        Dim col As New ColorDialog
        col.Color = PictureBox1.BackColor
        If col.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBox1.BackColor = col.Color
            CheckBox2_CheckedChanged(Nothing, Nothing)
        End If
        col.Dispose()
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox2.CheckedChanged
        PicContainer1.TransparencyColor = If(CheckBox2.Checked, PictureBox1.BackColor, Nothing)
        PictureBox1.Enabled = CheckBox2.Checked
        TrackBar1.Enabled = CheckBox2.Checked

    End Sub

    Private Sub TrackBar1_Scroll(sender As System.Object, e As System.EventArgs) Handles TrackBar1.Scroll
        PicContainer1.TransparencyRange = TrackBar1.Value
    End Sub

    Private Sub TrackBar2_Scroll(sender As System.Object, e As System.EventArgs) Handles TrackBar2.Scroll
        PicContainer1.Transparency = TrackBar2.Value
    End Sub


    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        PicContainer1.CoverImage = Nothing
        Button3.Enabled = False
        CheckBox1.Enabled = False
        RadioButton1.Enabled = False
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        PicContainer1.ProfileImage = Nothing
        Button4.Enabled = False
        CheckBox3.Enabled = False
        GroupBox2.Enabled = False
        RadioButton2.Enabled = False
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        PicContainer1.ExtendCoverToProfile = CheckBox1.Checked
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox3.CheckedChanged
        PicContainer1.ExtendProfileToCover = CheckBox3.Checked
    End Sub

   

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        If RadioButton1.Enabled AndAlso RadioButton1.Checked Then
            PicContainer1.ResizeRects(PicContainer.SelectedRect.Cover, PicContainer.ResizeMode.Stretch)
        ElseIf RadioButton2.Enabled AndAlso RadioButton2.Checked Then
            PicContainer1.ResizeRects(PicContainer.SelectedRect.Profile, PicContainer.ResizeMode.Stretch)
        End If


    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        If RadioButton1.Enabled AndAlso RadioButton1.Checked Then
            PicContainer1.ResizeRects(PicContainer.SelectedRect.Cover, PicContainer.ResizeMode.Zoom)
        ElseIf RadioButton2.Enabled AndAlso RadioButton2.Checked Then
            PicContainer1.ResizeRects(PicContainer.SelectedRect.Profile, PicContainer.ResizeMode.Zoom)
        End If

    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        If RadioButton1.Enabled AndAlso RadioButton1.Checked Then
            PicContainer1.ResizeRects(PicContainer.SelectedRect.Cover, PicContainer.ResizeMode.Normal)
        ElseIf RadioButton2.Enabled AndAlso RadioButton2.Checked Then
            PicContainer1.ResizeRects(PicContainer.SelectedRect.Profile, PicContainer.ResizeMode.Normal)
        End If
    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click
        If RadioButton1.Enabled AndAlso RadioButton1.Checked Then
            PicContainer1.ResizeRects(PicContainer.SelectedRect.Cover, PicContainer.ResizeMode.Autosize)
        ElseIf RadioButton2.Enabled AndAlso RadioButton2.Checked Then
            PicContainer1.ResizeRects(PicContainer.SelectedRect.Profile, PicContainer.ResizeMode.Autosize)
        End If
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click
        Dim sfd As New SaveFileDialog
        sfd.FileName = "Cover Photo.jpg"
        sfd.Filter = "Jpeg Image (*.jpg)|*.jpg"
        If sfd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Dim Params As New EncoderParameters()
                Params.Param(0) = New EncoderParameter(Imaging.Encoder.Quality, 100)
                PicContainer1.GetCover.Save(sfd.FileName, JpegEncoder, Params)
                Params.Dispose()
                MsgBox("File Saved.", MsgBoxStyle.Information, "Sucess")
            Catch ex As Exception
                MsgBox("File Error : " & ex.Message, , "Error")
            End Try

        End If
    End Sub

    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click
        Dim sfd As New SaveFileDialog
        sfd.FileName = "Profile Photo.jpg"
        sfd.Filter = "Jpeg Image (*.jpg)|*.jpg"
        If sfd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Dim Params As New EncoderParameters()
                Params.Param(0) = New EncoderParameter(Imaging.Encoder.Quality, 100)
                PicContainer1.GetProfile.Save(sfd.FileName, JpegEncoder, Params)
                Params.Dispose()

                MsgBox("File Saved.", MsgBoxStyle.Information, "Sucess")
            Catch ex As Exception
                MsgBox("File Error : " & ex.Message, , "Error")
            End Try
        End If
    End Sub

    Private Shared Function GetEncoder(ByVal ImageFormat As ImageFormat) As ImageCodecInfo
        For Each Encode In ImageCodecInfo.GetImageEncoders
            If Encode.FormatID = ImageFormat.Guid Then Return Encode
        Next
        Return Nothing
    End Function
    Public Shared JpegEncoder As ImageCodecInfo = GetEncoder(ImageFormat.Jpeg)
     
    Private Sub OpenCoverPhotoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OpenCoverPhotoToolStripMenuItem.Click
        Button1_Click(Nothing, Nothing)
    End Sub

    Private Sub OpenProfilePhotoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OpenProfilePhotoToolStripMenuItem.Click
        Button2_Click(Nothing, Nothing)
    End Sub

    Private Sub SaveCoverPhotoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveCoverPhotoToolStripMenuItem.Click
        Button9_Click(Nothing, Nothing)
    End Sub

    Private Sub SaveProfilePhotoToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveProfilePhotoToolStripMenuItem.Click
        Button10_Click(Nothing, Nothing)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("Facebook Cover Photo Creator v1.0 Created by Brahim Hadriche." & vbNewLine & "Please wait for the release of version 2, much more fun!" & vbNewLine & "For any suggestions, please contact us through our facebook fan page.", MsgBoxStyle.Information, "About")
    End Sub
    Private Sub HelpToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles HelpToolStripMenuItem.Click
        MsgBox("Help Section under construction, Please check the facebook fan page for any future updates", MsgBoxStyle.Information, "Help")
    End Sub

    Private Sub FacebookPageToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FacebookPageToolStripMenuItem.Click
        Process.Start("http://www.facebook.com/CoverPhotoCreator")
    End Sub

    
    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        Dim col As New ColorDialog
        col.Color = ToolStripButton3.BackColor
        If col.ShowDialog = Windows.Forms.DialogResult.OK Then
            ToolStripButton3.BackColor = col.Color
            PicContainer1.Backcolor2 = col.Color
        End If
        col.Dispose()
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        If ToolStripButton4.Checked Then
            Opacity = 0.8
        Else
            Opacity = 1
        End If
    End Sub
     
    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        ToolStripButton2.Enabled = TreeView1.SelectedNode IsNot Nothing
    End Sub

   
End Class
