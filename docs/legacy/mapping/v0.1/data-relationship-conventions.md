---
title: DXCOGE Legacy Mapping v0.1
author: SELCO / AI-assisted mapping
status: Draft for senior review
scope: Repository + schema-only DB export analysis
repository: FCastiglia-Oakenshield/DXCOGE
created: 2026-05-22
---

# DXCOGE - Data Relationship Conventions v0.1

## 1. Principio

Non assumere che le relazioni siano espresse da foreign key SQL Server. Nel legacy SELCO molte relazioni sono implicite.

## 2. Fonti da usare per dedurre relazioni

1. Campi `Rif` e varianti.
2. Convenzioni di naming.
3. Stored procedure.
4. Viste ufficiali.
5. Codice VB.NET.
6. Validazione senior.

## 3. Esempi di pattern da cercare

- `CliRif`, `ForRif`, `AnaRif` per relazioni anagrafiche.
- `TesRif`, `RigaRif`, `MovRif` per testata/righe/movimenti.
- `ComRif`, `CdcRif`, `DcgRif` per commesse/centri di costo/contabilita.
- Tabelle o viste con prefissi `Tb`, `V`, `X_`, `Fn`.

## 4. Regola per AI/Codex

Quando una relazione non e' espressa da FK, non inventarla. Segnalarla come ipotesi e inserirla negli open point senior.
