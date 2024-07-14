namespace DatenTresorNET.View
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    using DatenTresorNET.BaseFunction;
    using DatenTresorNET.Controls;
    using DatenTresorNET.Core;
    using DatenTresorNET.View.ViewControl;

    using LiteDB;

    /// <summary>
    /// Interaktionslogik für StartScreen.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        private INotificationService _notificationService = new NotificationService();

        public StartScreen()
        {
            this.InitializeComponent();

            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnApplicationExit, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnAddDatabase, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnDatabaseDelete, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnInfo, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnDatabaseStart, "Click", this.OnButtonClick);

            App.EventAgg.Subscribe<SelectDatabaseEventArgs>(this.SetDatabase);
            App.EventAgg.Subscribe<MessageEventArgs>(this.SetMessageQuestion);

            NotificationService.RegisterDialog<MessageOK>();

            this.DataContext = this;
        }

        public XamlProperty<UserControl> CurrentControl { get; set; } = XamlProperty.Set<UserControl>();

        private string DatabaseLocation { get; set; }

        private DatabaseParameter CurrentSelectedDatabase { get; set; }

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

            if (string.IsNullOrEmpty(this.DatabaseLocation) == false)
            {
                using (DatabaseSearcher ds = new DatabaseSearcher(this.DatabaseLocation))
                {
                    if (await ds.SearchDatabaseAsync() == false)
                    {
                        this.BtnAddDatabase.IsEnabled = false;
                        this.BtnInfo.IsEnabled = false;
                        this.BtnDatabaseDelete.IsEnabled = false;
                        this.BtnDatabaseStart.IsEnabled = false;
                        this.CurrentControl.Value = new AddNewDatabaseUC();
                        if (await ds.SearchDatabaseAsync() == true)
                        {
                            this.BtnAddDatabase.IsEnabled = true;
                            this.BtnInfo.IsEnabled = true;
                            this.BtnDatabaseDelete.IsEnabled = true;
                            this.BtnDatabaseStart.IsEnabled = true;
                        }
                    }
                    else
                    {
                        this.SetCurrentDialog(SelectDialog.SelectDatabase);
                    }
                }
            }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ToolbarButtons tagEnum = (ToolbarButtons)button.Tag;
            if (tagEnum == ToolbarButtons.ApplicationExit)
            {
                this.DialogResult = false;
                this.Close();
            }
            else if (tagEnum == ToolbarButtons.AddDatabase)
            {
                this.CreateNewDatabase();
            }
            else if (tagEnum == ToolbarButtons.DeleteDatabase)
            {
                this.DeleteDatabase();
            }
            else if (tagEnum == ToolbarButtons.InfoDatabase)
            {
                this.InfoDatabase();
            }
            else if (tagEnum == ToolbarButtons.StartDatabase)
            {
                this.DatabaseStart();
            }
        }

        private void SetDatabase(SelectDatabaseEventArgs args)
        {
            this.CurrentSelectedDatabase = args.SelectDatabase;
        }

        private void SetMessageQuestion(MessageEventArgs args)
        {
            if (args.Sender == typeof(QuestionDlg) && args.MsgQuestion == MessageQuestion.Yes)
            {
            }
            else if (args.Sender == typeof(QuestionDlg) && args.MsgQuestion == MessageQuestion.No)
            {
                this.SetCurrentDialog(SelectDialog.SelectDatabase);
            }
            else if (args.Sender == typeof(QuestionDlg) && args.MsgQuestion == MessageQuestion.Ok)
            {
                this.SetCurrentDialog(SelectDialog.SelectDatabase);
            }
            else if (args.Sender == typeof(FoundDatabaseUC) && args.MsgQuestion == MessageQuestion.Delete)
            {
                string deleteDB = Path.Combine(this.CurrentSelectedDatabase.DatabaseFolder, this.CurrentSelectedDatabase.DatabaseName);
                if (File.Exists(deleteDB) == true)
                {
                    File.Delete(deleteDB);
                }

                this.CheckOfDatebase();
                this.SetCurrentDialog(SelectDialog.SelectDatabase);
            }
            if (args.Sender == typeof(AddNewDatabaseUC) && args.MsgQuestion == MessageQuestion.Add)
            {
                this.CheckOfDatebase();
                this.SetCurrentDialog(SelectDialog.SelectDatabase);
            }
            else
            {
                this.CheckOfDatebase();
                this.SetCurrentDialog(SelectDialog.SelectDatabase);
            }
        }

        private void DeleteDatabase()
        {
            if (this.CurrentSelectedDatabase != null)
            {
                string dbname = this.CurrentSelectedDatabase.DatabaseName;
                QuestionDlg msgDlg = new QuestionDlg();
                msgDlg.Title = "Ausgewählte Datenbank löschen";
                msgDlg.Description = $"Wollen Sie die ausgewählte Datenbank '{dbname}' entgültig löschen?\nDie Daten können nach dem löschen nicht mehr wiederhergestellt werden.";
                msgDlg.DescFontWeight = FontWeights.Bold;
                this.CurrentControl.Value = msgDlg;
            }
        }

        private void InfoDatabase()
        {
            if (this.CurrentSelectedDatabase != null)
            {
                string dbname = this.CurrentSelectedDatabase.DatabaseName;
                QuestionDlg msgDlg = new QuestionDlg();
                msgDlg.Title = "Information zur ausgewählten Datenbank";
                msgDlg.Description = $"Info.";
                msgDlg.DescFontWeight = FontWeights.Bold;
                msgDlg.ShowButtonNo = false;
                msgDlg.ShowButtonYes = true;
                this.CurrentControl.Value = msgDlg;
            }
            else
            {
                _ = this._notificationService.HinweisOk("Es kann keine Datenbankinformationen angezeigt werden!");
            }
        }

        private void CreateNewDatabase()
        {
            this.SetCurrentDialog(SelectDialog.AddNewDatabase);
        }

        private void SetCurrentDialog(SelectDialog selectDlg)
        {
            if (selectDlg == SelectDialog.AddNewDatabase)
            {
                this.CurrentControl.Value = new AddNewDatabaseUC();
            }
            else if (selectDlg == SelectDialog.SelectDatabase)
            {
                this.CurrentControl.Value = new FoundDatabaseUC();
            }
        }

        private async void CheckOfDatebase()
        {
            if (string.IsNullOrEmpty(this.DatabaseLocation) == false)
            {
                using (DatabaseSearcher ds = new DatabaseSearcher(this.DatabaseLocation))
                {
                    if (await ds.SearchDatabaseAsync() == false)
                    {
                        this.BtnAddDatabase.IsEnabled = false;
                        this.BtnInfo.IsEnabled = false;
                        this.BtnDatabaseDelete.IsEnabled = false;
                        this.BtnDatabaseStart.IsEnabled = false;
                        this.CurrentControl.Value = new AddNewDatabaseUC();
                        if (await ds.SearchDatabaseAsync() == true)
                        {
                            this.BtnAddDatabase.IsEnabled = true;
                            this.BtnInfo.IsEnabled = true;
                            this.BtnDatabaseDelete.IsEnabled = true;
                            this.BtnDatabaseStart.IsEnabled = true;
                        }
                    }
                    else
                    {
                        this.BtnAddDatabase.IsEnabled = true;
                        this.BtnInfo.IsEnabled = true;
                        this.BtnDatabaseDelete.IsEnabled = true;
                        this.BtnDatabaseStart.IsEnabled = true;
                        this.SetCurrentDialog(SelectDialog.SelectDatabase);
                    }
                }
            }
        }

        private void DatabaseStart()
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

                        string selectedName = this.CurrentSelectedDatabase.DatabaseName;
                        DatabaseParameter dp = settings.Databases.First(f => f.DatabaseName.ToLower() == selectedName.ToLower());
                        dp.Default = true;
                        settings.Save();
                    }
                }
            }
        }
    }
}
