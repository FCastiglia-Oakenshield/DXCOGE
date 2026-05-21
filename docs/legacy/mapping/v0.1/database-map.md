---
title: DXCOGE Legacy Mapping v0.1
author: SELCO / AI-assisted mapping
status: Draft for senior review
scope: Repository + schema-only DB export analysis
repository: FCastiglia-Oakenshield/DXCOGE
created: 2026-05-22
---

# DXCOGE - Database Map v0.1

## 1. Scopo

Mappare i database SQL Server dell'installazione XSEL collegata a DXCOGE e chiarire il loro ruolo presunto.

## 2. Export analizzato

Path:

```text
docs/legacy/database/schema-export/2026-05-21/
```

L'export e' schema-only e include metadata CSV, script SQL e note per ogni database.

## 3. Database inclusi

| Database | Tabelle | Colonne | Viste | Stored procedure | Funzioni | FK distinte | Trigger | Indici | Ruolo presunto |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| COGE | 121 | 1395 | 100 | 102 | 9 | 4 | 0 | 74 | Contabile / amministrativo |
| GEVE | 115 | 1549 | 33 | 66 | 9 | 0 | 0 | 74 | Gestionale collegato / tabelle operative condivise |
| DCDC | 11 | 61 | 5 | 15 | 3 | 0 | 0 | 7 | Centri di costo / componente collegata |
| VDOX | 17 | 163 | 21 | 22 | 4 | 1 | 0 | 15 | Documentale / utenti / servizi condivisi |

## 4. Lettura delle connessioni applicative

`NSTARTX/ConfigClass.vb` apre tre connessioni principali e una opzionale:

- `cnDb` = database gestionale principale, probabilmente `GEVE`;
- `cnVd` = documentale/utenti/servizi condivisi, probabilmente `VDOX`;
- `cnCo` = contabile/amministrativo, probabilmente `COGE`;
- `CnDc` = opzionale, probabilmente `DCDC`.

Open point: confermare se questa mappatura e' sempre valida per XSEL oppure se varia in base a `Config.xml` e alle installazioni.

## 5. Rischi interpretativi

- Le relazioni non sono sempre esposte da foreign key SQL Server.
- Alcune connessioni sono globali e usate da moduli diversi.
- `CnDc` e' opzionale: il codice deve essere letto distinguendo installazioni con o senza DCDC.
- I ruoli funzionali dei DB sono presunti e vanno validati.

## 6. Nota per AI/Codex

Non inferire relazioni solo dalle foreign key. Usare con priorita': campi `Rif`, convenzioni di naming, viste, stored procedure, codice VB.NET e review senior.
