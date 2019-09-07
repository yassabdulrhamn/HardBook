Imports System.IO
Public Class Form
    'امر متعلق بصور من داخل اللعبة (Games)
    Dim ListOfImage As New List(Of Image)
    Dim iclick As Integer
    'امر تمدد البرنامج
    Dim ProportionsArray() As CtrlProportions
    Private Structure CtrlProportions
        Dim HeightProportions As Single
        Dim WidthProportions As Single
        Dim TopProportions As Single
        Dim LeftProportions As Single
    End Structure
    Private ctrl As New List(Of Control)
    Private Sub Form1_HandleCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.HandleCreated
        Informload()
    End Sub
    Sub Informload()
        On Error Resume Next
        Application.DoEvents()
        ctrl = GetControls(Me)
        For i = 0 To ctrl.Count - 1
            If TypeOf (ctrl(i)) Is Form Then ctrl.Remove(ctrl(i))
        Next
        ReDim ProportionsArray(0 To ctrl.Count - 1)
        For I As Integer = 0 To ctrl.Count - 1
            With ProportionsArray(I)
                .HeightProportions = ctrl(I).Height / Height
                .WidthProportions = ctrl(I).Width / Width
                .TopProportions = ctrl(I).Top / Height
                .LeftProportions = ctrl(I).Left / Width
            End With
        Next
    End Sub
    Private Function GetControls(ByVal Ctrl As Control) As List(Of Control)
        GetControls = New List(Of Control)
        GetControls.Add(Ctrl)
        For Each c As Control In Ctrl.Controls
            GetControls.AddRange(GetControls(c))
        Next
    End Function
    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Resizeform()
    End Sub
    Public Sub Resizeform()
        On Error Resume Next
        For I As Integer = 0 To ctrl.Count - 1
            ctrl(I).Left = ProportionsArray(I).LeftProportions * Me.Width
            ctrl(I).Top = ProportionsArray(I).TopProportions * Me.Height
            ctrl(I).Width = ProportionsArray(I).WidthProportions * Me.Width
            ctrl(I).Height = ProportionsArray(I).HeightProportions * Me.Height
        Next
    End Sub
    'حفظ الجلسة
    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        ' عند إغلاق الفورم نبدء بحفظ الجلسه
        ' نضع البيانات في المتغير مع ديلميتر التقسيم
        Try
            Dim data As String
            data = ":" & ListBoxMoviesName.SelectedIndex.ToString() & ":" & ListBoxSeries.SelectedIndex.ToString() & ":" & ComboBoxSeriesSe.SelectedIndex.ToString() & ":" & ComboBoxSeriesEp.SelectedIndex.ToString() & ":" & ListBoxDramaName.SelectedIndex.ToString() & ":" & ListBoxDramaEP.SelectedIndex.ToString() & ":" & ListBoxAnimeName.SelectedIndex.ToString() & ":" & ListBoxAnimeEP.SelectedIndex.ToString() & ":" & ComboBoxAnimationName.SelectedIndex.ToString() & ":" & ComboBoxGame.SelectedIndex.ToString() & ":" & ListViewPro.SelectedItems(0).Index.ToString()
            System.IO.File.WriteAllText("Ya8S8eR Book Program Files\session.txt", data)
        Catch ex As Exception
        End Try
    End Sub
    'امر نقل اسماء المجلدات الموجودة لتالي و امر مكمل لامر التلوين اللست بوكس
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For Each Dir As String In Directory.GetDirectories(Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Movies")
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Movies\"
            Dir = Dir.Replace(path, "")
            ListBoxMoviesName.Items.Add(Dir)
        Next
        For Each Dir As String In Directory.GetDirectories(Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Animation")
            Dim FullPath As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Animation\"
            Dir = Dir.Replace(FullPath, "")
            ComboBoxAnimationName.Items.Add(Dir)
        Next
        For Each Dir As String In Directory.GetDirectories(Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Series")
            Dim FullPath As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Series\"
            Dir = Dir.Replace(FullPath, "")
            ListBoxSeries.Items.Add(Dir)
        Next
        For Each Dir As String In Directory.GetDirectories(Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Dramas")
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Dramas\"
            Dir = Dir.Replace(path, "")
            ListBoxDramaName.Items.Add(Dir)
        Next
        For Each Dir As String In Directory.GetDirectories(Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Anime")
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Anime\"
            Dir = Dir.Replace(path, "")
            ListBoxAnimeName.Items.Add(Dir)

        Next
        'ايقونات البرامج في االقائمة
        For Each Dir As String In My.Computer.FileSystem.GetDirectories(Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Programs")
            For Each icn In IO.Directory.GetFiles(Dir, "*.png")
                ImageListPrograms.Images.Add(Image.FromFile(icn))
            Next
            ListViewPro.Items.Add(My.Computer.FileSystem.GetName(Dir), GetIcon())
            intIndex = intIndex + 1
        Next
        For Each Dir As String In Directory.GetDirectories(Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Games\Pc")
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Games\Pc\"
            Dir = Dir.Replace(path, "")
            ComboBoxGame.Items.Add(Dir)
        Next

        With ComboBoxSeriesEp
            .DrawMode = DrawMode.OwnerDrawFixed
        End With
        With ComboBoxSeriesSe
            .DrawMode = DrawMode.OwnerDrawFixed
        End With
        With ListBoxDramaName
            .DrawMode = DrawMode.OwnerDrawFixed
        End With
        With ListBoxAnimeName
            .DrawMode = DrawMode.OwnerDrawFixed
        End With
        With ListBoxSeries
            .DrawMode = DrawMode.OwnerDrawFixed
        End With
        With ListBoxMoviesName
            .DrawMode = DrawMode.OwnerDrawFixed
        End With
        With ListBoxAnimeEP
            .DrawMode = DrawMode.OwnerDrawFixed
        End With
        With ListBoxDramaEP
            .DrawMode = DrawMode.OwnerDrawFixed
        End With
        With ComboBoxAnimationName
            .DrawMode = DrawMode.OwnerDrawFixed
        End With
        'تكملة حفظ الجلسة
        ' نقرأ محتوى الملف ونقسم البيانات الموجودة
        Try
            Dim data() As String = Split(System.IO.File.ReadAllText("Ya8S8eR Book Program Files\session.txt"), ":")
            ' نحدد العنصر اللذي تم تحديده سابقا
            'ListViewPro.Items(Integer.Parse(data(1))).Selected = True
            ListBoxMoviesName.SelectedIndex = Integer.Parse(data(1))
            ListBoxSeries.SelectedIndex = Integer.Parse(data(2))
            ComboBoxSeriesSe.SelectedIndex = Integer.Parse(data(3))
            ComboBoxSeriesEp.SelectedIndex = Integer.Parse(data(4))
            ListBoxDramaName.SelectedIndex = Integer.Parse(data(5))
            ListBoxDramaEP.SelectedIndex = Integer.Parse(data(6))
            ListBoxAnimeName.SelectedIndex = Integer.Parse(data(7))
            ListBoxAnimeEP.SelectedIndex = Integer.Parse(data(8))
            ComboBoxAnimationName.SelectedIndex = Integer.Parse(data(9))
            ComboBoxGame.SelectedIndex = Integer.Parse(data(10))
            ListViewPro.Items(Integer.Parse(data(11))).Selected = True
        Catch ex As Exception
        End Try
    End Sub
    'ازرار الانتقال بين الاقسام
    Private Sub ButtonMovies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMovies.Click
        GroupBoxMovies.Location = New Point(GroupBoxMovies.Location.X + Me.Size.Width, GroupBoxMovies.Location.Y)
        GroupBoxMovies.BringToFront()
        GroupBoxMovies_Move.Start()
    End Sub
    Private Sub ButtonSeries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSeries.Click
        GroupBoxSeriesName.Location = New Point(GroupBoxSeriesName.Location.X + Me.Size.Width, GroupBoxSeriesName.Location.Y)
        GroupBoxSeriesName.BringToFront()
        GroupBoxSeriesName_Move.Start()
    End Sub
    Private Sub ButtonDrama_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDrama.Click
        GroupBoxDrama.Location = New Point(GroupBoxDrama.Location.X + Me.Size.Width, GroupBoxDrama.Location.Y)
        GroupBoxDrama.BringToFront()
        GroupBoxDrama_Move.Start()
    End Sub
    Private Sub ButtonAnime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnime.Click
        GroupBoxAnime.Location = New Point(GroupBoxAnime.Location.X + Me.Size.Width, GroupBoxAnime.Location.Y)
        GroupBoxAnime.BringToFront()
        GroupBoxAnime_Move.Start()
    End Sub
    Private Sub ButtonAnimation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnimation.Click
        GroupBoxAnimation.Location = New Point(GroupBoxAnimation.Location.X + Me.Size.Width, GroupBoxAnimation.Location.Y)
        GroupBoxAnimation.BringToFront()
        GroupBoxAnimation_Move.Start()
    End Sub
    Private Sub ButtonPrograms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPrograms.Click
        GroupBoxPro.Location = New Point(GroupBoxPro.Location.X + Me.Size.Width, GroupBoxPro.Location.Y)
        GroupBoxPro.BringToFront()
        GroupBoxPro_Move.Start()
    End Sub
    Private Sub ButtonGames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGames.Click
        GroupBoxGames.Location = New Point(GroupBoxGames.Location.X + Me.Size.Width, GroupBoxGames.Location.Y)
        GroupBoxGames.BringToFront()
        GroupBoxGames_Move.Start()
    End Sub
    'امر استعراض الصورة و النص
    Private Sub ListBoxMoviesName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBoxMoviesName.SelectedIndexChanged
        If Me.Size.Width < 1200 Then
            PictureBoxMovies.Size = New Size(Me.Size.Width / 3.18, Me.Size.Height / 1.6)
            PictureBoxMovies.Location = New Point(Me.Size.Width / 1.67, Me.Size.Height / 5.5)
            TimerMovies.Start()
        End If
        Try
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Movies\"
            For Each txt As FileInfo In New DirectoryInfo(path & ListBoxMoviesName.SelectedItem).GetFiles("*.txt")
                TextBoxMovies.Text = My.Computer.FileSystem.ReadAllText(txt.FullName)
            Next
            For Each Img2 As FileInfo In New DirectoryInfo(path & ListBoxMoviesName.SelectedItem).GetFiles("*.jpg")
                PictureBoxMovies.Image = Image.FromFile(Img2.FullName)
            Next
        Catch ex As Exception
        End Try
        LabelMovieLabelMovieName1.Text = ListBoxMoviesName.SelectedItem
        LabelNumpersMovies.Text = ListBoxMoviesName.SelectedIndices(0) + 1 & "\" & ListBoxMoviesName.Items.Count
        Dim Mpath As String = (Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Movies\")
        Dim Mindex As Integer = ListBoxMoviesName.SelectedIndex
        Try
            PictureBoxMovies2.Image = Image.FromFile(Mpath & ListBoxMoviesName.Items(Mindex + 1).ToString & "\" & ListBoxMoviesName.Items(Mindex + 1).ToString & ".jpg")
        Catch ex As Exception
            If Mindex = ListBoxMoviesName.Items.Count - 1 Then
                PictureBoxMovies2.Image = Image.FromFile(Mpath & ListBoxMoviesName.Items(Mindex - 1).ToString & "\" & ListBoxMoviesName.Items(Mindex - 1).ToString & ".jpg")
            End If
        End Try
        Try
            Dim path2 As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Movies\"
            Dim fileDetail = My.Computer.FileSystem.GetFileInfo(path2 & ListBoxMoviesName.SelectedItem & "\" & ListBoxMoviesName.SelectedItem + ".mkv")
            If fileDetail.Length >= 8000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(0)
            End If
            If fileDetail.Length <= 8000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(1)
            End If
            If fileDetail.Length <= 2000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(2)
            End If
        Catch ex As Exception
        End Try
        Try
            Dim path2 As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Movies\"
            Dim fileDetail = My.Computer.FileSystem.GetFileInfo(path2 & ListBoxMoviesName.SelectedItem & "\" & ListBoxMoviesName.SelectedItem + ".mp4")
            If fileDetail.Length >= 8000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(0)
            End If
            If fileDetail.Length <= 8000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(1)
            End If
            If fileDetail.Length <= 2000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(2)
            End If
        Catch ex As Exception
        End Try
        Try
            Dim path2 As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Movies\"
            Dim fileDetail = My.Computer.FileSystem.GetFileInfo(path2 & ListBoxMoviesName.SelectedItem & "\" & ListBoxMoviesName.SelectedItem + ".3pg")
            If fileDetail.Length >= 8000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(0)
            End If
            If fileDetail.Length <= 8000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(1)
            End If
            If fileDetail.Length <= 2000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(2)
            End If
        Catch ex As Exception
        End Try
        Try
            Dim path2 As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Movies\"
            Dim fileDetail = My.Computer.FileSystem.GetFileInfo(path2 & ListBoxMoviesName.SelectedItem & "\" & ListBoxMoviesName.SelectedItem + ".avi")
            If fileDetail.Length >= 8000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(0)
            End If
            If fileDetail.Length <= 8000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(1)
            End If
            If fileDetail.Length <= 2000000000 Then
                PictureBoxMovieQuality.Image = ImageListMovies.Images(2)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ListBoxSeries_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBoxSeries.SelectedIndexChanged
        If Me.Size.Width < 1200 Then
            PictureBoxSeries.Size = New Size(Me.Size.Width / 3.18, Me.Size.Height / 1.6)
            PictureBoxSeries.Location = New Point(Me.Size.Width / 1.67, Me.Size.Height / 5.5)
            TimerSeries.Start()
        End If
        ComboBoxSeriesSe.Items.Clear()
        ComboBoxSeriesEp.Items.Clear()
        Dim FDri As New DirectoryInfo(Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Series\" & ListBoxSeries.SelectedItem)
        For Each SecDir As DirectoryInfo In FDri.GetDirectories
            ComboBoxSeriesSe.Items.Add(SecDir.Name)
        Next
        Try
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Series\"
            For Each Img2 As FileInfo In New DirectoryInfo(path & ListBoxSeries.SelectedItem).GetFiles("*.jpg")
                PictureBoxSeries.Image = Image.FromFile(Img2.FullName)
            Next
            For Each txt As FileInfo In New DirectoryInfo(path & ListBoxSeries.SelectedItem).GetFiles("*.txt")
                TextBoxSeries.Text = My.Computer.FileSystem.ReadAllText(txt.FullName)
            Next
        Catch ex As Exception
        End Try
        LabelSeries.Text = ListBoxSeries.SelectedItem

        ComboBoxSeriesSe.SelectedIndex = 0
        ComboBoxSeriesEp.SelectedIndex = 0
        Dim Mpath As String = (Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Series\")
        Dim Mindex As Integer = ListBoxSeries.SelectedIndex
        Try
            PictureBoxSeries2.Image = Image.FromFile(Mpath & ListBoxSeries.Items(Mindex + 1).ToString & "\" & ListBoxSeries.Items(Mindex + 1).ToString & ".jpg")
        Catch ex As Exception
            If Mindex = ListBoxSeries.Items.Count - 1 Then
                PictureBoxSeries2.Image = Image.FromFile(Mpath & ListBoxSeries.Items(Mindex - 1).ToString & "\" & ListBoxSeries.Items(Mindex - 1).ToString & ".jpg")
            End If
        End Try
    End Sub
    Private Sub ListBoxDramaName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBoxDramaName.SelectedIndexChanged
        If Me.Size.Width < 1200 Then
            PictureBoxDrama.Size = New Size(Me.Size.Width / 3.18, Me.Size.Height / 1.6)
            PictureBoxDrama.Location = New Point(Me.Size.Width / 1.67, Me.Size.Height / 5.5)
            TimerDrama.Start()
        End If
        Dim FullPath As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Dramas\" & ListBoxDramaName.SelectedItem
        Try
            ListBoxDramaEP.Items.Clear()
            Dim Mydir As New IO.DirectoryInfo(FullPath)
            Dim A001 As IO.FileInfo() = Mydir.GetFiles("*.mkv")
            Dim A002 As IO.FileInfo
            For Each A002 In A001
                ListBoxDramaEP.Items.Add(A002.Name)
            Next
            Dim Mydi As New IO.DirectoryInfo(FullPath)
            Dim B001 As IO.FileInfo() = Mydi.GetFiles("*.avi")
            Dim B002 As IO.FileInfo
            For Each B002 In B001
                ListBoxDramaEP.Items.Add(B002.Name)
            Next
            Dim Myd As New IO.DirectoryInfo(FullPath)
            Dim C001 As IO.FileInfo() = Myd.GetFiles("*.rmvb")
            Dim C002 As IO.FileInfo
            For Each C002 In C001
                ListBoxDramaEP.Items.Add(C002.Name)
            Next
            Dim MydD As New IO.DirectoryInfo(FullPath)
            Dim D001 As IO.FileInfo() = Myd.GetFiles("*.mp4")
            Dim D002 As IO.FileInfo
            For Each D002 In D001
                ListBoxDramaEP.Items.Add(D002.Name)
            Next
        Catch ex As Exception
        End Try
        Try
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Dramas\"
            For Each Img6 As FileInfo In New DirectoryInfo(path & ListBoxDramaName.SelectedItem).GetFiles("*.jpg")
                PictureBoxDrama.Image = Image.FromFile(Img6.FullName)
            Next
        Catch ex As Exception
        End Try
        LabelDrama.Text = ListBoxDramaName.SelectedItem
        ListBoxDramaEP.SelectedIndex = 0
        Dim Mpath As String = (Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Dramas\")
        Dim Mindex As Integer = ListBoxDramaName.SelectedIndex
        Try
            PictureBoxDrama2.Image = Image.FromFile(Mpath & ListBoxDramaName.Items(Mindex + 1).ToString & "\" & ListBoxDramaName.Items(Mindex + 1).ToString & ".jpg")
        Catch ex As Exception
            If Mindex = ListBoxDramaName.Items.Count - 1 Then
                PictureBoxDrama2.Image = Image.FromFile(Mpath & ListBoxDramaName.Items(Mindex - 1).ToString & "\" & ListBoxDramaName.Items(Mindex - 1).ToString & ".jpg")
            End If
        End Try
    End Sub
    Private Sub ListBoxAnimeName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBoxAnimeName.SelectedIndexChanged
        If Me.Size.Width < 1200 Then
            PictureBoxAnime.Size = New Size(Me.Size.Width / 3.18, Me.Size.Height / 1.6)
            PictureBoxAnime.Location = New Point(Me.Size.Width / 1.67, Me.Size.Height / 5.5)
            TimerAnime.Start()
        End If

        Try
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Anime\"
            For Each txt As FileInfo In New DirectoryInfo(path & ListBoxAnimeName.SelectedItem).GetFiles("*.txt")
                TextBoxAnime.Text = My.Computer.FileSystem.ReadAllText(txt.FullName)
            Next
            For Each Img2 As FileInfo In New DirectoryInfo(path & ListBoxAnimeName.SelectedItem).GetFiles("*.jpg")
                PictureBoxAnime.Image = Image.FromFile(Img2.FullName)
            Next
        Catch ex As Exception
        End Try
        Dim FullPath As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Anime\" & ListBoxAnimeName.SelectedItem
        Try
            ListBoxAnimeEP.Items.Clear()
            Dim Mydir As New IO.DirectoryInfo(FullPath)
            Dim A001 As IO.FileInfo() = Mydir.GetFiles("*.mkv")
            Dim A002 As IO.FileInfo
            For Each A002 In A001
                ListBoxAnimeEP.Items.Add(A002.Name)
            Next
            Dim Mydi As New IO.DirectoryInfo(FullPath)
            Dim B001 As IO.FileInfo() = Mydi.GetFiles("*.avi")
            Dim B002 As IO.FileInfo
            For Each B002 In B001
                ListBoxAnimeEP.Items.Add(B002.Name)
            Next
            Dim Myd As New IO.DirectoryInfo(FullPath)
            Dim C001 As IO.FileInfo() = Myd.GetFiles("*.rmvb")
            Dim C002 As IO.FileInfo
            For Each C002 In C001
                ListBoxAnimeEP.Items.Add(C002.Name)
            Next
            Dim MydD As New IO.DirectoryInfo(FullPath)
            Dim D001 As IO.FileInfo() = Myd.GetFiles("*.mp4")
            Dim D002 As IO.FileInfo
            For Each D002 In D001
                ListBoxAnimeEP.Items.Add(D002.Name)
            Next
        Catch ex As Exception
        End Try
        LabelAnime.Text = ListBoxAnimeName.SelectedItem
        ListBoxAnimeEP.SelectedIndex = 0
        Dim Mpath As String = (Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Anime\")
        Dim Mindex As Integer = ListBoxAnimeName.SelectedIndex
        Try
            PictureBoxAnime2.Image = Image.FromFile(Mpath & ListBoxAnimeName.Items(Mindex + 1).ToString & "\" & ListBoxAnimeName.Items(Mindex + 1).ToString & ".jpg")
        Catch ex As Exception
            If Mindex = ListBoxAnimeName.Items.Count - 1 Then
                PictureBoxAnime2.Image = Image.FromFile(Mpath & ListBoxAnimeName.Items(Mindex - 1).ToString & "\" & ListBoxAnimeName.Items(Mindex - 1).ToString & ".jpg")
            End If
        End Try
    End Sub

    Private Sub LabelAnimationName_Click(sender As System.Object, e As System.EventArgs) Handles LabelAnimationName.Click
        ComboBoxAnimationName.DroppedDown = True
    End Sub
    Private Sub ComboBoxAnimationName_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxAnimationName.SelectedIndexChanged
        PictureBoxAnimation.Size = New Size(Me.Size.Width / 2.99, Me.Size.Height / 1.301)
        PictureBoxAnimation.Location = New Point(Me.Size.Width / 3.15, Me.Size.Height / 13)
        TimerAnimation.Start()
        Try
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Animation\"
            For Each Img2 As FileInfo In New DirectoryInfo(path & ComboBoxAnimationName.SelectedItem).GetFiles("*.jpg")
                PictureBoxAnimation.Image = Image.FromFile(Img2.FullName)
            Next
        Catch ex As Exception
        End Try
        LabelAnimationName.Text = ComboBoxAnimationName.SelectedItem
        LabelNumpersAnimation.Text = ComboBoxAnimationName.SelectedIndex + 1 & "\" & ComboBoxAnimationName.Items.Count
        Dim Mpath As String = (Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Animation\")
        Dim Mindex As Integer = ComboBoxAnimationName.SelectedIndex
        Try
            PictureBoxAnimation2.Image = Image.FromFile(Mpath & ComboBoxAnimationName.Items(Mindex + 1).ToString & "\" & ComboBoxAnimationName.Items(Mindex + 1).ToString & ".jpg")
            PictureBoxAnimation0.Image = Image.FromFile(Mpath & ComboBoxAnimationName.Items(Mindex - 1).ToString & "\" & ComboBoxAnimationName.Items(Mindex - 1).ToString & ".jpg")
        Catch ex As Exception
            If Mindex = ComboBoxAnimationName.Items.Count - 1 Then
                PictureBoxAnimation2.Image = Image.FromFile(Mpath & ComboBoxAnimationName.Items(0).ToString & "\" & ComboBoxAnimationName.Items(0).ToString & ".jpg")
                PictureBoxAnimation0.Image = Image.FromFile(Mpath & ComboBoxAnimationName.Items(Mindex - 1).ToString & "\" & ComboBoxAnimationName.Items(Mindex - 1).ToString & ".jpg")
            End If
            If Mindex = 0 Then
                PictureBoxAnimation0.Image = Image.FromFile(Mpath & ComboBoxAnimationName.Items(ComboBoxAnimationName.Items.Count - 1).ToString & "\" & ComboBoxAnimationName.Items(ComboBoxAnimationName.Items.Count - 1).ToString & ".jpg")
                PictureBoxAnimation2.Image = Image.FromFile(Mpath & ComboBoxAnimationName.Items(Mindex + 1).ToString & "\" & ComboBoxAnimationName.Items(Mindex + 1).ToString & ".jpg")
            End If
        End Try
    End Sub
    'ازرار تشغيل الفيديو
    Private Sub ButtonPlayMovies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPlayMovies.Click
        Try
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Movies\"
            For Each imkv As FileInfo In New DirectoryInfo(path & ListBoxMoviesName.SelectedItem).GetFiles("*.mkv")
                Dim url As String
                url = imkv.FullName
                Process.Start(url)
            Next
            For Each iavi As FileInfo In New DirectoryInfo(path & ListBoxMoviesName.SelectedItem).GetFiles("*.avi")
                Dim url As String
                url = iavi.FullName
                Process.Start(url)
            Next
            For Each imp4 As FileInfo In New DirectoryInfo(path & ListBoxMoviesName.SelectedItem).GetFiles("*.mp4")
                Dim url As String
                url = imp4.FullName
                Process.Start(url)
            Next
            For Each i3pg As FileInfo In New DirectoryInfo(path & ListBoxMoviesName.SelectedItem).GetFiles("*.3pg")
                Dim url As String
                url = i3pg.FullName
                Process.Start(url)
            Next
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ButtonPlayAnimation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPlayAnimation.Click
        Try
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Animation\"
            For Each imkv As FileInfo In New DirectoryInfo(path & ComboBoxAnimationName.SelectedItem).GetFiles("*.mkv")
                Dim url As String
                url = imkv.FullName
                Process.Start(url)
            Next
            For Each iavi As FileInfo In New DirectoryInfo(path & ComboBoxAnimationName.SelectedItem).GetFiles("*.avi")
                Dim url As String
                url = iavi.FullName
                Process.Start(url)
            Next
            For Each imp4 As FileInfo In New DirectoryInfo(path & ComboBoxAnimationName.SelectedItem).GetFiles("*.mp4")
                Dim url As String
                url = imp4.FullName
                Process.Start(url)
            Next
            For Each i3pg As FileInfo In New DirectoryInfo(path & ComboBoxAnimationName.SelectedItem).GetFiles("*.rm")
                Dim url As String
                url = i3pg.FullName
                Process.Start(url)
            Next
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ButtonPlaySeries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPlaySeries.Click
        Dim path As String = Application.StartupPath & "Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Series\"
        Process.Start(path & ListBoxSeries.SelectedItem & "\" & ComboBoxSeriesSe.SelectedItem & "\" & ComboBoxSeriesEp.SelectedItem)
    End Sub
    Private Sub ButtonPlayDrama_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPlayDrama.Click
        Try
            Dim path As String = Application.StartupPath & "Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Dramas\"
            Process.Start(path & ListBoxDramaName.SelectedItem & "\" & ListBoxDramaEP.SelectedItem)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ButtonPlayAnime_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPlayAnime.Click
        Try
            Dim path As String = Application.StartupPath & "Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Anime\"
            Process.Start(path + ListBoxAnimeName.SelectedItem & "\" & ListBoxAnimeEP.SelectedItem)
        Catch ex As Exception
        End Try
    End Sub
    'اللوان الخلفية لتالي
    Private Sub ListBoxMoviesName_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListBoxMoviesName.DrawItem
        Dim myListBox As ListBox = CType(sender, ListBox)
        Dim sb As SolidBrush = New SolidBrush(Color.White)
        If e.Index Mod 1 = 0 Then
            sb.Color = Color.DodgerBlue
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If e.Index Mod 2 = 0 Then
            sb.Color = Color.DeepSkyBlue
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.Red, e.Bounds)
        End If
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
    End Sub
    Private Sub ComboBoxSeris_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ComboBoxSeriesSe.DrawItem
        Try
            Dim myListBox As ComboBox = CType(sender, ComboBox)
            Dim sb As SolidBrush = New SolidBrush(Color.White)
            If e.Index Mod 1 = 0 Then
                sb.Color = Color.LimeGreen
                e.Graphics.FillRectangle(sb, e.Bounds)

                e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
            End If
            If e.Index Mod 2 = 0 Then
                sb.Color = Color.PaleGreen
                e.Graphics.FillRectangle(sb, e.Bounds)

                e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
            End If

            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds)
            End If
            Using ssb As New SolidBrush(e.ForeColor)
                e.Graphics.DrawString(ComboBoxSeriesSe.GetItemText(ComboBoxSeriesSe.Items(e.Index)), e.Font, ssb, e.Bounds)
            End Using
            e.DrawFocusRectangle()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ComboBoxSeriesEp_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles ComboBoxSeriesEp.DrawItem
        Try
            Dim myListBox As ComboBox = CType(sender, ComboBox)
            Dim sb As SolidBrush = New SolidBrush(Color.White)
            If e.Index Mod 1 = 0 Then
                sb.Color = Color.PaleGreen
                e.Graphics.FillRectangle(sb, e.Bounds)

                e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
            End If
            If e.Index Mod 2 = 0 Then
                sb.Color = Color.LimeGreen
                e.Graphics.FillRectangle(sb, e.Bounds)

                e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
            End If

            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds)
            End If
            Using ssb As New SolidBrush(e.ForeColor)
                e.Graphics.DrawString(ComboBoxSeriesEp.GetItemText(ComboBoxSeriesEp.Items(e.Index)), e.Font, ssb, e.Bounds)
            End Using
            e.DrawFocusRectangle()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ListBoxSeriesEP_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListBoxSeries.DrawItem
        Dim myListBox As ListBox = CType(sender, ListBox)
        Dim sb As SolidBrush = New SolidBrush(Color.White)
        If e.Index Mod 1 = 0 Then
            sb.Color = Color.LimeGreen
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If e.Index Mod 2 = 0 Then
            sb.Color = Color.PaleGreen
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.Red, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Yellow), e.Bounds)
        End If
        Using ssb As New SolidBrush(e.ForeColor)
            e.Graphics.DrawString(ListBoxSeries.GetItemText(ListBoxSeries.Items(e.Index)), e.Font, ssb, e.Bounds)
        End Using
        e.DrawFocusRectangle()

    End Sub
    Private Sub ListBoxDramaName_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListBoxDramaName.DrawItem
        Dim myListBox As ListBox = CType(sender, ListBox)
        Dim sb As SolidBrush = New SolidBrush(Color.White)
        If e.Index Mod 1 = 0 Then
            sb.Color = Color.Pink
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If e.Index Mod 2 = 0 Then
            sb.Color = Color.HotPink
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.White), e.Bounds)
        End If
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.Red, e.Bounds)
        End If
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If

    End Sub
    Private Sub ListBoxDramaEP_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListBoxDramaEP.DrawItem
        Dim myListBox As ListBox = CType(sender, ListBox)
        Dim sb As SolidBrush = New SolidBrush(Color.White)
        If e.Index Mod 1 = 0 Then
            sb.Color = Color.Pink
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If e.Index Mod 2 = 0 Then
            sb.Color = Color.HotPink
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.Red, e.Bounds)
        End If
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If

    End Sub
    Private Sub ListBoxAnimeEP_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListBoxAnimeEP.DrawItem
        Dim myListBox As ListBox = CType(sender, ListBox)
        Dim sb As SolidBrush = New SolidBrush(Color.White)
        If e.Index Mod 1 = 0 Then
            sb.Color = Color.Khaki
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If e.Index Mod 2 = 0 Then
            sb.Color = Color.LightYellow
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.Red, e.Bounds)
        End If
        Using ssb As New SolidBrush(e.ForeColor)
            e.Graphics.DrawString(ComboBoxSeriesSe.GetItemText(ListBoxAnimeEP.Items(e.Index)), e.Font, ssb, e.Bounds)
        End Using
        e.DrawFocusRectangle()

    End Sub
    Private Sub ListBoxAnimeName_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListBoxAnimeName.DrawItem
        Dim myListBox As ListBox = CType(sender, ListBox)
        Dim sb As SolidBrush = New SolidBrush(Color.White)
        If e.Index Mod 1 = 0 Then
            sb.Color = Color.Khaki
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If e.Index Mod 2 = 0 Then
            sb.Color = Color.LightYellow
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.Red, e.Bounds)
        End If
        Using ssb As New SolidBrush(e.ForeColor)
            e.Graphics.DrawString(ListBoxAnimeName.GetItemText(ListBoxAnimeName.Items(e.Index)), e.Font, ssb, e.Bounds)
        End Using
        e.DrawFocusRectangle()

    End Sub
    Private Sub ComboBoxAnimationName_DrawItem(sender As System.Object, e As System.Windows.Forms.DrawItemEventArgs) Handles ComboBoxAnimationName.DrawItem
        Dim myListBox As ComboBox = CType(sender, ComboBox)
        Dim sb As SolidBrush = New SolidBrush(Color.White)
        If e.Index Mod 1 = 0 Then
            sb.Color = Color.MediumOrchid
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If e.Index Mod 2 = 0 Then
            sb.Color = Color.Orchid
            e.Graphics.FillRectangle(sb, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds)
        End If
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds)
            e.Graphics.DrawString(myListBox.Items(e.Index).ToString, e.Font, New SolidBrush(Color.Black), e.Bounds)
        End If
    End Sub
    'Programs ListView
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListViewPro.SelectedIndexChanged

        Try
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Programs\"
            For Each Img2 As FileInfo In New DirectoryInfo(path + ListViewPro.SelectedItems(0).Text).GetFiles("*.bmp")
                PictureBoxPro.Image = Image.FromFile(Img2.FullName)
            Next
            For Each txt As FileInfo In New DirectoryInfo(path + ListViewPro.SelectedItems(0).Text).GetFiles("*.txt")
                TextBoxPro.Text = My.Computer.FileSystem.ReadAllText(txt.FullName)
            Next
        Catch ex As Exception
        End Try

        VersionPro.Text = "V:" & TextBoxPro.Lines(0)


        Try
            Dim path As String = ListViewPro.SelectedItems(0).Text
            NamePro.Text = path

        Catch ex As Exception
        End Try
        ListViewPro.MultiSelect = False

    End Sub
    Private intIndex As Integer = 0
    Private Function GetIcon() As Integer
        GetIcon = Nothing
        Try

            Return intIndex
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function
    Private Sub ButtonProSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonProSetup.Click
        Try
            Dim path As String = Application.StartupPath & "Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Programs\"
            For Each iexe As FileInfo In New DirectoryInfo(path & ListViewPro.SelectedItems(0).Text).GetFiles("*.exe")
                Dim url As String
                url = iexe.FullName
                Process.Start(url)
            Next
            Dim path2 As String = Application.StartupPath & "Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Programs\"
            For Each iexe As FileInfo In New DirectoryInfo(path2 & ListViewPro.SelectedItems(0).Text).GetFiles("*.msi")
                Dim url As String
                url = iexe.FullName
                Process.Start(url)
            Next
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ButtonProCrack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonProCrack.Click
        GroupBoxGames.Hide()
        FormCRack.Show()
    End Sub
    Private Sub ButtonProTools_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonProTools.Click
        GroupBoxGames.Hide()
        FormTools.Show()
    End Sub
    Private Sub ComboBoxSeris_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxSeriesSe.SelectedIndexChanged
        Dim FullPath As String = (Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Series\" & ListBoxSeries.SelectedItem) & "\" & ComboBoxSeriesSe.SelectedItem
        Try
            ComboBoxSeriesEp.Items.Clear()
            Dim Mydir As New IO.DirectoryInfo(FullPath)
            Dim A001 As IO.FileInfo() = Mydir.GetFiles("*.mkv")
            Dim A002 As IO.FileInfo
            For Each A002 In A001
                ComboBoxSeriesEp.Items.Add(A002.Name)
            Next
            Dim Mydi As New IO.DirectoryInfo(FullPath)
            Dim B001 As IO.FileInfo() = Mydi.GetFiles("*.avi")
            Dim B002 As IO.FileInfo
            For Each B002 In B001
                ComboBoxSeriesEp.Items.Add(B002.Name)
            Next
            Dim Myd As New IO.DirectoryInfo(FullPath)
            Dim C001 As IO.FileInfo() = Myd.GetFiles("*.mp4")
            Dim C002 As IO.FileInfo
            For Each C002 In C001
                ComboBoxSeriesEp.Items.Add(C002.Name)
            Next
        Catch ex As Exception
        End Try
        ComboBoxSeriesEp.Sorted = True
        ComboBoxSeriesEp.SelectedIndex = 0
        Try
            Dim path2 As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Series\"
            Dim fileDetail = My.Computer.FileSystem.GetFileInfo(path2 & ListBoxSeries.SelectedItem & "\" & ComboBoxSeriesSe.SelectedItem & "\" & ComboBoxSeriesEp.SelectedItem)
            If fileDetail.Length >= 2000000000 Then
                PictureBoxSeriesQuality.Image = ImageListMovies.Images(0)
            End If
            If fileDetail.Length <= 2000000000 Then
                PictureBoxSeriesQuality.Image = ImageListMovies.Images(1)
            End If
            If fileDetail.Length <= 600000000 Then
                PictureBoxSeriesQuality.Image = ImageListMovies.Images(2)
            End If
        Catch ex As Exception
        End Try
    End Sub

    'Games بداية صفحة الـ
    Private Sub ComboBoxGames_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxGame.SelectedIndexChanged
        Try
            Dim path As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Games\Pc\"
            For Each Img2 As FileInfo In New DirectoryInfo(path & ComboBoxGame.SelectedItem).GetFiles("*.jpg")
                PictureBoxGameImage.Image = Image.FromFile(Img2.FullName)
            Next
            For Each Img3 As FileInfo In New DirectoryInfo(path & ComboBoxGame.SelectedItem & "\Images\").GetFiles("*.jpg")
                PictureBoxReviewGame.Image = Image.FromFile(Img3.FullName)
            Next

        Catch ex As Exception
        End Try
        LabelGameName.Text = ComboBoxGame.SelectedItem
        Try
            Dim txtfile As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Games\Pc\" & ComboBoxGame.SelectedItem & "\" & ComboBoxGame.SelectedItem + ".txt"
            TextBoxGames.Text = My.Computer.FileSystem.ReadAllText(txtfile)
        Catch ex As Exception
            TextBoxGames.Text = "No Text"
        End Try
        ListOfImage.Clear()
        Dim FullPatsh As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Games\Pc\" & ComboBoxGame.SelectedItem & "\Images\"
        For Each Img As FileInfo In New DirectoryInfo(FullPatsh).GetFiles("*.jpg")
            ListOfImage.Add(Image.FromFile(Img.FullName))
            iclick = ListOfImage.Count + 1
        Next

    End Sub
    Private Sub ButtonSetupGame_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSetupGame.Click
        Try
            Dim file As String = Application.StartupPath & "Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Games\Pc\" & ComboBoxGame.SelectedItem & "\Setup\"
            Process.Start(file & "Setup.exe")
        Catch ex As Exception
            MsgBox("عذرا جرب بالفتح مع")
        End Try
    End Sub
    Private Sub ButtonNextImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNextImage.Click

        If ListOfImage.Count = 0 Then Exit Sub
        iclick = iclick - 1
        If iclick <= 0 Then Exit Sub
        If iclick > ListOfImage.Count Then iclick = ListOfImage.Count - 1
        For i = 0 To ListOfImage.Count - iclick
            PictureBoxReviewGame.Image = ListOfImage(i)
        Next
    End Sub
    Private Sub ButtonBehindImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBehindImage.Click
        If ListOfImage.Count = 0 Then Exit Sub
        If iclick <= 0 Then iclick = 1
        iclick = iclick + 1
        For i = 0 To ListOfImage.Count - iclick
            PictureBoxReviewGame.Image = ListOfImage(i)
        Next
    End Sub
    Private Sub ButtonGameTools_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGameTools.Click
        GroupBoxPro.Hide()
        FormTools.Show()
    End Sub
    Private Sub ButtonGameCrack_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGameCrack.Click
        GroupBoxPro.Hide()
        FormCRack.Show()
    End Sub
    Private Sub ButtonSRtextfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSRtextfile.Click
        Try
            Dim txtfile As String = Application.StartupPath & "\Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Games\Pc\" & ComboBoxGame.SelectedItem & "\" & "Minimum System Requirements." & ComboBoxGame.SelectedItem + ".txt"
            TextBoxGames.Text = My.Computer.FileSystem.ReadAllText(txtfile)
        Catch ex As Exception
            TextBoxGames.Text = "No Text"
        End Try
    End Sub
    Private Sub LabelGameName_Click(sender As System.Object, e As System.EventArgs) Handles LabelGameName.Click
        ComboBoxGame.DroppedDown = True
    End Sub
    'Games نهاية صفحة الـ
    'تصفير الجلسة
    Private Sub ButtonReSession_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReSession.Click
        Try
            ListViewPro.Items(Integer.Parse(0)).Selected = True
            ComboBoxAnimationName.SelectedIndex = 0
            ListBoxAnimeName.SelectedIndex = 0
            ListBoxMoviesName.SelectedIndex = 0
            ListBoxDramaName.SelectedIndex = 0
            ListBoxSeries.SelectedIndex = 0
            ComboBoxGame.SelectedIndex = 0
        Catch ex As Exception
        End Try
    End Sub
    'بداية حركة البرامج التلفزونية
    Private Sub TimerMovies_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerMovies.Tick
        PictureBoxMovies.Size = New Size(PictureBoxMovies.Size.Width + 5, PictureBoxMovies.Size.Height + 9.5)
        PictureBoxMovies.Location = New Point(PictureBoxMovies.Location.X - 20, PictureBoxMovies.Location.Y - 6.8)
        If PictureBoxMovies.Size.Width > Me.Size.Width / 2.7 Then
            PictureBoxMovies.Size = New Size(Me.Size.Width / 2.64, Me.Size.Height / 1.26)
            PictureBoxMovies.Location = New Point(Me.Size.Width / 2.65, Me.Size.Height / 17.1)
            TimerMovies.Stop()
        End If
    End Sub
    Private Sub TimerAnime_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerAnime.Tick
        PictureBoxAnime.Size = New Size(PictureBoxAnime.Size.Width + 5, PictureBoxAnime.Size.Height + 9.5)
        PictureBoxAnime.Location = New Point(PictureBoxAnime.Location.X - 20, PictureBoxAnime.Location.Y - 6.8)
        If PictureBoxAnime.Size.Width > Me.Size.Width / 2.7 Then
            PictureBoxAnime.Size = New Size(Me.Size.Width / 2.64, Me.Size.Height / 1.26)
            PictureBoxAnime.Location = New Point(Me.Size.Width / 2.65, Me.Size.Height / 17.1)
            TimerAnime.Stop()
        End If
    End Sub
    Private Sub TimerSeries_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerSeries.Tick
        PictureBoxSeries.Size = New Size(PictureBoxSeries.Size.Width + 5, PictureBoxSeries.Size.Height + 9.5)
        PictureBoxSeries.Location = New Point(PictureBoxSeries.Location.X - 20, PictureBoxSeries.Location.Y - 6.8)
        If PictureBoxSeries.Size.Width > Me.Size.Width / 2.7 Then
            PictureBoxSeries.Size = New Size(Me.Size.Width / 2.64, Me.Size.Height / 1.26)
            PictureBoxSeries.Location = New Point(Me.Size.Width / 2.65, Me.Size.Height / 17.1)
            TimerSeries.Stop()
        End If
    End Sub
    Private Sub TimerAnimation_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerAnimation.Tick
        PictureBoxAnimation.Size = New Size(PictureBoxAnimation.Size.Width + 2, PictureBoxAnimation.Size.Height + 2)
        PictureBoxAnimation.Location = New Point(PictureBoxAnimation.Location.X - 1, PictureBoxAnimation.Location.Y - 1)
        If PictureBoxAnimation.Size.Width > 286 Then
            PictureBoxAnimation.Size = New Size(Me.Size.Width / 2.78, Me.Size.Height / 1.244)
            PictureBoxAnimation.Location = New Point(Me.Size.Width / 3.29, Me.Size.Height / 16)
        End If
    End Sub
    Private Sub TimerDrama_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerDrama.Tick
        PictureBoxDrama.Size = New Size(PictureBoxDrama.Size.Width + 5, PictureBoxDrama.Size.Height + 9.5)
        PictureBoxDrama.Location = New Point(PictureBoxDrama.Location.X - 20, PictureBoxDrama.Location.Y - 6.8)
        If PictureBoxDrama.Size.Width > Me.Size.Width / 2.7 Then
            PictureBoxDrama.Size = New Size(Me.Size.Width / 2.64, Me.Size.Height / 1.26)
            PictureBoxDrama.Location = New Point(Me.Size.Width / 2.65, Me.Size.Height / 17.1)
            TimerDrama.Stop()
        End If
    End Sub
    'نهاية حركة صور البرامج التلفزونية
    'حركة دخول المجموعات
    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles GroupBoxAnimation_Move.Tick
        GroupBoxAnimation.Location = New Point(GroupBoxAnimation.Location.X - (Me.Size.Width / 10), GroupBoxAnimation.Location.Y)
        If GroupBoxAnimation.Location.X < GroupBoxAnime.Location.X + 10 Then
            GroupBoxAnimation.Location = GroupBoxAnime.Location
            GroupBoxAnimation_Move.Stop()
        End If
    End Sub
    Private Sub GroupBoxMovies_Move_Tick(sender As System.Object, e As System.EventArgs) Handles GroupBoxMovies_Move.Tick
        GroupBoxMovies.Location = New Point(GroupBoxMovies.Location.X - (Me.Size.Width / 10), GroupBoxMovies.Location.Y)
        If GroupBoxMovies.Location.X < GroupBoxSeriesName.Location.X + 10 Then
            GroupBoxMovies.Location = GroupBoxSeriesName.Location
            GroupBoxMovies_Move.Stop()
        End If
    End Sub
    Private Sub GroupBoxSeriesName_Move_Tick(sender As System.Object, e As System.EventArgs) Handles GroupBoxSeriesName_Move.Tick
        GroupBoxSeriesName.Location = New Point(GroupBoxSeriesName.Location.X - (Me.Size.Width / 10), GroupBoxSeriesName.Location.Y)
        If GroupBoxSeriesName.Location.X < GroupBoxDrama.Location.X + 10 Then
            GroupBoxSeriesName.Location = GroupBoxDrama.Location
            GroupBoxSeriesName_Move.Stop()
        End If
    End Sub
    Private Sub GroupBoxDrama_Move_Tick(sender As System.Object, e As System.EventArgs) Handles GroupBoxDrama_Move.Tick
        GroupBoxDrama.Location = New Point(GroupBoxDrama.Location.X - (Me.Size.Width / 10), GroupBoxDrama.Location.Y)
        If GroupBoxDrama.Location.X < GroupBoxAnime.Location.X + 10 Then
            GroupBoxDrama.Location = GroupBoxAnime.Location
            GroupBoxDrama_Move.Stop()
        End If
    End Sub
    Private Sub GroupBoxAnime_Move_Tick(sender As System.Object, e As System.EventArgs) Handles GroupBoxAnime_Move.Tick
        GroupBoxAnime.Location = New Point(GroupBoxAnime.Location.X - (Me.Size.Width / 10), GroupBoxAnime.Location.Y)
        If GroupBoxAnime.Location.X < GroupBoxPro.Location.X + 10 Then
            GroupBoxAnime.Location = GroupBoxPro.Location
            GroupBoxAnime_Move.Stop()
        End If
    End Sub
    Private Sub GroupBoxPro_Move_Tick(sender As System.Object, e As System.EventArgs) Handles GroupBoxPro_Move.Tick
        GroupBoxPro.Location = New Point(GroupBoxPro.Location.X - (Me.Size.Width / 10), GroupBoxPro.Location.Y)
        If GroupBoxPro.Location.X < GroupBoxGames.Location.X + 10 Then
            GroupBoxPro.Location = GroupBoxGames.Location
            GroupBoxPro_Move.Stop()
        End If
    End Sub
    Private Sub GroupBoxGames_Move_Tick(sender As System.Object, e As System.EventArgs) Handles GroupBoxGames_Move.Tick
        GroupBoxGames.Location = New Point(GroupBoxGames.Location.X - (Me.Size.Width / 10), GroupBoxGames.Location.Y)
        If GroupBoxGames.Location.X < GroupBoxAnimation.Location.X + 10 Then
            GroupBoxGames.Location = GroupBoxAnimation.Location
            GroupBoxGames_Move.Stop()
        End If
    End Sub
    'نهاية حركة دخول المجموعات
    'copy
    Private Sub ButtonCopyAnimation_Click(sender As System.Object, e As System.EventArgs) Handles ButtonCopyAnimation.Click
        Try
            Dim data As String
            data = "*" & ComboBoxAnimationName.SelectedItem.ToString()
            System.IO.File.AppendAllText("Ya8S8eR Book Program Files\copylistAnimation.txt", data)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ButtonCopyMovies_Click(sender As System.Object, e As System.EventArgs) Handles ButtonCopyMovies.Click
        Try
            Dim data As String
            data = "*" & ListBoxMoviesName.SelectedItem.ToString()
            System.IO.File.AppendAllText("Ya8S8eR Book Program Files\copylistMovies.txt", data)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ButtonCopyDrama_Click(sender As System.Object, e As System.EventArgs) Handles ButtonCopyDrama.Click
        Try
            Dim data As String
            data = "*" & ListBoxDramaName.SelectedItem.ToString()
            System.IO.File.AppendAllText("Ya8S8eR Book Program Files\copylistDrama.txt", data)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ButtonCopyAnime_Click(sender As System.Object, e As System.EventArgs) Handles ButtonCopyAnime.Click
        Try
            Dim data As String
            data = "*" & ListBoxAnimeName.SelectedItem.ToString()
            System.IO.File.AppendAllText("Ya8S8eR Book Program Files\copylistAnime.txt", data)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ButtonCopySeriesSeason_Click(sender As System.Object, e As System.EventArgs) Handles ButtonCopySeriesSeason.Click
        Try
            Dim data As String
            data = "*" & ListBoxSeries.SelectedItem.ToString() & "\" & ComboBoxSeriesSe.SelectedItem
            System.IO.File.AppendAllText("Ya8S8eR Book Program Files\copylistSeriesSe.txt", data)
        Catch ex As Exception
        End Try
    End Sub
    'end copy
    'بخصوص صفحة الانميشن
    Private Sub PictureBoxAnimation2_Click(sender As System.Object, e As System.EventArgs) Handles PictureBoxAnimation2.Click
        Try
            ComboBoxAnimationName.SelectedIndex = ComboBoxAnimationName.SelectedIndex + 1
        Catch ex As Exception
            ComboBoxAnimationName.SelectedIndex = 0
        End Try
    End Sub
    Private Sub PictureBoxAnimation0_Click(sender As System.Object, e As System.EventArgs) Handles PictureBoxAnimation0.Click
        If ComboBoxAnimationName.SelectedIndex = 0 Then
            ComboBoxAnimationName.SelectedIndex = ComboBoxAnimationName.Items.Count - 1
        Else
            ComboBoxAnimationName.SelectedIndex = ComboBoxAnimationName.SelectedIndex - 1
        End If
    End Sub
    'النهاية
End Class
