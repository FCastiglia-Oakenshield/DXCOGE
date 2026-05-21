# DCDC - Export notes

- Export schema-only generato da SSMS 22.
- Istanza SQL Server: SVRSELCO21\XSEL.
- File generato: `DCDC_schema_only.sql`.
- Non sono stati esportati dati cliente.
- Non sono stati inclusi file `.bak`, `.mdf` o `.ldf`.
- Eventuali `INSERT INTO` presenti nello script sono da considerarsi codice interno di stored procedure/funzioni/trigger, non export dati tabellari.
- Da confermare ruolo funzionale del DB: Centri di costo / componente collegata.
- Non effettuate modifiche sul database.

## Note metadata vuoti

- La query sulle foreign key non ha restituito righe: `DCDC_foreign-keys.csv` è mantenuto con sola intestazione.
- La query sui trigger non ha restituito righe: `DCDC_triggers.csv` è mantenuto con sola intestazione.
- In questa istanza non risulta presente GQUA; al suo posto è stato individuato ed esportato DCDC.

## Redazioni effettuate

- Riferimenti a database user generati da SSMS redatti nello script.
- Riferimenti a schema/utente [selco] redatti nello script.
