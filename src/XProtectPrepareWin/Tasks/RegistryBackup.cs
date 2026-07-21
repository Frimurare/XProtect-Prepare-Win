using System.Globalization;
using System.IO;

namespace XProtectPrepareWin.Tasks;

public static class RegistryBackup
{
    private static readonly string[] Keys =
    {
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate",
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services",
    };

    /// <summary>reg.exe export of the keys this tool touches, one .reg file per key.
    /// Returns the backup folder path. Per-key failures are logged, not fatal
    /// (the WindowsUpdate policy key may simply not exist yet).</summary>
    public static async Task<string> CreateAsync(PrepContext ctx, string name)
    {
        var folder = Path.Combine(PrepContext.LogFolder, "RegistryBackups");
        Directory.CreateDirectory(folder);
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);

        foreach (var key in Keys)
        {
            var keyFileName = key.Replace("\\", "_").Replace(":", "");
            var target = Path.Combine(folder, $"{name}_{keyFileName}_{timestamp}.reg");
            var r = await ctx.RunAsync("reg.exe", $"export \"{key}\" \"{target}\" /y", 300);
            if (!r.Ok)
                ctx.Log($"Registry backup of {key} failed: {Ps.Truncate(r.AllOutput, 200)}");
        }
        return folder;
    }
}
