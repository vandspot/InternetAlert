Imports IWshRuntimeLibrary
Imports Shell32
Public Class Settings

    Private Sub Settings_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Icon = Form1.Icon
        'ComboBox1.SelectedIndex = 0
        'ComboBox2.SelectedIndex = 0
        'ComboBox3.SelectedIndex = 0
        'ComboBox4.SelectedIndex = 0
    End Sub
    Public Sub CreateShortcutInStartUp()
        Dim WshShell As WshShell = New WshShell()
        Dim ShortcutPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
        Dim Shortcut As IWshShortcut = CType(WshShell.CreateShortcut(System.IO.Path.Combine(ShortcutPath, Application.ProductName) & ".lnk"), IWshShortcut)
        Shortcut.TargetPath = Application.ExecutablePath
        Shortcut.WorkingDirectory = Application.StartupPath
        Shortcut.Description = "Internet Alert!"
        Shortcut.Save()
    End Sub
    Public Sub DeleteShortcutInStartUp()
        Dim ShortcutPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Startup)
        Dim LNK As String = System.IO.Path.Combine(ShortcutPath, Application.ProductName) & ".lnk"
        If IO.File.Exists(LNK) Then
            IO.File.Delete(LNK)
        End If
    End Sub
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Form1.opNotifC = ComboBox1.SelectedIndex
        Form1.opNotifDC = ComboBox2.SelectedIndex
        Form1.opNotifBatteryLow = ComboBox3.SelectedIndex
        Form1.opNotifBatteryHigh = ComboBox4.SelectedIndex

        Form1.opSources = TextBox1.Text.Trim.Replace(vbCrLf, "{|~!#@|}")
        Form1.opInterval = TextBox2.Text
        Form1.opBatteryLow = TrackBar1.Value
        Form1.opBatteryHigh = TrackBar2.Value

        Form1.opSoundC = ComboBox5.Text
        Form1.opSoundDC = ComboBox6.Text
        Form1.opSoundBatLow = ComboBox7.Text
        Form1.opSoundBatHigh = ComboBox8.Text

        If CheckBox3.Checked Then
            CreateShortcutInStartUp()
            'My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True).SetValue(Application.ProductName, Application.ExecutablePath)
            Form1.opStartup = True
        Else
            DeleteShortcutInStartUp()
            'My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True).DeleteValue(Application.ProductName)
            Form1.opStartup = False
        End If
        Form1.SAVE_OPT()
        Form1.READ_OPT()

        'Form1.Ticker.CancelAsync()
        'Form1.BatteryMan.CancelAsync()
        'Form1.Pinger.CancelAsync()
        'Form1.Pinger1.CancelAsync()
        'Form1.Pinger2.CancelAsync()
        ''System.Threading.Thread.Sleep(5000)
        'Form1.INIT()

        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Label6.Text = TrackBar1.Value & "%"
    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        Label7.Text = TrackBar2.Value & "%"
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            My.Computer.Audio.Play(Application.StartupPath & "\" & ComboBox5.Text, AudioPlayMode.Background)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            My.Computer.Audio.Play(Application.StartupPath & "\" & ComboBox6.Text, AudioPlayMode.Background)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            My.Computer.Audio.Play(Application.StartupPath & "\" & ComboBox7.Text, AudioPlayMode.Background)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            My.Computer.Audio.Play(Application.StartupPath & "\" & ComboBox8.Text, AudioPlayMode.Background)
        Catch ex As Exception

        End Try
    End Sub
End Class