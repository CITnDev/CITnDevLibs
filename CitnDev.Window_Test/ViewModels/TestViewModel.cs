using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace CitnDev.Windows_Tests.ViewModels
{
    public class TestViewModel : NotificationObject
    {
        public TestViewModel()
        {
            SwitchIsBusyCommand = new DelegateCommand(() => IsBusy = !IsBusy);
            SwitchHasErrorCommand = new DelegateCommand(() => HasError = !HasError);
        }

        #region IsBusy

        /// <summary>
        /// Description
        /// </summary>
        private bool _isBusy;

        /// <summary>
        /// Description
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                //ignore if values are equal
                if (value == _isBusy) return;

                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        #endregion

        #region HasError

        /// <summary>
        /// Description
        /// </summary>
        private bool _hasError;

        /// <summary>
        /// Description
        /// </summary>
        public bool HasError
        {
            get { return _hasError; }
            set
            {
                //ignore if values are equal
                if (value == _hasError) return;

                _hasError = value;
                RaisePropertyChanged(() => HasError);
            }
        }

        #endregion

        public DelegateCommand SwitchIsBusyCommand { get; private set; }
        public DelegateCommand SwitchHasErrorCommand { get; private set; }
    }
}
