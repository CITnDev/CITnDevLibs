using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CitnDev.Windows
{
    [TemplatePart(Name = ElementNotificationControl)]
    [TemplatePart(Name = ElementBtnOkControl, Type = typeof(Button))]
    public class CnDPanelNotification : ContentControl
    {
        private const string ElementNotificationControl = "PART_Notification";
        private const string ElementBtnOkControl = "PART_BTNOK";
        private UIElement _notificationElement;
        private Button _btnOk;

        static CnDPanelNotification()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CnDPanelNotification), new FrameworkPropertyMetadata(typeof(CnDPanelNotification)));
        }

        #region HasNotification

        //public static readonly DependencyProperty HasNotificationProperty =
        //    DependencyProperty.Register("HasNotification", typeof(bool), typeof(CnDPanelNotification), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Journal | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (o, args) => ((CnDPanelNotification)o).OnHasNotificationChanged((bool)args.NewValue)));
        public static readonly DependencyProperty HasNotificationProperty =
            DependencyProperty.Register("HasNotification", typeof(bool), typeof(CnDPanelNotification), new PropertyMetadata(default(bool), (o, args) => ((CnDPanelNotification)o).OnHasNotificationChanged((bool)args.NewValue)));


        private void OnHasNotificationChanged(bool value)
        {
            if (_notificationElement == null)
                return;

            _notificationElement.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool HasNotification
        {
            get { return (bool)GetValue(HasNotificationProperty); }
            set { SetValue(HasNotificationProperty, value); }
        }

        #endregion

        #region Notification

        public static readonly DependencyProperty NotificationProperty =
            DependencyProperty.Register("Notification", typeof(object), typeof(CnDPanelNotification), new PropertyMetadata(default(object)));

        public object Notification
        {
            get { return GetValue(NotificationProperty); }
            set { SetValue(NotificationProperty, value); }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _notificationElement = GetTemplateChild(ElementNotificationControl) as UIElement;
            _btnOk = GetTemplateChild(ElementBtnOkControl) as Button;

            if (_btnOk != null)
                _btnOk.Click += (sender, args) =>
                                    {
                                        HasNotification = false;
                                    };

            OnHasNotificationChanged(HasNotification);
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (HasNotification)
                e.Handled = true;
            else
                base.OnPreviewKeyDown(e);
        }

    }
}
