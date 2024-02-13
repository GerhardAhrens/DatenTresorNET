namespace DatenTresorNET.View.ViewControl
{
    using System.Windows;
    using System.Windows.Controls;

    using DatenTresorNET.Core;

    /// <summary>
    /// Interaktionslogik für FoundDatabaseUC.xaml
    /// </summary>
    public partial class FoundDatabaseUC : UserControl
    {
        public FoundDatabaseUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private string DatabaseLocation { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            using (ApplicationSettings settings = new ApplicationSettings())
            {
                if (settings.IsExitSettings() == true)
                {
                    settings.Load();
                    this.DatabaseLocation = settings.DatabaseFolder;
                }
            }

        }

        private void TxtCurrentPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null && sender is PasswordBox passwordBox)
            {
                ((dynamic)this.DataContext).CurrentPassword = passwordBox.Password;
                if (passwordBox.Password.Length > 0)
                {
                    //this.BtnCreatePassword.IsEnabled = true;
                }
                else
                {
                    //this.BtnCreatePassword.IsEnabled = false;
                }
            }
        }

        private void BtnApplicationExit_Click(object sender, RoutedEventArgs e)
        {
            Window window = Application.Current.Windows.Cast<Window>().Single(s => s.IsActive == true);
            if (window.IsActive == true)
            {
                window.DialogResult = false;
                window.Close();
            }
        }
    }
}
