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
    }
}
