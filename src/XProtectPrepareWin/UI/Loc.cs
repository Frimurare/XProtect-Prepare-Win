using System.Globalization;

namespace XProtectPrepareWin.UI;

/// <summary>
/// Tiny localization engine. Strings live in <see cref="LocStrings"/> as
/// [en, sv, da, no] arrays. Init once at startup - language changes need an
/// app restart, so lookups never have to be observable.
/// </summary>
public static class Loc
{
    /// <summary>Active language code: "en", "sv", "da" or "no".</summary>
    public static string Lang { get; private set; } = "en";

    private static int _index;

    /// <summary>
    /// Resolve the active language from the persisted setting ("auto", "en",
    /// "sv", "da" or "no"). "auto" follows the Windows display language,
    /// with nb/nn/no all mapping to Norwegian bokmål.
    /// </summary>
    public static void Init(string? setting)
    {
        var code = (setting ?? "auto").Trim().ToLowerInvariant();
        if (code.Length == 0 || code == "auto")
        {
            var ui = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
            code = ui switch
            {
                "sv" => "sv",
                "da" => "da",
                "nb" or "nn" or "no" => "no",
                _ => "en",
            };
        }

        Lang = code switch { "sv" => "sv", "da" => "da", "no" => "no", _ => "en" };
        _index = Lang switch { "sv" => 1, "da" => 2, "no" => 3, _ => 0 };
    }

    /// <summary>Translate a key. Falls back to English, then to the key itself.</summary>
    public static string T(string key)
    {
        if (LocStrings.Table.TryGetValue(key, out var arr))
        {
            if (_index < arr.Length && !string.IsNullOrEmpty(arr[_index])) return arr[_index];
            if (arr.Length > 0 && !string.IsNullOrEmpty(arr[0])) return arr[0];
        }
        return key;
    }

    /// <summary>Translate a key. Falls back to English, then to the supplied fallback
    /// (used for task names/descriptions where the engine string is the source of truth).</summary>
    public static string T(string key, string fallback)
    {
        if (LocStrings.Table.TryGetValue(key, out var arr))
        {
            if (_index < arr.Length && !string.IsNullOrEmpty(arr[_index])) return arr[_index];
            if (arr.Length > 0 && !string.IsNullOrEmpty(arr[0])) return arr[0];
        }
        return fallback;
    }
}
