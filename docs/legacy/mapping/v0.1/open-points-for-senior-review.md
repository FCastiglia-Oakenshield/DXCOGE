---
title: DXCOGE Legacy Mapping v0.1
author: SELCO / AI-assisted mapping
status: Draft for senior review
scope: Repository + schema-only DB export analysis
repository: FCastiglia-Oakenshield/DXCOGE
created: 2026-05-22
---

# DXCOGE - Open Points for Senior Review v0.1

## 1. Scopo

Domande mirate per validare la mappatura codice + database. L'obiettivo non e' spiegare tutto DXCOGE, ma chiudere le ambiguita' che impediscono una documentazione affidabile e una futura migrazione controllata.

## 2. Domande prioritarie

1. Confermate questa mappa connessioni?
   - `cnDb` = `GEVE`
   - `cnVd` = `VDOX`
   - `cnCo` = `COGE`
   - `CnDc` = `DCDC`
2. `DXCOGE` e' da considerare il gestionale contabile principale o una libreria/app di supporto agli altri ERP?
3. L'installazione XSEL e' rappresentativa di DXCOGE o contiene personalizzazioni specifiche?
4. Quali clienti/istanze usano davvero `DCDC`?
5. Il menu guidato da `VDXmenu` su `cnCo` e' la fonte ufficiale dei permessi?

## 3. Domande su NCOGE

6. Quali maschere sono ancora operative in produzione?
7. Quali maschere sono storiche o non piu' usate?
8. Quali tabelle di COGE contengono le scritture contabili ufficiali?
9. Esistono stored procedure da considerare business-critical?
10. Quali operazioni non devono mai essere replicate senza validazione manuale?

## 4. Domande su NCDCO / DCDC

11. `DCDC` contiene centri di costo, contabilita analitica o altro?
12. `MenuCDCO` e `MenuCOAN` corrispondono a due domini diversi?
13. I controlli su `UserId` (`PASTAECO`, `PASTANEW`, `PASTAGROUP`, `MONDOMARINE`) sono ancora attivi?
14. `DCDC` e' necessario per avviare DXCOGE o solo per alcune installazioni?

## 5. Domande su NCCOM / NGEVE

15. `NCCOM` cosa contiene esattamente: componenti comuni commerciali, contabili o integrazione con ERP verticali?
16. `NGEVE` in DXCOGE e' usato solo come supporto o espone moduli operativi?
17. Quali dati GEVE sono letti o scritti da DXCOGE?
18. Esistono flussi che sincronizzano GEVE e COGE?

## 6. Domande su VDOX

19. `VDOX` e' la fonte ufficiale per utenti, gruppi e risorse?
20. `VUteGrup` e `VTbRisorse` sono ancora le viste ufficiali per autenticazione/autorizzazione?
21. Le password sono ancora gestite in chiaro/cifrate lato applicazione o in altro modo?

## 7. Domande sul primo pilot

22. Quale consultazione read-only sarebbe utile e a basso rischio?
23. Quale vista/tabelle usereste per consultare piano dei conti o saldi?
24. Quale modulo contabile e' piu' stabile e meno personalizzato?
25. Cosa deve restare assolutamente fuori dal primo ciclo AI/Codex?
