Imports Mysql.Data.MySqlClient
Public Class Form1

    Dim str As String = "server=localhost; uid=root; pwd=; database=recordsn"
    Dim con As New MySqlConnection(str)

    Sub load()
        Dim query As String = "select * from fct order by created_at desc"
        Dim adpt As New MySqlDataAdapter(query, con)
        Dim ds As New DataSet()
        adpt.Fill(ds, "Emp")
        DataGridView1.DataSource = ds.Tables(0)
        con.Close()
        TextBox1.Clear()
        TextBox1.Select()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load()
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim cmd As MySqlCommand
        Dim timeStamp As DateTime = DateTime.Now
        con.Open()
        Try
            cmd = con.CreateCommand
            cmd.CommandText = "select serial_number from fct where serial_number = '" + TextBox1.Text + "'"
            Dim lrd As MySqlDataReader = cmd.ExecuteReader()
            If lrd.HasRows Then
                'MsgBox("Serial Number Already Exist")
                con.Close()
                con.Open()
                cmd = con.CreateCommand
                cmd.CommandText = "update fct set source = 'FUN107 1', created_at = @created where serial_number = '" + TextBox1.Text + "'"
                cmd.Parameters.AddWithValue("@created", DateTime.Now)
                cmd.ExecuteNonQuery()
                load()
            Else
                con.Close()
                con.Open()
                cmd = con.CreateCommand
                cmd.CommandText = "insert into fct(paco_id ,serial_number, source, created_at)values(@paco_id,@sernum,@source,@created);"
                cmd.Parameters.AddWithValue("@paco_id", Nothing)
                cmd.Parameters.AddWithValue("@source", "FUN107 1")
                cmd.Parameters.AddWithValue("@sernum", TextBox1.Text)
                cmd.Parameters.AddWithValue("@created", DateTime.Now)
                cmd.ExecuteNonQuery()
                load()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub TextBox1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress

        If (e.KeyChar = Chr(13)) Then
            btnSubmit.PerformClick()
        End If

    End Sub

End Class
