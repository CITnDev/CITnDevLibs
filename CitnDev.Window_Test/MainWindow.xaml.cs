using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CitnDev.Windows_Tests.ViewModels;

namespace CitnDev.Windows_Tests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestViewModel _testViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _testViewModel = new TestViewModel();
            DataContext = _testViewModel;
            LstTest.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(LstTest_SelectionChanged);
        }

        void LstTest_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Task.Factory.StartNew(() =>
                                      {
                                          _testViewModel.IsBusy = true;
                                          Thread.Sleep(3000);
                                          _testViewModel.IsBusy = false;
                                      });
        }
    }
}
