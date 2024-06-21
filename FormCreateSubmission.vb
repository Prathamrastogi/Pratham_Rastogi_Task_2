Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Text
Imports System.Text.RegularExpressions

Public Class FormCreateSubmission
    Private stopwatch As New Stopwatch()

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        ' Validate inputs
        If Not ValidateInputs() Then
            Return
        End If

        Dim submission As New Submission With {
            .Name = txtName.Text,
            .Email = txtEmail.Text,
            .PhoneNum = txtPhoneNum.Text,
            .GithubLink = txtGithubLink.Text,
            .StopwatchTime = lblStopwatchTime.Text
        }
        SubmitForm(submission)
    End Sub

    Private Function ValidateInputs() As Boolean
        ' Validate email
        If Not IsValidEmail(txtEmail.Text) Then
            MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        ' Validate phone number (must be exactly 10 digits)
        If Not IsValidPhoneNumber(txtPhoneNum.Text) Then
            MessageBox.Show("Phone number must be exactly 10 digits.", "Invalid Phone Number", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        ' Validate GitHub link format (must start with www and end with .com)
        If Not IsValidGithubLink(txtGithubLink.Text) Then
            MessageBox.Show("GitHub link must be in format www.example.com.", "Invalid GitHub Link", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        Return True
    End Function

    Private Function IsValidEmail(email As String) As Boolean
        ' Basic email format validation
        Dim emailRegex As New Regex("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")
        Return emailRegex.IsMatch(email)
    End Function

    Private Function IsValidPhoneNumber(phoneNum As String) As Boolean
        ' Phone number length validation
        Return phoneNum.Length = 10 AndAlso IsNumeric(phoneNum)
    End Function

    Private Function IsValidGithubLink(githubLink As String) As Boolean
        ' GitHub link format validation
        Dim githubRegex As New Regex("^www\.\w+\.\w+$")
        Return githubRegex.IsMatch(githubLink)
    End Function

    Private Async Sub SubmitForm(submission As Submission)
        Using client As New HttpClient()
            Dim content = New StringContent(JsonConvert.SerializeObject(submission), Encoding.UTF8, "application/json")
            Await client.PostAsync("http://localhost:3000/submit", content)
        End Using

        MessageBox.Show("Submission done.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Me.Close() ' Close the current form after submission
    End Sub

    Private Sub btnToggleStopwatch_Click(sender As Object, e As EventArgs) Handles btnToggleStopwatch.Click
        If stopwatch.IsRunning Then
            stopwatch.Stop()
            btnToggleStopwatch.Text = "Resume Stopwatch (Ctrl + T)"
        Else
            stopwatch.Start()
            btnToggleStopwatch.Text = "Pause Stopwatch (Ctrl + T)"
        End If
        UpdateStopwatchTime()
    End Sub

    Private Sub UpdateStopwatchTime()
        lblStopwatchTime.Text = String.Format("{0:hh\:mm\:ss}", stopwatch.Elapsed)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If stopwatch.IsRunning Then
            UpdateStopwatchTime()
        End If
    End Sub

    Private Sub FormCreateSubmission_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.S Then
            btnSubmit.PerformClick()
        ElseIf e.Control AndAlso e.KeyCode = Keys.T Then
            btnToggleStopwatch.PerformClick()
        End If
    End Sub

    Private Sub FormCreateSubmission_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
