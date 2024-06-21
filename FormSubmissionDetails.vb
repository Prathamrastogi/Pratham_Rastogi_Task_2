Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Text

Public Class FormSubmissionDetails

    Private ReadOnly _submission As Submission

    Public Sub New(submission As Submission)
        InitializeComponent()
        _submission = submission
        DisplaySubmissionDetails()
    End Sub

    Private Sub DisplaySubmissionDetails()
        txtName.Text = _submission.Name
        txtEmail.Text = _submission.Email
        txtPhoneNum.Text = _submission.PhoneNum
        txtGithubLink.Text = _submission.GithubLink
        lblStopwatchTime.Text = _submission.StopwatchTime
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        _submission.Name = txtName.Text
        _submission.Email = txtEmail.Text
        _submission.PhoneNum = txtPhoneNum.Text
        _submission.GithubLink = txtGithubLink.Text

        Try
            Using client As New HttpClient()
                Dim jsonSubmission = JsonConvert.SerializeObject(_submission)
                Dim content = New StringContent(jsonSubmission, Encoding.UTF8, "application/json")
                Dim response = Await client.PutAsync($"http://localhost:3000/edit/{_submission.Id}", content)

                If response.IsSuccessStatusCode Then
                    MessageBox.Show("Submission updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Close()
                Else
                    MessageBox.Show("Failed to update submission.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close() ' Close the details form if Cancel button is clicked
    End Sub

    Private Sub FormSubmissionDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
