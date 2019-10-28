Partial Class DsVent
    Partial Class TMPVENTILADataTable

        Private Sub TMPVENTILADataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.IvaVAnnoColumn.ColumnName) Then
                'Aggiungere qui il codice utente
            End If

        End Sub

        Private Sub TMPVENTILADataTable_TMPVENTILARowChanging(ByVal sender As System.Object, ByVal e As TMPVENTILARowChangeEvent) Handles Me.TMPVENTILARowChanging

        End Sub

    End Class

End Class

Namespace DsVentTableAdapters

    Partial Public Class TMPVENTILATableAdapter
    End Class
End Namespace
