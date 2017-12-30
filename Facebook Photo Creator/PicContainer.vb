Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Public Class PicContainer
    Inherits UserControl
    Private Background As Bitmap = My.Resources.pic2.Clone
    Private ProfileCadre As New Bitmap(170, 170)
    Protected Overrides Sub OnHandleCreated(e As System.EventArgs)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.Selectable, True)
        ia2.SetColorMatrix(m2)
        Dim g As Graphics = Graphics.FromImage(ProfileCadre)
        g.DrawImage(Background, New Rectangle(0, 0, 170, 170), 199, 209, 170, 170, GraphicsUnit.Pixel)
        g.Dispose()
        ProfileCadre.SetPixel(0, 169, Color.Transparent)
        ProfileCadre.SetPixel(169, 169, Color.Transparent)
        MyBase.OnHandleCreated(e)
    End Sub
    Private _CoverImage As Bitmap
    Property CoverImage As Bitmap
        Get
            Return _CoverImage
        End Get
        Set(value As Bitmap)
            _CoverImage = value
            _CoverRect = _PrimCoverRect
            Invalidate()
        End Set
    End Property
    Private _ProfileImage As Bitmap
    Property ProfileImage As Bitmap
        Get
            Return _ProfileImage
        End Get
        Set(value As Bitmap)
            _ProfileImage = value
            _ProfileRect = _PrimProfileRect
            Invalidate()
        End Set
    End Property
    Private _Backcolor As Color = Color.Gray
    Property Backcolor2 As Color
        Get
            Return _Backcolor
        End Get
        Set(value As Color)
            _Backcolor = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnResize(e As System.EventArgs)
        Offset = New Point(Width / 2 - Background.Width / 2, 37)
        Invalidate()
        MyBase.OnResize(e)
    End Sub
    Dim Offset As New Point(Width / 2 - _PrimUnion.Width / 2, 37)
    Dim m2 As New ColorMatrix With {.Matrix00 = 0.4, .Matrix11 = 0.4, .Matrix22 = 0.4}
    Dim ia2 As New ImageAttributes
    Dim sbback As New SolidBrush(Color.FromArgb(24, 36, 61))
    Dim bpen As New Pen(Color.FromArgb(8, 22, 52))
    Dim bpen2 As New Pen(Color.FromArgb(76, 78, 80))
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
        e.Graphics.FillRectangle(sbback, 0, 0, Width, 37)
        e.Graphics.DrawLine(bpen, 0, 37, Width, 37)
        e.Graphics.DrawLine(bpen2, 0, 38, Width, 38)
        e.Graphics.DrawImage(Background, New Rectangle(New Point, Background.Size), -Offset.X, 0, Background.Width, Background.Height, GraphicsUnit.Pixel, ia2)

        e.Graphics.TranslateTransform(Offset.X + 184, Offset.Y)
        Dim sb As New SolidBrush(_Backcolor)
        e.Graphics.FillRectangle(sb, _PrimCoverRect)
        e.Graphics.DrawRectangle(Pens.Black, _PrimCoverRect)
        e.Graphics.FillRectangle(sb, _PrimProfileRect)

        'Draw Cover
        If _CoverImage IsNot Nothing Then
            Dim L As Integer = Math.Max(((_PrimCoverRect.Left - _CoverRect.Left) / _CoverRect.Width) * _CoverImage.Width, 0)
            Dim T As Integer = Math.Max(((_PrimCoverRect.Top - _CoverRect.Top) / _CoverRect.Height) * _CoverImage.Height, 0)
            Dim R As Integer = Math.Min(((_PrimCoverRect.Right - _CoverRect.Left) / _CoverRect.Width) * _CoverImage.Width, _CoverImage.Width)
            Dim B As Integer = Math.Min(((_PrimCoverRect.Bottom - _CoverRect.Top) / _CoverRect.Height) * _CoverImage.Height, _CoverImage.Height)
            Dim rc As Rectangle = Rectangle.FromLTRB(L, T, R, B)
            Dim intersec As Rectangle = Rectangle.Intersect(_PrimCoverRect, _CoverRect)
            e.Graphics.DrawImage(_CoverImage, intersec, rc, GraphicsUnit.Pixel)


            If _ExtendCoverToProfile Then
                L = Math.Max(((_PrimProfileRect.Left - _CoverRect.Left) / _CoverRect.Width) * _CoverImage.Width, 0)
                T = Math.Max(((_PrimProfileRect.Top - _CoverRect.Top) / _CoverRect.Height) * _CoverImage.Height, 0)
                R = Math.Min(((_PrimProfileRect.Right - _CoverRect.Left) / _CoverRect.Width) * _CoverImage.Width, _CoverImage.Width)
                B = Math.Min(((_PrimProfileRect.Bottom - _CoverRect.Top) / _CoverRect.Height) * _CoverImage.Height, _CoverImage.Height)
                rc = Rectangle.FromLTRB(L, T, R, B)
                intersec = Rectangle.Intersect(_PrimProfileRect, _CoverRect)
                e.Graphics.DrawImage(_CoverImage, intersec, rc, GraphicsUnit.Pixel)
            Else
                e.Graphics.FillRectangle(sb, _PrimProfileRect)
            End If
            DrawRects(e.Graphics, _CoverRect, SelectedRect.Cover)

        End If
        e.Graphics.DrawRectangle(Pens.Black, _PrimProfileRect)

        'Draw Profile
        If _ProfileImage IsNot Nothing Then
            Dim ia As New ImageAttributes
            If _TransparencyColor <> Nothing Then
                Dim lowC As Color = Color.FromArgb(Math.Max(CInt(_TransparencyColor.A) - _TransRange, 0), Math.Max(CInt(_TransparencyColor.R) - _TransRange, 0), Math.Max(CInt(_TransparencyColor.G) - _TransRange, 0), Math.Max(CInt(_TransparencyColor.B) - _TransRange, 0))
                Dim HighC As Color = Color.FromArgb(Math.Min(CInt(_TransparencyColor.A) + _TransRange, 255), Math.Min(CInt(_TransparencyColor.R) + _TransRange, 255), Math.Min(CInt(_TransparencyColor.G) + _TransRange, 255), Math.Min(CInt(_TransparencyColor.B) + _TransRange, 255))
                ia.SetColorKey(lowC, HighC)
            End If
            If _transparency <> 255 Then
                Dim m As New ColorMatrix
                m.Matrix33 = _transparency / 255
                ia.SetColorMatrix(m)
            End If
            Dim L, T, R, B As Integer
            Dim intersec, rc As Rectangle
            If _ExtendProfileToCover Then
                L = Math.Max(((_PrimCoverRect.Left - _ProfileRect.Left) / _ProfileRect.Width) * _ProfileImage.Width, 0)
                T = Math.Max(((_PrimCoverRect.Top - _ProfileRect.Top) / _ProfileRect.Height) * _ProfileImage.Height, 0)
                R = Math.Min(((_PrimCoverRect.Right - _ProfileRect.Left) / _ProfileRect.Width) * _ProfileImage.Width, _ProfileImage.Width)
                B = Math.Min(((_PrimCoverRect.Bottom - _ProfileRect.Top) / _ProfileRect.Height) * _ProfileImage.Height, _ProfileImage.Height)
                rc = Rectangle.FromLTRB(L, T, R, B)
                intersec = Rectangle.Intersect(_PrimCoverRect, _ProfileRect)
                e.Graphics.DrawImage(_ProfileImage, intersec, rc.X, rc.Y, rc.Width, rc.Height, GraphicsUnit.Pixel, ia)


                Dim rect As Rectangle = Rectangle.FromLTRB(_PrimProfileRect.Left, _PrimCoverRect.Bottom, _PrimProfileRect.Right, _PrimProfileRect.Bottom)


                L = Math.Max(((rect.Left - _ProfileRect.Left) / _ProfileRect.Width) * _ProfileImage.Width, 0)
                T = Math.Max(((rect.Top - _ProfileRect.Top) / _ProfileRect.Height) * _ProfileImage.Height, 0)
                R = Math.Min(((rect.Right - _ProfileRect.Left) / _ProfileRect.Width) * _ProfileImage.Width, _ProfileImage.Width)
                B = Math.Min(((rect.Bottom - _ProfileRect.Top) / _ProfileRect.Height) * _ProfileImage.Height, _ProfileImage.Height)
                rc = Rectangle.FromLTRB(L, T, R, B)
                intersec = Rectangle.Intersect(rect, _ProfileRect)
                e.Graphics.DrawImage(_ProfileImage, intersec, rc.X, rc.Y, rc.Width, rc.Height, GraphicsUnit.Pixel, ia)

            Else

                L = Math.Max(((_PrimProfileRect.Left - _ProfileRect.Left) / _ProfileRect.Width) * _ProfileImage.Width, 0)
                T = Math.Max(((_PrimProfileRect.Top - _ProfileRect.Top) / _ProfileRect.Height) * _ProfileImage.Height, 0)
                R = Math.Min(((_PrimProfileRect.Right - _ProfileRect.Left) / _ProfileRect.Width) * _ProfileImage.Width, _ProfileImage.Width)
                B = Math.Min(((_PrimProfileRect.Bottom - _ProfileRect.Top) / _ProfileRect.Height) * _ProfileImage.Height, _ProfileImage.Height)
                rc = Rectangle.FromLTRB(L, T, R, B)
                intersec = Rectangle.Intersect(_PrimProfileRect, _ProfileRect)
                e.Graphics.DrawImage(_ProfileImage, intersec, rc.X, rc.Y, rc.Width, rc.Height, GraphicsUnit.Pixel, ia)

            End If
            e.Graphics.DrawImage(ProfileCadre, New Rectangle(16, 173, 170, 170), 0, 0, 170, 170, GraphicsUnit.Pixel)
            e.Graphics.DrawRectangle(Pens.Black, _PrimProfileRect)
            DrawRects(e.Graphics, _ProfileRect, SelectedRect.Profile)
            ia.Dispose()
        Else
            e.Graphics.DrawImage(ProfileCadre, New Rectangle(16, 173, 170, 170), 0, 0, 170, 170, GraphicsUnit.Pixel)
            e.Graphics.DrawRectangle(Pens.Black, _PrimProfileRect)
        End If
        sb.Dispose()
        MyBase.OnPaint(e)
    End Sub
    Private _CoverRect As New Rectangle(0, 0, 850, 314)
    Private _ProfileRect As New Rectangle(20, 177, 160, 160)
    Private _PrimCoverRect As New Rectangle(0, 0, 850, 314)
    Private _PrimProfileRect As New Rectangle(20, 177, 160, 160)
    Private _PrimUnion As Rectangle = Rectangle.Union(_PrimCoverRect, _PrimProfileRect)
    Private _TransparencyColor As Color
    Property TransparencyColor As Color
        Get
            Return _TransparencyColor
        End Get
        Set(value As Color)
            If value <> _TransparencyColor Then
                _TransparencyColor = value
                Invalidate()
            End If
        End Set
    End Property
    Private _TransRange As Integer
    Property TransparencyRange As Integer
        Get
            Return _TransRange
        End Get
        Set(value As Integer)
            If value <> _TransRange Then
                _TransRange = value
                Invalidate()
            End If
        End Set
    End Property
    Private _transparency As Integer = 255
    Property Transparency As Integer
        Get
            Return _transparency
        End Get
        Set(value As Integer)
            If value <> _transparency AndAlso 0 <= value AndAlso value < 256 Then
                _transparency = value
                Invalidate()
            End If
        End Set
    End Property
    Private _ExtendCoverToProfile As Boolean = True
    Property ExtendCoverToProfile As Boolean
        Get
            Return _ExtendCoverToProfile
        End Get
        Set(value As Boolean)
            If value <> _ExtendCoverToProfile Then
                _ExtendCoverToProfile = value
                Invalidate()
            End If
        End Set
    End Property
    Private _ExtendProfileToCover As Boolean = False
    Property ExtendProfileToCover As Boolean
        Get
            Return _ExtendProfileToCover
        End Get
        Set(value As Boolean)
            If value <> _ExtendProfileToCover Then
                _ExtendProfileToCover = value
                Invalidate()
            End If
        End Set
    End Property
    Sub ResizeRects(ByVal rect As SelectedRect, ByVal mode As ResizeMode)
        If rect = SelectedRect.Cover AndAlso _CoverImage IsNot Nothing Then
            Select Case mode
                Case ResizeMode.Autosize
                    Dim ratio As Double = Math.Max(_CoverImage.Width / _PrimCoverRect.Width, _CoverImage.Height / _PrimCoverRect.Height)
                    _CoverRect.Size = New Size(_CoverImage.Size.Width / ratio, _CoverImage.Size.Height / ratio)
                    _CoverRect.Location = New Point(_PrimCoverRect.Width / 2 - _CoverRect.Width / 2 + _PrimCoverRect.X, _PrimCoverRect.Height / 2 - _CoverRect.Height / 2 + _PrimCoverRect.Y)

                Case ResizeMode.Normal
                    _CoverRect = New Rectangle(_PrimCoverRect.Location, _CoverImage.Size)
                Case ResizeMode.Stretch
                    _CoverRect = _PrimCoverRect
                Case ResizeMode.Zoom
                    Dim ratio As Double = Math.Min(_CoverImage.Width / _PrimCoverRect.Width, _CoverImage.Height / _PrimCoverRect.Height)
                    _CoverRect.Size = New Size(_CoverImage.Size.Width / ratio, _CoverImage.Size.Height / ratio)
                    _CoverRect.Location = New Point(_PrimCoverRect.Width / 2 - _CoverRect.Width / 2 + _PrimCoverRect.X, _PrimCoverRect.Height / 2 - _CoverRect.Height / 2 + _PrimCoverRect.Y)
            End Select
        ElseIf rect = SelectedRect.Profile AndAlso _ProfileImage IsNot Nothing Then
            Select Case mode
                Case ResizeMode.Autosize
                    Dim ratio As Double = Math.Max(_ProfileImage.Width / _PrimProfileRect.Width, _ProfileImage.Height / _PrimProfileRect.Height)
                    _ProfileRect.Size = New Size(_ProfileImage.Size.Width / ratio, _ProfileImage.Size.Height / ratio)
                    _ProfileRect.Location = New Point(_PrimProfileRect.Width / 2 - _ProfileRect.Width / 2 + _PrimProfileRect.X, _PrimProfileRect.Height / 2 - _ProfileRect.Height / 2 + _PrimProfileRect.Y)
                Case ResizeMode.Normal
                    _ProfileRect = New Rectangle(_PrimProfileRect.Location, _ProfileImage.Size)
                Case ResizeMode.Stretch
                    _ProfileRect = _PrimProfileRect
                Case ResizeMode.Zoom
                    Dim ratio As Double = Math.Min(_ProfileImage.Width / _PrimProfileRect.Width, _ProfileImage.Height / _PrimProfileRect.Height)
                    _ProfileRect.Size = New Size(_ProfileImage.Size.Width / ratio, _ProfileImage.Size.Height / ratio)
                    _ProfileRect.Location = New Point(_PrimProfileRect.Width / 2 - _ProfileRect.Width / 2 + _PrimProfileRect.X, _PrimProfileRect.Height / 2 - _ProfileRect.Height / 2 + _PrimProfileRect.Y)
            End Select
        End If
        Invalidate()
    End Sub
    Enum ResizeMode
        Autosize
        Stretch
        Zoom
        Normal
    End Enum
    Private Sub DrawRects(ByVal g As Graphics, ByVal rect As Rectangle, ByVal selected As SelectedRect)
        g.DrawRectangle(If(selected = _CurrentSelectedRect, Pens.Lime, Pens.Red), rect)
        If selected = _CurrentSelectedRect Then
            Select Case selected
                Case SelectedRect.Cover
                    If rect.IntersectsWith(_PrimCoverRect) Then
                        rect.Intersect(_PrimCoverRect)
                    End If
                Case SelectedRect.Profile
                    If rect.IntersectsWith(_PrimProfileRect) Then
                        rect.Intersect(_PrimProfileRect)
                    End If
            End Select
            g.FillRectangle(Brushes.Blue, New Rectangle(rect.Left - 3, rect.Top - 3, 7, 7))
            g.FillRectangle(Brushes.Blue, New Rectangle(rect.Right - 3, rect.Top - 3, 7, 7))
            g.FillRectangle(Brushes.Blue, New Rectangle(rect.Left - 3, rect.Bottom - 3, 7, 7))
            g.FillRectangle(Brushes.Blue, New Rectangle(rect.Right - 3, rect.Bottom - 3, 7, 7))
        End If
    End Sub
    Enum SelectedRect
        Cover
        Profile
        None
    End Enum

    Enum MouseRegion
        CovTopLeft
        CovTopRight
        CovBottomLeft
        CovBottomRight
        CovDrag
        ProfTopLeft
        ProfTopRight
        ProfBottomLeft
        ProfBottomRight
        ProfDrag
        None
    End Enum
    Function GetMouseRegion(ByVal Location As Point) As MouseRegion

        Location.Offset(-Offset.X - 184, -Offset.Y)
        Dim rect, rct As Rectangle

        If _ProfileImage IsNot Nothing Then
            rct = _ProfileRect
            If rct.IntersectsWith(_PrimProfileRect) Then rct.Intersect(_PrimProfileRect)

            rect = New Rectangle(rct.Left - 3, rct.Top - 3, 7, 7)
            If rect.Contains(Location) Then Return MouseRegion.ProfTopLeft

            rect = New Rectangle(rct.Right - 3, rct.Top - 3, 7, 7)
            If rect.Contains(Location) Then Return MouseRegion.ProfTopRight

            rect = New Rectangle(rct.Left - 3, rct.Bottom - 3, 7, 7)
            If rect.Contains(Location) Then Return MouseRegion.ProfBottomLeft

            rect = New Rectangle(rct.Right - 3, rct.Bottom - 3, 7, 7)
            If rect.Contains(Location) Then Return MouseRegion.ProfBottomRight

            If _ProfileRect.Contains(Location) Then Return MouseRegion.ProfDrag

        End If
        If _CoverImage IsNot Nothing Then
            rct = _CoverRect
            If rct.IntersectsWith(_PrimCoverRect) Then rct.Intersect(_PrimCoverRect)

            rect = New Rectangle(rct.Left - 3, rct.Top - 3, 7, 7)
            If rect.Contains(Location) Then Return MouseRegion.CovTopLeft

            rect = New Rectangle(rct.Right - 3, rct.Top - 3, 7, 7)
            If rect.Contains(Location) Then Return MouseRegion.CovTopRight

            rect = New Rectangle(rct.Left - 3, rct.Bottom - 3, 7, 7)
            If rect.Contains(Location) Then Return MouseRegion.CovBottomLeft

            rect = New Rectangle(rct.Right - 3, rct.Bottom - 3, 7, 7)
            If rect.Contains(Location) Then Return MouseRegion.CovBottomRight

            If _CoverRect.Contains(Location) Then Return MouseRegion.CovDrag

        End If

        Return MouseRegion.None
    End Function
    Dim CurrentMouseRegion As MouseRegion
    Dim _CurrentSelectedRect As SelectedRect
    Dim MouseDownState As Boolean
    Dim MouseDownLocation As Point
    Dim TempRect As Rectangle
    Property CurrentSelectedRect As SelectedRect
        Get
            Return _CurrentSelectedRect
        End Get
        Set(value As SelectedRect)
            If value <> _CurrentSelectedRect Then
                _CurrentSelectedRect = value
                Invalidate()
            End If
        End Set
    End Property
    Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
        If Not MouseDownState Then
            CurrentMouseRegion = GetMouseRegion(e.Location)
            If _CoverImage IsNot Nothing Then
                Select Case CurrentMouseRegion
                    Case MouseRegion.CovTopLeft, MouseRegion.CovTopRight, MouseRegion.CovBottomLeft, MouseRegion.CovBottomRight
                        Cursor = Cursors.Hand
                        CurrentSelectedRect = SelectedRect.Cover
                    Case MouseRegion.CovDrag
                        Cursor = Cursors.SizeAll
                        CurrentSelectedRect = SelectedRect.Cover
                    Case MouseRegion.None
                        Cursor = Cursors.Default
                        CurrentSelectedRect = SelectedRect.None
                End Select
            End If

            If _ProfileImage IsNot Nothing Then
                Select Case CurrentMouseRegion
                    Case MouseRegion.ProfTopLeft, MouseRegion.ProfTopRight, MouseRegion.ProfBottomLeft, MouseRegion.ProfBottomRight
                        Cursor = Cursors.Hand
                        CurrentSelectedRect = SelectedRect.Profile
                    Case MouseRegion.ProfDrag
                        Cursor = Cursors.SizeAll
                        CurrentSelectedRect = SelectedRect.Profile
                    Case MouseRegion.None
                        Cursor = Cursors.Default
                        CurrentSelectedRect = SelectedRect.None
                End Select
            End If
        Else
            Select Case CurrentMouseRegion
                Case MouseRegion.ProfTopLeft
                    _ProfileRect.Location = New Point(TempRect.X + e.Location.X - MouseDownLocation.X, TempRect.Y + e.Location.Y - MouseDownLocation.Y)
                    _ProfileRect.Size = Size.Add(TempRect.Size, New Size(-e.Location.X + MouseDownLocation.X, -e.Location.Y + MouseDownLocation.Y))
                    Invalidate()
                Case MouseRegion.ProfTopRight
                    _ProfileRect.Location = New Point(_ProfileRect.X, TempRect.Y + e.Location.Y - MouseDownLocation.Y)
                    _ProfileRect.Width = TempRect.Width + e.Location.X - MouseDownLocation.X
                    _ProfileRect.Height = TempRect.Height - e.Location.Y + MouseDownLocation.Y
                    Invalidate()
                Case MouseRegion.ProfBottomLeft
                    _ProfileRect.Location = New Point(TempRect.X + e.Location.X - MouseDownLocation.X, _ProfileRect.Y)
                    _ProfileRect.Width = TempRect.Width - e.Location.X + MouseDownLocation.X
                    _ProfileRect.Height = TempRect.Height + e.Location.Y - MouseDownLocation.Y
                    Invalidate()
                Case MouseRegion.ProfBottomRight
                    _ProfileRect.Size = Size.Add(TempRect.Size, New Size(e.Location.X - MouseDownLocation.X, e.Location.Y - MouseDownLocation.Y))
                    Invalidate()
                Case MouseRegion.ProfDrag
                    _ProfileRect.Location = New Point(e.Location.X - MouseDownLocation.X + TempRect.X, e.Location.Y - MouseDownLocation.Y + TempRect.Y)
                    Invalidate()

                Case MouseRegion.CovTopLeft
                    _CoverRect.Location = New Point(TempRect.X + e.Location.X - MouseDownLocation.X, TempRect.Y + e.Location.Y - MouseDownLocation.Y)
                    _CoverRect.Size = Size.Add(TempRect.Size, New Size(-e.Location.X + MouseDownLocation.X, -e.Location.Y + MouseDownLocation.Y))
                    Invalidate()
                Case MouseRegion.CovTopRight
                    _CoverRect.Location = New Point(_CoverRect.X, TempRect.Y + e.Location.Y - MouseDownLocation.Y)
                    _CoverRect.Width = TempRect.Width + e.Location.X - MouseDownLocation.X
                    _CoverRect.Height = TempRect.Height - e.Location.Y + MouseDownLocation.Y
                    Invalidate()
                Case MouseRegion.CovBottomLeft
                    _CoverRect.Location = New Point(TempRect.X + e.Location.X - MouseDownLocation.X, _CoverRect.Y)
                    _CoverRect.Width = TempRect.Width - e.Location.X + MouseDownLocation.X
                    _CoverRect.Height = TempRect.Height + e.Location.Y - MouseDownLocation.Y
                    Invalidate()
                Case MouseRegion.CovBottomRight
                    _CoverRect.Size = Size.Add(TempRect.Size, New Size(e.Location.X - MouseDownLocation.X, e.Location.Y - MouseDownLocation.Y))
                    Invalidate()
                Case MouseRegion.CovDrag
                    _CoverRect.Location = New Point(e.Location.X - MouseDownLocation.X + TempRect.X, e.Location.Y - MouseDownLocation.Y + TempRect.Y)
                    Invalidate()

            End Select
             
        End If

        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseDown(e As System.Windows.Forms.MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            MouseDownState = True
            Select Case CurrentSelectedRect
                Case SelectedRect.Cover
                    TempRect = _CoverRect
                Case SelectedRect.Profile
                    TempRect = _ProfileRect
            End Select
            MouseDownLocation = e.Location
        End If
        MyBase.OnMouseDown(e)
    End Sub
    Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            MouseDownState = False
        End If
        MyBase.OnMouseUp(e)
    End Sub
    
    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, keyData As System.Windows.Forms.Keys) As Boolean
        Select Case keyData
            Case Keys.Up
                If CurrentSelectedRect = SelectedRect.Cover Then
                    _CoverRect.Offset(0, -1)
                ElseIf CurrentSelectedRect = SelectedRect.Profile Then
                    _ProfileRect.Offset(0, -1)
                End If
            Case Keys.Down
                If CurrentSelectedRect = SelectedRect.Cover Then
                    _CoverRect.Offset(0, 1)
                ElseIf CurrentSelectedRect = SelectedRect.Profile Then
                    _ProfileRect.Offset(0, 1)
                End If
            Case Keys.Right
                If CurrentSelectedRect = SelectedRect.Cover Then
                    _CoverRect.Offset(1, 0)
                ElseIf CurrentSelectedRect = SelectedRect.Profile Then
                    _ProfileRect.Offset(1, 0)
                End If
            Case Keys.Left
                If CurrentSelectedRect = SelectedRect.Cover Then
                    _CoverRect.Offset(-1, 0)
                ElseIf CurrentSelectedRect = SelectedRect.Profile Then
                    _ProfileRect.Offset(-1, 0)
                End If
            Case Else
                Return MyBase.ProcessCmdKey(msg, keyData)
        End Select
        Invalidate()
        Return True
    End Function
    Function GetCover() As Bitmap
        Dim bitm As New Bitmap(_PrimCoverRect.Width, _PrimCoverRect.Height)
        Dim g As Graphics = Graphics.FromImage(bitm)
        g.SmoothingMode = SmoothingMode.HighQuality
        g.FillRectangle(New SolidBrush(_Backcolor), 0, 0, bitm.Width, bitm.Height)
        If _CoverImage IsNot Nothing Then
            Dim L As Integer = Math.Max(((0 - _CoverRect.Left) / _CoverRect.Width) * _CoverImage.Width, 0)
            Dim T As Integer = Math.Max(((0 - _CoverRect.Top) / _CoverRect.Height) * _CoverImage.Height, 0)
            Dim R As Integer = Math.Min(((bitm.Width - _CoverRect.Left) / _CoverRect.Width) * _CoverImage.Width, _CoverImage.Width)
            Dim B As Integer = Math.Min(((bitm.Height - _CoverRect.Top) / _CoverRect.Height) * _CoverImage.Height, _CoverImage.Height)
            Dim rc As Rectangle = Rectangle.FromLTRB(L, T, R, B)
            Dim intersec = Rectangle.Intersect(New Rectangle(0, 0, bitm.Width, bitm.Height), _CoverRect)
            g.DrawImage(_CoverImage, intersec, rc, GraphicsUnit.Pixel)
        End If
        If _ProfileImage IsNot Nothing AndAlso _ExtendProfileToCover Then
            If _PrimCoverRect.IntersectsWith(_ProfileRect) Then
                Dim ia As New ImageAttributes
                If _TransparencyColor <> Nothing Then
                    Dim lowC As Color = Color.FromArgb(Math.Max(CInt(_TransparencyColor.A) - _TransRange, 0), Math.Max(CInt(_TransparencyColor.R) - _TransRange, 0), Math.Max(CInt(_TransparencyColor.G) - _TransRange, 0), Math.Max(CInt(_TransparencyColor.B) - _TransRange, 0))
                    Dim HighC As Color = Color.FromArgb(Math.Min(CInt(_TransparencyColor.A) + _TransRange, 255), Math.Min(CInt(_TransparencyColor.R) + _TransRange, 255), Math.Min(CInt(_TransparencyColor.G) + _TransRange, 255), Math.Min(CInt(_TransparencyColor.B) + _TransRange, 255))
                    ia.SetColorKey(lowC, HighC)
                End If
                If _transparency <> 255 Then
                    Dim m As New ColorMatrix
                    m.Matrix33 = _transparency / 255
                    ia.SetColorMatrix(m)
                End If
                Dim L As Integer = Math.Max(((_PrimCoverRect.Left - _ProfileRect.Left) / _ProfileRect.Width) * _ProfileImage.Width, 0)
                Dim T As Integer = Math.Max(((_PrimCoverRect.Top - _ProfileRect.Top) / _ProfileRect.Height) * _ProfileImage.Height, 0)
                Dim R As Integer = Math.Min(((_PrimCoverRect.Right - _ProfileRect.Left) / _ProfileRect.Width) * _ProfileImage.Width, _ProfileImage.Width)
                Dim B As Integer = Math.Min(((_PrimCoverRect.Bottom - _ProfileRect.Top) / _ProfileRect.Height) * _ProfileImage.Height, _ProfileImage.Height)
                Dim rc As Rectangle = Rectangle.FromLTRB(L, T, R, B)
                Dim intersec = Rectangle.Intersect(_PrimCoverRect, _ProfileRect)
                g.DrawImage(_ProfileImage, intersec, rc.X, rc.Y, rc.Width, rc.Height, GraphicsUnit.Pixel, ia)
            End If
        End If
        g.Dispose()
        Return bitm
    End Function
    Function GetProfile() As Bitmap
        Dim bitm As New Bitmap(_PrimProfileRect.Width, _PrimProfileRect.Height)
        Dim g As Graphics = Graphics.FromImage(bitm)
        g.SmoothingMode = SmoothingMode.HighQuality
        g.FillRectangle(New SolidBrush(_Backcolor), 0, 0, bitm.Width, bitm.Height)
        If _CoverImage IsNot Nothing Then
            If _ExtendCoverToProfile Then
                Dim L As Integer = Math.Max(((_PrimProfileRect.Left - _CoverRect.Left) / _CoverRect.Width) * _CoverImage.Width, 0)
                Dim T As Integer = Math.Max(((_PrimProfileRect.Top - _CoverRect.Top) / _CoverRect.Height) * _CoverImage.Height, 0)
                Dim R As Integer = Math.Min(((_PrimProfileRect.Right - _CoverRect.Left) / _CoverRect.Width) * _CoverImage.Width, _CoverImage.Width)
                Dim B As Integer = Math.Min(((_PrimProfileRect.Bottom - _CoverRect.Top) / _CoverRect.Height) * _CoverImage.Height, _CoverImage.Height)
                Dim rc As Rectangle = Rectangle.FromLTRB(L, T, R, B)
                Dim intersec = Rectangle.Intersect(_PrimProfileRect, _CoverRect)
                intersec.Offset(-_PrimProfileRect.X, -_PrimProfileRect.Y)
                g.DrawImage(_CoverImage, intersec, rc, GraphicsUnit.Pixel)
            Else
                Dim rect As Rectangle = Rectangle.Intersect(_PrimCoverRect, _PrimProfileRect)
                Dim L As Integer = Math.Max(((rect.Left - _CoverRect.Left) / _CoverRect.Width) * _CoverImage.Width, 0)
                Dim T As Integer = Math.Max(((rect.Top - _CoverRect.Top) / _CoverRect.Height) * _CoverImage.Height, 0)
                Dim R As Integer = Math.Min(((rect.Right - _CoverRect.Left) / _CoverRect.Width) * _CoverImage.Width, _CoverImage.Width)
                Dim B As Integer = Math.Min(((rect.Bottom - _CoverRect.Top) / _CoverRect.Height) * _CoverImage.Height, _CoverImage.Height)
                Dim rc As Rectangle = Rectangle.FromLTRB(L, T, R, B)
                Dim intersec = Rectangle.Intersect(rect, _CoverRect)
                intersec.Offset(-_PrimProfileRect.X, -_PrimProfileRect.Y)
                g.DrawImage(_CoverImage, intersec, rc, GraphicsUnit.Pixel)
            End If
        End If
      
        If _ProfileImage IsNot Nothing Then
            If _PrimProfileRect.IntersectsWith(_ProfileRect) Then
                Dim ia As New ImageAttributes
                If _TransparencyColor <> Nothing Then
                    Dim lowC As Color = Color.FromArgb(Math.Max(CInt(_TransparencyColor.A) - _TransRange, 0), Math.Max(CInt(_TransparencyColor.R) - _TransRange, 0), Math.Max(CInt(_TransparencyColor.G) - _TransRange, 0), Math.Max(CInt(_TransparencyColor.B) - _TransRange, 0))
                    Dim HighC As Color = Color.FromArgb(Math.Min(CInt(_TransparencyColor.A) + _TransRange, 255), Math.Min(CInt(_TransparencyColor.R) + _TransRange, 255), Math.Min(CInt(_TransparencyColor.G) + _TransRange, 255), Math.Min(CInt(_TransparencyColor.B) + _TransRange, 255))
                    ia.SetColorKey(lowC, HighC)
                End If
                If _transparency <> 255 Then
                    Dim m As New ColorMatrix
                    m.Matrix33 = _transparency / 255
                    ia.SetColorMatrix(m)
                End If
                Dim L As Integer = Math.Max(((_PrimProfileRect.Left - _ProfileRect.Left) / _ProfileRect.Width) * _ProfileImage.Width, 0)
                Dim T As Integer = Math.Max(((_PrimProfileRect.Top - _ProfileRect.Top) / _ProfileRect.Height) * _ProfileImage.Height, 0)
                Dim R As Integer = Math.Min(((_PrimProfileRect.Right - _ProfileRect.Left) / _ProfileRect.Width) * _ProfileImage.Width, _ProfileImage.Width)
                Dim B As Integer = Math.Min(((_PrimProfileRect.Bottom - _ProfileRect.Top) / _ProfileRect.Height) * _ProfileImage.Height, _ProfileImage.Height)
                Dim rc As Rectangle = Rectangle.FromLTRB(L, T, R, B)
                Dim intersec = Rectangle.Intersect(_PrimProfileRect, _ProfileRect)
                intersec.Offset(-_PrimProfileRect.X, -_PrimProfileRect.Y)
                g.DrawImage(_ProfileImage, intersec, rc.X, rc.Y, rc.Width, rc.Height, GraphicsUnit.Pixel, ia)
            End If
        End If
        g.Dispose()

        Dim bitm2 As New Bitmap(bitm.Width * 2, bitm.Height * 2)
        Dim g2 As Graphics = Graphics.FromImage(bitm2)
        g2.DrawImage(bitm, 0, 0, bitm2.Width, bitm2.Height)
        g2.Dispose()
        bitm.Dispose()
        Return bitm2
    End Function
 End Class
Structure ImageLayer
    Public RowImage As Bitmap
    Public InProcessImage As Bitmap
    Public Rect As Rectangle
    Public DrawProfile As Boolean
    Public DrawCover As Boolean
    Public Filt As Filters
    Public FilterParams As Dictionary(Of Filters, Object)
    Event FilterChanges()
    Sub AddFilter(ByVal newFilt As Filters, ByVal params() As Object)
        Filt = Filt Or newFilt
        FilterParams.Add(newFilt, params)
        RaiseEvent FilterChanges()
    End Sub
    Sub RemoveFilter(ByVal DelFilt As Filters)
        If Filt And DelFilt = DelFilt Then Filt = Filt Xor DelFilt
        FilterParams.Remove(DelFilt)
        RaiseEvent FilterChanges()
    End Sub
    Sub UpdateParams(ByVal Filter As Filters, ByVal params() As Object)
        If ((Filt And Filter) = Filter) AndAlso FilterParams.ContainsKey(Filter) Then
            FilterParams(Filter) = params
            RaiseEvent FilterChanges()
        End If
    End Sub
End Structure
<Flags()> Enum Filters
    None = 0
    TransparencyColor = 1
    Transparency = 2
    Blur = 4
    GrayScale = 8
End Enum
Class FilterApp
    Shared Sub ApplyFilters(ByRef ImLayer As ImageLayer)
        'Call to other filters
    End Sub
    'Source from Filters, Possibility to convert to bitlocks.
    Shared Sub Transparency(ByRef ImLayer As ImageLayer)
        If ImLayer.Filt And Filters.Transparency = Filters.Transparency Then
            Dim trans As Integer = CInt(ImLayer.FilterParams(Filters.Transparency))
            Dim mx As New ColorMatrix
            mx.Matrix33 = trans / 255
            Dim ia As New ImageAttributes
            ia.SetColorMatrix(mx)
            Dim btm As New Bitmap(ImLayer.InProcessImage.Width, ImLayer.InProcessImage.Height)
            Dim g As Graphics = Graphics.FromImage(btm)
            g.Clear(Color.Transparent)
            g.DrawImage(ImLayer.InProcessImage, New Rectangle(0, 0, btm.Width, btm.Height), 0, 0, btm.Width, btm.Height, GraphicsUnit.Pixel, ia)
            g.Dispose()
            ia.Dispose()
        End If
    End Sub
End Class