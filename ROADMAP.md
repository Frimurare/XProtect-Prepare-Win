# Roadmap

Planned features and improvements for **XProtect Prepare Win**. Nothing here is a
promise of a date — it's what's on the workbench.

## v1.0.3 "Happy Hippo" (planned)

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

## Backlog / ideas

- Same "bundle it, don't download it" treatment for anything else that currently
  relies on a Windows Update pull (evaluate SNMP).
- Optional CLI flag to write the check-only **System report** to a chosen path
  (handover documentation without opening the UI).
