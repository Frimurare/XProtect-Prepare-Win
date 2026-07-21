using System.Windows;
using Microsoft.Win32;

namespace XProtectPrepareWin.UI;

/// <summary>
/// Follows the Windows apps-light/dark preference (AppsUseLightTheme) by merging
/// UI/ThemeLight.xaml or UI/ThemeDark.xaml into the application resources, and
/// swaps them live when the user changes the Windows setting.
/// </summary>
internal static class ThemeManager
{
    private const string PersonalizeKey = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

    private static bool _initialized;
    private static bool _isLight;

    /// <summary>Raised on the UI thread after the palette dictionary has been swapped.</summary>
    public static event Action? ThemeChanged;

    public static bool IsLight => _isLight;

    /// <summary>Read the Windows preference and merge the matching palette. Call once at startup.</summary>
    public static void Init()
    {
        if (_initialized) return;
        _initialized = true;
        _isLight = ReadSystemIsLight();
        ApplyDictionary(_isLight);
        SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;
    }

    /// <summary>Unhook the SystemEvents subscription (static event - must be released on exit).</summary>
    public static void Shutdown()
    {
        if (!_initialized) return;
        _initialized = false;
        try { SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged; } catch { }
    }

    private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
    {
        if (e.Category != UserPreferenceCategory.General) return;
        var app = Application.Current;
        if (app == null) return;
        app.Dispatcher.InvokeAsync(() =>
        {
            bool light = ReadSystemIsLight();
            if (light == _isLight) return;
            _isLight = light;
            ApplyDictionary(light);
            ThemeChanged?.Invoke();
        });
    }

    /// <summary>
    /// HKCU ...\Themes\Personalize\AppsUseLightTheme: 1 = light, 0 = dark.
    /// A truly absent value means the OS renders apps light (default Windows
    /// Server images normally carry the value 1 explicitly).
    /// </summary>
    private static bool ReadSystemIsLight()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(PersonalizeKey);
            if (key?.GetValue("AppsUseLightTheme") is int value)
                return value != 0;
        }
        catch { }
        return true;
    }

    private static void ApplyDictionary(bool light)
    {
        var app = Application.Current;
        if (app == null) return;

        var dict = new ResourceDictionary
        {
            Source = new Uri(light
                ? "pack://application:,,,/UI/ThemeLight.xaml"
                : "pack://application:,,,/UI/ThemeDark.xaml", UriKind.Absolute)
        };

        var merged = app.Resources.MergedDictionaries;
        for (int i = merged.Count - 1; i >= 0; i--)
        {
            var src = merged[i].Source?.OriginalString ?? "";
            if (src.Contains("ThemeDark.xaml", StringComparison.OrdinalIgnoreCase) ||
                src.Contains("ThemeLight.xaml", StringComparison.OrdinalIgnoreCase))
            {
                merged.RemoveAt(i);
            }
        }
        merged.Add(dict); // after Theme.xaml, which stays at index 0
    }
}
