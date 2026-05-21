---
title: DXCOGE Legacy Mapping v0.1
author: SELCO / AI-assisted mapping
status: Draft for senior review
scope: Repository + schema-only DB export analysis
repository: FCastiglia-Oakenshield/DXCOGE
created: 2026-05-22
---

# DXCOGE - Migration Candidates v0.1

## 1. Obiettivo

Individuare candidati realistici per eventuali analisi pilota, senza avviare ancora una migrazione.

## 2. Candidati a basso rischio

### A. Consultazione piano dei conti / anagrafiche contabili

Pro: dominio leggibile, utile, potenzialmente read-only.  
Contro: serve capire viste/tabelle ufficiali.

### B. Consultazione centri di costo

Pro: perimetro DCDC limitato.  
Contro: DCDC e' opzionale e va validato.

### C. Mappa menu/permessi

Pro: utile per documentazione e junior; basso rischio se read-only.  
Contro: non produce subito valore cliente diretto.

### D. Consultazione utenti/risorse

Pro: dati gia' letti da VDOX.  
Contro: possibile tema sicurezza/privacy.

## 3. Candidati da evitare nel primo ciclo

- registrazioni contabili;
- chiusure e aperture contabili;
- scritture automatiche COGE/GEVE;
- logiche fiscali;
- modifica centri di costo;
- funzioni che aggiornano piu' database.

## 4. Raccomandazione

DXCOGE e' utile come mappa centrale del dominio contabile SELCO, ma non lo sceglierei come primo gestionale per migrazione operativa se l'obiettivo e' ridurre rischio. Lo sceglierei invece se l'obiettivo e' capire la base contabile comune usata dagli altri gestionali.
