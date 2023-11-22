namespace DatenTresorNET.View
{
    using System;
    using System.Security.Cryptography.Xml;
    using System.Windows;
    using System.Windows.Controls;

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

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
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
    }
}
