﻿Imports System.Data.SqlClient

Public Class sales_class
    Public command As Integer
    Public commands As String
    Public transNo As String
    Public transcount As Integer
    Public transType As String
    Public refNo As String
    Public cardId As String
    Public remarks As String
    Public totFAmnt As Decimal
    Public totDiscount As Decimal
    Public totAmount As Decimal
    Public itemCode As String
    Public qty As Integer
    Public uprice As Decimal
    Public discount As Decimal
    Public amount As Decimal
    Public status As String
    Public checkno As String
    Public checkdate As DateTime
    Public amountApplied As Decimal
    Public outBalance As Decimal
    Public overallBal As Decimal
    Public accNo As String
    Public paymethod As String
    Public dtable As New DataTable
    Public searchValue As String
    Public cost As Decimal
    Public collectionNo As String
    Sub insert_update_sales()
        Dim cmd As New SqlCommand("insert_update_sales", conn)
        conn.Close()
        ConnectDatabase()
        With cmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.AddWithValue("@processCount", SqlDbType.Int).Value = transcount
            .Parameters.AddWithValue("@salesMode", SqlDbType.VarChar).Value = transType
            .Parameters.AddWithValue("@salesno", SqlDbType.VarChar).Value = transNo
            .Parameters.AddWithValue("@refNo", SqlDbType.VarChar).Value = refNo
            .Parameters.AddWithValue("@cardID", SqlDbType.VarChar).Value = cardId
            .Parameters.AddWithValue("@totFAmnt", SqlDbType.Decimal).Value = totFAmnt
            .Parameters.AddWithValue("@totDis", SqlDbType.Decimal).Value = totDiscount
            .Parameters.AddWithValue("@totalAmount", SqlDbType.Decimal).Value = totAmount
            .Parameters.AddWithValue("@userid", SqlDbType.VarChar).Value = MainForm.LBLID.Text
            .Parameters.AddWithValue("@status", SqlDbType.VarChar).Value = status
            .Parameters.AddWithValue("@itemNo", SqlDbType.VarChar).Value = itemCode
            .Parameters.AddWithValue("@qty", SqlDbType.Int).Value = qty
            .Parameters.AddWithValue("@uprice", SqlDbType.Decimal).Value = uprice
            .Parameters.AddWithValue("@amount", SqlDbType.Decimal).Value = amount
            .Parameters.AddWithValue("@discount", SqlDbType.Decimal).Value = discount
            .Parameters.AddWithValue("@cost", SqlDbType.Decimal).Value = cost
        End With
        cmd.ExecuteNonQuery()
    End Sub
    Sub recieve_payments()
        Dim cmd As New SqlCommand("insert_ReceivedPayments", conn)
        conn.Close()
        ConnectDatabase()
        With cmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.AddWithValue("@paymentNo", SqlDbType.VarChar).Value = transNo
            .Parameters.AddWithValue("@paymethod", SqlDbType.VarChar).Value = paymethod
            .Parameters.AddWithValue("@cardID", SqlDbType.VarChar).Value = cardId
            .Parameters.AddWithValue("@userid", SqlDbType.VarChar).Value = MainForm.LBLID.Text
            .Parameters.AddWithValue("@checkno", SqlDbType.VarChar).Value = checkNo
            .Parameters.AddWithValue("@checkdate", SqlDbType.Date).Value = checkdate.ToString("yyyy/MM/dd")
            .Parameters.AddWithValue("@amountReceived", SqlDbType.Decimal).Value = totAmount
            .Parameters.AddWithValue("@outOFBalance", SqlDbType.Decimal).Value = outBalance
            .Parameters.AddWithValue("@overAllBalance", SqlDbType.Decimal).Value = overallBal
            .Parameters.AddWithValue("@invoiceNo", SqlDbType.VarChar).Value = refNo
            .Parameters.AddWithValue("@amountDiscount", SqlDbType.Decimal).Value = discount
            .Parameters.AddWithValue("@amountApplied", SqlDbType.Decimal).Value = amountApplied
            .Parameters.AddWithValue("@depositToAccount", SqlDbType.VarChar).Value = accNo
        End With
        cmd.ExecuteNonQuery()
    End Sub
    Sub insert_collection()
        Dim cmd As New SqlCommand("insert_collection", conn)
        conn.Close()
        ConnectDatabase()
        With cmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.AddWithValue("@collectionNo", SqlDbType.VarChar).Value = collectionNo
            .Parameters.AddWithValue("@paymentNo", SqlDbType.VarChar).Value = transNo
        End With
        cmd.ExecuteNonQuery()
    End Sub
    Sub get_invoice_list()
        checkConn()
        Dim cmd As New SqlCommand("get_invoice_list", conn)
        With cmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.AddWithValue("@searchValue", SqlDbType.VarChar).Value = searchValue
        End With
        Dim da As New SqlDataAdapter(cmd)
        da.SelectCommand = cmd
        da.Fill(dtable)
    End Sub
    Sub get_sales_items()
        checkConn()
        Dim cmd As New SqlCommand("get_sales_items", conn)
        With cmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.AddWithValue("@command", SqlDbType.VarChar).Value = commands
            .Parameters.AddWithValue("@searchValue", SqlDbType.VarChar).Value = searchValue
        End With
        Dim da As New SqlDataAdapter(cmd)
        da.SelectCommand = cmd
        da.Fill(dtable)
    End Sub
    Sub get_order_no()
        checkConn()
        Dim cmd As New SqlCommand("SELECT dbo.[Sales Invoice].refNo FROM dbo.[Sales Invoice] where [No] = '" & searchValue & "'", conn)
        Dim da As New SqlDataAdapter(cmd)
        da.SelectCommand = cmd
        da.Fill(dtable)
    End Sub

    Function prepareJob(ByVal id As String, ByVal ft As String) As DataTable

        checkConn()
        If ft <> "ref" Then
            Dim cmd As New SqlCommand("SELECT * FROM JOB_PREPARATION_VIEW where TRNO = '" & id & "'", conn)
            Dim da As New SqlDataAdapter(cmd)
            da.SelectCommand = cmd
            da.Fill(dtable)
        Else
            Dim cmd As New SqlCommand("SELECT * FROM JOB_PREPARATION_VIEW ", conn)
            Dim da As New SqlDataAdapter(cmd)
            da.SelectCommand = cmd
            da.Fill(dtable)
        End If
        Return dtable
    End Function
    Public Sub printjob(ByVal search As String)
        frmLoading.Show()
        checkConn()

        Dim cmd As New SqlCommand("SELECT * FROM job_view where jobID = '" & search & "'", conn)
        Dim da As New SqlDataAdapter(cmd)
        da.SelectCommand = cmd
        da.Fill(dtable)


        Dim ds As New salesDS
        For Each row As DataRow In dtable.Rows
            ds.Tables("JO").Rows.Add(row(0), row(1), row(2), row(3), row(4), row(5), row(6), row(7), row(8))
        Next
        Dim rptDoc As CrystalDecisions.CrystalReports.Engine.ReportDocument
        rptDoc = New JOForm
        rptDoc.SetDataSource(ds.Tables("JO"))
        print_slip_viewer.CrystalReportViewer1.ReportSource = rptDoc
        frmLoading.Close()
        print_slip_viewer.ShowDialog()
    End Sub
    Public Sub insertData(ByVal tbl As String, ByVal data As String)
        checkConn()
        Dim cmd As New SqlCommand("insert into " & tbl & " values('" & data & "')", conn)
        cmd.ExecuteNonQuery()
    End Sub

    Public Sub updateData(ByVal tbl As String, ByVal data As String, ByVal condition As String)
        checkConn()
        Dim cmd As New SqlCommand("update " & tbl & " set " & data & " where " & Condition & "", conn)
        cmd.ExecuteNonQuery()
    End Sub
End Class
