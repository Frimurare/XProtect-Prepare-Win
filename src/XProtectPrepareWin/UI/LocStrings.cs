namespace XProtectPrepareWin.UI;

/// <summary>
/// All UI strings, one key per entry, value = [en, sv, da, no].
/// English is the source of truth; task/category English entries mirror the
/// engine strings verbatim. Engine output (CheckResult.Detail, logs) stays English.
/// </summary>
public static class LocStrings
{
    public static readonly Dictionary<string, string[]> Table = new()
    {
        // ---------- Navigation / sidebar ----------
        ["nav.dashboard"] = new[] { "Dashboard", "Översikt", "Oversigt", "Oversikt" },
        ["nav.system"] = new[] { "System", "System", "System", "System" },
        ["nav.preparation"] = new[] { "Preparation", "Förberedelse", "Forberedelse", "Forberedelse" },
        ["nav.tools"] = new[] { "Tools", "Verktyg", "Værktøjer", "Verktøy" },
        ["nav.logs"] = new[] { "Logs", "Loggar", "Logfiler", "Logger" },
        ["nav.settings"] = new[] { "Settings", "Inställningar", "Indstillinger", "Innstillinger" },
        ["nav.about"] = new[] { "About", "Om", "Om", "Om" },
        ["nav.exit"] = new[] { "Exit", "Avsluta", "Afslut", "Avslutt" },
        ["sidebar.disclaimer"] = new[]
        {
            "Prepares Windows for XProtect\nby Milestone best practice.\nNot affiliated with Milestone Systems.",
            "Förbereder Windows för XProtect\nenligt Milestones best practice.\nInte ansluten till Milestone Systems.",
            "Forbereder Windows til XProtect\nefter Milestones best practice.\nIkke tilknyttet Milestone Systems.",
            "Forbereder Windows for XProtect\netter Milestones beste praksis.\nIkke tilknyttet Milestone Systems.",
        },

        // ---------- Brand line (stays English in every language) ----------
        ["brand.headline1"] = new[] { "Prepare XProtect. ", "Prepare XProtect. ", "Prepare XProtect. ", "Prepare XProtect. " },
        ["brand.headline2"] = new[] { "Not Windows.", "Not Windows.", "Not Windows.", "Not Windows." },

        // ---------- Shared ----------
        ["common.detecting"] = new[] { "Detecting…", "Identifierar…", "Registrerer…", "Registrerer…" },
        ["app.unexpectedErrorFmt"] = new[] { "Unexpected error: {0}", "Oväntat fel: {0}", "Uventet fejl: {0}", "Uventet feil: {0}" },

        // ---------- Run/status texts ----------
        ["status.notrun"] = new[] { "Not run", "Inte körd", "Ikke kørt", "Ikke kjørt" },
        ["status.checking"] = new[] { "Checking…", "Kontrollerar…", "Kontrollerer…", "Kontrollerer…" },
        ["status.applying"] = new[] { "Applying…", "Åtgärdar…", "Udfører…", "Utfører…" },
        ["status.alreadydone"] = new[] { "Already done", "Redan klart", "Allerede udført", "Allerede utført" },
        ["status.corrected"] = new[] { "Corrected now", "Åtgärdat nu", "Rettet nu", "Rettet nå" },
        ["status.warning"] = new[] { "Warning", "Varning", "Advarsel", "Advarsel" },
        ["status.failed"] = new[] { "Failed", "Misslyckades", "Mislykkedes", "Mislyktes" },
        ["status.skipped"] = new[] { "Skipped", "Överhoppad", "Sprunget over", "Hoppet over" },
        ["status.na"] = new[] { "N/A", "N/A", "N/A", "N/A" },
        ["status.running"] = new[] { "Running…", "Kör…", "Kører…", "Kjører…" },
        ["status.completed"] = new[] { "Completed", "Klart", "Fuldført", "Fullført" },
        ["status.issues"] = new[] { "Issues", "Anmärkningar", "Problemer", "Problemer" },

        // ---------- Dashboard ----------
        ["dash.subtitle"] = new[]
        {
            "Automate server preparation by Milestone best practice.",
            "Automatisera serverförberedelser enligt Milestones best practice.",
            "Automatisér serverforberedelse efter Milestones best practice.",
            "Automatiser serverforberedelse etter Milestones beste praksis.",
        },
        ["dash.card.system"] = new[] { "System detected", "Identifierat system", "Registreret system", "Registrert system" },
        ["dash.card.xp"] = new[] { "XProtect version", "XProtect-version", "XProtect-version", "XProtect-versjon" },
        ["dash.card.lastrun"] = new[] { "Last run", "Senaste körning", "Seneste kørsel", "Siste kjøring" },
        ["dash.prepstatus"] = new[] { "Preparation status", "Förberedelsestatus", "Forberedelsesstatus", "Forberedelsesstatus" },
        ["dash.completed"] = new[] { "Completed", "Klart", "Fuldført", "Fullført" },
        ["dash.noRunYet"] = new[]
        {
            "No preparation run yet",
            "Ingen förberedelsekörning ännu",
            "Ingen forberedelseskørsel endnu",
            "Ingen forberedelseskjøring ennå",
        },
        ["dash.runToOptimize"] = new[]
        {
            "Run preparation to optimize this server for XProtect.",
            "Kör förberedelsen för att optimera den här servern för XProtect.",
            "Kør forberedelsen for at optimere denne server til XProtect.",
            "Kjør forberedelsen for å optimalisere denne serveren for XProtect.",
        },
        ["dash.viewReport"] = new[] { "View preparation report", "Visa förberedelserapport", "Vis forberedelsesrapport", "Vis forberedelsesrapport" },
        ["dash.run"] = new[] { "Run preparation", "Kör förberedelse", "Kør forberedelse", "Kjør forberedelse" },
        ["dash.runAgain"] = new[] { "Run preparation again", "Kör förberedelsen igen", "Kør forberedelsen igen", "Kjør forberedelsen igjen" },
        ["dash.exportLog"] = new[] { "Export log", "Exportera logg", "Eksportér log", "Eksporter logg" },
        ["dash.openPstools"] = new[] { "Open MilestonePSTools", "Öppna MilestonePSTools", "Åbn MilestonePSTools", "Åpne MilestonePSTools" },
        ["dash.never"] = new[] { "Never", "Aldrig", "Aldrig", "Aldri" },
        ["dash.durationFmt"] = new[] { "Duration: {0}", "Varaktighet: {0}", "Varighed: {0}", "Varighet: {0}" },
        ["dash.detectedHere"] = new[]
        {
            "Detected on this machine",
            "Identifierad på den här maskinen",
            "Fundet på denne maskine",
            "Funnet på denne maskinen",
        },
        ["dash.notInstalled"] = new[] { "Not installed", "Inte installerat", "Ikke installeret", "Ikke installert" },
        ["dash.readyToPrepare"] = new[] { "Ready to prepare", "Redo att förberedas", "Klar til forberedelse", "Klar til forberedelse" },
        ["dash.preparing"] = new[] { "Preparing…", "Förbereder…", "Forbereder…", "Forbereder…" },
        ["dash.preparingSub"] = new[]
        {
            "Tasks are being checked and corrected. You can follow the details on the Preparation and Logs pages.",
            "Uppgifterna kontrolleras och åtgärdas. Du kan följa detaljerna på sidorna Förberedelse och Loggar.",
            "Opgaverne kontrolleres og rettes. Du kan følge detaljerne på siderne Forberedelse og Logfiler.",
            "Oppgavene kontrolleres og rettes. Du kan følge detaljene på sidene Forberedelse og Logger.",
        },
        ["dash.systemReady"] = new[] { "🎉 System ready!", "🎉 Systemet är redo!", "🎉 Systemet er klar!", "🎉 Systemet er klart!" },
        ["dash.systemReadySub"] = new[]
        {
            "Your server is optimized and ready for XProtect installation.",
            "Servern är optimerad och redo för XProtect-installation.",
            "Serveren er optimeret og klar til XProtect-installation.",
            "Serveren er optimalisert og klar for XProtect-installasjon.",
        },
        ["dash.issuesTitle"] = new[] { "Completed with issues", "Klar med anmärkningar", "Fuldført med problemer", "Fullført med problemer" },
        ["dash.issuesSubFmt"] = new[]
        {
            "{0} task(s) failed — check the Preparation page and the log.",
            "{0} uppgift(er) misslyckades — se sidan Förberedelse och loggen.",
            "{0} opgave(r) mislykkedes — se siden Forberedelse og loggen.",
            "{0} oppgave(r) mislyktes — se siden Forberedelse og loggen.",
        },
        ["dash.noLogYet"] = new[]
        {
            "No preparation log exists yet. Run a preparation first.",
            "Det finns ingen förberedelselogg ännu. Kör en förberedelse först.",
            "Der findes ingen forberedelseslog endnu. Kør en forberedelse først.",
            "Det finnes ingen forberedelseslogg ennå. Kjør en forberedelse først.",
        },
        ["dash.exportTitle"] = new[] { "Export preparation log", "Exportera förberedelselogg", "Eksportér forberedelseslog", "Eksporter forberedelseslogg" },
        ["dash.logFilter"] = new[]
        {
            "Log files (*.txt)|*.txt|All files (*.*)|*.*",
            "Loggfiler (*.txt)|*.txt|Alla filer (*.*)|*.*",
            "Logfiler (*.txt)|*.txt|Alle filer (*.*)|*.*",
            "Loggfiler (*.txt)|*.txt|Alle filer (*.*)|*.*",
        },
        ["dash.exportFailedFmt"] = new[]
        {
            "Could not export the log: {0}",
            "Kunde inte exportera loggen: {0}",
            "Loggen kunne ikke eksporteres: {0}",
            "Kunne ikke eksportere loggen: {0}",
        },

        // ---------- Categories (keys match the engine strings in Tasks/Common.cs) ----------
        ["cat.Antivirus exclusions"] = new[] { "Antivirus exclusions", "Antivirusundantag", "Antivirusundtagelser", "Antivirusunntak" },
        ["cat.Storage optimization"] = new[] { "Storage optimization", "Lagringsoptimering", "Lageroptimering", "Lagringsoptimalisering" },
        ["cat.Performance tuning"] = new[] { "Performance tuning", "Prestandaoptimering", "Ydelsesoptimering", "Ytelsesoptimalisering" },
        ["cat.NTP time server"] = new[] { "NTP time server", "NTP-tidsserver", "NTP-tidsserver", "NTP-tidsserver" },
        ["cat.Remote Desktop"] = new[] { "Remote Desktop", "Fjärrskrivbord", "Fjernskrivebord", "Eksternt skrivebord" },
        ["cat.Windows cleanup"] = new[] { "Windows cleanup", "Windows-rensning", "Windows-oprydning", "Windows-opprydding" },
        ["cat.MilestonePSTools"] = new[] { "MilestonePSTools", "MilestonePSTools", "MilestonePSTools", "MilestonePSTools" },
        ["cat.Logging and verification"] = new[] { "Logging and verification", "Loggning och verifiering", "Logning og verificering", "Logging og verifisering" },
        ["cat.Optional extras"] = new[] { "Optional extras", "Valfria tillägg", "Valgfrie tilføjelser", "Valgfrie tillegg" },

        // ---------- Dashboard category subtitles ----------
        ["catsub.Antivirus exclusions"] = new[]
        {
            "Windows Defender exclusions configured",
            "Windows Defender-undantag konfigurerade",
            "Windows Defender-undtagelser konfigureret",
            "Windows Defender-unntak konfigurert",
        },
        ["catsub.Storage optimization"] = new[]
        {
            "Drives verified and optimized for recording",
            "Diskar verifierade och optimerade för inspelning",
            "Drev verificeret og optimeret til optagelse",
            "Disker verifisert og optimalisert for opptak",
        },
        ["catsub.Performance tuning"] = new[]
        {
            "System and power settings optimized",
            "System- och energiinställningar optimerade",
            "System- og strømindstillinger optimeret",
            "System- og strøminnstillinger optimalisert",
        },
        ["catsub.NTP time server"] = new[]
        {
            "Time synchronization configured",
            "Tidssynkronisering konfigurerad",
            "Tidssynkronisering konfigureret",
            "Tidssynkronisering konfigurert",
        },
        ["catsub.Remote Desktop"] = new[]
        {
            "Remote access configured and enabled",
            "Fjärråtkomst konfigurerad och aktiverad",
            "Fjernadgang konfigureret og aktiveret",
            "Fjerntilgang konfigurert og aktivert",
        },
        ["catsub.Windows cleanup"] = new[]
        {
            "Unnecessary features and files removed",
            "Onödiga funktioner och filer borttagna",
            "Unødvendige funktioner og filer fjernet",
            "Unødvendige funksjoner og filer fjernet",
        },
        ["catsub.MilestonePSTools"] = new[]
        {
            "MilestonePSTools installed and configured",
            "MilestonePSTools installerat och konfigurerat",
            "MilestonePSTools installeret og konfigureret",
            "MilestonePSTools installert og konfigurert",
        },
        ["catsub.Logging and verification"] = new[]
        {
            "System validated and log created",
            "Systemet validerat och logg skapad",
            "Systemet valideret og log oprettet",
            "Systemet validert og logg opprettet",
        },

        // ---------- System page ----------
        ["sys.subtitle"] = new[]
        {
            "Hardware, operating system and drive details for this machine.",
            "Maskinvara, operativsystem och diskdetaljer för den här maskinen.",
            "Hardware, operativsystem og drevdetaljer for denne maskine.",
            "Maskinvare, operativsystem og diskdetaljer for denne maskinen.",
        },
        ["sys.details"] = new[] { "System details", "Systemdetaljer", "Systemdetaljer", "Systemdetaljer" },
        ["sys.os"] = new[] { "Operating system", "Operativsystem", "Operativsystem", "Operativsystem" },
        ["sys.version"] = new[] { "Version", "Version", "Version", "Versjon" },
        ["sys.cpu"] = new[] { "CPU", "CPU", "CPU", "CPU" },
        ["sys.memory"] = new[] { "Memory", "Minne", "Hukommelse", "Minne" },
        ["sys.domain"] = new[] { "Domain", "Domän", "Domæne", "Domene" },
        ["sys.xprotect"] = new[] { "XProtect", "XProtect", "XProtect", "XProtect" },
        ["sys.drives"] = new[] { "Drives", "Diskar", "Drev", "Disker" },
        ["sys.drive"] = new[] { "Drive", "Disk", "Drev", "Disk" },
        ["sys.label"] = new[] { "Label", "Etikett", "Etiket", "Etikett" },
        ["sys.size"] = new[] { "Size", "Storlek", "Størrelse", "Størrelse" },
        ["sys.free"] = new[] { "Free", "Ledigt", "Ledig", "Ledig" },
        ["sys.blocksize"] = new[] { "Block size", "Blockstorlek", "Blokstørrelse", "Blokkstørrelse" },
        ["sys.indexing"] = new[] { "Indexing", "Indexering", "Indeksering", "Indeksering" },
        ["sys.detectingDrives"] = new[] { "Detecting drives…", "Identifierar diskar…", "Registrerer drev…", "Registrerer disker…" },
        ["sys.noDrives"] = new[] { "No drives detected.", "Inga diskar hittades.", "Ingen drev fundet.", "Ingen disker funnet." },
        ["sys.domainJoined"] = new[] { "Domain joined", "Domänansluten", "Domænetilsluttet", "Domenetilknyttet" },
        ["sys.workgroup"] = new[] { "Workgroup", "Arbetsgrupp", "Arbejdsgruppe", "Arbeidsgruppe" },
        ["sys.notInstalledReady"] = new[]
        {
            "Not installed (ready to prepare)",
            "Inte installerat (redo att förberedas)",
            "Ikke installeret (klar til forberedelse)",
            "Ikke installert (klar til forberedelse)",
        },
        ["sys.systemTag"] = new[] { "System", "System", "System", "System" },
        ["sys.on"] = new[] { "On", "På", "Til", "På" },
        ["sys.off"] = new[] { "Off", "Av", "Fra", "Av" },

        // ---------- Preparation page ----------
        ["prep.subtitle"] = new[]
        {
            "Select the tasks to include, then run. Every task checks first and only corrects what is missing.",
            "Välj vilka uppgifter som ska ingå och kör sedan. Varje uppgift kontrollerar först och åtgärdar bara det som saknas.",
            "Vælg hvilke opgaver der skal med, og kør derefter. Hver opgave kontrollerer først og retter kun det, der mangler.",
            "Velg hvilke oppgaver som skal med, og kjør deretter. Hver oppgave kontrollerer først og retter bare det som mangler.",
        },
        ["prep.checkonly"] = new[]
        {
            "Check only (no changes)",
            "Kontrollera endast (inga ändringar)",
            "Kontrollér kun (ingen ændringer)",
            "Kun kontroll (ingen endringer)",
        },

        // ---------- Tools page ----------
        ["tools.subtitle"] = new[]
        {
            "Standalone utilities for XProtect servers.",
            "Fristående verktyg för XProtect-servrar.",
            "Selvstændige værktøjer til XProtect-servere.",
            "Frittstående verktøy for XProtect-servere.",
        },
        ["tools.ntp.title"] = new[] { "NTP Recovery", "NTP-återställning", "NTP-gendannelse", "NTP-gjenoppretting" },
        ["tools.ntp.desc"] = new[]
        {
            "Detects and fixes the Windows time service polling bug that lets server clocks drift.",
            "Hittar och åtgärdar buggen i Windows tidstjänst som gör att serverklockor kan driva.",
            "Finder og retter fejlen i Windows' tidstjeneste, der lader serverure drive.",
            "Finner og retter feilen i Windows' tidstjeneste som lar serverklokker drive.",
        },
        ["tools.ntp.apply"] = new[] { "Apply fix", "Åtgärda", "Anvend rettelse", "Utfør retting" },
        ["tools.ntp.recheck"] = new[] { "Re-check", "Kontrollera igen", "Kontrollér igen", "Kontroller på nytt" },
        ["tools.ntp.checkingCfg"] = new[]
        {
            "Checking NTP configuration…",
            "Kontrollerar NTP-konfigurationen…",
            "Kontrollerer NTP-konfigurationen…",
            "Kontrollerer NTP-konfigurasjonen…",
        },
        ["tools.ntp.bugDetectedFmt"] = new[]
        {
            "Bug detected — current value: {0}",
            "Bugg upptäckt — aktuellt värde: {0}",
            "Fejl fundet — aktuel værdi: {0}",
            "Feil funnet — gjeldende verdi: {0}",
        },
        ["tools.ntp.okFmt"] = new[] { "OK — {0}", "OK — {0}", "OK — {0}", "OK — {0}" },
        ["tools.ntp.applying"] = new[] { "Applying fix…", "Åtgärdar…", "Anvender rettelse…", "Utfører retting…" },
        ["tools.ntp.fixNotApplied"] = new[]
        {
            "The fix could not be applied — see the Logs page.",
            "Åtgärden kunde inte genomföras — se sidan Loggar.",
            "Rettelsen kunne ikke anvendes — se siden Logfiler.",
            "Rettingen kunne ikke utføres — se siden Logger.",
        },
        ["tools.ntp.fixFailedFmt"] = new[]
        {
            "Fix failed: {0}",
            "Åtgärden misslyckades: {0}",
            "Rettelsen mislykkedes: {0}",
            "Rettingen mislyktes: {0}",
        },
        ["tools.checkFailedFmt"] = new[]
        {
            "Check failed: {0}",
            "Kontrollen misslyckades: {0}",
            "Kontrollen mislykkedes: {0}",
            "Kontrollen mislyktes: {0}",
        },
        ["tools.wu.title"] = new[] { "Windows Update", "Windows Update", "Windows Update", "Windows Update" },
        ["tools.wu.desc"] = new[]
        {
            "Turn Windows Update on or off. Keep it off on recording servers during business hours - drain updates before go-live instead.",
            "Slå på eller av Windows Update. Håll det avstängt på inspelningsservrar under arbetstid – dränera uppdateringar före driftsättning i stället.",
            "Slå Windows Update til eller fra. Hold det slået fra på optagelsesservere i arbejdstiden – træk i stedet opdateringerne inden idriftsættelse.",
            "Slå Windows Update på eller av. Hold det avslått på opptaksservere i arbeidstiden – tøm heller oppdateringene før idriftsettelse.",
        },
        ["tools.wu.enable"] = new[] { "Enable", "Aktivera", "Aktivér", "Aktiver" },
        ["tools.wu.disable"] = new[] { "Disable", "Inaktivera", "Deaktivér", "Deaktiver" },
        ["tools.wu.refresh"] = new[] { "Refresh", "Uppdatera", "Opdatér", "Oppdater" },
        ["tools.wu.checkingState"] = new[]
        {
            "Checking Windows Update state…",
            "Kontrollerar Windows Update-status…",
            "Kontrollerer Windows Update-status…",
            "Kontrollerer Windows Update-status…",
        },
        ["tools.wu.enabled"] = new[]
        {
            "Windows Update is ENABLED.",
            "Windows Update är AKTIVERAT.",
            "Windows Update er AKTIVERET.",
            "Windows Update er AKTIVERT.",
        },
        ["tools.wu.disabled"] = new[]
        {
            "Windows Update is DISABLED.",
            "Windows Update är INAKTIVERAT.",
            "Windows Update er DEAKTIVERET.",
            "Windows Update er DEAKTIVERT.",
        },
        ["tools.wu.enabling"] = new[]
        {
            "Enabling Windows Update…",
            "Aktiverar Windows Update…",
            "Aktiverer Windows Update…",
            "Aktiverer Windows Update…",
        },
        ["tools.wu.disabling"] = new[]
        {
            "Disabling Windows Update…",
            "Inaktiverar Windows Update…",
            "Deaktiverer Windows Update…",
            "Deaktiverer Windows Update…",
        },
        ["tools.wu.changeFailedFmt"] = new[]
        {
            "Change failed: {0}",
            "Ändringen misslyckades: {0}",
            "Ændringen mislykkedes: {0}",
            "Endringen mislyktes: {0}",
        },
        ["tools.pstools.title"] = new[] { "MilestonePSTools", "MilestonePSTools", "MilestonePSTools", "MilestonePSTools" },
        ["tools.pstools.desc"] = new[]
        {
            "Opens a PowerShell window with the MilestonePSTools module loaded, ready for scripting against the VMS.",
            "Öppnar ett PowerShell-fönster med MilestonePSTools-modulen laddad, redo för skript mot VMS:et.",
            "Åbner et PowerShell-vindue med MilestonePSTools-modulet indlæst, klar til scripting mod VMS'et.",
            "Åpner et PowerShell-vindu med MilestonePSTools-modulen lastet, klar for skripting mot VMS-et.",
        },
        ["tools.logfolder.title"] = new[] { "Log folder", "Loggmapp", "Logmappe", "Loggmappe" },
        ["tools.logfolder.desc"] = new[]
        {
            "All preparation logs and system reports are written to C:\\Milestonecheck-ulfh.",
            "Alla förberedelseloggar och systemrapporter skrivs till C:\\Milestonecheck-ulfh.",
            "Alle forberedelseslogfiler og systemrapporter skrives til C:\\Milestonecheck-ulfh.",
            "Alle forberedelseslogger og systemrapporter skrives til C:\\Milestonecheck-ulfh.",
        },
        ["tools.logfolder.open"] = new[] { "Open log folder", "Öppna loggmappen", "Åbn logmappen", "Åpne loggmappen" },
        ["tools.report.title"] = new[] { "System report", "Systemrapport", "Systemrapport", "Systemrapport" },
        ["tools.report.desc"] = new[]
        {
            "Runs a check-only sweep of every preparation task and writes a text report - nothing is changed. Handy as a pre-install audit or for handover documentation.",
            "Kör en ren kontroll av alla förberedelseuppgifter och skriver en textrapport – ingenting ändras. Praktiskt som granskning före installation eller som överlämningsdokumentation.",
            "Kører en ren kontrol af alle forberedelsesopgaver og skriver en tekstrapport – intet ændres. Praktisk som audit før installation eller til overdragelsesdokumentation.",
            "Kjører en ren kontroll av alle forberedelsesoppgaver og skriver en tekstrapport – ingenting endres. Praktisk som revisjon før installasjon eller som overleveringsdokumentasjon.",
        },
        ["tools.report.generate"] = new[] { "Generate system report", "Skapa systemrapport", "Generér systemrapport", "Generer systemrapport" },
        ["tools.report.running"] = new[]
        {
            "Running check-only sweep… this can take a few minutes.",
            "Kör kontrollsvep… det kan ta några minuter.",
            "Kører kontrolgennemgang… det kan tage et par minutter.",
            "Kjører kontrollgjennomgang… det kan ta noen minutter.",
        },
        ["tools.report.writtenFmt"] = new[] { "Report written: {0}", "Rapport skriven: {0}", "Rapport skrevet: {0}", "Rapport skrevet: {0}" },
        ["tools.report.failedFmt"] = new[] { "Report failed: {0}", "Rapporten misslyckades: {0}", "Rapporten mislykkedes: {0}", "Rapporten mislyktes: {0}" },

        // ---------- Logs page ----------
        ["logs.subtitle"] = new[]
        {
            "Live output from the current session and previous preparation logs.",
            "Direktutdata från den aktuella sessionen och tidigare förberedelseloggar.",
            "Live-output fra den aktuelle session og tidligere forberedelseslogfiler.",
            "Direkte utdata fra gjeldende økt og tidligere forberedelseslogger.",
        },
        ["logs.autoscroll"] = new[] { "Autoscroll", "Rulla automatiskt", "Rul automatisk", "Rull automatisk" },
        ["logs.previous"] = new[]
        {
            "Previous logs (double-click to open)",
            "Tidigare loggar (dubbelklicka för att öppna)",
            "Tidligere logfiler (dobbeltklik for at åbne)",
            "Tidligere logger (dobbeltklikk for å åpne)",
        },

        // ---------- Settings page ----------
        ["settings.subtitle"] = new[]
        {
            "Saved automatically. Applies to both the UI and silent runs.",
            "Sparas automatiskt. Gäller både gränssnittet och tysta körningar.",
            "Gemmes automatisk. Gælder både brugerfladen og tavse kørsler.",
            "Lagres automatisk. Gjelder både grensesnittet og stille kjøringer.",
        },
        ["settings.lang.title"] = new[] { "Language", "Språk", "Sprog", "Språk" },
        ["settings.lang.desc"] = new[]
        {
            "Display language for the app.",
            "Visningsspråk för appen.",
            "Visningssprog for appen.",
            "Visningsspråk for appen.",
        },
        ["settings.lang.auto"] = new[]
        {
            "System default (Windows)",
            "Systemstandard (Windows)",
            "Systemstandard (Windows)",
            "Systemstandard (Windows)",
        },
        ["settings.lang.restartHint"] = new[]
        {
            "Restart the app to apply the new language.",
            "Starta om appen för att byta språk.",
            "Genstart appen for at anvende det nye sprog.",
            "Start appen på nytt for å ta i bruk det nye språket.",
        },
        ["settings.av.title"] = new[]
        {
            "Antivirus whole-drive exclusions",
            "Antivirusundantag för hela diskar",
            "Antivirusundtagelser for hele drev",
            "Antivirusunntak for hele disker",
        },
        ["settings.av.desc"] = new[]
        {
            "Which drives get a whole-drive Windows Defender exclusion for recording storage.",
            "Vilka diskar som får ett Windows Defender-undantag för hela disken som inspelningslagring.",
            "Hvilke drev der får en Windows Defender-undtagelse for hele drevet som optagelseslager.",
            "Hvilke disker som får et Windows Defender-unntak for hele disken som opptakslagring.",
        },
        ["settings.av.storage"] = new[]
        {
            "All storage drives (recommended)",
            "Alla lagringsdiskar (rekommenderas)",
            "Alle lagerdrev (anbefales)",
            "Alle lagringsdisker (anbefales)",
        },
        ["settings.av.storageDesc"] = new[]
        {
            "Every non-OS drive is excluded - the normal choice for recording servers.",
            "Alla diskar utom systemdisken undantas – normalvalet för inspelningsservrar.",
            "Alle drev undtagen systemdrevet undtages – det normale valg til optagelsesservere.",
            "Alle disker unntatt systemdisken unntas – normalvalget for opptaksservere.",
        },
        ["settings.av.none"] = new[]
        {
            "Only Milestone folders (no whole-drive)",
            "Endast Milestone-mappar (inga hela diskar)",
            "Kun Milestone-mapper (ingen hele drev)",
            "Kun Milestone-mapper (ingen hele disker)",
        },
        ["settings.av.noneDesc"] = new[]
        {
            "Only the Milestone program and data folders are excluded.",
            "Endast Milestones program- och datamappar undantas.",
            "Kun Milestones program- og datamapper undtages.",
            "Kun Milestones program- og datamapper unntas.",
        },
        ["settings.av.specific"] = new[] { "Specific drives:", "Specifika diskar:", "Bestemte drev:", "Bestemte disker:" },
        ["settings.av.specificHint"] = new[]
        {
            "Comma-separated drive letters, e.g. D,E",
            "Kommaseparerade enhetsbokstäver, t.ex. D,E",
            "Kommaseparerede drevbogstaver, f.eks. D,E",
            "Kommaseparerte stasjonsbokstaver, f.eks. D,E",
        },
        ["settings.ntp.title"] = new[]
        {
            "NTP upstream time source",
            "Överordnad NTP-tidskälla",
            "Overordnet NTP-tidskilde",
            "Overordnet NTP-tidskilde",
        },
        ["settings.ntp.desc"] = new[]
        {
            "The time source this machine syncs from while acting as NTP server for your cameras. Pick the pool closest to the site.",
            "Tidskällan som den här maskinen synkroniserar från medan den agerar NTP-server för dina kameror. Välj poolen närmast anläggningen.",
            "Tidskilden som denne maskine synkroniserer fra, mens den fungerer som NTP-server for dine kameraer. Vælg poolen tættest på anlægget.",
            "Tidskilden denne maskinen synkroniserer fra mens den fungerer som NTP-server for kameraene dine. Velg poolen nærmest anlegget.",
        },
        ["settings.ntp.peersFmt"] = new[] { "Peers: {0}", "Peers: {0}", "Peers: {0}", "Peers: {0}" },
        ["settings.options"] = new[] { "Options", "Alternativ", "Valgmuligheder", "Alternativer" },
        ["settings.ssh"] = new[]
        {
            "Include SSH server (optional task)",
            "Inkludera SSH-server (valfri uppgift)",
            "Inkludér SSH-server (valgfri opgave)",
            "Inkluder SSH-server (valgfri oppgave)",
        },
        ["settings.sshDesc"] = new[]
        {
            "Installs and enables OpenSSH Server for remote administration.",
            "Installerar och aktiverar OpenSSH Server för fjärradministration.",
            "Installerer og aktiverer OpenSSH Server til fjernadministration.",
            "Installerer og aktiverer OpenSSH Server for fjernadministrasjon.",
        },
        ["settings.snmp"] = new[]
        {
            "Include SNMP (optional task)",
            "Inkludera SNMP (valfri uppgift)",
            "Inkludér SNMP (valgfri opgave)",
            "Inkluder SNMP (valgfri oppgave)",
        },
        ["settings.snmpDesc"] = new[]
        {
            "Installs the SNMP service for monitoring systems.",
            "Installerar SNMP-tjänsten för övervakningssystem.",
            "Installerer SNMP-tjenesten til overvågningssystemer.",
            "Installerer SNMP-tjenesten for overvåkingssystemer.",
        },
        ["settings.restartExplorer"] = new[]
        {
            "Restart Explorer after clock-seconds change",
            "Starta om Utforskaren efter ändring av sekunder i klockan",
            "Genstart Stifinder efter ændring af sekunder i uret",
            "Start Utforsker på nytt etter endring av sekunder i klokken",
        },
        ["settings.restartExplorerDesc"] = new[]
        {
            "Showing seconds in the system clock requires an Explorer restart to take effect immediately.",
            "Att visa sekunder i systemklockan kräver en omstart av Utforskaren för att slå igenom direkt.",
            "Visning af sekunder i systemuret kræver en genstart af Stifinder for at slå igennem med det samme.",
            "Visning av sekunder i systemklokken krever en omstart av Utforsker for å tre i kraft umiddelbart.",
        },

        // ---------- About page ----------
        ["about.version"] = new[] { "Version", "Version", "Version", "Versjon" },
        ["about.builtBy"] = new[]
        {
            "Built by Ulf Holmström, Happy Problem Solver at Manvarg AB, Sweden",
            "Byggd av Ulf Holmström, Happy Problem Solver på Manvarg AB, Sverige",
            "Bygget af Ulf Holmström, Happy Problem Solver hos Manvarg AB, Sverige",
            "Bygget av Ulf Holmström, Happy Problem Solver hos Manvarg AB, Sverige",
        },
        ["about.tagline"] = new[]
        {
            "Prepares Windows for XProtect installations by Milestone best practice — check first, fix only what's needed.",
            "Förbereder Windows för XProtect-installationer enligt Milestones best practice — kontrollera först, åtgärda bara det som behövs.",
            "Forbereder Windows til XProtect-installationer efter Milestones best practice — kontrollér først, ret kun det nødvendige.",
            "Forbereder Windows for XProtect-installasjoner etter Milestones beste praksis — kontroller først, rett bare det som trengs.",
        },
        ["about.disclaimer"] = new[]
        {
            "This tool is NOT developed by, endorsed by, or affiliated with Milestone Systems A/S. It is a personal project by Ulf Holmström to make life easier for technicians who work with Milestone XProtect.",
            "Det här verktyget är INTE utvecklat av, godkänt av eller anslutet till Milestone Systems A/S. Det är ett personligt projekt av Ulf Holmström för att göra livet enklare för tekniker som arbetar med Milestone XProtect.",
            "Dette værktøj er IKKE udviklet af, godkendt af eller tilknyttet Milestone Systems A/S. Det er et personligt projekt af Ulf Holmström for at gøre livet lettere for teknikere, der arbejder med Milestone XProtect.",
            "Dette verktøyet er IKKE utviklet av, godkjent av eller tilknyttet Milestone Systems A/S. Det er et personlig prosjekt av Ulf Holmström for å gjøre livet enklere for teknikere som jobber med Milestone XProtect.",
        },
        ["about.license"] = new[]
        {
            "Released under the MIT License.",
            "Släppt under MIT-licensen.",
            "Udgivet under MIT-licensen.",
            "Utgitt under MIT-lisensen.",
        },

        // ---------- NTP source display names (engine keys from NtpCore.Sources) ----------
        ["ntp.windows"] = new[]
        {
            "Windows default (time.windows.com)",
            "Windows-standard (time.windows.com)",
            "Windows-standard (time.windows.com)",
            "Windows-standard (time.windows.com)",
        },
        ["ntp.global"] = new[]
        {
            "Global pool (pool.ntp.org — picks nearest)",
            "Global pool (pool.ntp.org — väljer närmaste)",
            "Global pool (pool.ntp.org — vælger nærmeste)",
            "Global pool (pool.ntp.org — velger nærmeste)",
        },
        ["ntp.ar"] = new[] { "Argentina", "Argentina", "Argentina", "Argentina" },
        ["ntp.au"] = new[] { "Australia", "Australien", "Australien", "Australia" },
        ["ntp.at"] = new[] { "Austria", "Österrike", "Østrig", "Østerrike" },
        ["ntp.be"] = new[] { "Belgium", "Belgien", "Belgien", "Belgia" },
        ["ntp.br"] = new[] { "Brazil", "Brasilien", "Brasilien", "Brasil" },
        ["ntp.ca"] = new[] { "Canada", "Kanada", "Canada", "Canada" },
        ["ntp.cl"] = new[] { "Chile", "Chile", "Chile", "Chile" },
        ["ntp.cn"] = new[] { "China", "Kina", "Kina", "Kina" },
        ["ntp.cz"] = new[] { "Czechia", "Tjeckien", "Tjekkiet", "Tsjekkia" },
        ["ntp.dk"] = new[] { "Denmark", "Danmark", "Danmark", "Danmark" },
        ["ntp.fi"] = new[] { "Finland", "Finland", "Finland", "Finland" },
        ["ntp.fr"] = new[] { "France", "Frankrike", "Frankrig", "Frankrike" },
        ["ntp.de"] = new[] { "Germany", "Tyskland", "Tyskland", "Tyskland" },
        ["ntp.hk"] = new[] { "Hong Kong", "Hongkong", "Hongkong", "Hongkong" },
        ["ntp.in"] = new[] { "India", "Indien", "Indien", "India" },
        ["ntp.ie"] = new[] { "Ireland", "Irland", "Irland", "Irland" },
        ["ntp.it"] = new[] { "Italy", "Italien", "Italien", "Italia" },
        ["ntp.jp"] = new[] { "Japan", "Japan", "Japan", "Japan" },
        ["ntp.my"] = new[] { "Malaysia", "Malaysia", "Malaysia", "Malaysia" },
        ["ntp.mx"] = new[] { "Mexico", "Mexiko", "Mexico", "Mexico" },
        ["ntp.nl"] = new[] { "Netherlands", "Nederländerna", "Nederlandene", "Nederland" },
        ["ntp.nz"] = new[] { "New Zealand", "Nya Zeeland", "New Zealand", "New Zealand" },
        ["ntp.no"] = new[] { "Norway", "Norge", "Norge", "Norge" },
        ["ntp.pl"] = new[] { "Poland", "Polen", "Polen", "Polen" },
        ["ntp.pt"] = new[] { "Portugal", "Portugal", "Portugal", "Portugal" },
        ["ntp.sg"] = new[] { "Singapore", "Singapore", "Singapore", "Singapore" },
        ["ntp.za"] = new[] { "South Africa", "Sydafrika", "Sydafrika", "Sør-Afrika" },
        ["ntp.kr"] = new[] { "South Korea", "Sydkorea", "Sydkorea", "Sør-Korea" },
        ["ntp.es"] = new[] { "Spain", "Spanien", "Spanien", "Spania" },
        ["ntp.se"] = new[] { "Sweden", "Sverige", "Sverige", "Sverige" },
        ["ntp.ch"] = new[] { "Switzerland", "Schweiz", "Schweiz", "Sveits" },
        ["ntp.tw"] = new[] { "Taiwan", "Taiwan", "Taiwan", "Taiwan" },
        ["ntp.th"] = new[] { "Thailand", "Thailand", "Thailand", "Thailand" },
        ["ntp.tr"] = new[] { "Turkey", "Turkiet", "Tyrkiet", "Tyrkia" },
        ["ntp.ae"] = new[]
        {
            "United Arab Emirates",
            "Förenade Arabemiraten",
            "De Forenede Arabiske Emirater",
            "De forente arabiske emirater",
        },
        ["ntp.uk"] = new[] { "United Kingdom", "Storbritannien", "Storbritannien", "Storbritannia" },
        ["ntp.us"] = new[] { "United States", "USA", "USA", "USA" },

        // ---------- Task names and descriptions (English = engine strings verbatim) ----------
        ["task.av-milestone-folders.name"] = new[]
        {
            "Milestone folder exclusions",
            "Undantag för Milestone-mappar",
            "Undtagelser for Milestone-mapper",
            "Unntak for Milestone-mapper",
        },
        ["task.av-milestone-folders.desc"] = new[]
        {
            "Defender path exclusions for the five Milestone program and data folders on C:",
            "Defender-sökvägsundantag för de fem Milestone-program- och datamapparna på C:",
            "Defender-stiundtagelser for de fem Milestone-program- og datamapper på C:",
            "Defender-baneunntak for de fem Milestone-program- og datamappene på C:",
        },
        ["task.av-extensions.name"] = new[]
        {
            "Media file extension exclusions",
            "Undantag för mediefiländelser",
            "Undtagelser for mediefiltyper",
            "Unntak for mediefiltyper",
        },
        ["task.av-extensions.desc"] = new[]
        {
            "Defender exclusions for XProtect media database extensions (blk, idx, pic, pqz, sts, ts)",
            "Defender-undantag för XProtects mediedatabas-filändelser (blk, idx, pic, pqz, sts, ts)",
            "Defender-undtagelser for XProtects mediedatabase-filtyper (blk, idx, pic, pqz, sts, ts)",
            "Defender-unntak for XProtects mediedatabase-filtyper (blk, idx, pic, pqz, sts, ts)",
        },
        ["task.av-processes.name"] = new[]
        {
            "VideoOS process exclusions",
            "Undantag för VideoOS-processer",
            "Undtagelser for VideoOS-processer",
            "Unntak for VideoOS-prosesser",
        },
        ["task.av-processes.desc"] = new[]
        {
            "Defender process exclusions for the VideoOS services, VictoriaLogs (2026 R1) and legacy Log Server",
            "Defender-processundantag för VideoOS-tjänsterna, VictoriaLogs (2026 R1) och äldre Log Server",
            "Defender-procesundtagelser for VideoOS-tjenesterne, VictoriaLogs (2026 R1) og ældre Log Server",
            "Defender-prosessunntak for VideoOS-tjenestene, VictoriaLogs (2026 R1) og eldre Log Server",
        },
        ["task.av-smartclient.name"] = new[]
        {
            "Smart Client exclusions",
            "Undantag för Smart Client",
            "Undtagelser for Smart Client",
            "Unntak for Smart Client",
        },
        ["task.av-smartclient.desc"] = new[]
        {
            "Defender exclusions for XProtect Smart Client and its per-user cache (only when installed)",
            "Defender-undantag för XProtect Smart Client och dess cache per användare (endast om installerad)",
            "Defender-undtagelser for XProtect Smart Client og dens cache pr. bruger (kun hvis installeret)",
            "Defender-unntak for XProtect Smart Client og dens cache per bruker (kun hvis installert)",
        },
        ["task.av-storage-drives.name"] = new[]
        {
            "Storage drive exclusions",
            "Undantag för lagringsdiskar",
            "Undtagelser for lagerdrev",
            "Unntak for lagringsdisker",
        },
        ["task.av-storage-drives.desc"] = new[]
        {
            "Defender path exclusions for whole recording storage drives (C: always excluded)",
            "Defender-sökvägsundantag för hela inspelningsdiskar (C: undantas alltid)",
            "Defender-stiundtagelser for hele optagelsesdrev (C: undtages altid)",
            "Defender-baneunntak for hele opptaksdisker (C: unntas alltid)",
        },
        ["task.storage-blocksize.name"] = new[]
        {
            "64 KB allocation unit check",
            "Kontroll av 64 KB allokeringsenhet",
            "Kontrol af 64 KB allokeringsenhed",
            "Kontroll av 64 KB allokeringsenhet",
        },
        ["task.storage-blocksize.desc"] = new[]
        {
            "Verifies that storage drives are formatted with 64 KB NTFS clusters (check only)",
            "Verifierar att lagringsdiskarna är formaterade med 64 KB NTFS-kluster (endast kontroll)",
            "Verificerer at lagerdrev er formateret med 64 KB NTFS-klynger (kun kontrol)",
            "Verifiserer at lagringsdiskene er formatert med 64 KB NTFS-klynger (kun kontroll)",
        },
        ["task.storage-indexing.name"] = new[]
        {
            "Disable indexing on storage drives",
            "Inaktivera indexering på lagringsdiskar",
            "Deaktivér indeksering på lagerdrev",
            "Deaktiver indeksering på lagringsdisker",
        },
        ["task.storage-indexing.desc"] = new[]
        {
            "Turns off Windows Search indexing on all non-OS fixed drives",
            "Stänger av Windows Search-indexering på alla fasta diskar utom systemdisken",
            "Slår Windows Search-indeksering fra på alle faste drev undtagen systemdrevet",
            "Slår av Windows Search-indeksering på alle faste disker unntatt systemdisken",
        },
        ["task.perf-powerplan.name"] = new[]
        {
            "Power plan: High Performance, never sleep",
            "Energischema: Hög prestanda, aldrig viloläge",
            "Strømstyringsplan: Høj ydeevne, aldrig slumre",
            "Strømplan: Høy ytelse, aldri hvilemodus",
        },
        ["task.perf-powerplan.desc"] = new[]
        {
            "High Performance plan with sleep, display and disk timeouts off (critical for 24/7 recording)",
            "Hög prestanda-schema med viloläge, skärm- och disktimeout avstängda (kritiskt för 24/7-inspelning)",
            "Høj ydeevne-plan med slumre-, skærm- og disk-timeout slået fra (kritisk for 24/7-optagelse)",
            "Høy ytelse-plan med hvile-, skjerm- og disk-timeout slått av (kritisk for 24/7-opptak)",
        },
        ["task.perf-services.name"] = new[]
        {
            "Disable unnecessary services",
            "Inaktivera onödiga tjänster",
            "Deaktivér unødvendige tjenester",
            "Deaktiver unødvendige tjenester",
        },
        ["task.perf-services.desc"] = new[]
        {
            "Stops and disables Fax, Windows Search, Print Spooler, SysMain, HomeGroup, Themes and Tablet Input",
            "Stoppar och inaktiverar Fax, Windows Search, Print Spooler, SysMain, HomeGroup, Themes och Tablet Input",
            "Stopper og deaktiverer Fax, Windows Search, Print Spooler, SysMain, HomeGroup, Themes og Tablet Input",
            "Stopper og deaktiverer Fax, Windows Search, Print Spooler, SysMain, HomeGroup, Themes og Tablet Input",
        },
        ["task.perf-largesystemcache.name"] = new[]
        {
            "LargeSystemCache",
            "LargeSystemCache",
            "LargeSystemCache",
            "LargeSystemCache",
        },
        ["task.perf-largesystemcache.desc"] = new[]
        {
            "Enables LargeSystemCache for file server style workloads (video recording)",
            "Aktiverar LargeSystemCache för filserverliknande arbetslaster (videoinspelning)",
            "Aktiverer LargeSystemCache til filserverlignende arbejdsbelastninger (videooptagelse)",
            "Aktiverer LargeSystemCache for filserverlignende arbeidsbelastninger (videoopptak)",
        },
        ["task.rdp.name"] = new[]
        {
            "Enable and optimize Remote Desktop",
            "Aktivera och optimera Fjärrskrivbord",
            "Aktivér og optimér Fjernskrivebord",
            "Aktiver og optimaliser Eksternt skrivebord",
        },
        ["task.rdp.desc"] = new[]
        {
            "Enables RDP, opens its firewall group and turns off wallpaper/animations over RDP",
            "Aktiverar RDP, öppnar dess brandväggsgrupp och stänger av skrivbordsbakgrund/animeringar över RDP",
            "Aktiverer RDP, åbner dets firewallgruppe og slår baggrund/animationer fra over RDP",
            "Aktiverer RDP, åpner brannmurgruppen og slår av bakgrunn/animasjoner over RDP",
        },
        ["task.cleanup-bloatware.name"] = new[]
        {
            "Remove bloatware apps",
            "Ta bort bloatware-appar",
            "Fjern bloatware-apps",
            "Fjern bloatware-apper",
        },
        ["task.cleanup-bloatware.desc"] = new[]
        {
            "Removes 36 preinstalled consumer apps (installed and provisioned). Xbox apps untouched",
            "Tar bort 36 förinstallerade konsumentappar (installerade och provisionerade). Xbox-appar rörs inte",
            "Fjerner 36 forudinstallerede forbruger-apps (installerede og provisionerede). Xbox-apps røres ikke",
            "Fjerner 36 forhåndsinstallerte forbruker-apper (installerte og provisjonerte). Xbox-apper røres ikke",
        },
        ["task.cleanup-onedrive.name"] = new[]
        {
            "Remove OneDrive",
            "Ta bort OneDrive",
            "Fjern OneDrive",
            "Fjern OneDrive",
        },
        ["task.cleanup-onedrive.desc"] = new[]
        {
            "Uninstalls OneDrive and deletes its leftover folders",
            "Avinstallerar OneDrive och tar bort kvarlämnade mappar",
            "Afinstallerer OneDrive og sletter efterladte mapper",
            "Avinstallerer OneDrive og sletter gjenværende mapper",
        },
        ["task.cleanup-teams.name"] = new[]
        {
            "Remove Microsoft Teams",
            "Ta bort Microsoft Teams",
            "Fjern Microsoft Teams",
            "Fjern Microsoft Teams",
        },
        ["task.cleanup-teams.desc"] = new[]
        {
            "Removes new (AppX) and classic Teams plus leftover folders",
            "Tar bort nya (AppX) och klassiska Teams samt kvarlämnade mappar",
            "Fjerner nye (AppX) og klassiske Teams samt efterladte mapper",
            "Fjerner nye (AppX) og klassiske Teams samt gjenværende mapper",
        },
        ["task.cleanup-outlook-officehub.name"] = new[]
        {
            "Remove Outlook App and Office Hub",
            "Ta bort Outlook-appen och Office Hub",
            "Fjern Outlook-appen og Office Hub",
            "Fjern Outlook-appen og Office Hub",
        },
        ["task.cleanup-outlook-officehub.desc"] = new[]
        {
            "Removes the new Outlook app and the Office Hub/365 installer (installed and provisioned)",
            "Tar bort nya Outlook-appen och Office Hub/365-installeraren (installerade och provisionerade)",
            "Fjerner den nye Outlook-app og Office Hub/365-installationsprogrammet (installeret og provisioneret)",
            "Fjerner den nye Outlook-appen og Office Hub/365-installasjonsprogrammet (installert og provisjonert)",
        },
        ["task.cleanup-copilot-recall.name"] = new[]
        {
            "Disable Copilot and Recall",
            "Inaktivera Copilot och Recall",
            "Deaktivér Copilot og Recall",
            "Deaktiver Copilot og Recall",
        },
        ["task.cleanup-copilot-recall.desc"] = new[]
        {
            "Group Policy: TurnOffWindowsCopilot=1 and DisableAIDataAnalysis=1",
            "Grupprincip: TurnOffWindowsCopilot=1 och DisableAIDataAnalysis=1",
            "Gruppepolitik: TurnOffWindowsCopilot=1 og DisableAIDataAnalysis=1",
            "Gruppepolicy: TurnOffWindowsCopilot=1 og DisableAIDataAnalysis=1",
        },
        ["task.cleanup-telemetry.name"] = new[]
        {
            "Disable telemetry scheduled tasks",
            "Inaktivera schemalagda telemetriuppgifter",
            "Deaktivér planlagte telemetriopgaver",
            "Deaktiver planlagte telemetrioppgaver",
        },
        ["task.cleanup-telemetry.desc"] = new[]
        {
            "Disables the four CEIP/compatibility/disk telemetry scheduled tasks",
            "Inaktiverar de fyra schemalagda uppgifterna för CEIP-/kompatibilitets-/disktelemetri",
            "Deaktiverer de fire planlagte opgaver for CEIP-/kompatibilitets-/disktelemetri",
            "Deaktiverer de fire planlagte oppgavene for CEIP-/kompatibilitets-/disktelemetri",
        },
        ["task.ntp-server.name"] = new[]
        {
            "NTP time server for cameras",
            "NTP-tidsserver för kameror",
            "NTP-tidsserver til kameraer",
            "NTP-tidsserver for kameraer",
        },
        ["task.ntp-server.desc"] = new[]
        {
            "Windows Time as reliable NTP server (upstream source selectable in Settings, UDP 123 open, seconds in taskbar clock)",
            "Windows Time som tillförlitlig NTP-server (överordnad källa väljs i Inställningar, UDP 123 öppen, sekunder i aktivitetsfältets klocka)",
            "Windows Time som pålidelig NTP-server (overordnet kilde vælges i Indstillinger, UDP 123 åben, sekunder i proceslinjens ur)",
            "Windows Time som pålitelig NTP-server (overordnet kilde velges i Innstillinger, UDP 123 åpen, sekunder i oppgavelinjens klokke)",
        },
        ["task.pstools.name"] = new[]
        {
            "MilestonePSTools module",
            "MilestonePSTools-modulen",
            "MilestonePSTools-modulet",
            "MilestonePSTools-modulen",
        },
        ["task.pstools.desc"] = new[]
        {
            "Installs MilestonePSTools via the official web installer (2026 R1 compatible)",
            "Installerar MilestonePSTools via den officiella webbinstalleraren (kompatibel med 2026 R1)",
            "Installerer MilestonePSTools via det officielle webinstallationsprogram (kompatibelt med 2026 R1)",
            "Installerer MilestonePSTools via det offisielle webinstallasjonsprogrammet (kompatibelt med 2026 R1)",
        },
        ["task.ssh-server.name"] = new[]
        {
            "SSH server",
            "SSH-server",
            "SSH-server",
            "SSH-server",
        },
        ["task.ssh-server.desc"] = new[]
        {
            "OpenSSH Server with password auth for admins, autostart and TCP 22 open (optional)",
            "OpenSSH Server med lösenordsinloggning för administratörer, autostart och TCP 22 öppen (valfri)",
            "OpenSSH Server med adgangskode-login for administratorer, autostart og TCP 22 åben (valgfri)",
            "OpenSSH Server med passordpålogging for administratorer, autostart og TCP 22 åpen (valgfri)",
        },
        ["task.snmp.name"] = new[]
        {
            "SNMP capabilities",
            "SNMP-funktioner",
            "SNMP-funktioner",
            "SNMP-funksjoner",
        },
        ["task.snmp.desc"] = new[]
        {
            "SNMP client and WMI SNMP provider for network monitoring systems (optional)",
            "SNMP-klient och WMI SNMP-provider för nätverksövervakningssystem (valfri)",
            "SNMP-klient og WMI SNMP-provider til netværksovervågningssystemer (valgfri)",
            "SNMP-klient og WMI SNMP-provider for nettverksovervåkingssystemer (valgfri)",
        },
    };
}
