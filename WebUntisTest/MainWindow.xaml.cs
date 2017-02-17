using System.Windows;
using WebUntisSharp;

namespace WebUntisTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            WebUnits untis = new WebUnits("mrousavy", "123", "", "mrousavy");
        }
    }
}
