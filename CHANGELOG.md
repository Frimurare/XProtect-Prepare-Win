# Changelog

## 1.0.2 "Happy Tekkie" — 2026-07-22

- **Light & dark theme, following Windows** — the app now reads the Windows app-theme setting and matches it, live: switch Windows between dark and light and the app follows without a restart.
- **Four languages** — English, Svenska, Dansk and Norsk. Follows the Windows display language by default, with an override in Settings.
- **Dark-themed dropdowns** — the NTP source picker (and all combo boxes) now match the theme instead of the bare Windows look.
- README overhauled with real screenshots of every page.

## 1.0.1 — 2026-07-21

- **Selectable NTP upstream source** — new Settings dropdown (and `--ntp` CLI flag) with Windows default (`time.windows.com`), the global `pool.ntp.org` zone, and 37 country pools. The NTP check now verifies against your chosen source, so changing country is itself detected and corrected on the next run. Default is `time.windows.com`.
- **Smarter XProtect detection** — the System page and Dashboard now report the actual installed product and release (e.g. "XProtect VMS 2025 R3") instead of whichever Milestone component happened to be found first in the registry. Hotfixes, device packs, MIP SDK and the tool itself are excluded from detection.
- Dashboard now restores each task's status from the last run at startup (no more "Not run" on a prepared machine).
- Toned-down branding: removed the "ex Milestone Solution Engineer" line throughout.

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
