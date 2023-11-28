namespace DatenTresorNET.View
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    using DatenTresorNET.BaseFunction;
    using DatenTresorNET.Core;
    using DatenTresorNET.Model;

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
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(this.MenuExit, "Click", this.MenuExit_Click);

            this.DataContext = this;
        }

        public XamlProperty<Visibility> ShowNoDatabase { get; set; } = XamlProperty.Set<Visibility>();

        public XamlProperty<Visibility> ShowDatabase { get; set; } = XamlProperty.Set<Visibility>();

        public XamlProperty<Visibility> ShowSearchWaiting { get; set; } = XamlProperty.Set<Visibility>();

        private string DatabaseLocation { get; set; }

        private List<DatabaseParameter> DatabaseName { get; set; }

        public bool IsDatabase { get; set; }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ShowNoDatabase.Value = Visibility.Collapsed;
            this.ShowDatabase.Value = Visibility.Collapsed;
            this.ShowSearchWaiting.Value = Visibility.Visible;

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
                if (await this.AsyncSearchDatabase() == false)
                {
                    this.ShowSearchWaiting.Value = Visibility.Collapsed;
                    this.ShowDatabase.Value = Visibility.Collapsed;
                    this.ShowNoDatabase.Value = Visibility.Visible;
                }
                else
                {
                    this.ShowSearchWaiting.Value = Visibility.Collapsed;
                    this.ShowDatabase.Value = Visibility.Visible;
                    this.ShowNoDatabase.Value = Visibility.Collapsed;
                }
            }
        }

        public async Task<bool> AsyncSearchDatabase()
        {
            return await Task.Run(() =>
            {
                return this.SearchDatabase();
            });
        }
        private bool SearchDatabase()
        {
            bool result = false;

            if (Directory.Exists(this.DatabaseLocation) == false)
            {
                Directory.CreateDirectory(this.DatabaseLocation);
            }

            this.Dispatcher.Invoke(new Action(() => {
                var dbs = Directory.EnumerateFiles(this.DatabaseLocation, "*.db", SearchOption.TopDirectoryOnly);
                this.IsDatabase = dbs.Any();
                if (this.IsDatabase == true)
                {
                    if (this.DatabaseName == null)
                    {
                        this.DatabaseName = new List<DatabaseParameter>();
                    }

                    foreach (string db in dbs)
                    {
                        ConnectionString dbconn = this.Connection(db, string.Empty);
                        LiteDatabase litedb = new LiteDatabase(dbconn);
                        ILiteCollection<DatabaseInformation> databaseInformationCollection = litedb.GetCollection<DatabaseInformation>(typeof(DatabaseInformation).Name);
                        if (databaseInformationCollection.Count() == 1)
                        {
                            DatabaseInformation dbinfo = databaseInformationCollection.FindAll().First();

                            DatabaseParameter p = new DatabaseParameter();
                            p.Default = true;
                            p.DatabaseFolder = Path.GetDirectoryName(db);
                            p.DatabaseName = Path.GetFileName(db);
                            p.Description = dbinfo.Description;
                            this.DatabaseName.Add(p);
                        }
                        else
                        {
                        }
                    }

                    result = true;
                }
                else
                {
                    StatusbarContent.DatabaseInfo = "Keine Datenbank vorhanden!";
                    Task.Delay(5000);
                }

            }), DispatcherPriority.Background);

            return result;
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsActive == true)
            {
                this.DialogResult = false;
                this.Close();
            }
        }

        private void OnApplicationStart(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void MenuSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDatabaseAdd_Click(object sender, RoutedEventArgs e)
        {
            string dataBase = this.TxtDatabaseName.Text;
            string password = this.TxtNewPassword.Password;
            string fullName = Path.Combine(this.DatabaseLocation, $"{dataBase}.db");

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
                }

                DatabaseParameter dp = new DatabaseParameter();
                dp.Default = true;
                dp.DatabaseName = dataBase;
                dp.DatabaseFolder = this.DatabaseLocation;
                dp.Description = this.TxtDescription.Text;
                dp.PasswordHash = password;
                if (settings.Databases == null)
                {
                    settings.Databases = new List<DatabaseParameter>();
                }

                settings.Databases.Add( dp );

                settings.Save();
            }

            ConnectionString dbconn = this.Connection(fullName, string.Empty);
            LiteDatabase litedb = new LiteDatabase(dbconn);
            litedb.UserVersion = 3;
            if (litedb != null)
            {
                DatabaseInformation di = new DatabaseInformation();
                di.Id = Guid.NewGuid();
                di.Name = dataBase;
                di.Description = this.TxtDescription.Text;
                di.CreatedBy = UserInfo.TS().CurrentUser;
                di.CreatedOn = UserInfo.TS().CurrentTime;
                ILiteCollection<DatabaseInformation> collection = litedb.GetCollection<DatabaseInformation>(typeof(DatabaseInformation).Name);
                collection.Insert(di);
                litedb.Commit();
            }
        }

        private void BtnApplicationExit_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsActive == true)
            {
                this.DialogResult = false;
                this.Close();
            }
        }

        private ConnectionString Connection(string databaseFile, string password)
        {
            ConnectionString conn = new ConnectionString(databaseFile);
            conn.Connection = ConnectionType.Shared;
            conn.Password = password;

            return conn;
        }
    }
}
