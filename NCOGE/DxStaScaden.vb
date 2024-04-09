Imports DXBASE
Imports NCCOM
Imports NPRINT
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports DevExpress.XtraReports.UI    'Importa l'interfaccia grafica di DevExpress.
Public Class DxStaScaden
    Dim DsRip As DataTable
    Dim DaRip As SqlDataAdapter
    Dim RwRip As DataRow

    Dim Rpt As ReportClass
    Dim Rpt1 As New BanSca
    Dim Rpt2 As New ClFoSca
    Dim Rpt3 As New Scade

    Dim SW As Int16 = 0
    Dim MiglioFo, TipoStampa As Int32
    Dim TipoSta, Periodo, StrPrint As String

    Private Sub DxStaScaden_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown, ButtonF5.Click
        If SW = 0 Then
            PrimoMiglio()
            SW = 1
        End If
        Pulizia()
    End Sub
    Sub PrimoMiglio()
        Dim Cmd As New SqlCommand("SELECT * FROM TbGrp Where GrpCod = 'FO'", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            MiglioFo = dataRd.Item("GrpMigl")
        End While
        dataRd.Close()
        Cmd = New SqlCommand("SELECT top 1 * from TbEse Order by EseAnno desc", cnCo)
        dataRd = Cmd.ExecuteReader
        While dataRd.Read
            TipoStampa = dataRd.Item("EseFormato")
        End While
        dataRd.Close()
        DateEdit2.EditValue = CDate(Date.DaysInMonth(Today.Year, Today.Month) & "/" & Today.Month & "/" & Today.Year)
        DateEdit1.EditValue = CDate("01/01/" & Today.Year)
    End Sub
    Sub Pulizia()
        RadioGroup1.SelectedIndex = 1
        CheckEdit1.Checked = False
    End Sub
    Private Sub ButtonF9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonF9.Click

        'StampaCr()         'Lancia  CRISTALREPORT
        StampaDevexp()    'Lancia DevExpress

    End Sub

    Private Sub StampaCr()
        If DateEdit1.EditValue > DateEdit2.EditValue Then
            DateEdit1.Focus()
            Exit Sub
        End If
        Cursor.Current = Cursors.WaitCursor
        Dim frm As New LpDs
        Dim TAa As Int16 = 1
        Rpt = New ReportClass
        Rpt1 = New BanSca
        Rpt2 = New ClFoSca
        Rpt3 = New Scade
        TipoSta = RadioGroup1.EditValue
        If CheckEdit1.Checked = True Then
            If RadioGroup1.SelectedIndex = 2 Then
                Rpt = Rpt2
            Else
                Rpt = Rpt1
            End If
        Else
            Rpt = Rpt3
        End If
        If RadioGroup1.SelectedIndex = 2 And CheckEdit1.Checked = False Then TAa = 2 : TipoSta = "CL"

        For J As Int16 = 1 To TAa
            If TAa = 2 And J = 2 Then
                frm = New LpDs
                Rpt = New ReportClass
                Rpt3 = New Scade
                Rpt = Rpt3
                TipoSta = "FO"
            End If
            StrPrint = "select * from crg1 where  scadsca between '" & CDate(DateEdit1.EditValue).ToShortDateString & "' AND '" & CDate(DateEdit2.EditValue).ToShortDateString & "' order by scaban,scadsca,clfopi,scadesc "
            Cursor.Current = Cursors.WaitCursor
            Periodo = "Scadenze dal " & CDate(DateEdit1.EditValue).ToShortDateString & " al " & CDate(DateEdit2.EditValue).ToShortDateString
            DsRip = New DataTable
            DaRip = New SqlDataAdapter(StrPrint, cnCo)
            DaRip.SelectCommand.CommandTimeout = 300
            DaRip.Fill(DsRip)
            Rpt.SetDataSource(DsRip)
            Rpt.SetParameterValue("Marchio", Marchio)
            Rpt.SetParameterValue("tiposta", TipoSta)
            Rpt.SetParameterValue("PERIODO", Periodo)
            If TipoStampa = 1 Then
                PdfStart(Rpt, Me.Text & "_" & TipoSta)
            Else
                frm.reportsource = Rpt
                frm.Text = Me.Text
                frm.ShowDialog()
            End If
        Next
    End Sub

    Private Sub StampaDevexp()
        If DateEdit1.EditValue > DateEdit2.EditValue Then
            DateEdit1.Focus()
            Exit Sub
        End If
        Cursor.Current = Cursors.WaitCursor
        Dim REPORT As New XtraReport
        Dim DsRip As DataTable      'Rivedere nomi
        Dim DaRip As SqlDataAdapter 'Rivedere nomi
        Dim TAa As Int16 = 1
        'REPORT = New XScade                   'Spunta CLIENTI 
        TipoSta = RadioGroup1.EditValue
        If CheckEdit1.Checked = True Then
            If RadioGroup1.SelectedIndex = 2 Then
                REPORT = New XClFoSca                 'Spunta COMPLETO e x BANCA subito sotto
            Else
                REPORT = New XBanSca                  'Spunta CLIENTI e x BANCA subito sotto.
            End If
        Else
            REPORT = New XScade            'Spunta CLIENTI
        End If
        If RadioGroup1.SelectedIndex = 2 And CheckEdit1.Checked = False Then TAa = 2 : TipoSta = "CL" 'Imposta clienti.

        For J As Int16 = 1 To TAa
            If TAa = 2 And J = 2 Then

                'REPORT = New XScade     'Sembra che questo non serva.
                TipoSta = "FO"
            End If
            'StrPrint = "select * from crg1 where  scadsca between '" & CDate(DateEdit1.EditValue).ToShortDateString & "' AND '" & CDate(DateEdit2.EditValue).ToShortDateString & "' and CLFOPI ='" & TipoSta & "' and ScaScopRata <> 0 order by scaban,scadsca asc,clfopi,scadesc "'Prova precedente.
            StrPrint = "select * from crg1 where  scadsca between '" & CDate(DateEdit1.EditValue).ToShortDateString & "' AND '" & CDate(DateEdit2.EditValue).ToShortDateString & "' order by scaban,scadsca,scadesc "  'Da usare,funziona
            'StrPrint = "select * from crg1 where  scadsca between '" & CDate(DateEdit1.EditValue).ToShortDateString & "' AND '" & CDate(DateEdit2.EditValue).ToShortDateString & "' and CLFOPI ='CL' and ScaScopRata<> '0' order by scadesc asc,scaban,scadsca"' Prova mia
            Cursor.Current = Cursors.WaitCursor
            Periodo = "Scadenze dal " & CDate(DateEdit1.EditValue).ToShortDateString & " al " & CDate(DateEdit2.EditValue).ToShortDateString
            DsRip = New DataTable
            DaRip = New SqlDataAdapter(StrPrint, cnCo)
            DaRip.SelectCommand.CommandTimeout = 300
            DaRip.Fill(DsRip)
            REPORT.DataSource = (DsRip)
            REPORT.DataMember = StrPrint
            REPORT.Parameters.Item("Marchio").Value = Marchio()
            REPORT.Parameters.Item("tiposta").Value = TipoSta   'Pare che sia quello che comanda la stampa di Clienti o fornitori.
            REPORT.Parameters.Item("PERIODO").Value = Periodo
            If TipoStampa = 1 Then
                'PdfStart(Rpt, Me.Text & "_" & TipoSta) Questo in Cristal Report,fa stampare solo i campi che hanno la voce CLFOPI = CL
            Else
                Dim opt As New DevExpress.XtraPrinting.PdfExportOptions  'Da qui fino a End Sub prepara e crea il pdf
                opt.Compressed = True
                REPORT.CreateDocument()
                REPORT.ShowPreviewDialog
            End If
        Next
    End Sub

End Class