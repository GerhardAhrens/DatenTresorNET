namespace DatenTresorNET.View
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(this.MenuExit, "Click", this.BtnApplicationExit_Click);

            this.DataContext = this;
        }

        public XamlProperty<Visibility> ShowNoDatabase { get; set; } = XamlProperty.Set<Visibility>();

        public XamlProperty<Visibility> ShowDatabase { get; set; } = XamlProperty.Set<Visibility>();

        public XamlProperty<Visibility> ShowSearchWaiting { get; set; } = XamlProperty.Set<Visibility>();

        public XamlProperty<List<DatabaseParameter>> DatabaseNamesSource { get; set; } = XamlProperty.Set<List<DatabaseParameter>>();

        public XamlProperty<DatabaseParameter> DatabaseNameSelected { get; set; } = XamlProperty.Set<DatabaseParameter>(x => { StatusbarContent.DatabaseInfo = x.DatabaseName;});

        public bool IsDatabase { get; set; }

        public string CurrentPassword { get; set; }

        private string DatabaseLocation { get; set; }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.SetCurrentDialog(SelectDialog.Waiting);

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

        public async Task<bool> SearchDatabaseAsync()
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
                    if (this.DatabaseNamesSource.Value == null)
                    {
                        this.DatabaseNamesSource.Value = new List<DatabaseParameter>();
                    }

                    foreach (string db in dbs)
                    {
                        ConnectionString dbconn = this.Connection(db, null);
                        LiteDatabase litedb = new LiteDatabase(dbconn);
                        ILiteCollection<DatabaseInformation> databaseInformationCollection = litedb.GetCollection<DatabaseInformation>(typeof(DatabaseInformation).Name);
                        DatabaseInformation dbinfo = databaseInformationCollection.FindAll().First();

                        DatabaseParameter dbparam = new DatabaseParameter();
                        if (dbs.Count() == 1)
                        {
                            dbparam.Default = true;
                        }
                        else
                        {
                            dbparam.Default = false;
                        }

                        dbparam.DatabaseFolder = System.IO.Path.GetDirectoryName(db);
                        dbparam.DatabaseName = System.IO.Path.GetFileName(db);
                        dbparam.Description = dbinfo.Description;
                        this.DatabaseNamesSource.Value.Add(dbparam);
                        litedb.Dispose();
                        litedb = null;
                    }

                    using (ApplicationSettings settings = new ApplicationSettings())
                    {
                        if (settings.IsExitSettings() == true)
                        {
                            settings.Load();
                        }

                        if (settings.Databases.Count() != dbs.Count())
                        {
                            settings.Databases.Clear();
                            if (this.DatabaseNamesSource.Value != null)
                            {
                                foreach (DatabaseParameter item in this.DatabaseNamesSource.Value)
                                {
                                    DatabaseParameter dp = new DatabaseParameter();
                                    dp.DatabaseName = item.DatabaseName;
                                    dp.DatabaseFolder = this.DatabaseLocation;
                                    dp.Description = item.Description;
                                    dp.PasswordHash = item.PasswordHash;
                                    settings.Databases.Add(dp);
                                }

                                settings.Save();
                            }

                        }
                        else
                        {
                            foreach (DatabaseParameter item in this.DatabaseNamesSource.Value)
                            {
                                if (settings.Databases.Count() == 1)
                                {
                                    item.Default = true;
                                    break;
                                }
                                else
                                {
                                    if (item.DatabaseName == settings.Databases.FirstOrDefault(f => f.Default == true).DatabaseName)
                                    {
                                        item.Default = true;
                                    }
                                }
                            }

                            this.DatabaseNamesSource.Value = this.DatabaseNamesSource.Value.OrderBy(x => x.Default).ToList();
                            this.DatabaseNameSelected.Value = this.DatabaseNamesSource.Value.FirstOrDefault(f => f.Default == true);
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

        private void BtnDatabaseAddNew_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.DatabaseLocation) == false)
            {
                this.ShowSearchWaiting.Value = Visibility.Collapsed;
                this.ShowDatabase.Value = Visibility.Collapsed;
                this.ShowNoDatabase.Value = Visibility.Visible;
                this.BtnDatabaseDelete.Visibility = Visibility.Collapsed;
                this.BtnDatabaseAdd.IsEnabled = false;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.ShowSearchWaiting.Value = Visibility.Collapsed;
            this.ShowDatabase.Value = Visibility.Visible;
            this.ShowNoDatabase.Value = Visibility.Collapsed;
            this.BtnDatabaseDelete.Visibility = Visibility.Visible;
        }

        private void BtnDatabaseStart_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsActive == true)
            {
                this.DialogResult = true;
                this.Close();

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

                        string selectedName = DatabaseNameSelected.Value.DatabaseName;
                        DatabaseParameter dp = settings.Databases.First(f => f.DatabaseName.ToLower() == selectedName.ToLower());
                        dp.Default = true;
                        settings.Save();
                    }
                }

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

        private void MenuSettings_Click(object sender, RoutedEventArgs e)
        {
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

        private async void BtnDatabaseDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectEntry = this.cbDatabaseNamesSource.SelectedValue;
            string dbroot = ((DatabaseParameter)selectEntry).DatabaseFolder;
            string dbname = ((DatabaseParameter)selectEntry).DatabaseName;
            string fullname = System.IO.Path.Combine(dbroot, dbname);
            if (File.Exists(fullname) == true)
            {
                File.Delete(fullname);

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

        private void TxtCurrentPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null && sender is PasswordBox passwordBox)
            {
                ((dynamic)this.DataContext).CurrentPassword = passwordBox.Password;
                if (passwordBox.Password.Length > 0)
                {
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
                this.ShowSearchWaiting.Value = Visibility.Collapsed;
                this.ShowDatabase.Value = Visibility.Collapsed;
                this.ShowNoDatabase.Value = Visibility.Visible;
                this.BtnBack.Visibility = Visibility.Collapsed;
                this.BtnDatabaseDelete.Visibility = Visibility.Collapsed;

                this.BtnDatabaseDelete.IsEnabled = false;
                this.BtnDatabaseStart.IsEnabled = false;
            }
            else if (selectDlg == SelectDialog.SelectDatabase)
            {
                this.ShowSearchWaiting.Value = Visibility.Collapsed;
                this.ShowDatabase.Value = Visibility.Visible;
                this.ShowNoDatabase.Value = Visibility.Collapsed;
                this.BtnBack.Visibility = Visibility.Visible;
                this.BtnDatabaseDelete.Visibility = Visibility.Visible;

                this.BtnDatabaseDelete.IsEnabled = true;
                this.BtnDatabaseStart.IsEnabled = true;
            }
            else if (selectDlg == SelectDialog.Waiting)
            {
                this.ShowNoDatabase.Value = Visibility.Collapsed;
                this.ShowDatabase.Value = Visibility.Collapsed;
                this.ShowSearchWaiting.Value = Visibility.Visible;
            }
            else if (selectDlg == SelectDialog.DeleteDatabase)
            {
            }
        }
    }
}
