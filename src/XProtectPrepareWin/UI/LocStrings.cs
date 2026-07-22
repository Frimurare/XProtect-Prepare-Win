namespace XProtectPrepareWin.UI;

/// <summary>
/// All UI strings, one key per entry, value = [en, sv, da, no, fi].
/// English is the source of truth; task/category English entries mirror the
/// engine strings verbatim. Engine output (CheckResult.Detail, logs) stays English.
/// </summary>
public static class LocStrings
{
    public static readonly Dictionary<string, string[]> Table = new()
    {
        // ---------- Navigation / sidebar ----------
        ["nav.dashboard"] = new[] { "Dashboard", "Översikt", "Oversigt", "Oversikt", "Yleiskatsaus" },
        ["nav.system"] = new[] { "System", "System", "System", "System", "Järjestelmä" },
        ["nav.preparation"] = new[] { "Preparation", "Förberedelse", "Forberedelse", "Forberedelse", "Valmistelu" },
        ["nav.tools"] = new[] { "Tools", "Verktyg", "Værktøjer", "Verktøy", "Työkalut" },
        ["nav.logs"] = new[] { "Logs", "Loggar", "Logfiler", "Logger", "Lokit" },
        ["nav.settings"] = new[] { "Settings", "Inställningar", "Indstillinger", "Innstillinger", "Asetukset" },
        ["nav.about"] = new[] { "About", "Om", "Om", "Om", "Tietoja" },
        ["nav.exit"] = new[] { "Exit", "Avsluta", "Afslut", "Avslutt", "Poistu" },
        ["sidebar.disclaimer"] = new[]
        {
            "Prepares Windows for XProtect\nby Milestone best practice.\nNot affiliated with Milestone Systems.",
            "Förbereder Windows för XProtect\nenligt Milestones best practice.\nInte ansluten till Milestone Systems.",
            "Forbereder Windows til XProtect\nefter Milestones best practice.\nIkke tilknyttet Milestone Systems.",
            "Forbereder Windows for XProtect\netter Milestones beste praksis.\nIkke tilknyttet Milestone Systems.",
            "Valmistelee Windowsin XProtectia varten\nMilestonen parhaiden käytäntöjen mukaan.\nEi sidoksissa Milestone Systemsiin.",
        },

        // ---------- Brand line (stays English in every language) ----------
        ["brand.headline1"] = new[] { "Prepare XProtect. ", "Prepare XProtect. ", "Prepare XProtect. ", "Prepare XProtect. ", "Prepare XProtect. " },
        ["brand.headline2"] = new[] { "Not Windows.", "Not Windows.", "Not Windows.", "Not Windows.", "Not Windows." },

        // ---------- Shared ----------
        ["common.detecting"] = new[] { "Detecting…", "Identifierar…", "Registrerer…", "Registrerer…", "Tunnistetaan…" },
        ["app.unexpectedErrorFmt"] = new[] { "Unexpected error: {0}", "Oväntat fel: {0}", "Uventet fejl: {0}", "Uventet feil: {0}", "Odottamaton virhe: {0}" },

        // ---------- Run/status texts ----------
        ["status.notrun"] = new[] { "Not run", "Inte körd", "Ikke kørt", "Ikke kjørt", "Ei suoritettu" },
        ["status.checking"] = new[] { "Checking…", "Kontrollerar…", "Kontrollerer…", "Kontrollerer…", "Tarkistetaan…" },
        ["status.applying"] = new[] { "Applying…", "Åtgärdar…", "Udfører…", "Utfører…", "Korjataan…" },
        ["status.alreadydone"] = new[] { "Already done", "Redan klart", "Allerede udført", "Allerede utført", "Jo tehty" },
        ["status.corrected"] = new[] { "Corrected now", "Åtgärdat nu", "Rettet nu", "Rettet nå", "Korjattu nyt" },
        ["status.warning"] = new[] { "Warning", "Varning", "Advarsel", "Advarsel", "Varoitus" },
        ["status.failed"] = new[] { "Failed", "Misslyckades", "Mislykkedes", "Mislyktes", "Epäonnistui" },
        ["status.skipped"] = new[] { "Skipped", "Överhoppad", "Sprunget over", "Hoppet over", "Ohitettu" },
        ["status.na"] = new[] { "N/A", "N/A", "N/A", "N/A", "N/A" },
        ["status.running"] = new[] { "Running…", "Kör…", "Kører…", "Kjører…", "Suoritetaan…" },
        ["status.completed"] = new[] { "Completed", "Klart", "Fuldført", "Fullført", "Valmis" },
        ["status.issues"] = new[] { "Issues", "Anmärkningar", "Problemer", "Problemer", "Ongelmia" },

        // ---------- Dashboard ----------
        ["dash.subtitle"] = new[]
        {
            "Automate server preparation by Milestone best practice.",
            "Automatisera serverförberedelser enligt Milestones best practice.",
            "Automatisér serverforberedelse efter Milestones best practice.",
            "Automatiser serverforberedelse etter Milestones beste praksis.",
            "Automatisoi palvelimen valmistelu Milestonen parhaiden käytäntöjen mukaan.",
        },
        ["dash.card.system"] = new[] { "System detected", "Identifierat system", "Registreret system", "Registrert system", "Tunnistettu järjestelmä" },
        ["dash.card.xp"] = new[] { "XProtect version", "XProtect-version", "XProtect-version", "XProtect-versjon", "XProtect-versio" },
        ["dash.card.lastrun"] = new[] { "Last run", "Senaste körning", "Seneste kørsel", "Siste kjøring", "Viimeisin suoritus" },
        ["dash.prepstatus"] = new[] { "Preparation status", "Förberedelsestatus", "Forberedelsesstatus", "Forberedelsesstatus", "Valmistelun tila" },
        ["dash.completed"] = new[] { "Completed", "Klart", "Fuldført", "Fullført", "Valmis" },
        ["dash.noRunYet"] = new[]
        {
            "No preparation run yet",
            "Ingen förberedelsekörning ännu",
            "Ingen forberedelseskørsel endnu",
            "Ingen forberedelseskjøring ennå",
            "Valmistelua ei ole vielä suoritettu",
        },
        ["dash.runToOptimize"] = new[]
        {
            "Run preparation to optimize this server for XProtect.",
            "Kör förberedelsen för att optimera den här servern för XProtect.",
            "Kør forberedelsen for at optimere denne server til XProtect.",
            "Kjør forberedelsen for å optimalisere denne serveren for XProtect.",
            "Suorita valmistelu optimoidaksesi tämän palvelimen XProtectia varten.",
        },
        ["dash.viewReport"] = new[] { "View preparation report", "Visa förberedelserapport", "Vis forberedelsesrapport", "Vis forberedelsesrapport", "Näytä valmisteluraportti" },
        ["dash.run"] = new[] { "Run preparation", "Kör förberedelse", "Kør forberedelse", "Kjør forberedelse", "Suorita valmistelu" },
        ["dash.runAgain"] = new[] { "Run preparation again", "Kör förberedelsen igen", "Kør forberedelsen igen", "Kjør forberedelsen igjen", "Suorita valmistelu uudelleen" },
        ["dash.exportLog"] = new[] { "Export log", "Exportera logg", "Eksportér log", "Eksporter logg", "Vie loki" },
        ["dash.openPstools"] = new[] { "Open MilestonePSTools", "Öppna MilestonePSTools", "Åbn MilestonePSTools", "Åpne MilestonePSTools", "Avaa MilestonePSTools" },
        ["dash.never"] = new[] { "Never", "Aldrig", "Aldrig", "Aldri", "Ei koskaan" },
        ["dash.durationFmt"] = new[] { "Duration: {0}", "Varaktighet: {0}", "Varighed: {0}", "Varighet: {0}", "Kesto: {0}" },
        ["dash.detectedHere"] = new[]
        {
            "Detected on this machine",
            "Identifierad på den här maskinen",
            "Fundet på denne maskine",
            "Funnet på denne maskinen",
            "Tunnistettu tällä koneella",
        },
        ["dash.notInstalled"] = new[] { "Not installed", "Inte installerat", "Ikke installeret", "Ikke installert", "Ei asennettu" },
        ["dash.readyToPrepare"] = new[] { "Ready to prepare", "Redo att förberedas", "Klar til forberedelse", "Klar til forberedelse", "Valmis valmisteltavaksi" },
        ["dash.preparing"] = new[] { "Preparing…", "Förbereder…", "Forbereder…", "Forbereder…", "Valmistellaan…" },
        ["dash.preparingSub"] = new[]
        {
            "Tasks are being checked and corrected. You can follow the details on the Preparation and Logs pages.",
            "Uppgifterna kontrolleras och åtgärdas. Du kan följa detaljerna på sidorna Förberedelse och Loggar.",
            "Opgaverne kontrolleres og rettes. Du kan følge detaljerne på siderne Forberedelse og Logfiler.",
            "Oppgavene kontrolleres og rettes. Du kan følge detaljene på sidene Forberedelse og Logger.",
            "Tehtäviä tarkistetaan ja korjataan. Voit seurata yksityiskohtia Valmistelu- ja Lokit-sivuilla.",
        },
        ["dash.systemReady"] = new[] { "🎉 System ready!", "🎉 Systemet är redo!", "🎉 Systemet er klar!", "🎉 Systemet er klart!", "🎉 Järjestelmä on valmis!" },
        ["dash.systemReadySub"] = new[]
        {
            "Your server is optimized and ready for XProtect installation.",
            "Servern är optimerad och redo för XProtect-installation.",
            "Serveren er optimeret og klar til XProtect-installation.",
            "Serveren er optimalisert og klar for XProtect-installasjon.",
            "Palvelin on optimoitu ja valmis XProtect-asennusta varten.",
        },
        ["dash.issuesTitle"] = new[] { "Completed with issues", "Klar med anmärkningar", "Fuldført med problemer", "Fullført med problemer", "Valmis, mutta ongelmia ilmeni" },
        ["dash.issuesSubFmt"] = new[]
        {
            "{0} task(s) failed — check the Preparation page and the log.",
            "{0} uppgift(er) misslyckades — se sidan Förberedelse och loggen.",
            "{0} opgave(r) mislykkedes — se siden Forberedelse og loggen.",
            "{0} oppgave(r) mislyktes — se siden Forberedelse og loggen.",
            "{0} tehtävä(ä) epäonnistui — katso Valmistelu-sivu ja loki.",
        },
        ["dash.noLogYet"] = new[]
        {
            "No preparation log exists yet. Run a preparation first.",
            "Det finns ingen förberedelselogg ännu. Kör en förberedelse först.",
            "Der findes ingen forberedelseslog endnu. Kør en forberedelse først.",
            "Det finnes ingen forberedelseslogg ennå. Kjør en forberedelse først.",
            "Valmistelulokia ei vielä ole. Suorita ensin valmistelu.",
        },
        ["dash.exportTitle"] = new[] { "Export preparation log", "Exportera förberedelselogg", "Eksportér forberedelseslog", "Eksporter forberedelseslogg", "Vie valmisteluloki" },
        ["dash.logFilter"] = new[]
        {
            "Log files (*.txt)|*.txt|All files (*.*)|*.*",
            "Loggfiler (*.txt)|*.txt|Alla filer (*.*)|*.*",
            "Logfiler (*.txt)|*.txt|Alle filer (*.*)|*.*",
            "Loggfiler (*.txt)|*.txt|Alle filer (*.*)|*.*",
            "Lokitiedostot (*.txt)|*.txt|Kaikki tiedostot (*.*)|*.*",
        },
        ["dash.exportFailedFmt"] = new[]
        {
            "Could not export the log: {0}",
            "Kunde inte exportera loggen: {0}",
            "Loggen kunne ikke eksporteres: {0}",
            "Kunne ikke eksportere loggen: {0}",
            "Lokia ei voitu viedä: {0}",
        },

        // ---------- Categories (keys match the engine strings in Tasks/Common.cs) ----------
        ["cat.Antivirus exclusions"] = new[] { "Antivirus exclusions", "Antivirusundantag", "Antivirusundtagelser", "Antivirusunntak", "Virustorjunnan poikkeukset" },
        ["cat.Storage optimization"] = new[] { "Storage optimization", "Lagringsoptimering", "Lageroptimering", "Lagringsoptimalisering", "Tallennustilan optimointi" },
        ["cat.Performance tuning"] = new[] { "Performance tuning", "Prestandaoptimering", "Ydelsesoptimering", "Ytelsesoptimalisering", "Suorituskyvyn optimointi" },
        ["cat.NTP time server"] = new[] { "NTP time server", "NTP-tidsserver", "NTP-tidsserver", "NTP-tidsserver", "NTP-aikapalvelin" },
        ["cat.Remote Desktop"] = new[] { "Remote Desktop", "Fjärrskrivbord", "Fjernskrivebord", "Eksternt skrivebord", "Etätyöpöytä" },
        ["cat.Windows cleanup"] = new[] { "Windows cleanup", "Windows-rensning", "Windows-oprydning", "Windows-opprydding", "Windowsin siivous" },
        ["cat.MilestonePSTools"] = new[] { "MilestonePSTools", "MilestonePSTools", "MilestonePSTools", "MilestonePSTools", "MilestonePSTools" },
        ["cat.Logging and verification"] = new[] { "Logging and verification", "Loggning och verifiering", "Logning og verificering", "Logging og verifisering", "Lokikirjaus ja varmennus" },
        ["cat.Optional extras"] = new[] { "Optional extras", "Valfria tillägg", "Valgfrie tilføjelser", "Valgfrie tillegg", "Valinnaiset lisätoiminnot" },

        // ---------- Dashboard category subtitles ----------
        ["catsub.Antivirus exclusions"] = new[]
        {
            "Windows Defender exclusions configured",
            "Windows Defender-undantag konfigurerade",
            "Windows Defender-undtagelser konfigureret",
            "Windows Defender-unntak konfigurert",
            "Windows Defender -poikkeukset määritetty",
        },
        ["catsub.Storage optimization"] = new[]
        {
            "Drives verified and optimized for recording",
            "Diskar verifierade och optimerade för inspelning",
            "Drev verificeret og optimeret til optagelse",
            "Disker verifisert og optimalisert for opptak",
            "Asemat tarkistettu ja optimoitu tallennusta varten",
        },
        ["catsub.Performance tuning"] = new[]
        {
            "System and power settings optimized",
            "System- och energiinställningar optimerade",
            "System- og strømindstillinger optimeret",
            "System- og strøminnstillinger optimalisert",
            "Järjestelmä- ja virta-asetukset optimoitu",
        },
        ["catsub.NTP time server"] = new[]
        {
            "Time synchronization configured",
            "Tidssynkronisering konfigurerad",
            "Tidssynkronisering konfigureret",
            "Tidssynkronisering konfigurert",
            "Aikasynkronointi määritetty",
        },
        ["catsub.Remote Desktop"] = new[]
        {
            "Remote access configured and enabled",
            "Fjärråtkomst konfigurerad och aktiverad",
            "Fjernadgang konfigureret og aktiveret",
            "Fjerntilgang konfigurert og aktivert",
            "Etäkäyttö määritetty ja otettu käyttöön",
        },
        ["catsub.Windows cleanup"] = new[]
        {
            "Unnecessary features and files removed",
            "Onödiga funktioner och filer borttagna",
            "Unødvendige funktioner og filer fjernet",
            "Unødvendige funksjoner og filer fjernet",
            "Tarpeettomat ominaisuudet ja tiedostot poistettu",
        },
        ["catsub.MilestonePSTools"] = new[]
        {
            "MilestonePSTools installed and configured",
            "MilestonePSTools installerat och konfigurerat",
            "MilestonePSTools installeret og konfigureret",
            "MilestonePSTools installert og konfigurert",
            "MilestonePSTools asennettu ja määritetty",
        },
        ["catsub.Logging and verification"] = new[]
        {
            "System validated and log created",
            "Systemet validerat och logg skapad",
            "Systemet valideret og log oprettet",
            "Systemet validert og logg opprettet",
            "Järjestelmä tarkistettu ja loki luotu",
        },

        // ---------- System page ----------
        ["sys.subtitle"] = new[]
        {
            "Hardware, operating system and drive details for this machine.",
            "Maskinvara, operativsystem och diskdetaljer för den här maskinen.",
            "Hardware, operativsystem og drevdetaljer for denne maskine.",
            "Maskinvare, operativsystem og diskdetaljer for denne maskinen.",
            "Tämän koneen laitteiston, käyttöjärjestelmän ja asemien tiedot.",
        },
        ["sys.details"] = new[] { "System details", "Systemdetaljer", "Systemdetaljer", "Systemdetaljer", "Järjestelmätiedot" },
        ["sys.os"] = new[] { "Operating system", "Operativsystem", "Operativsystem", "Operativsystem", "Käyttöjärjestelmä" },
        ["sys.version"] = new[] { "Version", "Version", "Version", "Versjon", "Versio" },
        ["sys.cpu"] = new[] { "CPU", "CPU", "CPU", "CPU", "Suoritin" },
        ["sys.memory"] = new[] { "Memory", "Minne", "Hukommelse", "Minne", "Muisti" },
        ["sys.domain"] = new[] { "Domain", "Domän", "Domæne", "Domene", "Toimialue" },
        ["sys.xprotect"] = new[] { "XProtect", "XProtect", "XProtect", "XProtect", "XProtect" },
        ["sys.drives"] = new[] { "Drives", "Diskar", "Drev", "Disker", "Asemat" },
        ["sys.drive"] = new[] { "Drive", "Disk", "Drev", "Disk", "Asema" },
        ["sys.label"] = new[] { "Label", "Etikett", "Etiket", "Etikett", "Nimi" },
        ["sys.size"] = new[] { "Size", "Storlek", "Størrelse", "Størrelse", "Koko" },
        ["sys.free"] = new[] { "Free", "Ledigt", "Ledig", "Ledig", "Vapaana" },
        ["sys.blocksize"] = new[] { "Block size", "Blockstorlek", "Blokstørrelse", "Blokkstørrelse", "Lohkokoko" },
        ["sys.indexing"] = new[] { "Indexing", "Indexering", "Indeksering", "Indeksering", "Indeksointi" },
        ["sys.detectingDrives"] = new[] { "Detecting drives…", "Identifierar diskar…", "Registrerer drev…", "Registrerer disker…", "Tunnistetaan asemia…" },
        ["sys.noDrives"] = new[] { "No drives detected.", "Inga diskar hittades.", "Ingen drev fundet.", "Ingen disker funnet.", "Asemia ei löytynyt." },
        ["sys.domainJoined"] = new[] { "Domain joined", "Domänansluten", "Domænetilsluttet", "Domenetilknyttet", "Liitetty toimialueeseen" },
        ["sys.workgroup"] = new[] { "Workgroup", "Arbetsgrupp", "Arbejdsgruppe", "Arbeidsgruppe", "Työryhmä" },
        ["sys.notInstalledReady"] = new[]
        {
            "Not installed (ready to prepare)",
            "Inte installerat (redo att förberedas)",
            "Ikke installeret (klar til forberedelse)",
            "Ikke installert (klar til forberedelse)",
            "Ei asennettu (valmis valmisteltavaksi)",
        },
        ["sys.systemTag"] = new[] { "System", "System", "System", "System", "Järjestelmä" },
        ["sys.on"] = new[] { "On", "På", "Til", "På", "Käytössä" },
        ["sys.off"] = new[] { "Off", "Av", "Fra", "Av", "Ei käytössä" },

        // ---------- Preparation page ----------
        ["prep.subtitle"] = new[]
        {
            "Select the tasks to include, then run. Every task checks first and only corrects what is missing.",
            "Välj vilka uppgifter som ska ingå och kör sedan. Varje uppgift kontrollerar först och åtgärdar bara det som saknas.",
            "Vælg hvilke opgaver der skal med, og kør derefter. Hver opgave kontrollerer først og retter kun det, der mangler.",
            "Velg hvilke oppgaver som skal med, og kjør deretter. Hver oppgave kontrollerer først og retter bare det som mangler.",
            "Valitse mukaan otettavat tehtävät ja suorita sitten. Jokainen tehtävä tarkistaa ensin ja korjaa vain sen, mikä puuttuu.",
        },
        ["prep.checkonly"] = new[]
        {
            "Check only (no changes)",
            "Kontrollera endast (inga ändringar)",
            "Kontrollér kun (ingen ændringer)",
            "Kun kontroll (ingen endringer)",
            "Vain tarkistus (ei muutoksia)",
        },

        // ---------- Tools page ----------
        ["tools.subtitle"] = new[]
        {
            "Standalone utilities for XProtect servers.",
            "Fristående verktyg för XProtect-servrar.",
            "Selvstændige værktøjer til XProtect-servere.",
            "Frittstående verktøy for XProtect-servere.",
            "Itsenäisiä apuohjelmia XProtect-palvelimille.",
        },
        ["tools.ntp.title"] = new[] { "NTP Recovery", "NTP-återställning", "NTP-gendannelse", "NTP-gjenoppretting", "NTP-palautus" },
        ["tools.ntp.desc"] = new[]
        {
            "Detects and fixes the Windows time service polling bug that lets server clocks drift.",
            "Hittar och åtgärdar buggen i Windows tidstjänst som gör att serverklockor kan driva.",
            "Finder og retter fejlen i Windows' tidstjeneste, der lader serverure drive.",
            "Finner og retter feilen i Windows' tidstjeneste som lar serverklokker drive.",
            "Tunnistaa ja korjaa Windowsin aikapalvelun kyselyvirheen, jonka takia palvelinten kellot voivat ajautua väärään aikaan.",
        },
        ["tools.ntp.apply"] = new[] { "Apply fix", "Åtgärda", "Anvend rettelse", "Utfør retting", "Korjaa" },
        ["tools.ntp.recheck"] = new[] { "Re-check", "Kontrollera igen", "Kontrollér igen", "Kontroller på nytt", "Tarkista uudelleen" },
        ["tools.ntp.checkingCfg"] = new[]
        {
            "Checking NTP configuration…",
            "Kontrollerar NTP-konfigurationen…",
            "Kontrollerer NTP-konfigurationen…",
            "Kontrollerer NTP-konfigurasjonen…",
            "Tarkistetaan NTP-määrityksiä…",
        },
        ["tools.ntp.bugDetectedFmt"] = new[]
        {
            "Bug detected — current value: {0}",
            "Bugg upptäckt — aktuellt värde: {0}",
            "Fejl fundet — aktuel værdi: {0}",
            "Feil funnet — gjeldende verdi: {0}",
            "Virhe havaittu — nykyinen arvo: {0}",
        },
        ["tools.ntp.okFmt"] = new[] { "OK — {0}", "OK — {0}", "OK — {0}", "OK — {0}", "OK — {0}" },
        ["tools.ntp.applying"] = new[] { "Applying fix…", "Åtgärdar…", "Anvender rettelse…", "Utfører retting…", "Korjataan…" },
        ["tools.ntp.fixNotApplied"] = new[]
        {
            "The fix could not be applied — see the Logs page.",
            "Åtgärden kunde inte genomföras — se sidan Loggar.",
            "Rettelsen kunne ikke anvendes — se siden Logfiler.",
            "Rettingen kunne ikke utføres — se siden Logger.",
            "Korjausta ei voitu tehdä — katso Lokit-sivu.",
        },
        ["tools.ntp.fixFailedFmt"] = new[]
        {
            "Fix failed: {0}",
            "Åtgärden misslyckades: {0}",
            "Rettelsen mislykkedes: {0}",
            "Rettingen mislyktes: {0}",
            "Korjaus epäonnistui: {0}",
        },
        ["tools.checkFailedFmt"] = new[]
        {
            "Check failed: {0}",
            "Kontrollen misslyckades: {0}",
            "Kontrollen mislykkedes: {0}",
            "Kontrollen mislyktes: {0}",
            "Tarkistus epäonnistui: {0}",
        },
        ["tools.wu.title"] = new[] { "Windows Update", "Windows Update", "Windows Update", "Windows Update", "Windows Update" },
        ["tools.wu.desc"] = new[]
        {
            "Turn Windows Update on or off. Keep it off on recording servers during business hours - drain updates before go-live instead.",
            "Slå på eller av Windows Update. Håll det avstängt på inspelningsservrar under arbetstid – dränera uppdateringar före driftsättning i stället.",
            "Slå Windows Update til eller fra. Hold det slået fra på optagelsesservere i arbejdstiden – træk i stedet opdateringerne inden idriftsættelse.",
            "Slå Windows Update på eller av. Hold det avslått på opptaksservere i arbeidstiden – tøm heller oppdateringene før idriftsettelse.",
            "Ota Windows Update käyttöön tai poista se käytöstä. Pidä se poissa käytöstä tallennuspalvelimilla työaikana – asenna päivitykset valmiiksi ennen käyttöönottoa.",
        },
        ["tools.wu.enable"] = new[] { "Enable", "Aktivera", "Aktivér", "Aktiver", "Ota käyttöön" },
        ["tools.wu.disable"] = new[] { "Disable", "Inaktivera", "Deaktivér", "Deaktiver", "Poista käytöstä" },
        ["tools.wu.refresh"] = new[] { "Refresh", "Uppdatera", "Opdatér", "Oppdater", "Päivitä" },
        ["tools.wu.checkingState"] = new[]
        {
            "Checking Windows Update state…",
            "Kontrollerar Windows Update-status…",
            "Kontrollerer Windows Update-status…",
            "Kontrollerer Windows Update-status…",
            "Tarkistetaan Windows Update -tilaa…",
        },
        ["tools.wu.enabled"] = new[]
        {
            "Windows Update is ENABLED.",
            "Windows Update är AKTIVERAT.",
            "Windows Update er AKTIVERET.",
            "Windows Update er AKTIVERT.",
            "Windows Update on KÄYTÖSSÄ.",
        },
        ["tools.wu.disabled"] = new[]
        {
            "Windows Update is DISABLED.",
            "Windows Update är INAKTIVERAT.",
            "Windows Update er DEAKTIVERET.",
            "Windows Update er DEAKTIVERT.",
            "Windows Update on POISSA KÄYTÖSTÄ.",
        },
        ["tools.wu.enabling"] = new[]
        {
            "Enabling Windows Update…",
            "Aktiverar Windows Update…",
            "Aktiverer Windows Update…",
            "Aktiverer Windows Update…",
            "Otetaan Windows Update käyttöön…",
        },
        ["tools.wu.disabling"] = new[]
        {
            "Disabling Windows Update…",
            "Inaktiverar Windows Update…",
            "Deaktiverer Windows Update…",
            "Deaktiverer Windows Update…",
            "Poistetaan Windows Update käytöstä…",
        },
        ["tools.wu.changeFailedFmt"] = new[]
        {
            "Change failed: {0}",
            "Ändringen misslyckades: {0}",
            "Ændringen mislykkedes: {0}",
            "Endringen mislyktes: {0}",
            "Muutos epäonnistui: {0}",
        },
        ["tools.pstools.title"] = new[] { "MilestonePSTools", "MilestonePSTools", "MilestonePSTools", "MilestonePSTools", "MilestonePSTools" },
        ["tools.pstools.desc"] = new[]
        {
            "Opens a PowerShell window with the MilestonePSTools module loaded, ready for scripting against the VMS.",
            "Öppnar ett PowerShell-fönster med MilestonePSTools-modulen laddad, redo för skript mot VMS:et.",
            "Åbner et PowerShell-vindue med MilestonePSTools-modulet indlæst, klar til scripting mod VMS'et.",
            "Åpner et PowerShell-vindu med MilestonePSTools-modulen lastet, klar for skripting mot VMS-et.",
            "Avaa PowerShell-ikkunan, jossa MilestonePSTools-moduuli on ladattuna, valmiina skriptaukseen VMS:ää vasten.",
        },
        ["tools.logfolder.title"] = new[] { "Log folder", "Loggmapp", "Logmappe", "Loggmappe", "Lokikansio" },
        ["tools.logfolder.desc"] = new[]
        {
            "All preparation logs and system reports are written to C:\\Milestonecheck-ulfh.",
            "Alla förberedelseloggar och systemrapporter skrivs till C:\\Milestonecheck-ulfh.",
            "Alle forberedelseslogfiler og systemrapporter skrives til C:\\Milestonecheck-ulfh.",
            "Alle forberedelseslogger og systemrapporter skrives til C:\\Milestonecheck-ulfh.",
            "Kaikki valmistelulokit ja järjestelmäraportit kirjoitetaan kansioon C:\\Milestonecheck-ulfh.",
        },
        ["tools.logfolder.open"] = new[] { "Open log folder", "Öppna loggmappen", "Åbn logmappen", "Åpne loggmappen", "Avaa lokikansio" },
        ["tools.report.title"] = new[] { "System report", "Systemrapport", "Systemrapport", "Systemrapport", "Järjestelmäraportti" },
        ["tools.report.desc"] = new[]
        {
            "Runs a check-only sweep of every preparation task and writes a text report - nothing is changed. Handy as a pre-install audit or for handover documentation.",
            "Kör en ren kontroll av alla förberedelseuppgifter och skriver en textrapport – ingenting ändras. Praktiskt som granskning före installation eller som överlämningsdokumentation.",
            "Kører en ren kontrol af alle forberedelsesopgaver og skriver en tekstrapport – intet ændres. Praktisk som audit før installation eller til overdragelsesdokumentation.",
            "Kjører en ren kontroll av alle forberedelsesoppgaver og skriver en tekstrapport – ingenting endres. Praktisk som revisjon før installasjon eller som overleveringsdokumentasjon.",
            "Suorittaa kaikkien valmistelutehtävien tarkistuksen ja kirjoittaa tekstiraportin – mitään ei muuteta. Kätevä asennusta edeltävään auditointiin tai luovutusdokumentaatioksi.",
        },
        ["tools.report.generate"] = new[] { "Generate system report", "Skapa systemrapport", "Generér systemrapport", "Generer systemrapport", "Luo järjestelmäraportti" },
        ["tools.report.running"] = new[]
        {
            "Running check-only sweep… this can take a few minutes.",
            "Kör kontrollsvep… det kan ta några minuter.",
            "Kører kontrolgennemgang… det kan tage et par minutter.",
            "Kjører kontrollgjennomgang… det kan ta noen minutter.",
            "Suoritetaan tarkistusta… tämä voi kestää muutaman minuutin.",
        },
        ["tools.report.writtenFmt"] = new[] { "Report written: {0}", "Rapport skriven: {0}", "Rapport skrevet: {0}", "Rapport skrevet: {0}", "Raportti kirjoitettu: {0}" },
        ["tools.report.failedFmt"] = new[] { "Report failed: {0}", "Rapporten misslyckades: {0}", "Rapporten mislykkedes: {0}", "Rapporten mislyktes: {0}", "Raportti epäonnistui: {0}" },

        // ---------- Logs page ----------
        ["logs.subtitle"] = new[]
        {
            "Live output from the current session and previous preparation logs.",
            "Direktutdata från den aktuella sessionen och tidigare förberedelseloggar.",
            "Live-output fra den aktuelle session og tidligere forberedelseslogfiler.",
            "Direkte utdata fra gjeldende økt og tidligere forberedelseslogger.",
            "Nykyisen istunnon reaaliaikainen tuloste ja aiemmat valmistelulokit.",
        },
        ["logs.autoscroll"] = new[] { "Autoscroll", "Rulla automatiskt", "Rul automatisk", "Rull automatisk", "Automaattinen vieritys" },
        ["logs.previous"] = new[]
        {
            "Previous logs (double-click to open)",
            "Tidigare loggar (dubbelklicka för att öppna)",
            "Tidligere logfiler (dobbeltklik for at åbne)",
            "Tidligere logger (dobbeltklikk for å åpne)",
            "Aiemmat lokit (avaa kaksoisnapsauttamalla)",
        },

        // ---------- Settings page ----------
        ["settings.subtitle"] = new[]
        {
            "Saved automatically. Applies to both the UI and silent runs.",
            "Sparas automatiskt. Gäller både gränssnittet och tysta körningar.",
            "Gemmes automatisk. Gælder både brugerfladen og tavse kørsler.",
            "Lagres automatisk. Gjelder både grensesnittet og stille kjøringer.",
            "Tallennetaan automaattisesti. Koskee sekä käyttöliittymää että hiljaisia suorituksia.",
        },
        ["settings.lang.title"] = new[] { "Language", "Språk", "Sprog", "Språk", "Kieli" },
        ["settings.lang.desc"] = new[]
        {
            "Display language for the app.",
            "Visningsspråk för appen.",
            "Visningssprog for appen.",
            "Visningsspråk for appen.",
            "Sovelluksen näyttökieli.",
        },
        ["settings.lang.auto"] = new[]
        {
            "System default (Windows)",
            "Systemstandard (Windows)",
            "Systemstandard (Windows)",
            "Systemstandard (Windows)",
            "Järjestelmän oletus (Windows)",
        },
        ["settings.lang.restartHint"] = new[]
        {
            "Restart the app to apply the new language.",
            "Starta om appen för att byta språk.",
            "Genstart appen for at anvende det nye sprog.",
            "Start appen på nytt for å ta i bruk det nye språket.",
            "Käynnistä sovellus uudelleen ottaaksesi uuden kielen käyttöön.",
        },
        ["settings.av.title"] = new[]
        {
            "Antivirus whole-drive exclusions",
            "Antivirusundantag för hela diskar",
            "Antivirusundtagelser for hele drev",
            "Antivirusunntak for hele disker",
            "Virustorjunnan poikkeukset kokonaisille asemille",
        },
        ["settings.av.desc"] = new[]
        {
            "Which drives get a whole-drive Windows Defender exclusion for recording storage.",
            "Vilka diskar som får ett Windows Defender-undantag för hela disken som inspelningslagring.",
            "Hvilke drev der får en Windows Defender-undtagelse for hele drevet som optagelseslager.",
            "Hvilke disker som får et Windows Defender-unntak for hele disken som opptakslagring.",
            "Mitkä asemat saavat koko aseman kattavan Windows Defender -poikkeuksen tallennuskäyttöä varten.",
        },
        ["settings.av.storage"] = new[]
        {
            "All storage drives (recommended)",
            "Alla lagringsdiskar (rekommenderas)",
            "Alle lagerdrev (anbefales)",
            "Alle lagringsdisker (anbefales)",
            "Kaikki tallennusasemat (suositus)",
        },
        ["settings.av.storageDesc"] = new[]
        {
            "Every non-OS drive is excluded - the normal choice for recording servers.",
            "Alla diskar utom systemdisken undantas – normalvalet för inspelningsservrar.",
            "Alle drev undtagen systemdrevet undtages – det normale valg til optagelsesservere.",
            "Alle disker unntatt systemdisken unntas – normalvalget for opptaksservere.",
            "Kaikki asemat käyttöjärjestelmäasemaa lukuun ottamatta lisätään poikkeuksiin – tavallinen valinta tallennuspalvelimille.",
        },
        ["settings.av.none"] = new[]
        {
            "Only Milestone folders (no whole-drive)",
            "Endast Milestone-mappar (inga hela diskar)",
            "Kun Milestone-mapper (ingen hele drev)",
            "Kun Milestone-mapper (ingen hele disker)",
            "Vain Milestone-kansiot (ei kokonaisia asemia)",
        },
        ["settings.av.noneDesc"] = new[]
        {
            "Only the Milestone program and data folders are excluded.",
            "Endast Milestones program- och datamappar undantas.",
            "Kun Milestones program- og datamapper undtages.",
            "Kun Milestones program- og datamapper unntas.",
            "Vain Milestonen ohjelma- ja datakansiot lisätään poikkeuksiin.",
        },
        ["settings.av.specific"] = new[] { "Specific drives:", "Specifika diskar:", "Bestemte drev:", "Bestemte disker:", "Tietyt asemat:" },
        ["settings.av.specificHint"] = new[]
        {
            "Comma-separated drive letters, e.g. D,E",
            "Kommaseparerade enhetsbokstäver, t.ex. D,E",
            "Kommaseparerede drevbogstaver, f.eks. D,E",
            "Kommaseparerte stasjonsbokstaver, f.eks. D,E",
            "Pilkulla erotetut asemakirjaimet, esim. D,E",
        },
        ["settings.ntp.title"] = new[]
        {
            "NTP upstream time source",
            "Överordnad NTP-tidskälla",
            "Overordnet NTP-tidskilde",
            "Overordnet NTP-tidskilde",
            "Ylätason NTP-aikalähde",
        },
        ["settings.ntp.desc"] = new[]
        {
            "The time source this machine syncs from while acting as NTP server for your cameras. Pick the pool closest to the site.",
            "Tidskällan som den här maskinen synkroniserar från medan den agerar NTP-server för dina kameror. Välj poolen närmast anläggningen.",
            "Tidskilden som denne maskine synkroniserer fra, mens den fungerer som NTP-server for dine kameraer. Vælg poolen tættest på anlægget.",
            "Tidskilden denne maskinen synkroniserer fra mens den fungerer som NTP-server for kameraene dine. Velg poolen nærmest anlegget.",
            "Aikalähde, josta tämä kone synkronoi toimiessaan NTP-palvelimena kameroillesi. Valitse asennuspaikkaa lähinnä oleva pooli.",
        },
        ["settings.ntp.peersFmt"] = new[] { "Peers: {0}", "Peers: {0}", "Peers: {0}", "Peers: {0}", "Peers: {0}" },
        ["settings.options"] = new[] { "Options", "Alternativ", "Valgmuligheder", "Alternativer", "Valinnat" },
        ["settings.ssh"] = new[]
        {
            "Include SSH server (optional task)",
            "Inkludera SSH-server (valfri uppgift)",
            "Inkludér SSH-server (valgfri opgave)",
            "Inkluder SSH-server (valgfri oppgave)",
            "Sisällytä SSH-palvelin (valinnainen tehtävä)",
        },
        ["settings.sshDesc"] = new[]
        {
            "Installs and enables OpenSSH Server for remote administration.",
            "Installerar och aktiverar OpenSSH Server för fjärradministration.",
            "Installerer og aktiverer OpenSSH Server til fjernadministration.",
            "Installerer og aktiverer OpenSSH Server for fjernadministrasjon.",
            "Asentaa ja ottaa käyttöön OpenSSH Serverin etähallintaa varten.",
        },
        ["settings.snmp"] = new[]
        {
            "Include SNMP (optional task)",
            "Inkludera SNMP (valfri uppgift)",
            "Inkludér SNMP (valgfri opgave)",
            "Inkluder SNMP (valgfri oppgave)",
            "Sisällytä SNMP (valinnainen tehtävä)",
        },
        ["settings.snmpDesc"] = new[]
        {
            "Installs the SNMP service for monitoring systems.",
            "Installerar SNMP-tjänsten för övervakningssystem.",
            "Installerer SNMP-tjenesten til overvågningssystemer.",
            "Installerer SNMP-tjenesten for overvåkingssystemer.",
            "Asentaa SNMP-palvelun valvontajärjestelmiä varten.",
        },
        ["settings.restartExplorer"] = new[]
        {
            "Restart Explorer after clock-seconds change",
            "Starta om Utforskaren efter ändring av sekunder i klockan",
            "Genstart Stifinder efter ændring af sekunder i uret",
            "Start Utforsker på nytt etter endring av sekunder i klokken",
            "Käynnistä Resurssienhallinta uudelleen kellon sekuntien muutoksen jälkeen",
        },
        ["settings.restartExplorerDesc"] = new[]
        {
            "Showing seconds in the system clock requires an Explorer restart to take effect immediately.",
            "Att visa sekunder i systemklockan kräver en omstart av Utforskaren för att slå igenom direkt.",
            "Visning af sekunder i systemuret kræver en genstart af Stifinder for at slå igennem med det samme.",
            "Visning av sekunder i systemklokken krever en omstart av Utforsker for å tre i kraft umiddelbart.",
            "Sekuntien näyttäminen järjestelmäkellossa vaatii Resurssienhallinnan uudelleenkäynnistyksen, jotta muutos tulee voimaan heti.",
        },

        // ---------- About page ----------
        ["about.version"] = new[] { "Version", "Version", "Version", "Versjon", "Versio" },
        ["about.builtBy"] = new[]
        {
            "Built by Ulf Holmström, Happy Problem Solver at Manvarg AB, Sweden",
            "Byggd av Ulf Holmström, Happy Problem Solver på Manvarg AB, Sverige",
            "Bygget af Ulf Holmström, Happy Problem Solver hos Manvarg AB, Sverige",
            "Bygget av Ulf Holmström, Happy Problem Solver hos Manvarg AB, Sverige",
            "Tehnyt Ulf Holmström, Happy Problem Solver, Manvarg AB, Ruotsi",
        },
        ["about.tagline"] = new[]
        {
            "Prepares Windows for XProtect installations by Milestone best practice — check first, fix only what's needed.",
            "Förbereder Windows för XProtect-installationer enligt Milestones best practice — kontrollera först, åtgärda bara det som behövs.",
            "Forbereder Windows til XProtect-installationer efter Milestones best practice — kontrollér først, ret kun det nødvendige.",
            "Forbereder Windows for XProtect-installasjoner etter Milestones beste praksis — kontroller først, rett bare det som trengs.",
            "Valmistelee Windowsin XProtect-asennuksia varten Milestonen parhaiden käytäntöjen mukaan — tarkista ensin, korjaa vain tarvittava.",
        },
        ["about.disclaimer"] = new[]
        {
            "This tool is NOT developed by, endorsed by, or affiliated with Milestone Systems A/S. It is a personal project by Ulf Holmström to make life easier for technicians who work with Milestone XProtect.",
            "Det här verktyget är INTE utvecklat av, godkänt av eller anslutet till Milestone Systems A/S. Det är ett personligt projekt av Ulf Holmström för att göra livet enklare för tekniker som arbetar med Milestone XProtect.",
            "Dette værktøj er IKKE udviklet af, godkendt af eller tilknyttet Milestone Systems A/S. Det er et personligt projekt af Ulf Holmström for at gøre livet lettere for teknikere, der arbejder med Milestone XProtect.",
            "Dette verktøyet er IKKE utviklet av, godkjent av eller tilknyttet Milestone Systems A/S. Det er et personlig prosjekt av Ulf Holmström for å gjøre livet enklere for teknikere som jobber med Milestone XProtect.",
            "Tämä työkalu EI ole Milestone Systems A/S:n kehittämä tai hyväksymä eikä siihen sidoksissa. Se on Ulf Holmströmin henkilökohtainen projekti, jonka tarkoitus on helpottaa Milestone XProtectin parissa työskentelevien teknikoiden elämää.",
        },
        ["about.license"] = new[]
        {
            "Released under the MIT License.",
            "Släppt under MIT-licensen.",
            "Udgivet under MIT-licensen.",
            "Utgitt under MIT-lisensen.",
            "Julkaistu MIT-lisenssillä.",
        },

        // ---------- NTP source display names (engine keys from NtpCore.Sources) ----------
        ["ntp.windows"] = new[]
        {
            "Windows default (time.windows.com)",
            "Windows-standard (time.windows.com)",
            "Windows-standard (time.windows.com)",
            "Windows-standard (time.windows.com)",
            "Windowsin oletus (time.windows.com)",
        },
        ["ntp.global"] = new[]
        {
            "Global pool (pool.ntp.org — picks nearest)",
            "Global pool (pool.ntp.org — väljer närmaste)",
            "Global pool (pool.ntp.org — vælger nærmeste)",
            "Global pool (pool.ntp.org — velger nærmeste)",
            "Globaali pooli (pool.ntp.org — valitsee lähimmän)",
        },
        ["ntp.ar"] = new[] { "Argentina", "Argentina", "Argentina", "Argentina", "Argentiina" },
        ["ntp.au"] = new[] { "Australia", "Australien", "Australien", "Australia", "Australia" },
        ["ntp.at"] = new[] { "Austria", "Österrike", "Østrig", "Østerrike", "Itävalta" },
        ["ntp.be"] = new[] { "Belgium", "Belgien", "Belgien", "Belgia", "Belgia" },
        ["ntp.br"] = new[] { "Brazil", "Brasilien", "Brasilien", "Brasil", "Brasilia" },
        ["ntp.ca"] = new[] { "Canada", "Kanada", "Canada", "Canada", "Kanada" },
        ["ntp.cl"] = new[] { "Chile", "Chile", "Chile", "Chile", "Chile" },
        ["ntp.cn"] = new[] { "China", "Kina", "Kina", "Kina", "Kiina" },
        ["ntp.cz"] = new[] { "Czechia", "Tjeckien", "Tjekkiet", "Tsjekkia", "Tšekki" },
        ["ntp.dk"] = new[] { "Denmark", "Danmark", "Danmark", "Danmark", "Tanska" },
        ["ntp.fi"] = new[] { "Finland", "Finland", "Finland", "Finland", "Suomi" },
        ["ntp.fr"] = new[] { "France", "Frankrike", "Frankrig", "Frankrike", "Ranska" },
        ["ntp.de"] = new[] { "Germany", "Tyskland", "Tyskland", "Tyskland", "Saksa" },
        ["ntp.hk"] = new[] { "Hong Kong", "Hongkong", "Hongkong", "Hongkong", "Hongkong" },
        ["ntp.in"] = new[] { "India", "Indien", "Indien", "India", "Intia" },
        ["ntp.ie"] = new[] { "Ireland", "Irland", "Irland", "Irland", "Irlanti" },
        ["ntp.it"] = new[] { "Italy", "Italien", "Italien", "Italia", "Italia" },
        ["ntp.jp"] = new[] { "Japan", "Japan", "Japan", "Japan", "Japani" },
        ["ntp.my"] = new[] { "Malaysia", "Malaysia", "Malaysia", "Malaysia", "Malesia" },
        ["ntp.mx"] = new[] { "Mexico", "Mexiko", "Mexico", "Mexico", "Meksiko" },
        ["ntp.nl"] = new[] { "Netherlands", "Nederländerna", "Nederlandene", "Nederland", "Alankomaat" },
        ["ntp.nz"] = new[] { "New Zealand", "Nya Zeeland", "New Zealand", "New Zealand", "Uusi-Seelanti" },
        ["ntp.no"] = new[] { "Norway", "Norge", "Norge", "Norge", "Norja" },
        ["ntp.pl"] = new[] { "Poland", "Polen", "Polen", "Polen", "Puola" },
        ["ntp.pt"] = new[] { "Portugal", "Portugal", "Portugal", "Portugal", "Portugali" },
        ["ntp.sg"] = new[] { "Singapore", "Singapore", "Singapore", "Singapore", "Singapore" },
        ["ntp.za"] = new[] { "South Africa", "Sydafrika", "Sydafrika", "Sør-Afrika", "Etelä-Afrikka" },
        ["ntp.kr"] = new[] { "South Korea", "Sydkorea", "Sydkorea", "Sør-Korea", "Etelä-Korea" },
        ["ntp.es"] = new[] { "Spain", "Spanien", "Spanien", "Spania", "Espanja" },
        ["ntp.se"] = new[] { "Sweden", "Sverige", "Sverige", "Sverige", "Ruotsi" },
        ["ntp.ch"] = new[] { "Switzerland", "Schweiz", "Schweiz", "Sveits", "Sveitsi" },
        ["ntp.tw"] = new[] { "Taiwan", "Taiwan", "Taiwan", "Taiwan", "Taiwan" },
        ["ntp.th"] = new[] { "Thailand", "Thailand", "Thailand", "Thailand", "Thaimaa" },
        ["ntp.tr"] = new[] { "Turkey", "Turkiet", "Tyrkiet", "Tyrkia", "Turkki" },
        ["ntp.ae"] = new[]
        {
            "United Arab Emirates",
            "Förenade Arabemiraten",
            "De Forenede Arabiske Emirater",
            "De forente arabiske emirater",
            "Yhdistyneet arabiemiirikunnat",
        },
        ["ntp.uk"] = new[] { "United Kingdom", "Storbritannien", "Storbritannien", "Storbritannia", "Iso-Britannia" },
        ["ntp.us"] = new[] { "United States", "USA", "USA", "USA", "Yhdysvallat" },

        // ---------- Task names and descriptions (English = engine strings verbatim) ----------
        ["task.av-milestone-folders.name"] = new[]
        {
            "Milestone folder exclusions",
            "Undantag för Milestone-mappar",
            "Undtagelser for Milestone-mapper",
            "Unntak for Milestone-mapper",
            "Milestone-kansioiden poikkeukset",
        },
        ["task.av-milestone-folders.desc"] = new[]
        {
            "Defender path exclusions for the five Milestone program and data folders on C:",
            "Defender-sökvägsundantag för de fem Milestone-program- och datamapparna på C:",
            "Defender-stiundtagelser for de fem Milestone-program- og datamapper på C:",
            "Defender-baneunntak for de fem Milestone-program- og datamappene på C:",
            "Defender-polkupoikkeukset viidelle Milestonen ohjelma- ja datakansiolle C:-asemalla",
        },
        ["task.av-extensions.name"] = new[]
        {
            "Media file extension exclusions",
            "Undantag för mediefiländelser",
            "Undtagelser for mediefiltyper",
            "Unntak for mediefiltyper",
            "Mediatiedostopäätteiden poikkeukset",
        },
        ["task.av-extensions.desc"] = new[]
        {
            "Defender exclusions for XProtect media database extensions (blk, idx, pic, pqz, sts, ts)",
            "Defender-undantag för XProtects mediedatabas-filändelser (blk, idx, pic, pqz, sts, ts)",
            "Defender-undtagelser for XProtects mediedatabase-filtyper (blk, idx, pic, pqz, sts, ts)",
            "Defender-unntak for XProtects mediedatabase-filtyper (blk, idx, pic, pqz, sts, ts)",
            "Defender-poikkeukset XProtectin mediatietokannan tiedostopäätteille (blk, idx, pic, pqz, sts, ts)",
        },
        ["task.av-processes.name"] = new[]
        {
            "VideoOS process exclusions",
            "Undantag för VideoOS-processer",
            "Undtagelser for VideoOS-processer",
            "Unntak for VideoOS-prosesser",
            "VideoOS-prosessien poikkeukset",
        },
        ["task.av-processes.desc"] = new[]
        {
            "Defender process exclusions for the VideoOS services, VictoriaLogs (2026 R1) and legacy Log Server",
            "Defender-processundantag för VideoOS-tjänsterna, VictoriaLogs (2026 R1) och äldre Log Server",
            "Defender-procesundtagelser for VideoOS-tjenesterne, VictoriaLogs (2026 R1) og ældre Log Server",
            "Defender-prosessunntak for VideoOS-tjenestene, VictoriaLogs (2026 R1) og eldre Log Server",
            "Defender-prosessipoikkeukset VideoOS-palveluille, VictoriaLogsille (2026 R1) ja vanhalle Log Serverille",
        },
        ["task.av-smartclient.name"] = new[]
        {
            "Smart Client exclusions",
            "Undantag för Smart Client",
            "Undtagelser for Smart Client",
            "Unntak for Smart Client",
            "Smart Client -poikkeukset",
        },
        ["task.av-smartclient.desc"] = new[]
        {
            "Defender exclusions for XProtect Smart Client and its per-user cache (only when installed)",
            "Defender-undantag för XProtect Smart Client och dess cache per användare (endast om installerad)",
            "Defender-undtagelser for XProtect Smart Client og dens cache pr. bruger (kun hvis installeret)",
            "Defender-unntak for XProtect Smart Client og dens cache per bruker (kun hvis installert)",
            "Defender-poikkeukset XProtect Smart Clientille ja sen käyttäjäkohtaiselle välimuistille (vain jos asennettu)",
        },
        ["task.av-storage-drives.name"] = new[]
        {
            "Storage drive exclusions",
            "Undantag för lagringsdiskar",
            "Undtagelser for lagerdrev",
            "Unntak for lagringsdisker",
            "Tallennusasemien poikkeukset",
        },
        ["task.av-storage-drives.desc"] = new[]
        {
            "Defender path exclusions for whole recording storage drives (C: always excluded)",
            "Defender-sökvägsundantag för hela inspelningsdiskar (C: undantas alltid)",
            "Defender-stiundtagelser for hele optagelsesdrev (C: undtages altid)",
            "Defender-baneunntak for hele opptaksdisker (C: unntas alltid)",
            "Defender-polkupoikkeukset kokonaisille tallennusasemille (C: jätetään aina pois)",
        },
        ["task.storage-blocksize.name"] = new[]
        {
            "64 KB allocation unit check",
            "Kontroll av 64 KB allokeringsenhet",
            "Kontrol af 64 KB allokeringsenhed",
            "Kontroll av 64 KB allokeringsenhet",
            "64 KB:n varausyksikön tarkistus",
        },
        ["task.storage-blocksize.desc"] = new[]
        {
            "Verifies that storage drives are formatted with 64 KB NTFS clusters (check only)",
            "Verifierar att lagringsdiskarna är formaterade med 64 KB NTFS-kluster (endast kontroll)",
            "Verificerer at lagerdrev er formateret med 64 KB NTFS-klynger (kun kontrol)",
            "Verifiserer at lagringsdiskene er formatert med 64 KB NTFS-klynger (kun kontroll)",
            "Varmistaa, että tallennusasemat on alustettu 64 KB:n NTFS-klustereilla (vain tarkistus)",
        },
        ["task.storage-indexing.name"] = new[]
        {
            "Disable indexing on storage drives",
            "Inaktivera indexering på lagringsdiskar",
            "Deaktivér indeksering på lagerdrev",
            "Deaktiver indeksering på lagringsdisker",
            "Poista indeksointi käytöstä tallennusasemilla",
        },
        ["task.storage-indexing.desc"] = new[]
        {
            "Turns off Windows Search indexing on all non-OS fixed drives",
            "Stänger av Windows Search-indexering på alla fasta diskar utom systemdisken",
            "Slår Windows Search-indeksering fra på alle faste drev undtagen systemdrevet",
            "Slår av Windows Search-indeksering på alle faste disker unntatt systemdisken",
            "Poistaa Windows Search -indeksoinnin käytöstä kaikilla kiinteillä asemilla käyttöjärjestelmäasemaa lukuun ottamatta",
        },
        ["task.perf-powerplan.name"] = new[]
        {
            "Power plan: High Performance, never sleep",
            "Energischema: Hög prestanda, aldrig viloläge",
            "Strømstyringsplan: Høj ydeevne, aldrig slumre",
            "Strømplan: Høy ytelse, aldri hvilemodus",
            "Virrankäyttösuunnitelma: Paras suorituskyky, ei koskaan lepotilaan",
        },
        ["task.perf-powerplan.desc"] = new[]
        {
            "High Performance plan with sleep, display and disk timeouts off (critical for 24/7 recording)",
            "Hög prestanda-schema med viloläge, skärm- och disktimeout avstängda (kritiskt för 24/7-inspelning)",
            "Høj ydeevne-plan med slumre-, skærm- og disk-timeout slået fra (kritisk for 24/7-optagelse)",
            "Høy ytelse-plan med hvile-, skjerm- og disk-timeout slått av (kritisk for 24/7-opptak)",
            "Paras suorituskyky -suunnitelma, jossa lepotilan, näytön ja levyjen aikakatkaisut on poistettu käytöstä (kriittinen 24/7-tallennukselle)",
        },
        ["task.perf-services.name"] = new[]
        {
            "Disable unnecessary services",
            "Inaktivera onödiga tjänster",
            "Deaktivér unødvendige tjenester",
            "Deaktiver unødvendige tjenester",
            "Poista tarpeettomat palvelut käytöstä",
        },
        ["task.perf-services.desc"] = new[]
        {
            "Stops and disables Fax, Windows Search, Print Spooler, SysMain, HomeGroup, Themes and Tablet Input",
            "Stoppar och inaktiverar Fax, Windows Search, Print Spooler, SysMain, HomeGroup, Themes och Tablet Input",
            "Stopper og deaktiverer Fax, Windows Search, Print Spooler, SysMain, HomeGroup, Themes og Tablet Input",
            "Stopper og deaktiverer Fax, Windows Search, Print Spooler, SysMain, HomeGroup, Themes og Tablet Input",
            "Pysäyttää ja poistaa käytöstä palvelut Fax, Windows Search, Print Spooler, SysMain, HomeGroup, Themes ja Tablet Input",
        },
        ["task.perf-largesystemcache.name"] = new[]
        {
            "LargeSystemCache",
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
            "Ottaa LargeSystemCachen käyttöön tiedostopalvelintyyppisiä kuormia varten (videotallennus)",
        },
        ["task.rdp.name"] = new[]
        {
            "Enable and optimize Remote Desktop",
            "Aktivera och optimera Fjärrskrivbord",
            "Aktivér og optimér Fjernskrivebord",
            "Aktiver og optimaliser Eksternt skrivebord",
            "Ota käyttöön ja optimoi Etätyöpöytä",
        },
        ["task.rdp.desc"] = new[]
        {
            "Enables RDP, opens its firewall group and turns off wallpaper/animations over RDP",
            "Aktiverar RDP, öppnar dess brandväggsgrupp och stänger av skrivbordsbakgrund/animeringar över RDP",
            "Aktiverer RDP, åbner dets firewallgruppe og slår baggrund/animationer fra over RDP",
            "Aktiverer RDP, åpner brannmurgruppen og slår av bakgrunn/animasjoner over RDP",
            "Ottaa RDP:n käyttöön, avaa sen palomuuriryhmän ja poistaa taustakuvan/animaatiot käytöstä RDP-yhteyksissä",
        },
        ["task.cleanup-bloatware.name"] = new[]
        {
            "Remove bloatware apps",
            "Ta bort bloatware-appar",
            "Fjern bloatware-apps",
            "Fjern bloatware-apper",
            "Poista bloatware-sovellukset",
        },
        ["task.cleanup-bloatware.desc"] = new[]
        {
            "Removes 36 preinstalled consumer apps (installed and provisioned). Xbox apps untouched",
            "Tar bort 36 förinstallerade konsumentappar (installerade och provisionerade). Xbox-appar rörs inte",
            "Fjerner 36 forudinstallerede forbruger-apps (installerede og provisionerede). Xbox-apps røres ikke",
            "Fjerner 36 forhåndsinstallerte forbruker-apper (installerte og provisjonerte). Xbox-apper røres ikke",
            "Poistaa 36 esiasennettua kuluttajasovellusta (asennetut ja provisioidut). Xbox-sovelluksiin ei kosketa",
        },
        ["task.cleanup-onedrive.name"] = new[]
        {
            "Remove OneDrive",
            "Ta bort OneDrive",
            "Fjern OneDrive",
            "Fjern OneDrive",
            "Poista OneDrive",
        },
        ["task.cleanup-onedrive.desc"] = new[]
        {
            "Uninstalls OneDrive and deletes its leftover folders",
            "Avinstallerar OneDrive och tar bort kvarlämnade mappar",
            "Afinstallerer OneDrive og sletter efterladte mapper",
            "Avinstallerer OneDrive og sletter gjenværende mapper",
            "Poistaa OneDriven asennuksen ja sen jäljelle jääneet kansiot",
        },
        ["task.cleanup-teams.name"] = new[]
        {
            "Remove Microsoft Teams",
            "Ta bort Microsoft Teams",
            "Fjern Microsoft Teams",
            "Fjern Microsoft Teams",
            "Poista Microsoft Teams",
        },
        ["task.cleanup-teams.desc"] = new[]
        {
            "Removes new (AppX) and classic Teams plus leftover folders",
            "Tar bort nya (AppX) och klassiska Teams samt kvarlämnade mappar",
            "Fjerner nye (AppX) og klassiske Teams samt efterladte mapper",
            "Fjerner nye (AppX) og klassiske Teams samt gjenværende mapper",
            "Poistaa uuden (AppX) ja perinteisen Teamsin sekä jäljelle jääneet kansiot",
        },
        ["task.cleanup-outlook-officehub.name"] = new[]
        {
            "Remove Outlook App and Office Hub",
            "Ta bort Outlook-appen och Office Hub",
            "Fjern Outlook-appen og Office Hub",
            "Fjern Outlook-appen og Office Hub",
            "Poista Outlook-sovellus ja Office Hub",
        },
        ["task.cleanup-outlook-officehub.desc"] = new[]
        {
            "Removes the new Outlook app and the Office Hub/365 installer (installed and provisioned)",
            "Tar bort nya Outlook-appen och Office Hub/365-installeraren (installerade och provisionerade)",
            "Fjerner den nye Outlook-app og Office Hub/365-installationsprogrammet (installeret og provisioneret)",
            "Fjerner den nye Outlook-appen og Office Hub/365-installasjonsprogrammet (installert og provisjonert)",
            "Poistaa uuden Outlook-sovelluksen ja Office Hub/365 -asennusohjelman (asennetut ja provisioidut)",
        },
        ["task.cleanup-copilot-recall.name"] = new[]
        {
            "Disable Copilot and Recall",
            "Inaktivera Copilot och Recall",
            "Deaktivér Copilot og Recall",
            "Deaktiver Copilot og Recall",
            "Poista Copilot ja Recall käytöstä",
        },
        ["task.cleanup-copilot-recall.desc"] = new[]
        {
            "Group Policy: TurnOffWindowsCopilot=1 and DisableAIDataAnalysis=1",
            "Grupprincip: TurnOffWindowsCopilot=1 och DisableAIDataAnalysis=1",
            "Gruppepolitik: TurnOffWindowsCopilot=1 og DisableAIDataAnalysis=1",
            "Gruppepolicy: TurnOffWindowsCopilot=1 og DisableAIDataAnalysis=1",
            "Ryhmäkäytäntö: TurnOffWindowsCopilot=1 ja DisableAIDataAnalysis=1",
        },
        ["task.cleanup-telemetry.name"] = new[]
        {
            "Disable telemetry scheduled tasks",
            "Inaktivera schemalagda telemetriuppgifter",
            "Deaktivér planlagte telemetriopgaver",
            "Deaktiver planlagte telemetrioppgaver",
            "Poista ajoitetut telemetriatehtävät käytöstä",
        },
        ["task.cleanup-telemetry.desc"] = new[]
        {
            "Disables the four CEIP/compatibility/disk telemetry scheduled tasks",
            "Inaktiverar de fyra schemalagda uppgifterna för CEIP-/kompatibilitets-/disktelemetri",
            "Deaktiverer de fire planlagte opgaver for CEIP-/kompatibilitets-/disktelemetri",
            "Deaktiverer de fire planlagte oppgavene for CEIP-/kompatibilitets-/disktelemetri",
            "Poistaa käytöstä neljä CEIP-/yhteensopivuus-/levytelemetrian ajoitettua tehtävää",
        },
        ["task.ntp-server.name"] = new[]
        {
            "NTP time server for cameras",
            "NTP-tidsserver för kameror",
            "NTP-tidsserver til kameraer",
            "NTP-tidsserver for kameraer",
            "NTP-aikapalvelin kameroille",
        },
        ["task.ntp-server.desc"] = new[]
        {
            "Windows Time as reliable NTP server (upstream source selectable in Settings, UDP 123 open, seconds in taskbar clock)",
            "Windows Time som tillförlitlig NTP-server (överordnad källa väljs i Inställningar, UDP 123 öppen, sekunder i aktivitetsfältets klocka)",
            "Windows Time som pålidelig NTP-server (overordnet kilde vælges i Indstillinger, UDP 123 åben, sekunder i proceslinjens ur)",
            "Windows Time som pålitelig NTP-server (overordnet kilde velges i Innstillinger, UDP 123 åpen, sekunder i oppgavelinjens klokke)",
            "Windows Time luotettavana NTP-palvelimena (ylätason lähde valittavissa Asetuksissa, UDP 123 avoinna, sekunnit tehtäväpalkin kellossa)",
        },
        ["task.pstools.name"] = new[]
        {
            "MilestonePSTools module",
            "MilestonePSTools-modulen",
            "MilestonePSTools-modulet",
            "MilestonePSTools-modulen",
            "MilestonePSTools-moduuli",
        },
        ["task.pstools.desc"] = new[]
        {
            "Installs MilestonePSTools via the official web installer (2026 R1 compatible)",
            "Installerar MilestonePSTools via den officiella webbinstalleraren (kompatibel med 2026 R1)",
            "Installerer MilestonePSTools via det officielle webinstallationsprogram (kompatibelt med 2026 R1)",
            "Installerer MilestonePSTools via det offisielle webinstallasjonsprogrammet (kompatibelt med 2026 R1)",
            "Asentaa MilestonePSToolsin virallisella verkkoasennusohjelmalla (yhteensopiva 2026 R1:n kanssa)",
        },
        ["task.ssh-server.name"] = new[]
        {
            "SSH server",
            "SSH-server",
            "SSH-server",
            "SSH-server",
            "SSH-palvelin",
        },
        ["task.ssh-server.desc"] = new[]
        {
            "OpenSSH Server with password auth for admins, autostart and TCP 22 open (optional)",
            "OpenSSH Server med lösenordsinloggning för administratörer, autostart och TCP 22 öppen (valfri)",
            "OpenSSH Server med adgangskode-login for administratorer, autostart og TCP 22 åben (valgfri)",
            "OpenSSH Server med passordpålogging for administratorer, autostart og TCP 22 åpen (valgfri)",
            "OpenSSH Server salasanakirjautumisella järjestelmänvalvojille, automaattinen käynnistys ja TCP 22 avoinna (valinnainen)",
        },
        ["task.snmp.name"] = new[]
        {
            "SNMP capabilities",
            "SNMP-funktioner",
            "SNMP-funktioner",
            "SNMP-funksjoner",
            "SNMP-ominaisuudet",
        },
        ["task.snmp.desc"] = new[]
        {
            "SNMP client and WMI SNMP provider for network monitoring systems (optional)",
            "SNMP-klient och WMI SNMP-provider för nätverksövervakningssystem (valfri)",
            "SNMP-klient og WMI SNMP-provider til netværksovervågningssystemer (valgfri)",
            "SNMP-klient og WMI SNMP-provider for nettverksovervåkingssystemer (valgfri)",
            "SNMP-asiakas ja WMI SNMP -provider verkonvalvontajärjestelmiä varten (valinnainen)",
        },
    };
}
