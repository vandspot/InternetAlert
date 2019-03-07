Public Class Aboutvb
    Private Sub RichTextBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles RichTextBox1.MouseDoubleClick
        TextBox1.Text = RichTextBox1.Rtf
    End Sub

    Private Sub Aboutvb_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RichTextBox1.Rtf = "{\rtf1\fbidis\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fprq2\fcharset0 Calibri;}}
{\colortbl ;\red231\green230\blue230;\red0\green176\blue80;}
\viewkind4\uc1\pard\ltrpar\sa160\sl252\slmult1\f0\fs22 Product: \b Internet Alert\b0\par
Version: \b 2.1\b0\par
Developer: \b Amin Behdarvand\b0\par
Contact: \b www.\cf1 neo\cf2 Market\cf0 .ir\b0\par
\pard\ltrpar\par"
    End Sub
End Class