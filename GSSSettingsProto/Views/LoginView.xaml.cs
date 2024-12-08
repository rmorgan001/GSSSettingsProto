
namespace GSSSettingsProto.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView
    {
        public LoginView()
        {
            Settings.Profile.LoadDefaults();
            InitializeComponent();
        }
    }
}
