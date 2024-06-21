Imports System.Net.Http
Imports Newtonsoft.Json

Public Class FormViewSubmissions
    Private currentIndex As Integer = 0
    Private submission As Submission ' Store the current submission data
    Private isEditing As Boolean = False ' Flag to track whether we are in edit mode

    Private Async Sub FormViewSubmissions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadSubmission(currentIndex)
    End Sub

    Private Async Function LoadSubmission(index As Integer) As Task
        Using client As New HttpClient()
            Dim response = Await client.GetAsync($"http://localhost:3000/read?index={index}")

            If response.IsSuccessStatusCode Then
                Dim jsonResponse = Await response.Content.ReadAsStringAsync()
                submission = JsonConvert.DeserializeObject(Of Submission)(jsonResponse)
                DisplaySubmission(submission)
            Else
                ' Handle 404 Not Found error
                ResetFields()
                MessageBox.Show("Submission not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Using
    End Function

    Private Sub DisplaySubmission(submission As Submission)
        txtName.Text = submission.Name
        txtEmail.Text = submission.Email
        txtPhoneNum.Text = submission.PhoneNum
        txtGithubLink.Text = submission.GithubLink
        lblStopwatchTime.Text = submission.StopwatchTime
    End Sub

    Private Sub ResetFields()
        txtName.Text = ""
        txtEmail.Text = ""
        txtPhoneNum.Text = ""
        txtGithubLink.Text = ""
        lblStopwatchTime.Text = ""
    End Sub

    Private Async Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        currentIndex += 1
        Await LoadSubmission(currentIndex)
    End Sub

    Private Async Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If currentIndex > 0 Then
            currentIndex -= 1
            Await LoadSubmission(currentIndex)
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        EnableEditing(True)
    End Sub

    Private Sub EnableEditing(enable As Boolean)
        isEditing = enable
        txtName.Enabled = enable
        txtEmail.Enabled = enable
        txtPhoneNum.Enabled = enable
        txtGithubLink.Enabled = enable
        btnEdit.Enabled = Not enable
        btnNext.Enabled = Not enable
        btnPrevious.Enabled = Not enable
        btnSave.Enabled = enable
        btnCancel.Enabled = enable
        btnDelete.Enabled = Not enable ' Disable delete button during edit mode
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        EnableEditing(False)
        DisplaySubmission(submission) ' Restore original values
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If ValidateFields() Then
            ' Update submission object with new values
            submission.Name = txtName.Text
            submission.Email = txtEmail.Text
            submission.PhoneNum = txtPhoneNum.Text
            submission.GithubLink = txtGithubLink.Text

            ' Send updated submission to backend
            Using client As New HttpClient()
                Dim jsonSubmission = JsonConvert.SerializeObject(submission)
                Dim content = New StringContent(jsonSubmission, System.Text.Encoding.UTF8, "application/json")
                Dim response = Await client.PutAsync($"http://localhost:3000/edit/{currentIndex}", content)

                If response.IsSuccessStatusCode Then
                    MessageBox.Show("Submission updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    EnableEditing(False)
                Else
                    MessageBox.Show("Failed to update submission.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        End If
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim result = MessageBox.Show("Are you sure you want to delete this submission?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Using client As New HttpClient()
                Dim response = Await client.DeleteAsync($"http://localhost:3000/delete/{currentIndex}")

                If response.IsSuccessStatusCode Then
                    MessageBox.Show("Submission deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Await LoadSubmission(currentIndex) ' Reload next submission after deletion
                Else
                    MessageBox.Show("Failed to delete submission.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        End If
    End Sub

    Private Function ValidateFields() As Boolean
        ' Add validation logic here if needed
        Return True ' For simplicity, assuming fields are always valid
    End Function

    ' Event handler for KeyDown event
    Private Sub FormViewSubmissions_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control Then ' Check if Ctrl key is pressed
            Select Case e.KeyCode
                Case Keys.N ' Ctrl + N for Next
                    btnNext.PerformClick()
                Case Keys.P ' Ctrl + P for Previous
                    btnPrevious.PerformClick()
                Case Keys.E ' Ctrl + E for Edit
                    btnEdit.PerformClick()
                Case Keys.S ' Ctrl + S for Save
                    btnSave.PerformClick()
                Case Keys.C ' Ctrl + C for Cancel
                    btnCancel.PerformClick()
                Case Keys.D ' Ctrl + D for Delete
                    btnDelete.PerformClick()
            End Select
        End If
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub lblStopwatchTime_Click(sender As Object, e As EventArgs) Handles lblStopwatchTime.Click

    End Sub
End Class
