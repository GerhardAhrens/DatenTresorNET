﻿namespace DatenTresorNET.View.ViewControl
{
    using System.Windows;
    using System.Windows.Controls;

    using DatenTresorNET.BaseFunction;
    using DatenTresorNET.Core;
    using DatenTresorNET.Model;

    using LiteDB;

    /// <summary>
    /// Interaktionslogik für AddNewDatabaseUC.xaml
    /// </summary>
    public partial class AddNewDatabaseUC : UserControl
    {
        public AddNewDatabaseUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnYes, "Click", this.OnButtonClickYes);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnNo, "Click", this.OnButtonClickNo);
        }

        private string DatabaseLocation { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.txtNotes.Text = string.Empty;

            using (ApplicationSettings settings = new ApplicationSettings())
            {
                if (settings.IsExitSettings() == true)
                {
                    settings.Load();
                    this.DatabaseLocation = settings.DatabaseFolder;
                }
            }

        }

        private void OnButtonClickYes(object sender, RoutedEventArgs e)
        {
            MessageEventArgs args = new MessageEventArgs();
            args.Sender = typeof(AddNewDatabaseUC);
            args.MsgQuestion = MessageQuestion.Yes;
            args.CurrentDatabase = null;

            App.EventAgg.Publish<MessageEventArgs>(args);
        }

        private void OnButtonClickNo(object sender, RoutedEventArgs e)
        {
            MessageEventArgs args = new MessageEventArgs();
            args.Sender = typeof(AddNewDatabaseUC);
            args.MsgQuestion = MessageQuestion.No;
            args.CurrentDatabase = null;

            App.EventAgg.Publish<MessageEventArgs>(args);
        }

        private void CreateNewDatabase()
        {
            string dataBase = this.TxtDatabaseName.Text;
            string password = this.TxtPassword.Password;
            string fullName = System.IO.Path.Combine(this.DatabaseLocation, $"{dataBase}.db");

            ConnectionString dbconn = null;
            using (DBConnectionBuilder builder = new DBConnectionBuilder())
            {
                dbconn = builder.GetConnection(fullName, string.Empty);
            }

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
            }
        }
    }
}
