Imports System.Net
Imports System.Threading


Public Class Form1
    Public opNotifC As Short = 0
    Public opNotifDC As Short = 0
    Public opBatteryLow As Short = 10
    Public opBatteryHigh As Short = 97
    Public opNotifBatteryLow As Short = 0
    Public opNotifBatteryHigh As Short = 0
    Public opInterval As Integer = 900
    Public opSources As String = "8.8.8.8{|~!#@|}4.2.2.4{|~!#@|}neomarket.ir"
    Public opStartup = False
    Public opSoundC = "ON.wav"
    Public opSoundDC = "OFF.wav"
    Public opSoundBatLow = "OFF.wav"
    Public opSoundBatHigh = "ON.wav"
    Dim Sttate As Boolean = False
    Public Pause As Boolean = False

    Public Class ArgumentType
        Public Inrvl As Integer
        Public Source As String
        Public SourceNum As Short
        Public BatLow As Integer
        Public BatHigh As Integer
    End Class

    Dim statConnectd1, statConnectd2, statConnectd3 As Boolean
    Dim statNormal As Boolean
    Dim statFull As Boolean
    Dim statLow As Boolean

    Friend WithEvents Pinger1 As New System.ComponentModel.BackgroundWorker()
    Friend WithEvents Pinger2 As New System.ComponentModel.BackgroundWorker()

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Hide()
        Me.Left = Screen.PrimaryScreen.WorkingArea.Right - Me.Width - 40
        Me.Top = Screen.PrimaryScreen.WorkingArea.Bottom - Me.Height - 40

        If Process.GetProcessesByName _
          (Process.GetCurrentProcess.ProcessName).Length > 1 Then
            Application.Exit()
        End If

        READ_OPT()

        CANCELASYNC()
        INIT()

    End Sub
    Sub CANCELASYNC(Optional halter As Integer = 0)
        'RUNNING Pinger
        Do While Pinger.IsBusy = True Or Pinger1.IsBusy = True Or Pinger2.IsBusy = True Or Ticker.IsBusy = True Or BatteryMan.IsBusy = True
            Pinger.CancelAsync()
            Pinger1.CancelAsync()
            Pinger2.CancelAsync()
            Ticker.CancelAsync()
            BatteryMan.CancelAsync()

            If halter > 0 Then
                Thread.Sleep(halter)
            Else
                Thread.Sleep(200)
            End If
        Loop
        Me.Refresh()
    End Sub
    Sub INIT()

        'For i = 0 To opSources.Split(vbCrLf).Length - 1
        If Not opSources.Split("{|~!#@|}")(0).Trim = "" Then
            Dim args As ArgumentType = New ArgumentType()
            args.Source = opSources.Split("{|~!#@|}")(0)
            args.SourceNum = 0
            args.Inrvl = opInterval

            Pinger.WorkerSupportsCancellation = True
            Pinger.RunWorkerAsync(args)
        End If

        If Not opSources.Trim = "" Then
            Dim args As ArgumentType = New ArgumentType()
            args.Source = opSources.Split("{|~!#@|}?")(1)
            args.SourceNum = 1
            args.Inrvl = opInterval

            Pinger1.WorkerSupportsCancellation = True
            Pinger1.RunWorkerAsync(args)
        End If

        If Not opSources.Split("{|~!#@|}")(2).Trim = "" Then
            Dim args As ArgumentType = New ArgumentType()
            args.Source = opSources.Split("{|~!#@|}")(2)
            args.SourceNum = 2
            args.Inrvl = opInterval

            Pinger2.WorkerSupportsCancellation = True
            Pinger2.RunWorkerAsync(args)
        End If

        'Next

        'RUNNING BATTERY MAN
        Dim args1 As ArgumentType = New ArgumentType()
        args1.BatHigh = opBatteryHigh
        args1.BatLow = opBatteryLow
        BatteryMan.WorkerSupportsCancellation = True
        BatteryMan.RunWorkerAsync(args1)

        'RUNNING TICKER
        Dim args2 As ArgumentType = New ArgumentType()
        args2.Inrvl = opInterval
        Ticker.WorkerSupportsCancellation = True
        Ticker.RunWorkerAsync(args2)
    End Sub

    Sub Connected()
        NotifyIcon1.Icon = My.Resources.onn
        If opNotifC = 0 Or opNotifC = 2 Then
            If Sttate = False Then
                Try
                    My.Computer.Audio.Play(Application.StartupPath & "\" & opSoundC, AudioPlayMode.Background)
                Catch ex As Exception

                End Try

            End If
        End If

        Label4.Text = "Connected"
        Label4.ForeColor = Color.MediumSeaGreen
        Label3.Text = SystemInformation.PowerStatus.BatteryLifePercent * 100 & "%"
        Me.Refresh()
        If (opNotifC = 0 Or opNotifC = 1) And Sttate = False Then
            Me.Show()
        End If

        Sttate = True
    End Sub
    Sub Disconnected()
        NotifyIcon1.Icon = My.Resources.off
        If opNotifDC = 0 Or opNotifDC = 2 Then
            If Sttate = True Then
                Try
                    My.Computer.Audio.Play(Application.StartupPath & "\" & opSoundDC, AudioPlayMode.Background)
                Catch ex As Exception

                End Try

            End If
        End If

        Label4.Text = "Disonnected"
        Label4.ForeColor = Color.Tomato
        Label3.Text = SystemInformation.PowerStatus.BatteryLifePercent * 100 & "%"
        Me.Refresh()
        If (opNotifDC = 0 Or opNotifDC = 1) And Sttate = True Then
            Me.Show()
        End If

        Sttate = False
    End Sub
    Sub LowBat()
        Try
            If opNotifBatteryLow = 0 Or opNotifBatteryLow = 2 Then
                My.Computer.Audio.Play(Application.StartupPath & "\" & opSoundBatLow, AudioPlayMode.Background)
            End If

        Catch ex As Exception

        End Try
        Me.Label5.Text = "Attention: LOW Battery!"
        Me.Label5.Show()
        If opNotifBatteryLow = 0 Or opNotifBatteryLow = 1 Then
            Me.Show()
        End If
    End Sub
    Sub FullBat()
        Try
            If opNotifBatteryHigh = 0 Or opNotifBatteryHigh = 2 Then
                My.Computer.Audio.Play(Application.StartupPath & "\" & opSoundBatHigh, AudioPlayMode.Background)
            End If

        Catch ex As Exception

        End Try
        Me.Label5.Text = "Attention: Battery FULL!"
        Me.Label5.Visible = True
        If opNotifBatteryHigh = 0 Or opNotifBatteryHigh = 1 Then
            Me.Show()
        End If
    End Sub
    Sub NormalStat()
        Me.Label5.Visible = False
        'Me.Refresh()

    End Sub
    Private Sub Ticker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles Ticker.DoWork
        Dim args As ArgumentType = e.Argument
        While (Ticker.CancellationPending = False)

            If statConnectd1 Or statConnectd2 Or statConnectd3 Then
                Invoke(New MethodInvoker(AddressOf Connected))
            Else
                Invoke(New MethodInvoker(AddressOf Disconnected))
            End If

            If statFull Then
                Invoke(New MethodInvoker(AddressOf FullBat))
            ElseIf statLow
                Invoke(New MethodInvoker(AddressOf LowBat))
            ElseIf statNormal
                Invoke(New MethodInvoker(AddressOf NormalStat))
            End If



            If Ticker.CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If
            System.Threading.Thread.Sleep(args.Inrvl)


        End While

    End Sub
    Private Sub Pinger_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles Pinger.DoWork, Pinger1.DoWork, Pinger2.DoWork
        Dim args As ArgumentType = e.Argument
        While (Pinger.CancellationPending = False And Pinger1.CancellationPending = False And Pinger2.CancellationPending = False)


            If PingInternetConnection(args.Source, args.Inrvl - 100) Then
                Select Case args.SourceNum
                    Case 0
                        statConnectd1 = True
                    Case 1
                        statConnectd2 = True
                    Case 2
                        statConnectd3 = True
                    Case Else

                End Select

                'Invoke(New MethodInvoker(AddressOf Connected))
            Else
                Select Case args.SourceNum
                    Case 0
                        statConnectd1 = False
                    Case 1
                        statConnectd2 = False
                    Case 2
                        statConnectd3 = False
                    Case Else
                End Select
                'Invoke(New MethodInvoker(AddressOf Disconnected))
            End If

            'Pinger.ReportProgress(99)
            If Pinger.CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If
            If Pinger1.CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If
            If Pinger2.CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If

            System.Threading.Thread.Sleep(args.Inrvl)

        End While

    End Sub

    Private Sub BatteryMan_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BatteryMan.DoWork
        Dim args As ArgumentType = e.Argument
        While (BatteryMan.CancellationPending = False)

            If SystemInformation.PowerStatus.BatteryLifePercent >= args.BatHigh / 100 And SystemInformation.PowerStatus.PowerLineStatus = PowerLineStatus.Online Then
                statFull = True
                statLow = False
                statNormal = False
            ElseIf SystemInformation.PowerStatus.BatteryLifePercent <= args.BatLow / 100 And SystemInformation.PowerStatus.PowerLineStatus = PowerLineStatus.Offline Then
                statFull = False
                statLow = True
                statNormal = False
            Else
                statFull = False
                statLow = False
                statNormal = True
            End If

            If BatteryMan.CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If
            System.Threading.Thread.Sleep(1000)


        End While

    End Sub

    Public Function PingInternetConnection(ByVal Source As String, timeOut As Integer) As Boolean
        Try
            'If My.Computer.Network.IsAvailable Then
            If My.Computer.Network.Ping(Source, timeOut) Then
                Return True
            Else
                Return False
            End If
            'End If

        Catch ex As Exception
            Return False
        End Try

    End Function




    Private Sub AboutToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        'MsgBox("Programmed by: Amin Behdarvand" & vbCrLf & "neomarket.ir")
        Aboutvb.Show()
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SettingsToolStripMenuItem.Click, Button3.Click
        Settings.ComboBox1.SelectedIndex = opNotifC
        Settings.ComboBox2.SelectedIndex = opNotifDC
        Settings.ComboBox3.SelectedIndex = opNotifBatteryHigh
        Settings.ComboBox4.SelectedIndex = opNotifBatteryLow

        Settings.ComboBox5.Items.Clear()
        Settings.ComboBox6.Items.Clear()
        Settings.ComboBox7.Items.Clear()
        Settings.ComboBox8.Items.Clear()

        For Each File In System.IO.Directory.GetFiles(My.Application.Info.DirectoryPath)
            If File.ToLower.EndsWith(".wav") Then
                Dim GetParts As String() = Split(File, "\")
                Settings.ComboBox5.Items.Add(GetParts(GetParts.Length - 1))
                Settings.ComboBox6.Items.Add(GetParts(GetParts.Length - 1))
                Settings.ComboBox7.Items.Add(GetParts(GetParts.Length - 1))
                Settings.ComboBox8.Items.Add(GetParts(GetParts.Length - 1))
            End If
        Next
        Try
            Settings.ComboBox5.Text = opSoundC
        Catch ex As Exception
            Settings.ComboBox5.SelectedIndex = 0
        End Try
        Try
            Settings.ComboBox6.Text = opSoundDC
        Catch ex As Exception
            Settings.ComboBox6.SelectedIndex = 0
        End Try
        Try
            Settings.ComboBox7.Text = opSoundBatLow
        Catch ex As Exception
            Settings.ComboBox7.SelectedIndex = 0
        End Try
        Try
            Settings.ComboBox8.Text = opSoundBatHigh
        Catch ex As Exception
            Settings.ComboBox8.SelectedIndex = 0
        End Try


        Settings.TrackBar1.Value = opBatteryLow
        Settings.Label6.Text = opBatteryLow & "%"
        Settings.TrackBar2.Value = opBatteryHigh
        Settings.Label7.Text = opBatteryHigh & "%"

        If opStartup Then
            Settings.CheckBox3.Checked = True
        Else
            Settings.CheckBox3.Checked = False
        End If
        Settings.TextBox1.Text = opSources.Replace("{|~!#@|}", vbCrLf).Trim
        Settings.TextBox2.Text = opInterval
        Settings.Show()
    End Sub
    Public Sub READ_OPT()
        If System.IO.File.Exists(Application.StartupPath & "\options.inf") Then
            Dim Rdd As New System.IO.StreamReader(Application.StartupPath & "\options.inf")
            Dim LINES() As String = Split(Rdd.ReadToEnd, vbCrLf)
            Rdd.Close()
            For Each LINE As String In LINES
                If Not LINE = "" Then
                    If LINE.StartsWith("NOTIFC=") Then
                        opNotifC = Split(LINE, "NOTIFC=")(1)
                    End If
                    If LINE.StartsWith("NOTIFDC=") Then
                        opNotifDC = Split(LINE, "NOTIFDC=")(1)
                    End If

                    If LINE.StartsWith("INTERVAL=") Then
                        opInterval = Split(LINE, "INTERVAL=")(1)
                        Timer1.Interval = opInterval
                    End If
                    If LINE.StartsWith("SOURCE=") Then
                        opSources = Split(LINE, "SOURCE=")(1)
                    End If
                    If LINE.StartsWith("STARTUP=") Then
                        Select Case Split(LINE, "STARTUP=")(1)
                            Case 0
                                opStartup = False
                            Case Else
                                opStartup = True
                        End Select
                    End If
                    If LINE.StartsWith("BATTLOW=") Then
                        opBatteryLow = Split(LINE, "BATTLOW=")(1)
                        If IsNumeric(opBatteryLow) Then
                            If opBatteryLow < 0 Then
                                opBatteryLow = 0
                            End If
                        Else
                            opBatteryLow = 0
                        End If
                    End If
                    If LINE.StartsWith("BATTHIGH=") Then
                        opBatteryHigh = Split(LINE, "BATTHIGH=")(1)
                        If IsNumeric(opBatteryHigh) Then
                            If opBatteryHigh > 100 Then
                                opBatteryHigh = 100
                            End If
                        Else
                            opBatteryHigh = 100
                        End If
                    End If

                    If LINE.StartsWith("SOUNDC=") Then
                        opSoundC = Split(LINE, "SOUNDC=")(1)
                    End If
                    If LINE.StartsWith("SOUNDDC=") Then
                        opSoundDC = Split(LINE, "SOUNDDC=")(1)
                    End If

                    If LINE.StartsWith("SOUNDBATLOW=") Then
                        opSoundBatLow = Split(LINE, "SOUNDBATLOW=")(1)
                    End If
                    If LINE.StartsWith("SOUNDBATHIGH=") Then
                        opSoundBatHigh = Split(LINE, "SOUNDBATHIGH=")(1)
                    End If

                End If
            Next
        Else
            'There is no OPT file
            Me.Hide()
            Settings.Show()
        End If
    End Sub
    Public Sub SAVE_OPT()
        If System.IO.File.Exists("options.inf") Then
            System.IO.File.Delete("options.inf")
        End If
        Dim WRT As New System.IO.StreamWriter("options.inf")
        Dim Strng As String = ""
        Strng &= "NOTIFC=" & opNotifC & vbCrLf
        Strng &= "NOTIFDC=" & opNotifDC & vbCrLf
        Strng &= "INTERVAL=" & opInterval & vbCrLf &
        "SOURCES=" & opSources & vbCrLf
        If opStartup Then
            Strng &= "STARTUP=1" & vbCrLf
        Else
            Strng &= "STARTUP=0" & vbCrLf
        End If

        Strng &= "BATTLOW=" & opBatteryLow & vbCrLf
        Strng &= "BATTHIGH=" & opBatteryHigh & vbCrLf
        Strng &= "BATTLOWN=" & opNotifBatteryLow & vbCrLf
        Strng &= "BATTHIGHN=" & opNotifBatteryHigh & vbCrLf

        Strng &= "SOUNDC=" & opSoundC & vbCrLf
        Strng &= "SOUNDDC=" & opSoundDC & vbCrLf
        Strng &= "SOUNDBATLOW=" & opSoundBatLow & vbCrLf
        Strng &= "SOUNDBATHIGH=" & opSoundBatHigh

        WRT.Write(Strng)
        WRT.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub ContextMenuStrip1_Closing(sender As Object, e As ToolStripDropDownClosingEventArgs) Handles ContextMenuStrip1.Closing
        Timer1.Enabled = True
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        Timer1.Enabled = False
    End Sub

    Private Sub NotifyIcon1_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            'Timer1.Enabled = False
        Else
            Me.Show()
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Closing procedures
        Ticker.CancelAsync()
        BatteryMan.CancelAsync()
        Pinger.CancelAsync()
        'Threading.Thread.CurrentThread.Abort()
        'CANCELASYNC()
        Me.Close()
    End Sub

    Private Sub PausePlayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PausePlayToolStripMenuItem.Click, Button4.Click
        If Not PausePlayToolStripMenuItem.Text = "Resume Service" Then
            Pause = True
            PausePlayToolStripMenuItem.Text = "Resume Service"
            Button4.Text = "RESUME"
            Label4.Text = "Paused"
            Label3.Text = "?"
            Label4.ForeColor = Color.Gray
            NotifyIcon1.Icon = My.Resources.off
            Ticker.CancelAsync()
            BatteryMan.CancelAsync()
            Pinger.CancelAsync()
        Else
            PausePlayToolStripMenuItem.Text = "Pause Service"
            Button4.Text = "PAUSE"
            Pause = False
            'System.Threading.Thread.Sleep(4000)
            Me.Refresh()
            CANCELASYNC()
            Me.Refresh()
            INIT()
        End If

    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Aboutvb.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Hide()
        Settings.Show()
    End Sub

End Class


