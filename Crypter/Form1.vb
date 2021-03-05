Imports System
Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text

Public Class Form1
    Private Const sSecretKey As String = "hola"

    Sub main()
        Encriptar()
    End Sub

    Sub Encriptar(ByVal InputFileName As String, ByVal OutputFileName As String, ByVal skey As String)

        Dim fsInput As New FileStream(InputFileName, FileMode.Open, FileAccess.Read)
        Dim fsEncriptado As New FileStream(OutputFileName, FileMode.Create, FileAccess.Write)
        Dim DES As New DESCryptoServiceProvider()

        'Establecer la clave de desbloqueo para el algoritmo DES
        'Se necesita una clave de 64 bits y IV para este provedor

        DES.Key = ASCIIEncoding.ASCII.GetBytes(sSecretKey)

        'Establecer el vetor de inicio
        DES.IV = ASCIIEncoding.ASCII.GetBytes(skey)

        'Crear cifrado DES a partir de la instancia
        Dim desencriptador As ICryptoTransform = DES.CreateEncryptor()
        'Crear una secuencia de cifrado que transforma la secuencia 
        'En archivos mediante el cifrado DES

        Dim crypto As New CryptoStream(fsEncriptado, desencriptador, CryptoStreamMode.Write)

        'Lee el texto del archivo en la matriz de bytes
        Dim byteArray(fsInput.Length - 1) As Byte
        fsInput.Read(byteArray, 0, byteArray.Length)

        'Escribir el cifrado DES
        crypto.Write(byteArray, 0, byteArray.Length)
        crypto.Close()

    End Sub

    Sub Desencriptar(ByVal sInputFile As String, ByVal sOutputFile As String, ByVal Skey As String)
        Dim des As New DESCryptoServiceProvider()

        des.Key = ASCIIEncoding.ASCII.GetBytes(Skey)

        des.IV = ASCIIEncoding.ASCII.GetBytes(Skey)

        Dim fsRead As New FileStream(sInputFile, FileMode.Open, FileAccess.Read)

        Dim desenc As ICryptoTransform = des.CreateDecryptor()

        Dim cryptosStream As New CryptoStream(fsRead, des, CryptoStreamMode.Read)

        Dim fsDesenc As New StreamWriter(sOutputFile)
        fsDesenc.Write(New StreamReader(cryptosStream).ReadToEnd)
        fsDesenc.Flush()
        fsDesenc.Close()

    End Sub

    Private Sub btnEncriptar_Click(sender As Object, e As EventArgs) Handles btnEncriptar.Click
        Dim ruta As String

        ruta = Path.Combine(Application.StartupPath, direccion)

        Dim fichero As New IO.StreamWriter(ruta)
        fichero.WriteLine(txtclave.txt)
        fichero.Close()

        main()
    End Sub

    Private Sub btnDesencriptar_Click(sender As Object, e As EventArgs) Handles btnDesencriptar.Click
        Desencriptar("", "", sSecretKey)

    End Sub

    Dim direccion As String

    Private Sub AbrirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AbrirToolStripMenuItem.Click
        Dim openFileDialog As New OpenFileDialog()

        openFileDialog.Multiselect = False
        openFileDialog.RestoreDirectory = True
        openFileDialog.Title = "Crypter"
        Try
            If openFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                direccion = openFileDialog.FileName
                btnDesencriptar.Enabled = True
                btnEncriptar.Enabled = True
            End If
        Catch ex As Exception
            MsgBox("Error: " & ex.ToString)
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnDesencriptar.Enabled = False
        btnEncriptar.Enabled = False
    End Sub
End Class
