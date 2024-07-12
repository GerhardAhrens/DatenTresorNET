namespace DatenTresorNET.View
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using DatenTresorNET.Core;

    using DatenTresorNET.View.ViewControl;

    /// <summary>
    /// Interaktionslogik für QuestionDlg.xaml
    /// </summary>
    public partial class QuestionDlg : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(QuestionDlg), new PropertyMetadata(string.Empty, OnTitleChanged));

        public static readonly DependencyProperty DescFontWeightProperty =
            DependencyProperty.Register("DescFontWeight", typeof(FontWeight), typeof(QuestionDlg), new PropertyMetadata(FontWeights.Normal, OnDescFontWeightChanged));

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(QuestionDlg), new PropertyMetadata(string.Empty, OnDescriptionChanged));

        public static readonly DependencyProperty ShowButtonYesProperty =
            DependencyProperty.Register("ShowButtonYes", typeof(bool), typeof(QuestionDlg), new PropertyMetadata(true, OnShowButtonYesChanged));

        public static readonly DependencyProperty ShowButtonNoProperty =
            DependencyProperty.Register("ShowButtonNo", typeof(bool), typeof(QuestionDlg), new PropertyMetadata(true, OnShowButtonNoChanged));

        public QuestionDlg()
        {
            this.InitializeComponent();
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnYes, "Click", this.OnButtonClickYes);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnNo, "Click", this.OnButtonClickNo);
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

        public bool ShowButtonYes
        {
            get { return (bool)GetValue(ShowButtonYesProperty); }
            set { SetValue(ShowButtonYesProperty, value); }
        }

        public bool ShowButtonNo
        {
            get { return (bool)GetValue(ShowButtonNoProperty); }
            set { SetValue(ShowButtonNoProperty, value); }
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

        private static void OnShowButtonYesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QuestionDlg control)
            {
                try
                {
                    if (e.NewValue != null)
                    {
                        if ((bool)e.NewValue == true)
                        {
                            control.BtnYes.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            control.BtnYes.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private static void OnShowButtonNoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is QuestionDlg control)
            {
                try
                {
                    if (e.NewValue != null)
                    {
                        if ((bool)e.NewValue == true)
                        {
                            control.BtnNo.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            control.BtnNo.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void OnButtonClickYes(object sender, RoutedEventArgs e)
        {
            if (this.BtnNo.Visibility == Visibility.Collapsed)
            {
                App.EventAgg.Publish<MessageEventArgs>(new MessageEventArgs { Sender = typeof(FoundDatabaseUC), MsgQuestion = MessageQuestion.Ok });
            }
            else
            {
                App.EventAgg.Publish<MessageEventArgs>(new MessageEventArgs { Sender = typeof(FoundDatabaseUC), MsgQuestion = MessageQuestion.Delete });
            }
        }

        private void OnButtonClickNo(object sender, RoutedEventArgs e)
        {
            App.EventAgg.Publish<MessageEventArgs>(new MessageEventArgs { Sender = typeof(FoundDatabaseUC), MsgQuestion = MessageQuestion.No });
        }
    }
}
