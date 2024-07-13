﻿namespace DatenTresorNET.View.ViewControl
{
    using System;
    using System.CodeDom;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    using DatenTresorNET.BaseFunction;
    using DatenTresorNET.Core;

    using LiteDB;

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
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnPasswordChange, "Click", this.OnChangePassword);
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

            using (DatabaseSearcher ds = new DatabaseSearcher(this.DatabaseLocation, this.TxtCurrentPassword.Password))
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

        private void OnChangePassword(object sender, RoutedEventArgs e)
        {
            string selectedName = Path.Combine(this.DatabaseNameSelected.Value.DatabaseFolder, this.DatabaseNameSelected.Value.DatabaseName);
            if (File.Exists(selectedName) == true)
            {
                ConnectionString dbconn = Connection(selectedName, this.TxtCurrentPassword.Password);
                if (dbconn != null)
                {
                    LiteDatabase litedb = new LiteDatabase(dbconn);
                    if (litedb != null)
                    {

                    }
                }
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
    }
}
