# Changelog

All notable changes to XProtect Prepare Win are documented here.

## [1.0.0] — 2026-07-21

Initial release.

### Added
- WPF GUI with per-item checklist: every task reports **Already done** (was correct, untouched) or **Corrected now** (was wrong, fixed) — fully idempotent, safe to re-run any time
- Antivirus/Defender exclusions for all XProtect components, including VictoriaLogs and the Smart Client
- Recording storage checks: 64 KB allocation unit size verification and Windows indexing disabled on media drives
- Performance tuning: High Performance power plan, service tuning, `LargeSystemCache`
- Remote Desktop enablement
- Bloatware cleanup: OneDrive, Teams, new Outlook and other consumer apps removed (Xbox components deliberately kept — Windows screen recording depends on them)
- Privacy hardening: Copilot off, Recall off, telemetry minimized
- NTP time server configuration with the `,0x9` flag fix
- MilestonePSTools installation
- Optional OpenSSH server and SNMP installation
- Windows Update pause/resume toggle
- Full logging and registry backup of every changed value to `C:\Milestonecheck-ulfh`
- Silent mode for golden images and scripted deployment: `--silent [--with-ssh] [--drives D,E|storage]`
- Single-file, self-contained .NET 9 build — no runtime prerequisites on the target machine
- MSI installer (WiX 5): per-machine install, Start Menu shortcut, optional desktop shortcut, branded wizard, "View the Readme" on finish, clean major-upgrade path
