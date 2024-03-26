﻿namespace DatenTresorNET.View
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

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

            this.DataContext = this;
        }

        public XamlProperty<Visibility> ShowNoDatabase { get; set; } = XamlProperty.Set<Visibility>();

        public XamlProperty<Visibility> ShowDatabase { get; set; } = XamlProperty.Set<Visibility>();

        public XamlProperty<Visibility> ShowSearchWaiting { get; set; } = XamlProperty.Set<Visibility>();

        public XamlProperty<Visibility> ShowQuestionYesNo { get; set; } = XamlProperty.Set<Visibility>();

        public XamlProperty<List<DatabaseParameter>> DatabaseNamesSource { get; set; } = XamlProperty.Set<List<DatabaseParameter>>();

        public XamlProperty<DatabaseParameter> DatabaseNameSelected { get; set; } = XamlProperty.Set<DatabaseParameter>(x => { StatusbarContent.DatabaseInfo = x.DatabaseName;});

        public XamlProperty<UserControl> CurrentControl { get; set; } = XamlProperty.Set<UserControl>();

        public bool IsDatabase { get; set; }

        public string CurrentPassword { get; set; }

        private string DatabaseLocation { get; set; }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.SetCurrentDialog(SelectDialog.QuestionDlg);

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
                        this.CurrentControl.Value = new NoFoundDatabaseUC();
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

        private void BtnDatabaseAddNew_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.DatabaseLocation) == false)
            {
                this.ShowSearchWaiting.Value = Visibility.Collapsed;
                this.ShowDatabase.Value = Visibility.Collapsed;
                this.ShowNoDatabase.Value = Visibility.Visible;
                //this.BtnDatabaseDelete.Visibility = Visibility.Collapsed;
                //this.BtnDatabaseAdd.IsEnabled = false;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.ShowSearchWaiting.Value = Visibility.Collapsed;
            this.ShowDatabase.Value = Visibility.Visible;
            this.ShowNoDatabase.Value = Visibility.Collapsed;
            //this.BtnDatabaseDelete.Visibility = Visibility.Visible;
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

        private void BtnDatabaseDelete_Click(object sender, RoutedEventArgs e)
        {
            /*
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
            */
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
            /*
            if (selectDlg == SelectDialog.AddNewDatabase)
            {
                this.TxtDatabaseName.Text = string.Empty;
                this.TxtCurrentPassword.Password = string.Empty;
                this.TxtNewPassword.Password = string.Empty;
                this.TxtDescription.Text = string.Empty;
                this.ShowSearchWaiting.Value = Visibility.Collapsed;
                this.ShowDatabase.Value = Visibility.Collapsed;
                this.ShowNoDatabase.Value = Visibility.Visible;
                this.BtnBack.Visibility = Visibility.Collapsed;
                this.BtnDatabaseDelete.Visibility = Visibility.Collapsed;

                this.BtnCreatePassword.IsEnabled = false;
                this.BtnDatabaseAdd.IsEnabled = false;
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
                this.BtnDatabaseStart.IsEnabled = false;
            }
            else if (selectDlg == SelectDialog.QuestionDlg)
            {
                this.ShowNoDatabase.Value = Visibility.Collapsed;
                this.ShowDatabase.Value = Visibility.Collapsed;
                this.ShowSearchWaiting.Value = Visibility.Collapsed;
                this.ShowQuestionYesNo.Value = Visibility.Visible;
            }
            */
        }
    }
}
