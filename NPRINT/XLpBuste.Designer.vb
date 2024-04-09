<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class XLpBuste
    Inherits DevExpress.XtraReports.UI.XtraReport

    'XtraReport overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Designer
    'It can be modified using the Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.DetailBand1 = New DevExpress.XtraReports.UI.DetailBand()
        Me.Text34 = New DevExpress.XtraReports.UI.XRLabel()
        Me.Field1 = New DevExpress.XtraReports.UI.XRLabel()
        Me.Field2 = New DevExpress.XtraReports.UI.XRLabel()
        Me.Field3 = New DevExpress.XtraReports.UI.XRLabel()
        Me.CITTA_1 = New DevExpress.XtraReports.UI.XRLabel()
        Me.ReportHeaderBand1 = New DevExpress.XtraReports.UI.ReportHeaderBand()
        Me.PageHeaderBand1 = New DevExpress.XtraReports.UI.PageHeaderBand()
        Me.ReportFooterBand1 = New DevExpress.XtraReports.UI.ReportFooterBand()
        Me.PageFooterBand1 = New DevExpress.XtraReports.UI.PageFooterBand()
        Me.Box2 = New DevExpress.XtraReports.UI.XRCrossBandBox()
        Me.CITTA = New DevExpress.XtraReports.UI.CalculatedField()
        Me.TopMarginBand1 = New DevExpress.XtraReports.UI.TopMarginBand()
        Me.BottomMarginBand1 = New DevExpress.XtraReports.UI.BottomMarginBand()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'DetailBand1
        '
        Me.DetailBand1.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.Text34, Me.Field1, Me.Field2, Me.Field3, Me.CITTA_1})
        Me.DetailBand1.Dpi = 254.0!
        Me.DetailBand1.HeightF = 981.6042!
        Me.DetailBand1.HierarchyPrintOptions.Indent = 50.8!
        Me.DetailBand1.Name = "DetailBand1"
        Me.DetailBand1.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254.0!)
        Me.DetailBand1.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand
        Me.DetailBand1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'Text34
        '
        Me.Text34.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom
        Me.Text34.BackColor = System.Drawing.Color.Transparent
        Me.Text34.BorderColor = System.Drawing.Color.Black
        Me.Text34.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.Text34.BorderWidth = 1.0!
        Me.Text34.CanGrow = False
        Me.Text34.Dpi = 254.0!
        Me.Text34.Font = New System.Drawing.Font("Verdana", 5.0!)
        Me.Text34.ForeColor = System.Drawing.Color.Black
        Me.Text34.LocationFloat = New DevExpress.Utils.PointFloat(1227.667!, 759.0013!)
        Me.Text34.Name = "Text34"
        Me.Text34.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.Text34.SizeF = New System.Drawing.SizeF(240.7708!, 32.10284!)
        Me.Text34.Text = "Spett.le"
        Me.Text34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'Field1
        '
        Me.Field1.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom
        Me.Field1.BackColor = System.Drawing.Color.Transparent
        Me.Field1.BorderColor = System.Drawing.Color.Black
        Me.Field1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.Field1.BorderWidth = 1.0!
        Me.Field1.CanGrow = False
        Me.Field1.Dpi = 254.0!
        Me.Field1.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AnaRag1]")})
        Me.Field1.Font = New System.Drawing.Font("Verdana", 8.0!)
        Me.Field1.ForeColor = System.Drawing.Color.Black
        Me.Field1.LocationFloat = New DevExpress.Utils.PointFloat(1227.667!, 791.1042!)
        Me.Field1.Name = "Field1"
        Me.Field1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.Field1.SizeF = New System.Drawing.SizeF(846.6667!, 33.8667!)
        Me.Field1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'Field2
        '
        Me.Field2.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom
        Me.Field2.BackColor = System.Drawing.Color.Transparent
        Me.Field2.BorderColor = System.Drawing.Color.Black
        Me.Field2.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.Field2.BorderWidth = 1.0!
        Me.Field2.CanGrow = False
        Me.Field2.Dpi = 254.0!
        Me.Field2.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AnaRag2]")})
        Me.Field2.Font = New System.Drawing.Font("Verdana", 8.0!)
        Me.Field2.ForeColor = System.Drawing.Color.Black
        Me.Field2.LocationFloat = New DevExpress.Utils.PointFloat(1227.667!, 833.4375!)
        Me.Field2.Name = "Field2"
        Me.Field2.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.Field2.SizeF = New System.Drawing.SizeF(846.6667!, 33.8667!)
        Me.Field2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'Field3
        '
        Me.Field3.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom
        Me.Field3.BackColor = System.Drawing.Color.Transparent
        Me.Field3.BorderColor = System.Drawing.Color.Black
        Me.Field3.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.Field3.BorderWidth = 1.0!
        Me.Field3.CanGrow = False
        Me.Field3.Dpi = 254.0!
        Me.Field3.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AnaIndirizzo]")})
        Me.Field3.Font = New System.Drawing.Font("Verdana", 8.0!)
        Me.Field3.ForeColor = System.Drawing.Color.Black
        Me.Field3.LocationFloat = New DevExpress.Utils.PointFloat(1227.667!, 875.7709!)
        Me.Field3.Name = "Field3"
        Me.Field3.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.Field3.SizeF = New System.Drawing.SizeF(846.6667!, 33.8667!)
        Me.Field3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'CITTA_1
        '
        Me.CITTA_1.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom
        Me.CITTA_1.BackColor = System.Drawing.Color.Transparent
        Me.CITTA_1.BorderColor = System.Drawing.Color.Black
        Me.CITTA_1.Borders = DevExpress.XtraPrinting.BorderSide.None
        Me.CITTA_1.BorderWidth = 1.0!
        Me.CITTA_1.CanGrow = False
        Me.CITTA_1.Dpi = 254.0!
        Me.CITTA_1.ExpressionBindings.AddRange(New DevExpress.XtraReports.UI.ExpressionBinding() {New DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CITTA]")})
        Me.CITTA_1.Font = New System.Drawing.Font("Verdana", 8.0!)
        Me.CITTA_1.ForeColor = System.Drawing.Color.Black
        Me.CITTA_1.LocationFloat = New DevExpress.Utils.PointFloat(1227.667!, 918.1042!)
        Me.CITTA_1.Name = "CITTA_1"
        Me.CITTA_1.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254.0!)
        Me.CITTA_1.SizeF = New System.Drawing.SizeF(846.6667!, 33.8667!)
        Me.CITTA_1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'ReportHeaderBand1
        '
        Me.ReportHeaderBand1.Dpi = 254.0!
        Me.ReportHeaderBand1.HeightF = 0!
        Me.ReportHeaderBand1.KeepTogether = True
        Me.ReportHeaderBand1.Name = "ReportHeaderBand1"
        Me.ReportHeaderBand1.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254.0!)
        Me.ReportHeaderBand1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        Me.ReportHeaderBand1.Visible = False
        '
        'PageHeaderBand1
        '
        Me.PageHeaderBand1.Dpi = 254.0!
        Me.PageHeaderBand1.HeightF = 0!
        Me.PageHeaderBand1.Name = "PageHeaderBand1"
        Me.PageHeaderBand1.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254.0!)
        Me.PageHeaderBand1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'ReportFooterBand1
        '
        Me.ReportFooterBand1.Dpi = 254.0!
        Me.ReportFooterBand1.HeightF = 0!
        Me.ReportFooterBand1.KeepTogether = True
        Me.ReportFooterBand1.Name = "ReportFooterBand1"
        Me.ReportFooterBand1.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254.0!)
        Me.ReportFooterBand1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        Me.ReportFooterBand1.Visible = False
        '
        'PageFooterBand1
        '
        Me.PageFooterBand1.Dpi = 254.0!
        Me.PageFooterBand1.HeightF = 104.14!
        Me.PageFooterBand1.Name = "PageFooterBand1"
        Me.PageFooterBand1.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254.0!)
        Me.PageFooterBand1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
        '
        'Box2
        '
        Me.Box2.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom
        Me.Box2.BorderWidth = 1.0!
        Me.Box2.Dpi = 254.0!
        Me.Box2.EndBand = Me.DetailBand1
        Me.Box2.EndPointFloat = New DevExpress.Utils.PointFloat(1206.5!, 981.4279!)
        Me.Box2.Name = "Box2"
        Me.Box2.StartBand = Me.DetailBand1
        Me.Box2.StartPointFloat = New DevExpress.Utils.PointFloat(1206.5!, 748.7708!)
        Me.Box2.WidthF = 889.0!
        '
        'CITTA
        '
        Me.CITTA.Expression = "[TbAna.AnaCap] + ' ' + [TbAna.AnaCitta] + ' ' + [TbAna.AnaProv]"
        Me.CITTA.FieldType = DevExpress.XtraReports.UI.FieldType.[String]
        Me.CITTA.Name = "CITTA"
        '
        'TopMarginBand1
        '
        Me.TopMarginBand1.Dpi = 254.0!
        Me.TopMarginBand1.HeightF = 0!
        Me.TopMarginBand1.Name = "TopMarginBand1"
        '
        'BottomMarginBand1
        '
        Me.BottomMarginBand1.Dpi = 254.0!
        Me.BottomMarginBand1.HeightF = 0!
        Me.BottomMarginBand1.Name = "BottomMarginBand1"
        '
        'XLpBuste
        '
        Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.DetailBand1, Me.ReportHeaderBand1, Me.PageHeaderBand1, Me.ReportFooterBand1, Me.PageFooterBand1, Me.TopMarginBand1, Me.BottomMarginBand1})
        Me.CalculatedFields.AddRange(New DevExpress.XtraReports.UI.CalculatedField() {Me.CITTA})
        Me.CrossBandControls.AddRange(New DevExpress.XtraReports.UI.XRCrossBandControl() {Me.Box2})
        Me.Dpi = 254.0!
        Me.Landscape = True
        Me.Margins = New System.Drawing.Printing.Margins(0, 0, 0, 0)
        Me.PageWidth = 2200
        Me.PaperKind = System.Drawing.Printing.PaperKind.DLEnvelope
        Me.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter
        Me.SnapGridSize = 31.75!
        Me.StyleSheetPath = ""
        Me.Version = "19.2"
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Friend WithEvents DetailBand1 As DevExpress.XtraReports.UI.DetailBand
    Friend WithEvents Text34 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents Field1 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents Field2 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents Field3 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents CITTA_1 As DevExpress.XtraReports.UI.XRLabel
    Friend WithEvents ReportHeaderBand1 As DevExpress.XtraReports.UI.ReportHeaderBand
    Friend WithEvents PageHeaderBand1 As DevExpress.XtraReports.UI.PageHeaderBand
    Friend WithEvents ReportFooterBand1 As DevExpress.XtraReports.UI.ReportFooterBand
    Friend WithEvents PageFooterBand1 As DevExpress.XtraReports.UI.PageFooterBand
    Friend WithEvents Box2 As DevExpress.XtraReports.UI.XRCrossBandBox
    Friend WithEvents CITTA As DevExpress.XtraReports.UI.CalculatedField
    Friend WithEvents TopMarginBand1 As DevExpress.XtraReports.UI.TopMarginBand
    Friend WithEvents BottomMarginBand1 As DevExpress.XtraReports.UI.BottomMarginBand
End Class
