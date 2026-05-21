---
title: DXCOGE Legacy Mapping v0.1
author: SELCO / AI-assisted mapping
status: Draft for senior review
scope: Repository + schema-only DB export analysis
repository: FCastiglia-Oakenshield/DXCOGE
created: 2026-05-22
---

# DXCOGE - Tables Inventory v0.1

## 1. Scopo

Sintesi iniziale degli oggetti tabellari per database. Questa versione non sostituisce i CSV sorgente dell'export, ma fornisce una lettura sintetica per review e confronto tra gestionali.

## 2. Conteggi da metadata

| Database | Tabelle | Colonne | Viste | FK distinte | Trigger | Indici distinti | Osservazione |
| --- | --- | --- | --- | --- | --- | --- | --- |
| COGE | 121 | 1395 | 100 | 4 | 0 | 74 | Area contabile ricca e strutturata |
| GEVE | 115 | 1549 | 33 | 0 | 0 | 74 | Gestionale collegato, nessuna FK rilevata |
| DCDC | 11 | 61 | 5 | 0 | 0 | 7 | Sottoinsieme centri di costo / analitica |
| VDOX | 17 | 163 | 21 | 1 | 0 | 15 | Utenti, documentale, servizi condivisi |

## 3. Lettura preliminare

`COGE` e' il database piu' rilevante per DXCOGE. `GEVE` resta presente come supporto gestionale collegato. `DCDC` va trattato come componente opzionale o condizionale da confermare. `VDOX` sembra centrale per utenti, gruppi e risorse.

## 4. Open point

- Quali tabelle di `COGE` sono master data contabili?
- Quali tabelle contengono movimenti/scritture ufficiali?
- Quali tabelle DCDC sono realmente usate in XSEL?
- Quali viste sono considerate ufficiali per letture read-only sicure?
