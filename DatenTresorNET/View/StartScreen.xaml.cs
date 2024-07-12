namespace DatenTresorNET.View
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Xml.Linq;

    using DatenTresorNET.BaseFunction;
    using DatenTresorNET.Core;
    using DatenTresorNET.View.ViewControl;

    using LiteDB;

    /// <summary>
    /// Interaktionslogik für StartScreen.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        public StartScreen()
        {
            this.InitializeComponent();

            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnApplicationExit, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnAddDatabase, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnDatabaseDelete, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnInfo, "Click", this.OnButtonClick);

            App.EventAgg.Subscribe<SelectDatabaseEventArgs>(this.SetDatabase);
            App.EventAgg.Subscribe<MessageEventArgs>(this.SetMessageQuestion);

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
                        this.CurrentControl.Value = new AddNewDatabaseUC();
                        if (await ds.SearchDatabaseAsync() == true)
                        {

                        }
                    }
                    else
                    {
                        this.SetCurrentDialog(SelectDialog.SelectDatabase);
                    }
                }
            }
        }

        /*
        private async void BtnDatabaseAdd_Click(object sender, RoutedEventArgs e)
        {
            string dataBase = this.TxtDatabaseName.Text;
            string password = this.TxtNewPassword.Password;
            string fullName = System.IO.Path.Combine(this.DatabaseLocation, $"{dataBase}.db");

            this.BtnDatabaseAdd.IsEnabled = false;

            ConnectionString dbconn = this.Connection(fullName, string.Empty);
            LiteDatabase litedb = new LiteDatabase(dbconn);
            litedb.UserVersion = 3;
            if (litedb != null)
            {
                DatabaseInformation di = new DatabaseInformation();
                di.Id = Guid.NewGuid();
                di.Name = dataBase;
                di.Password = password;
                di.Description = this.TxtDescription.Text;
                di.CreatedBy = UserInfo.TS().CurrentUser;
                di.CreatedOn = UserInfo.TS().CurrentTime;
                ILiteCollection<DatabaseInformation> collection = litedb.GetCollection<DatabaseInformation>(typeof(DatabaseInformation).Name);
                collection.Insert(di);
                litedb.Commit();

                if (string.IsNullOrEmpty(this.DatabaseLocation) == false)
                {
                    if (await this.SearchDatabaseAsync() == false)
                    {
                        this.SetCurrentDialog(SelectDialog.AddNewDatabase);
                    }
                    else
                    {
                        this.SetCurrentDialog(SelectDialog.SelectDatabase);
                    }
                }
            }
        }
        */

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
        }

        private void SetDatabase(SelectDatabaseEventArgs args)
        {
            this.CurrentSelectedDatabase = args.SelectDatabase;
        }

        private async void SetMessageQuestion(MessageEventArgs args)
        {
            if (args.Sender == typeof(QuestionDlg) && args.MsgQuestion == MessageQuestion.Yes)
            {
                string fullname = Path.Combine(this.CurrentSelectedDatabase.DatabaseFolder, this.CurrentSelectedDatabase.DatabaseName);
                if (File.Exists(fullname) == true)
                {
                    //File.Delete(fullname);

                    using (DatabaseSearcher ds = new DatabaseSearcher(this.DatabaseLocation))
                    {
                        if (await ds.SearchDatabaseAsync() == false)
                        {
                            this.SetCurrentDialog(SelectDialog.AddNewDatabase);
                        }
                        else
                        {
                            this.SetCurrentDialog(SelectDialog.SelectDatabase);
                        }
                    }
                }
            }
            else if (args.Sender == typeof(QuestionDlg) && args.MsgQuestion == MessageQuestion.No)
            {
                this.SetCurrentDialog(SelectDialog.SelectDatabase);
            }
            else if (args.Sender == typeof(QuestionDlg) && args.MsgQuestion == MessageQuestion.Ok)
            {
                this.SetCurrentDialog(SelectDialog.SelectDatabase);
            }
            if (args.Sender == typeof(AddNewDatabaseUC) && args.MsgQuestion == MessageQuestion.Yes)
            {
            }
            else
            {
                this.SetCurrentDialog(SelectDialog.SelectDatabase);
            }
        }

        private ConnectionString Connection(string databaseFile, string password)
        {
            ConnectionString conn = new ConnectionString(databaseFile);
            conn.Connection = ConnectionType.Shared;
            if (string.IsNullOrEmpty(password) == false)
            {
                conn.Password = password;
            }

            return conn;
        }

        private void DeleteDatabase()
        {
            string dbname = this.CurrentSelectedDatabase.DatabaseName;
            QuestionDlg msgDlg = new QuestionDlg();
            msgDlg.Title = "Ausgewählte Datenbank löschen";
            msgDlg.Description = $"Wollen Sie die ausgewählte Datenbank '{dbname}' entgültig löschen?\nDie Daten können nach dem löschen nicht mehr wiederhergestellt werden.";
            msgDlg.DescFontWeight = FontWeights.Bold;
            this.CurrentControl.Value = msgDlg;
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
        }

        private void CreateNewDatabase()
        {
            this.SetCurrentDialog(SelectDialog.AddNewDatabase);
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

        private void DatabaseName_TextChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            { 
                if (textBox.Text.Length > 0)
                {
                    //this.BtnDatabaseAdd.IsEnabled = true;
                }
                else
                {
                    //this.BtnDatabaseAdd.IsEnabled = false;
                }
            }
        }

        private void BtnCreatePassword_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
