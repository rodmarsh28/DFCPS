﻿Imports System.Data.SqlClient

Public Class prepare_job
    Public cardid As String
    Sub prepare_job()
        Dim sc As New sales_class
        dgv.Rows.Clear()
        For Each rows As DataRow In sc.prepareJob(TXTREF.Text, "dg").Rows
            dgv.Rows.Add(rows(3), rows(4), rows(5), rows(6), rows(7), If(rows(7) - rows(6) < 0, 0, rows(7) - rows(6)))
            dgv.ClearSelection()
            TXTCUS.Text = (rows(2))
            cardid = (rows(1))
        Next
    End Sub
    Sub autoref()
        Try
            Dim col As New AutoCompleteStringCollection
            Dim sc As New sales_class
            For Each row As DataRow In sc.prepareJob("", "ref").Rows
                col.Add(row(0))
            Next
            TXTREF.AutoCompleteSource = AutoCompleteSource.CustomSource
            TXTREF.AutoCompleteCustomSource = col
            TXTREF.AutoCompleteMode = AutoCompleteMode.Suggest
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Dim sc As New sales_class
            Dim data As String
            Dim cond As String
            For Each row As DataGridViewRow In dgv.Rows
                data = TXTJONO.Text & "',getdate(),'" & cardid & "','" & row.Cells(0).Value & "','" & row.Cells(5).Value & "','','','" & MainForm.LBLID.Text

            Next

            data = "status = 'JOB PREPARED SUCCESSFULLY'"
            cond = "salesOrderNo = '" & TXTREF.Text & "'"
            sc.updateData("tblSalesOrder", data, cond)
            MsgBox("success")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub prepare_job_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.MdiParent = frmSalesMain
        autoref()
        TXTJONO.Text = generateFormNo("tblJob", "JOBID", "JO-")
    End Sub

    Private Sub TXTREF_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TXTREF.TextChanged
        prepare_job()
    End Sub
End Class