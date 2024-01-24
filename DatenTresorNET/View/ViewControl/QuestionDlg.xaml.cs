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
        public static readonly DependencyProperty IsOkProperty =
            DependencyProperty.Register("IsOk", typeof(bool), typeof(QuestionDlg), new PropertyMetadata(false));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(QuestionDlg), new PropertyMetadata(string.Empty, OnTitleChanged));

        public static readonly DependencyProperty DescFontWeightProperty =
            DependencyProperty.Register("DescFontWeight", typeof(FontWeight), typeof(QuestionDlg), new PropertyMetadata(FontWeights.Normal, OnDescFontWeightChanged));

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(QuestionDlg), new PropertyMetadata(string.Empty, OnDescriptionChanged));

        public QuestionDlg()
        {
            this.InitializeComponent();
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnYes, "Click", this.OnButtonClickYes);
        }

        public bool IsOk
        {
            get { return (bool)GetValue(IsOkProperty); }
            set { SetValue(IsOkProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public FontWeight DescFontWeight
        {
            get { return (FontWeight)GetValue(DescFontWeightProperty); }
            set { SetValue(DescFontWeightProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QuestionDlg control)
            {
                try
                {
                    if (e.NewValue != null)
                    {
                        control.tbTitle.Text = e.NewValue.ToString();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private static void OnDescFontWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QuestionDlg control)
            {
                try
                {
                    if (e.NewValue != null)
                    {
                        control.tbDescription.FontWeight = (FontWeight)e.NewValue;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QuestionDlg control)
            {
                try
                {
                    if (e.NewValue != null)
                    {
                        control.tbDescription.Text = e.NewValue.ToString();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void OnButtonClickYes(object sender, RoutedEventArgs e)
        {
            this.IsOk = true;
        }
    }
}
