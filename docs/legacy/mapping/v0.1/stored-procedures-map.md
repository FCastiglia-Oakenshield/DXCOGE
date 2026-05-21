---
title: DXCOGE Legacy Mapping v0.1
author: SELCO / AI-assisted mapping
status: Draft for senior review
scope: Repository + schema-only DB export analysis
repository: FCastiglia-Oakenshield/DXCOGE
created: 2026-05-22
---

# DXCOGE - Stored Procedures and Functions Map v0.1

## 1. Scopo

Mappare il peso della logica SQL Server presente nei database collegati a DXCOGE.

## 2. Conteggi routine

| Database | Stored procedure | Funzioni | Lettura iniziale |
| --- | --- | --- | --- |
| COGE | 102 | 9 | Logica contabile/amministrativa molto rilevante |
| GEVE | 66 | 9 | Logica gestionale collegata |
| DCDC | 15 | 3 | Logica analitica/centri di costo da validare |
| VDOX | 22 | 4 | Servizi condivisi, utenti/documentale |

## 3. Implicazioni

La migrazione non puo' essere pianificata guardando solo il codice VB.NET. Una parte importante del comportamento e' in stored procedure, viste e funzioni.

## 4. Regola operativa

Per ogni futuro modulo candidato serve tracciare:

1. form VB.NET coinvolte;
2. tabelle lette/scritte;
3. viste ufficiali;
4. stored procedure chiamate;
5. funzioni usate;
6. impatti su COGE/GEVE/DCDC/VDOX;
7. conferma senior.
