namespace XProtectPrepareWin.Tasks;

public sealed class MilestonePSToolsTask : IPrepTask
{
    public string Id => "pstools";
    public string Name => "MilestonePSTools module";
    public string Category => Categories.PSTools;
    public string Description => "Installs MilestonePSTools via the official web installer (2026 R1 compatible)";
    public bool Optional => false;
    public bool CheckOnly => false;

    public async Task<CheckResult> CheckAsync(PrepContext ctx)
    {
        var r = await ctx.PowerShellAsync(
            "$m = Get-Module -ListAvailable -Name MilestonePSTools -ErrorAction SilentlyContinue | " +
            "Sort-Object Version -Descending | Select-Object -First 1\n" +
            "if ($m) { 'VERSION=' + $m.Version } else { 'VERSION=' }", 180);

        var version = r.StdOut.Split('\n')
            .Select(l => l.Trim())
            .FirstOrDefault(l => l.StartsWith("VERSION=", StringComparison.Ordinal))?[8..] ?? "";

        return version.Length > 0
            ? CheckResult.Done($"MilestonePSTools {version} installed")
            : CheckResult.NotDone("MilestonePSTools module not installed");
    }

    public async Task ApplyAsync(PrepContext ctx)
    {
        // Official one-line web installer - always pulls a build new enough for 2026 R1.
        // The old Install-Module path could leave a module too old to connect to 2026 R1.
        var r = await ctx.PowerShellAsync(
            "$ErrorActionPreference = 'Stop'\n" +
            "Set-ExecutionPolicy RemoteSigned -Scope Process -Force -ErrorAction SilentlyContinue\n" +
            "[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072\n" +
            "Invoke-Expression (Invoke-RestMethod 'https://www.milestonepstools.com/install.ps1')", 900);
        if (!r.Ok)
            throw new InvalidOperationException(
                "MilestonePSTools web installer failed - internet access to milestonepstools.com " +
                "and the PowerShell Gallery is required. " + Ps.Truncate(r.AllOutput, 400));
    }
}
