using System.Windows;
using System.Windows.Controls;

namespace CitnDev.Windows
{
    [TemplatePart(Name = ElementAnimationControl)]
    public class CnDPanelBusy : ContentControl
    {
        private const string ElementAnimationControl = "PART_Animation";
        private UIElement _animationElement;

        static CnDPanelBusy()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CnDPanelBusy), new FrameworkPropertyMetadata(typeof(CnDPanelBusy)));
        }

        #region IsBusy

        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(CnDPanelBusy), new PropertyMetadata(default(bool), (o, args) => ((CnDPanelBusy)o).OnIsBusyChanged((bool)args.NewValue)));

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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _animationElement = GetTemplateChild(ElementAnimationControl) as UIElement;

            OnIsBusyChanged(IsBusy);
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (IsBusy)
                e.Handled = true;
            else
                base.OnPreviewKeyDown(e);
        }
    }
}
