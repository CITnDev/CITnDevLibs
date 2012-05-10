using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        private const string ElementErrorControl = "PART_Error";
        private UIElement _animationElement;
        private UIElement _errorElement;


        static CnDPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CnDPanel), new FrameworkPropertyMetadata(typeof(CnDPanel)));
        }

        public CnDPanel()
        {
            HideErrorMessageCommand = new DelegateCommand(() =>
            {
                var be = GetBindingExpression(HasErrorProperty);
                SetValue(HasErrorProperty, false);
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

        #region HasError

        //public static readonly DependencyProperty HasErrorProperty =
        //    DependencyProperty.Register("HasError", typeof(bool), typeof(CnDPanel), new PropertyMetadata(default(bool), (o, args) => ((CnDPanel)o).OnHasErrorChanged((bool)args.NewValue)));

        public static readonly DependencyProperty HasErrorProperty =
            DependencyProperty.Register("HasError", typeof(bool), typeof(CnDPanel), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Journal | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (o, args) => ((CnDPanel)o).OnHasErrorChanged((bool)args.NewValue)));


        private void OnHasErrorChanged(bool value)
        {
            if (_errorElement == null)
                return;

            _errorElement.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }

        public bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            set { SetValue(HasErrorProperty, value); }
        }

        #endregion

        public static readonly DependencyProperty HideErrorMessageCommandProperty =
            DependencyProperty.Register("HideErrorMessageCommand", typeof(DelegateCommand), typeof(CnDPanel), new PropertyMetadata(default(DelegateCommand)));

        public DelegateCommand HideErrorMessageCommand
        {
            get { return (DelegateCommand)GetValue(HideErrorMessageCommandProperty); }
            set { SetValue(HideErrorMessageCommandProperty, value); }
        }

        #region Message

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(string), typeof(CnDPanel), new PropertyMetadata(default(string)));

        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _animationElement = GetTemplateChild(ElementAnimationControl) as UIElement;
            _errorElement = GetTemplateChild(ElementErrorControl) as UIElement;

            OnIsBusyChanged(IsBusy);
            OnHasErrorChanged(HasError);
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (IsBusy || HasError)
                e.Handled = true;
            else
                base.OnPreviewKeyDown(e);
        }
    }
}
