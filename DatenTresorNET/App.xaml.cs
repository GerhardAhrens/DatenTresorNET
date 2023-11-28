namespace DatenTresorNET
{
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography.Xml;
    using System.Text;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Threading;

    using DatenTresorNET.BaseFunction;
    using DatenTresorNET.Core;
    using DatenTresorNET.View;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string DEFAULTLANGUAGE = "de-DE";
        private const string SHORTNAME = "Data Container";
        private static readonly string MessageBoxTitle = "Data Container Application";
        private static readonly string UnexpectedError = "An unexpected error occured.";
        private string exePath = string.Empty;
        private string exeName = string.Empty;

        public App()
        {
            /* Name der EXE Datei*/
            exeName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
            /* Pfad der EXE-Datei*/
            exePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            /* Synchronisieren einer Textenigabe mit dem primären Windows (wegen Validierung von Eingaben)*/
            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;

            /* Initalisierung Spracheinstellung */
            InitializeCultures(DEFAULTLANGUAGE);

            App.Current.DispatcherUnhandledException += this.OnDispatcherUnhandledException;
        }

        /// <summary>
        /// Festlegung für Abfrage des Programmendedialog
        /// </summary>
        public static bool ExitApplicationQuestion { get; set; }

        /// <summary>
        /// Festlegung für das Speichern der Position des Main-Windows
        /// </summary>
        public static bool SaveLastWindowsPosition { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            /* Einstellungen für Applikation und Datenbank in gestrennten Settingsdateien*/
            InitializeSettings();

            /* Main Window aufrufen */
            MainWindow mainWindow = new MainWindow();
            this.MainWindow = mainWindow;
            mainWindow.Visibility = Visibility.Hidden;

            try
            { 
                /* StartScreen Window aufrufen */
                StartScreen startScreen = new StartScreen();
                if (startScreen.ShowDialog() == false)
                {
                    ApplicationExit();
                }
                else
                {
                    mainWindow.Activate();
                    mainWindow.Show();
                    mainWindow.Visibility = Visibility.Visible;
                    startScreen.Close();
                    startScreen = null;
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                ErrorMessage(ex, "General Error: ");
                ApplicationExit();
            }

        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine($"{exeName}-{(e.Exception as Exception).Message}");
        }

        public static void ErrorMessage(Exception ex, string message = "")
        {
            string expMsg = ex.Message;
            var aex = ex as AggregateException;

            if (aex != null && aex.InnerExceptions.Count == 1)
            {
                expMsg = aex.InnerExceptions[0].Message;
            }

            if (string.IsNullOrEmpty(message) == true)
            {
                message = UnexpectedError;
            }

            StringBuilder errorText = new StringBuilder();
            if (ex.Data != null && ex.Data.Count > 0)
            {
                foreach (DictionaryEntry item in ex.Data)
                {
                    errorText.AppendLine($"{item.Key} : {item.Value}");
                }
            }

            MessageBox.Show(
                message + $"{expMsg}\n{ex.Message}\n{errorText.ToString()}",
                MessageBoxTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private static void InitializeCultures(string language)
        {
            if (string.IsNullOrEmpty(language) == false)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(DEFAULTLANGUAGE);
            }

            if (string.IsNullOrEmpty(language) == false)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(DEFAULTLANGUAGE);
            }

            FrameworkPropertyMetadata frameworkMetadata = new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(new CultureInfo(language).IetfLanguageTag));
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), frameworkMetadata);
        }

        private static void InitializeSettings()
        {
            using (ApplicationSettings settings = new ApplicationSettings())
            {
                if (settings.IsExitSettings() == false)
                {
                    settings.DatabaseFolder = Path.Combine(settings.RootPathname,"Database");
                    settings.LastAccess = DateTime.Now;
                    settings.LastUser = UserInfo.TS().CurrentDomainUser;
                    settings.Save();
                }

                settings.Load();

                App.ExitApplicationQuestion = settings.ExitApplicationQuestion;
                App.SaveLastWindowsPosition = settings.SaveLastWindowsPosition;
            }
        }

        // <summary>
        /// Screen zum aktualisieren zwingen, Globale Funktion
        /// </summary>
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }

        /// <summary>
        /// Programmende erzwingen
        /// </summary>
        public static void ApplicationExit()
        {
            Application.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }
    }
}
