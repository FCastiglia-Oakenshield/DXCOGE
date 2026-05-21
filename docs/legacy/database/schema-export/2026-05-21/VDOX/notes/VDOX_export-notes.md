# VDOX - Export notes

- Export schema-only generato da SSMS 22.
- Istanza SQL Server: SVRSELCO21\XSEL.
- File generato: `VDOX_schema_only.sql`.
- Non sono stati esportati dati cliente.
- Non sono stati inclusi file `.bak`, `.mdf` o `.ldf`.
- Eventuali `INSERT INTO` presenti nello script sono da considerarsi codice interno di stored procedure/funzioni/trigger, non export dati tabellari.
- Da confermare ruolo funzionale del DB: Documentale / servizi condivisi / anagrafiche collegate.
- Non effettuate modifiche sul database.

## Note metadata vuoti

- La query sui trigger non ha restituito righe: `VDOX_triggers.csv` è mantenuto con sola intestazione.

## Redazioni effettuate

- Riferimenti a database user generati da SSMS redatti nello script.
- Riferimenti a schema/utente [selco] redatti nello script.
