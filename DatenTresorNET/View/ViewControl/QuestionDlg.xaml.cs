namespace DatenTresorNET.View
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für QuestionDlg.xaml
    /// </summary>
    public partial class QuestionDlg : UserControl
    {
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(QuestionDlg), new PropertyMetadata(string.Empty, OnDescriptionChanged));

        public QuestionDlg()
        {
            this.InitializeComponent();
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QuestionDlg control)
            {
                try
                {
                    if (e.NewValue != null)
                    {

                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
