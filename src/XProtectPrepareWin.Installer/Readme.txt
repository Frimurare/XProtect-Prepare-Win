=====================================================================
 XProtect Prepare Win  v1.0.0
 Prepare XProtect. Not Windows.
=====================================================================

WHAT THIS TOOL DOES
-------------------
XProtect Prepare Win prepares a Windows machine for a Milestone
XProtect installation, according to field-proven best practice.

The model is simple: CHECK FIRST, FIX ONLY WHAT IS NEEDED.

Every item is checked before anything is touched:

  * Already done   - the setting was correct, nothing was changed
  * Corrected now  - the setting was wrong and has been fixed

That means you can re-run the tool any time - after Windows updates,
after someone "helped" with the server, or just to verify a machine.
Running it twice is always safe.

What it covers (highlights):

  * Antivirus/Defender exclusions for all XProtect components
    (including VictoriaLogs and the Smart Client)
  * Recording storage checks - 64 KB allocation unit size,
    indexing off on media drives
  * Power plan, service tuning and LargeSystemCache
  * Remote Desktop enabled
  * Bloatware cleanup: OneDrive, Teams, new Outlook and friends
    (Xbox components are kept on purpose - Windows screen
    recording uses them)
  * Copilot, Recall and telemetry turned off
  * NTP time server configuration (with the ,0x9 flag fix)
  * MilestonePSTools installed for scripted administration
  * Optional: OpenSSH server and SNMP
  * Windows Update pause/resume toggle
  * Full logging plus a registry backup of everything it changes

QUICK START
-----------
1. Open the Start Menu and launch "XProtect Prepare Win".
2. Approve the elevation prompt (the tool needs admin rights -
   it will ask via UAC if you start it unelevated).
3. Review the checklist, press Run, read the results.
4. Re-run whenever you like.

SILENT / GOLDEN IMAGE USAGE
---------------------------
For scripted deployments and golden images:

  XProtectPrepareWin.exe --silent
  XProtectPrepareWin.exe --silent --with-ssh
  XProtectPrepareWin.exe --silent --drives D,E
  XProtectPrepareWin.exe --silent --drives storage

  --silent        run everything without UI, exit when done
  --with-ssh      also install/enable the OpenSSH server
  --drives D,E    treat these drive letters as recording storage
  --drives storage  auto-detect likely storage drives

LOGS AND BACKUPS
----------------
Everything the tool does is logged to:

  C:\Milestonecheck-ulfh

That folder also holds a registry backup of the values changed,
so you can always see - and undo - exactly what was modified.

IMPORTANT - NOT A MILESTONE PRODUCT
-----------------------------------
This tool is NOT made by, with, or on behalf of Milestone Systems
A/S, and it is not endorsed or supported by them. It is a personal
project by an ex Milestone Solution Engineer, built to help fellow
technicians get XProtect servers right the first time. "Milestone"
and "XProtect" are trademarks of Milestone Systems A/S. Use at your
own responsibility - though everything it does follows documented
best practice, and everything is logged.

LICENSE
-------
MIT License - free to use, modify and share.
Copyright (c) 2026 Ulf Holmström.

---------------------------------------------------------------------
 Ulf Holmström, Happy Problem Solver at Manvarg AB, Sweden
 https://github.com/Frimurare/XProtect-Prepare-Win
---------------------------------------------------------------------
