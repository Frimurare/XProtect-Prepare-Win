using System.Windows.Markup;

namespace XProtectPrepareWin.UI;

/// <summary>
/// XAML markup extension for localized strings:
///   xmlns:l="clr-namespace:XProtectPrepareWin.UI"
///   Text="{l:Loc dash.subtitle}"  or  Content="{l:Loc Key=nav.exit}"
/// Resolved once at parse time - language switches require an app restart.
/// </summary>
[MarkupExtensionReturnType(typeof(string))]
public sealed class LocExtension : MarkupExtension
{
    public string Key { get; set; }

    public LocExtension() => Key = "";

    public LocExtension(string key) => Key = key;

    public override object ProvideValue(IServiceProvider serviceProvider) => Loc.T(Key);
}
