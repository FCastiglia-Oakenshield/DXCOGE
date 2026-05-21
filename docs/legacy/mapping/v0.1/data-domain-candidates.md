---
title: DXCOGE Legacy Mapping v0.1
author: SELCO / AI-assisted mapping
status: Draft for senior review
scope: Repository + schema-only DB export analysis
repository: FCastiglia-Oakenshield/DXCOGE
created: 2026-05-22
---

# DXCOGE - Domain Candidates v0.1

## 1. Domini funzionali emersi

### 1. Contabilita generale

Dominio principale presunto, legato a `NCOGE` e database `COGE`.

### 2. Contabilita analitica / centri di costo

Dominio legato a `NCDCO`, database `DCDC`, menu `MenuCDCO` e `MenuCOAN`.

### 3. Sicurezza, utenti e menu

Dominio trasversale legato a `NSTARTX`, `VDOX`, `VUteGrup`, `VTbRisorse`, `VDXmenu`.

### 4. Componenti commerciali/contabili comuni

Dominio legato a `NCCOM`, da chiarire nei confini con `NCOGE` e con gli ERP verticali.

### 5. Gestionale collegato GEVE

Dominio di supporto, probabilmente usato per anagrafiche, tabelle comuni o integrazioni.

### 6. Stampe e reportistica

Dominio legato a `NPRINT`, Crystal Reports e output amministrativi.

## 2. Domini non ancora confermati

- fiscalita specifica;
- chiusure/riaperture;
- prima nota;
- partitari/scadenziari;
- centri di costo ufficiali;
- integrazione con fatturazione elettronica;
- integrazione con gestionali verticali.

## 3. Nota metodologica

Questi sono candidati funzionali, non microservizi. I confini tecnici vanno derivati solo dopo review senior e dopo un deep dive su un modulo pilota.
