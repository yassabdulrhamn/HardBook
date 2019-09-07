Imports System.IO
Public Class FormCRack
    Private Sub FormCRack_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Form.GroupBoxGames.Visible = True Then
                Dim FullPath As String = Application.StartupPath & "Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Games\Pc\" & Form.ComboBoxGame.SelectedItem & "\Crack\"
                Try
                    ListBoxCrack.Items.Clear()
                    Dim Mydir As New IO.DirectoryInfo(FullPath)
                    Dim A001 As IO.FileInfo() = Mydir.GetFiles()
                    Dim A002 As IO.FileInfo
                    For Each A002 In A001
                        ListBoxCrack.Items.Add(A002.Name)
                    Next
                Catch ex As Exception
                End Try
            End If
            If Form.GroupBoxPro.Visible = True Then
                Dim FullPath As String = Application.StartupPath + "Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Programs\" & Form.ListViewPro.SelectedItems(0).Text & "\Crack\"
                Try
                    ListBoxCrack.Items.Clear()
                    Dim Mydir As New IO.DirectoryInfo(FullPath)
                    Dim A001 As IO.FileInfo() = Mydir.GetFiles()
                    Dim A002 As IO.FileInfo
                    For Each A002 In A001
                        ListBoxCrack.Items.Add(A002.Name)
                    Next
                Catch ex As Exception
                End Try
            End If
            ListBoxCrack.SelectedIndex = 0
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ButtonRUN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRUN.Click
        Try
            If Form.GroupBoxGames.Visible = True Then
                Process.Start(Application.StartupPath & "Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Games\Pc\" & Form.ComboBoxGame.SelectedItem & "\Crack\" & ListBoxCrack.SelectedItem)
            End If
            If Form.GroupBoxPro.Visible = True Then
                Process.Start(Application.StartupPath + "Thumbs.ms.{208D2C60-3AEA-1069-A2D7-08002B30309D}\con\com3\com1\8\AUX\8\The Heart\Programs\" & Form.ListViewPro.SelectedItems(0).Text & "\Crack\" & ListBoxCrack.SelectedItem)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub FormCRack_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form.GroupBoxGames.Visible = True
        Form.GroupBoxPro.Visible = True
    End Sub
End Class