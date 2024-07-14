namespace DatenTresorNET.View.ViewControl
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

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
            WeakEventManager<TextBox, RoutedEventArgs>.AddHandler(this.TxtDatabaseName, "TextChanged", this.OnTextChanged);
            WeakEventManager<TextBox, RoutedEventArgs>.AddHandler(this.TxtDatabaseName, "Loaded", this.OnTextChanged);
            WeakEventManager<TextBox, KeyEventArgs>.AddHandler(this.TxtDatabaseName, "KeyDown", this.OnKeyDown);
            WeakEventManager<TextBox, KeyEventArgs>.AddHandler(this.TxtDescription, "KeyDown", this.OnKeyDown);
            WeakEventManager<PasswordBox, RoutedEventArgs>.AddHandler(this.TxtPassword, "PasswordChanged", this.OnTextChanged);
            WeakEventManager<PasswordBox, RoutedEventArgs>.AddHandler(this.TxtPassword, "Loaded", this.OnTextChanged);
            WeakEventManager<PasswordBox, KeyEventArgs>.AddHandler(this.TxtPassword, "KeyDown", this.OnKeyDown);
            WeakEventManager<PasswordBox, RoutedEventArgs>.AddHandler(this.TxtPasswordRepeat, "PasswordChanged", this.OnTextChanged);
            WeakEventManager<PasswordBox, RoutedEventArgs>.AddHandler(this.TxtPasswordRepeat, "Loaded", this.OnTextChanged);
            WeakEventManager<PasswordBox, KeyEventArgs>.AddHandler(this.TxtPasswordRepeat, "KeyDown", this.OnKeyDown);
        }

        private string DatabaseLocation { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.TxtDatabaseName.Focus();
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

        private void OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TxtDatabaseName.Text) == true
                || string.IsNullOrEmpty(this.TxtPassword.Password) == true
                || string.IsNullOrEmpty(this.TxtPasswordRepeat.Password) == true)
            {
                this.BtnYes.IsEnabled = false;
            }
            else
            {
                if (string.IsNullOrEmpty(this.TxtDatabaseName.Text) == false && this.TxtPassword.Password == this.TxtPasswordRepeat.Password)
                {
                    this.BtnYes.IsEnabled = true;
                }
                else
                {
                    this.BtnYes.IsEnabled = false;
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((sender as TextBox) != null)
            {
                if ((e.Key == Key.Enter || e.Key == Key.Tab) & (sender as TextBox).AcceptsReturn == false)
                {
                    this.MoveToNextUIElement(e);
                }
            }
            else if ((sender as PasswordBox) != null)
            {
                if ((e.Key == Key.Enter || e.Key == Key.Tab) & (sender as PasswordBox) != null)
                {
                    this.MoveToNextUIElement(e);
                }
            }
        }

        private void OnButtonClickYes(object sender, RoutedEventArgs e)
        {
            this.CreateNewDatabase();
            MessageEventArgs args = new MessageEventArgs();
            args.Sender = typeof(AddNewDatabaseUC);
            args.MsgQuestion = MessageQuestion.Add;
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
            string dataBase = this.TxtDatabaseName.Text.Trim();
            string password = this.TxtPassword.Password;
            string fullName = System.IO.Path.Combine(this.DatabaseLocation, $"{dataBase}.db");

            ConnectionString dbconn = null;
            using (DBConnectionBuilder builder = new DBConnectionBuilder())
            {
                dbconn = builder.GetConnection(fullName, password);
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

            using (ApplicationSettings settings = new ApplicationSettings())
            {
                if (settings.IsExitSettings() == true)
                {
                    settings.Load();
                }

                if (settings.Databases == null)
                {
                    settings.Databases = new List<DatabaseParameter>();
                }

                if (settings.Databases.Any(a => a.DatabaseName.Contains(dataBase.Trim())) == true)
                {
                    settings.Databases.Remove(settings.Databases.Single(s => s.DatabaseName.Contains(dataBase)));
                }

                DatabaseParameter dp = new DatabaseParameter();
                dp.DatabaseName = dataBase;
                dp.DatabaseFolder = this.DatabaseLocation;
                dp.Description = this.TxtDescription.Text;
                dp.PasswordHash = this.TxtPassword.Password;
                settings.Databases.Add(dp);
                settings.Save();
            }

        }

        private void MoveToNextUIElement(KeyEventArgs e)
        {
            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

            TraversalRequest request = new TraversalRequest(focusDirection);

            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            if (elementWithFocus != null)
            {
                if (elementWithFocus.MoveFocus(request) == true)
                {
                    e.Handled = true;
                }
            }
        }
    }
}
