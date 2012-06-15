using System.Windows;
using System.Windows.Controls;

namespace CitnDev.Windows
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CitnDev.Windows_4._0"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CitnDev.Windows_4._0;assembly=CitnDev.Windows_4._0"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    [TemplatePart(Name = ElementBusyControl)]
    [TemplatePart(Name = ElementNotificationControl)]
    public class CnDPanel : ContentControl
    {
        private const string ElementBusyControl = "PART_Busy";
        private const string ElementNotificationControl = "PART_Notification";

        static CnDPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CnDPanel), new FrameworkPropertyMetadata(typeof(CnDPanel)));
        }

        #region IsBusy

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(CnDPanel), new PropertyMetadata(default(bool)));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        #endregion

        #region HasNotification

        public static readonly DependencyProperty HasNotificationProperty =
            DependencyProperty.Register("HasNotification", typeof(bool), typeof(CnDPanel), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Journal | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool HasNotification
        {
            get { return (bool)GetValue(HasNotificationProperty); }
            set { SetValue(HasNotificationProperty, value); }
        }

        #endregion

        #region Notification

        public static readonly DependencyProperty NotificationProperty =
            DependencyProperty.Register("Notification", typeof(object), typeof(CnDPanel), new PropertyMetadata(default(object)));

        public object Notification
        {
            get { return GetValue(NotificationProperty); }
            set { SetValue(NotificationProperty, value); }
        }

        #endregion

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (IsBusy || HasNotification)
                e.Handled = true;
            else
                base.OnPreviewKeyDown(e);
        }
    }
}
