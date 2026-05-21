---
title: DXCOGE Legacy Mapping v0.1
author: SELCO / AI-assisted mapping
status: Draft for senior review
scope: Repository + schema-only DB export analysis
repository: FCastiglia-Oakenshield/DXCOGE
created: 2026-05-22
---

# DXCOGE - Legacy Map v0.1

## 1. Scopo del documento

Questa mappatura e' una prima fotografia tecnica/funzionale di DXCOGE, costruita a partire dal repository applicativo e dall'export schema-only SQL Server gia' presente nel repository.

Il documento resta una bozza per review senior: non e' una specifica di migrazione, non autorizza modifiche applicative e non sostituisce la validazione di Aldo/Massimo/Fabio.

Serve a:

- consolidare una base documentale ufficializzabile;
- collegare codice VB.NET e struttura SQL Server;
- far emergere domini funzionali candidati;
- identificare rischi, dipendenze e open point;
- creare contesto leggibile da senior, junior e agenti AI.

## 2. Fonti analizzate

### 2.1 Repository applicativo

Repository: `FCastiglia-Oakenshield/DXCOGE`, branch `master`.

File principali analizzati:

| File | Uso nella mappatura |
| --- | --- |
| DXCOGE.sln | Elenco progetti e struttura solution |
| NSTARTX/NSTARTX.vbproj | Progetto di avvio e riferimenti ai moduli |
| NSTARTX/ConfigClass.vb | Configurazione connessioni SQL Server |
| NSTARTX/Inizio.vb | Entry point, login utenti/risorse |
| NSTARTX/XMENU.vb | Shell MDI, menu, gating moduli e lancio maschere |
| docs/legacy/database/schema-export/2026-05-21/ | Export DB schema-only XSEL |

### 2.2 Solution e progetti

| Progetto | Tipo | Lettura iniziale |
| --- | --- | --- |
| NSTARTX | WinExe | Shell applicativa, avvio, login, menu MDI, configurazione connessioni |
| NPRINT | Library | Stampe/reportistica e servizi di output |
| NGEVE | Library | Funzioni gestionali collegate a GEVE, dati comuni e integrazioni |
| NCOGE | Library | Core contabile/amministrativo |
| NCDCO | Library | Centri di costo / contabilita analitica / componente DCDC |
| NCCOM | Library | Componenti commerciali/contabili comuni |

## 3. Sintesi esecutiva

DXCOGE appare come il blocco piu' contabile/amministrativo tra i gestionali analizzati finora. Rispetto ad ARGO/RACCA, non sembra essere solo una verticalizzazione gestionale cliente, ma una solution focalizzata su contabilita', menu contabili, centri di costo, servizi commerciali/contabili comuni e integrazione con database COGE/GEVE/VDOX/DCDC.

La solution e' VB.NET Windows Forms su .NET Framework 4.6.1, con DevExpress 19.2, Crystal Reports e dipendenze interne SELCO come `DXBASE`.

L'entry point e' `NSTARTX.Inizio`. Il progetto di avvio `NSTARTX` e' un `WinExe` e fa riferimento a `NCCOM`, `NCDCO`, `NCOGE`, `NGEVE` e `NPRINT`.

## 4. Mappa database iniziale

Path export:

```text
docs/legacy/database/schema-export/2026-05-21/
```

L'export si riferisce all'installazione `SVRSELCO21\XSEL` ed e' schema-only, senza dati cliente, backup, file MDF/LDF, password o connection string complete.

| Database | Tabelle | Colonne | Viste | Stored procedure | Funzioni | FK distinte | Trigger | Indici | Ruolo presunto |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| COGE | 121 | 1395 | 100 | 102 | 9 | 4 | 0 | 74 | Contabile / amministrativo |
| GEVE | 115 | 1549 | 33 | 66 | 9 | 0 | 0 | 74 | Gestionale collegato / tabelle operative condivise |
| DCDC | 11 | 61 | 5 | 15 | 3 | 0 | 0 | 7 | Centri di costo / componente collegata |
| VDOX | 17 | 163 | 21 | 22 | 4 | 1 | 0 | 15 | Documentale / utenti / servizi condivisi |

## 5. Lettura architetturale preliminare

### 5.1 Connessioni database

`ConfigClass.vb` legge `Config.xml`, sostituisce la password cifrata e apre le connessioni globali:

- `cnDb` da `ConnectStr(0)`;
- `cnVd` da `ConnectStr(1)`;
- `cnCo` da `ConnectStr(2)`;
- `CnDc` opzionale da `ConnectStr(3)` se presente.

La quarta connessione sembra legata a DCDC / centri di costo / contabilita analitica, ma il ruolo va validato dai senior.

### 5.2 Avvio e autenticazione

`Inizio.vb` gestisce risoluzione video, avvio configurazione, login e apertura di `XMENU`.

Il login lavora principalmente su VDOX:

- utenti: `VUteGrup` su `cnVd`, con controllo su `grupLavGest`;
- risorse: `VTbRisorse` su `cnVd`.

Questo conferma il ruolo di VDOX come DB di utenti/gruppi/servizi condivisi, almeno per questa parte.

### 5.3 Menu e gating funzionale

`XMENU.vb` importa direttamente:

- `NGEVE`;
- `NCCOM`;
- `NCDCO`;
- `NCOGE`;
- `DXBASE`.

Il menu abilita voci su `VDXmenu`, usando `cnCo` come connessione menu. Questo differenzia DXCOGE dai gestionali dove il menu era prevalentemente guidato da GEVE.

La presenza di logiche condizionali su `CnDc`, `UserId`, `MenuCDCO`, `MenuCOAN` e su valori come `PASTAECO`, `PASTANEW`, `PASTAGROUP`, `MONDOMARINE`, `BIENNE`, `GSSPA` indica che DXCOGE contiene molte personalizzazioni storiche per istanza/cliente.

## 6. Domini funzionali candidati

Emersi in prima lettura:

1. Contabilita generale e amministrazione.
2. Contabilita analitica / centri di costo.
3. Menu, sicurezza applicativa e abilitazioni.
4. Anagrafiche condivise e dati gestionali collegati.
5. Stampe, reportistica e output documentale.
6. Integrazione commerciale/contabile tramite NCCOM.
7. Gestione risorse/utenti collegata a VDOX.

## 7. Implicazioni per migrazione futura

DXCOGE e' probabilmente strategico ma non lo tratterei come primo candidato di migrazione operativa se l'obiettivo e' scegliere il gestionale piu' semplice per validare il metodo.

Pro:

- e' centrale per l'area amministrativa;
- espone moduli chiari per COGE/DCDC;
- puo' chiarire dipendenze comuni che ARGO e altri gestionali usano gia' come DLL;
- potrebbe diventare documentazione base per capire le integrazioni contabili aziendali.

Contro:

- e' piu' critico fiscalmente/contabilmente;
- ha molte personalizzazioni per cliente/istanza;
- usa logica DB e logica VB.NET fortemente intrecciate;
- una migrazione scrivente sarebbe ad alto rischio.

## 8. Raccomandazione iniziale

Per DXCOGE il primo pilot non dovrebbe essere una riscrittura.

Candidate sicure:

- consultazione read-only piano dei conti / anagrafiche contabili;
- consultazione centri di costo;
- consultazione movimenti o saldi, se esistono viste ufficiali da validare;
- mappa menu/permessi read-only.

Da evitare nel primo ciclo:

- registrazioni contabili;
- chiusure/riaperture;
- aggiornamenti centri di costo;
- scritture integrate con GEVE/COGE;
- qualsiasi logica fiscale o civilistica senza validazione senior.

## 9. Stato documento

La v0.1 e' adatta a una review senior mirata. Va validata soprattutto su:

- ruolo effettivo di `DCDC`;
- mappa connessioni;
- confine tra `NCOGE`, `NCDCO`, `NCCOM`, `NGEVE`;
- parti ancora usate vs storiche;
- personalizzazioni cliente ancora attive.
