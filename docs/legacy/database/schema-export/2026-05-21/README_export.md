# XSEL DB Schema Export

Data export: 2026-05-20  
Eseguito da: Fabio  
Istanza SQL Server: SVRSELCO21\XSEL  
Ambiente: produzione

## Obiettivo

Export schema-only dei database collegati all'installazione XSEL, senza dati cliente, finalizzato alla mappatura tecnica e funzionale del sistema legacy.

L'export non è un backup, non è una migrazione e non è destinato al deploy. Lo scopo è produrre documentazione tecnica leggibile, versionabile e utilizzabile come contesto per analisi successive.

## Database esportati

| Database | Ruolo presunto | Note |
|---|---|---|
| COGE | Contabile / amministrativo | Ruolo da confermare con Francesco o senior |
| GEVE | Gestionale principale | Ruolo da confermare con Francesco o senior |
| DCDC | Centri di costo / componente collegata | Presente in questa istanza al posto di GQUA |
| VDOX | Documentale / servizi condivisi / anagrafiche collegate | Ruolo da confermare con Francesco o senior |

## Struttura export

```text
XSEL_DB_SCHEMA_EXPORT_2026-05-20/
  README_export.md

  COGE/
    README_COGE.md
    schema/
      COGE_schema_only.sql
    metadata/
      COGE_objects-inventory.csv
      COGE_tables-columns.csv
      COGE_foreign-keys.csv
      COGE_indexes.csv
      COGE_triggers.csv
      COGE_routines.csv
    notes/
      COGE_export-notes.md
      COGE_validation-summary.md

  GEVE/
    README_GEVE.md
    schema/
      GEVE_schema_only.sql
    metadata/
      GEVE_objects-inventory.csv
      GEVE_tables-columns.csv
      GEVE_foreign-keys.csv
      GEVE_indexes.csv
      GEVE_triggers.csv
      GEVE_routines.csv
    notes/
      GEVE_export-notes.md
      GEVE_validation-summary.md

  DCDC/
    README_DCDC.md
    schema/
      DCDC_schema_only.sql
    metadata/
      DCDC_objects-inventory.csv
      DCDC_tables-columns.csv
      DCDC_foreign-keys.csv
      DCDC_indexes.csv
      DCDC_triggers.csv
      DCDC_routines.csv
    notes/
      DCDC_export-notes.md
      DCDC_validation-summary.md

  VDOX/
    README_VDOX.md
    schema/
      VDOX_schema_only.sql
    metadata/
      VDOX_objects-inventory.csv
      VDOX_tables-columns.csv
      VDOX_foreign-keys.csv
      VDOX_indexes.csv
      VDOX_triggers.csv
      VDOX_routines.csv
    notes/
      VDOX_export-notes.md
      VDOX_validation-summary.md
```

## Contenuto export

- Schema only: sì
- Tabelle: sì
- Viste: sì
- Stored procedure: sì
- Funzioni: sì, dove presenti
- Trigger: sì, dove presenti
- Indici: sì
- Foreign key: sì, dove presenti
- Metadata CSV: sì
- Dati cliente inclusi: NO
- Backup `.bak` inclusi: NO
- File `.mdf` inclusi: NO
- File `.ldf` inclusi: NO
- Password / segreti inclusi: NO
- Connection string complete incluse: NO

## Riepilogo controlli per database

| Database | Schema SQL | Metadata CSV | Foreign key | Trigger | Note |
|---|---|---|---|---|---|
| COGE | sì | sì | presenti | nessun trigger utente rilevato | Ruolo da confermare |
| GEVE | sì | sì | nessuna FK rilevata | nessun trigger utente rilevato | Ruolo da confermare |
| DCDC | sì | sì | nessuna FK rilevata | nessun trigger utente rilevato | GQUA assente; presente DCDC |
| VDOX | sì | sì | presenti | nessun trigger utente rilevato | Ruolo da confermare |

## Dubbi / anomalie

- I ruoli funzionali dei database sono presunti e da confermare con Francesco o con un senior.
- In `SVRSELCO21\XSEL` non risulta presente `GQUA`; è stato invece individuato ed esportato `DCDC`.
- Alcuni database non espongono foreign key tramite `sys.foreign_keys`; i relativi CSV sono stati mantenuti con sola intestazione per documentare il controllo effettuato.
- Alcuni database non espongono trigger utente tramite `sys.triggers`; i relativi CSV sono stati mantenuti con sola intestazione.
- In alcuni script SSMS possono comparire riferimenti testuali a file fisici `.mdf/.ldf` nella sezione `CREATE DATABASE`; non sono stati inclusi file `.mdf` o `.ldf` nello ZIP.
- Eventuali riferimenti a utenti/schema/valori potenzialmente sensibili sono stati redatti o segnalati nelle note dei singoli database.

## Controlli finali eseguiti

- Verificato che gli export siano schema-only.
- Verificato che non siano presenti backup `.bak`.
- Verificato che non siano presenti file `.mdf` o `.ldf`.
- Verificato che non siano inclusi dati cliente come export dati.
- Verificato che i database individuati siano stati esportati separatamente.
- Verificato che per ogni database siano presenti schema, metadata e note.
- Verificato che eventuali anomalie siano documentate nei file `notes/`.

## Consegna

Nome ZIP finale consigliato:

```text
XSEL_DB_SCHEMA_EXPORT_2026-05-20.zip
```

Uso interno SELCO - Non includere dati cliente.
