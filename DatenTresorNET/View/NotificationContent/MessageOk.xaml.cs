namespace DatenTresorNET.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using DatenTresorNET.BaseFunction;

    /// <summary>
    /// Interaktionslogik für MessageOK.xaml
    /// </summary>
    public partial class MessageOK : UserControl
    {
        public MessageOK()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Tuple<string, string, double> textOption = (Tuple<string, string, double>)this.Tag;

            this.TbHeader.Text = textOption.Item1;
            this.TbFull.Text = textOption.Item2;
            this.TbFull.FontSize = textOption.Item3;
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.Ok, null);
            window.DialogResult = true;
            window.Close();
        }
    }
}
