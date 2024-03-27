namespace DatenTresorNET.View.ViewControl
{
    using System;
    using System.CodeDom;
    using System.Windows;
    using System.Windows.Controls;

    using DatenTresorNET.BaseFunction;
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
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnApplicationExit, "Click", this.OnApplicationExit);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnDatabaseStart, "Click", this.OnDatabaseStart);
            WeakEventManager<ComboBox, SelectionChangedEventArgs>.AddHandler(this.cbDatabaseNamesSource, "SelectionChanged", this.OnSelectionChanged);

            this.DataContext = this;
        }

        public XamlProperty<List<DatabaseParameter>> DatabaseNamesSource { get; set; } = XamlProperty.Set<List<DatabaseParameter>>();

        public XamlProperty<DatabaseParameter> DatabaseNameSelected { get; set; } = XamlProperty.Set<DatabaseParameter>(x => { StatusbarContent.DatabaseInfo = x.DatabaseName; });

        private string DatabaseLocation { get; set; }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            using (ApplicationSettings settings = new ApplicationSettings())
            {
                if (settings.IsExitSettings() == true)
                {
                    settings.Load();
                    this.DatabaseLocation = settings.DatabaseFolder;
                }
            }

            using (DatabaseSearcher ds = new DatabaseSearcher(this.DatabaseLocation))
            {
                if (await ds.SearchDatabaseAsync() == true)
                {
                    this.DatabaseNamesSource.Value = ds.DatabaseNamesSource;
                    this.DatabaseNameSelected.Value = ds.DatabaseNameSelected;
                }
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DatabaseParameter current = e.AddedItems[0] as DatabaseParameter;
            if (current != null)
            {
                App.EventAgg.Publish<SelectDatabaseEventArgs>(new SelectDatabaseEventArgs { Sender = typeof(FoundDatabaseUC), SelectDatabase = current });
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

        private void OnApplicationExit(object sender, RoutedEventArgs e)
        {
            Window window = Application.Current.Windows.Cast<Window>().Single(s => s.IsActive == true);
            if (window.IsActive == true)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        private void OnDatabaseStart(object sender, RoutedEventArgs e)
        {
            Window window = Application.Current.Windows.Cast<Window>().Single(s => s.GetType() == typeof(StartScreen));
            if (window.IsActive == true)
            {
                window.DialogResult = true;
                window.Close();

                using (ApplicationSettings settings = new ApplicationSettings())
                {
                    if (settings.IsExitSettings() == true)
                    {
                        settings.Load();
                    }

                    if (settings.Databases?.Any() == true)
                    {
                        foreach (DatabaseParameter dbParam in settings.Databases)
                        {
                            dbParam.Default = false;
                        }

                        string selectedName = this.DatabaseNameSelected.Value.DatabaseName;
                        DatabaseParameter dp = settings.Databases.First(f => f.DatabaseName.ToLower() == selectedName.ToLower());
                        dp.Default = true;
                        settings.Save();
                    }
                }
            }
        }
    }
}
