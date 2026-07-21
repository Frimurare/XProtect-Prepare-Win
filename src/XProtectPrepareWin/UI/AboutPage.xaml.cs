using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;
using XProtectPrepareWin.Tasks;

namespace XProtectPrepareWin.UI;

public partial class AboutPage : UserControl
{
    public AboutPage()
    {
        InitializeComponent();
        VersionText.Text = Loc.T("about.version") + " " + AppVersion.Version;
    }

    private void GitHubLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        }
        catch { }
        e.Handled = true;
    }
}
