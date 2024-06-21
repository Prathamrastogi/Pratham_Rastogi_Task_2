Imports System.Net.Http
Imports Newtonsoft.Json

Public Class FormSearchSubmission

    Private Async Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim email As String = txtEmail.Text.Trim()
        If String.IsNullOrEmpty(email) Then
            MessageBox.Show("Please enter an email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            Await SearchSubmissionByEmail(email)
        Catch ex As Exception
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Close() ' Close the search form after handling the search
        End Try
    End Sub

    Private Async Function SearchSubmissionByEmail(email As String) As Task
        Try
            Using client As New HttpClient()
                Dim response = Await client.GetStringAsync($"http://localhost:3000/search?email={Uri.EscapeDataString(email)}")

                If response = "Submission not found" Then
                    MessageBox.Show("No submission found for the provided email.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim submission As Submission = JsonConvert.DeserializeObject(Of Submission)(response)
                    Dim detailsForm As New FormSubmissionDetails(submission)
                    detailsForm.ShowDialog()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close() ' Close the search form if Cancel button is clicked
    End Sub

    Private Sub FormSearchSubmission_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Optional: Any initialization logic for the form can go here
    End Sub
End Class
