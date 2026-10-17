# Roadmap

Planned features and improvements for **XProtect Prepare Win**. Nothing here is a
promise of a date — it's what's on the workbench.

## ✅ v1.0.4 "Leak Busters" (shipped) — motion detection HW acceleration Off

Works around the Recording Server memory leak with NVIDIA-accelerated motion
detection (*Automatic* leaks ~100 MB per camera and hour on 2025 R2-2026 R1).
**Remove the task once the vendor ships a fix.**

## ✅ v1.0.3 "Fat Hippo" (shipped) — bundled OpenSSH + Finnish

Shipped: OpenSSH is now bundled inside the app (see below), and Finnish (Suomi)
joins the language line-up alongside English, Swedish, Danish and Norwegian.

### Bundled OpenSSH — instant, offline SSH install

**Today:** the optional SSH task installs OpenSSH Server through the Windows
"Feature on Demand" mechanism (`Add-WindowsCapability -Online -Name
OpenSSH.Server`). That pull comes from Windows Update, so it is slow, needs
internet/WU access, and fails outright on machines where Windows Update is
blocked or WSUS-managed — exactly the locked-down servers where you most want
SSH for remote administration.

**Planned:** ship the official
[Win32-OpenSSH](https://github.com/PowerShell/Win32-OpenSSH) binaries **inside
the app** (it is MIT-licensed, so it's compatible with this project's license).
Installing SSH then becomes a local, offline, few-second operation:

- Extract the bundled `OpenSSH-Win64` to `C:\Program Files\OpenSSH`
- Run the shipped `install-sshd.ps1`, set the service to autostart
- Apply the same `sshd_config` hardening we do today (password auth for admins,
  the `Match Group administrators` handling) and open TCP 22
- **No Windows Update dependency, no download, works on air-gapped servers**

Keep `Add-WindowsCapability` as an automatic fallback only if the bundled path
fails, so behaviour never regresses. The single-file self-contained EXE grows by
a few MB — a fair trade for an install that always works and finishes instantly.

---

## NEXT: `ntp.se` as the DEFAULT time source + reachability check (requested 2026-10-14, default decided 2026-10-17)

Today the NTP task writes the chosen pool (`se` = `se.pool.ntp.org,0x9`,
`windows` = `time.windows.com,0x9`, ...) and verifies only the *registry
format*. Two things we now want, both from a real customer-server build where
the house standard became the Swedish national time service:

- **Make `ntp.se` (Netnod, the Swedish national time service) the default
  source** for new installs, in place of `time.windows.com`. Peers:
  `ntp.se,0x9 se.pool.ntp.org,0x9`, with the pool as fallback. Netnod runs
  stratum 1 servers traceable to Swedish national time (UTC(SP)) over anycast.
  It is more accurate and more reliable than `time.windows.com`, and it works
  from anywhere. Add `--ntp ntp.se` on the CLI and "ntp.se (Netnod)" at the
  top of the Settings dropdown. All the other sources stay selectable
  (`windows`, `global`, 37 country pools).
- **Migrating existing settings:** `settings.json` stores `NtpSource="windows"`
  both when the user chose it and when it was just the old default, so the two
  cannot be told apart. Add `NtpSourceExplicit` (true only when the user picked
  a source in Settings or with `--ntp`). When it is missing or false, treat
  `windows` as "old default" and move it to `ntp.se`. An explicit choice is
  never changed.
- **Test the server, don't just write it.** After configuring, the task should
  run `w32tm /stripchart /computer:<source> /samples:3 /dataonly` (or an NTP
  query of our own on UDP/123) and treat *no answer* as a **warning with a
  clear message** ("NTP source ntp.se did not respond - check outbound UDP/123
  or pick another source"), both in the UI row and in the silent-mode log/exit
  summary. Today a blocked UDP/123 passes silently and the server drifts until
  Milestone complains about certificate/time skew.
- Show the measured offset (ms) in the System report so handover documentation
  proves the clock is actually disciplined.

Also planned: run this task as part of every Prudencia server build with
`--ntp ntp.se` (it is already in the build recipe as of SANDSANDVI-SV01).

---

## Backlog / ideas

- Same "bundle it, don't download it" treatment for anything else that currently
  relies on a Windows Update pull (evaluate SNMP).
- Optional CLI flag to write the check-only **System report** to a chosen path
  (handover documentation without opening the UI).
