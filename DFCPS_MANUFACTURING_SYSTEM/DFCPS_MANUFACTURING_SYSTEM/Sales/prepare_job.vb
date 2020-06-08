Imports System.Data.SqlClient

Public Class prepare_job
    Dim totalAmountApplied As Double = 0
    Public totalBalance As Double
    Public cardID As String
    Dim dt As New DataTable
    Dim rowIndex As Integer
    Public succesPay As Boolean

    Sub prepare_job()
        Dim sc As New sales_class
        sc.searchValue = txtJobNo.Text
        sc.prepareJob()
        dgv.DataSource = sc.dtable
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgv.AutoResizeColumns()
    End Sub
   
    Private Sub dgv_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv.CellContentClick

    End Sub
End Class