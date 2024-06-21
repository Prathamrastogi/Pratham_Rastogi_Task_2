Imports System.Windows.Forms

Public Class Form1

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        Dim createForm As New FormCreateSubmission()
        createForm.ShowDialog()
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Dim viewForm As New FormViewSubmissions()
        viewForm.ShowDialog()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim searchForm As New FormSearchSubmission()
        searchForm.ShowDialog()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Register KeyDown event for the form to handle Ctrl+N, Ctrl+V, and Ctrl+S
        AddHandler Me.KeyDown, AddressOf Form1_KeyDown
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control Then
            Select Case e.KeyCode
                Case Keys.N ' Ctrl + N for Create Submission
                    btnCreate.PerformClick()
                Case Keys.V ' Ctrl + V for View Submissions
                    btnView.PerformClick()
                Case Keys.S ' Ctrl + S for Search Submission
                    btnSearch.PerformClick()
            End Select
        End If
    End Sub

End Class
