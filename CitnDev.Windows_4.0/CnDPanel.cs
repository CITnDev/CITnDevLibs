using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;

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
    [TemplatePart(Name = ElementAnimationControl)]
    [TemplatePart(Name = ElementErrorControl)]
    public class CnDPanel : ContentControl
    {
        private const string ElementAnimationControl = "PART_Animation";
        private const string ElementErrorControl = "PART_Notification";
        private UIElement _animationElement;
        private UIElement _errorElement; 


        static CnDPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CnDPanel), new FrameworkPropertyMetadata(typeof(CnDPanel)));
        }

        public CnDPanel()
        {
            HideNotificationCommand = new DelegateCommand(() =>
            {
                var be = GetBindingExpression(HasNotificationProperty);
                SetValue(HasNotificationProperty, false);
                if (be != null)
                    be.UpdateSource();
            });
        }

        #region IsBusy

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(CnDPanel), new PropertyMetadata(default(bool), (o, args) => ((CnDPanel)o).OnIsBusyChanged((bool)args.NewValue)));

        private void OnIsBusyChanged(bool value)
        {
            if (_animationElement == null)
                return;

            _animationElement.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        #endregion

        #region HasNotification

        public static readonly DependencyProperty HasNotificationProperty =
            DependencyProperty.Register("HasNotification", typeof(bool), typeof(CnDPanel), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Journal | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (o, args) => ((CnDPanel)o).OnHasNotificationChanged((bool)args.NewValue)));


        private void OnHasNotificationChanged(bool value)
        {
            if (_errorElement == null)
                return;

            _errorElement.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }

        public bool HasNotification
        {
            get { return (bool)GetValue(HasNotificationProperty); }
            set { SetValue(HasNotificationProperty, value); }
        }

        #endregion

        public static readonly DependencyProperty HideNotificationCommandProperty =
            DependencyProperty.Register("HideNotificationCommand", typeof(DelegateCommand), typeof(CnDPanel), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand HideNotificationCommand
        {
            get { return (DelegateCommand)GetValue(HideNotificationCommandProperty); }
            set { SetValue(HideNotificationCommandProperty, value); }
        }

        #region Message

        public static readonly DependencyProperty NotificationProperty =
            DependencyProperty.Register("Notification", typeof(object), typeof(CnDPanel), new PropertyMetadata(default(object)));

        public object Notification
        {
            get { return GetValue(NotificationProperty); }
            set { SetValue(NotificationProperty, value); }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _animationElement = GetTemplateChild(ElementAnimationControl) as UIElement;
            _errorElement = GetTemplateChild(ElementErrorControl) as UIElement;

            OnIsBusyChanged(IsBusy);
            OnHasNotificationChanged(HasNotification);
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (IsBusy || HasNotification)
                e.Handled = true;
            else
                base.OnPreviewKeyDown(e);
        }
    }
}
