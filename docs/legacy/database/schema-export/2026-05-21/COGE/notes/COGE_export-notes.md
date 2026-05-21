# COGE - Export notes

- Export schema-only generato da SSMS 22.
- Istanza SQL Server: SVRSELCO21\XSEL.
- File generato: `COGE_schema_only.sql`.
- Non sono stati esportati dati cliente.
- Non sono stati inclusi file `.bak`, `.mdf` o `.ldf`.
- Eventuali `INSERT INTO` presenti nello script sono da considerarsi codice interno di stored procedure/funzioni/trigger, non export dati tabellari.
- Da confermare ruolo funzionale del DB: Contabile / amministrativo.
- Non effettuate modifiche sul database.

## Note metadata vuoti

- La query sui trigger non ha restituito righe: `COGE_triggers.csv` è mantenuto con sola intestazione.

## Redazioni effettuate

- Riferimenti a database user generati da SSMS redatti nello script.
- Riferimenti a schema/utente [selco] redatti nello script.
