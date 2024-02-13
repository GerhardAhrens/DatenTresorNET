namespace DatenTresorNET.View.ViewControl
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Linq;

    using DatenTresorNET.Core;
    using LiteDB;
    using DatenTresorNET.BaseFunction;
    using DatenTresorNET.Model;

    /// <summary>
    /// Interaktionslogik für NoFoundDatabaseUC.xaml
    /// </summary>
    public partial class NoFoundDatabaseUC : UserControl
    {
        public NoFoundDatabaseUC()
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

        private void BtnDatabaseAdd_Click(object sender, RoutedEventArgs e)
        {
            string dataBase = this.TxtDatabaseName.Text;
            string password = this.TxtNewPassword.Password;
            string fullName = System.IO.Path.Combine(this.DatabaseLocation, $"{dataBase}.db");

            this.BtnDatabaseAdd.IsEnabled = false;

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
