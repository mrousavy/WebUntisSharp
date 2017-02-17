using System;
using System.Windows;
using WebUntisSharp;

namespace WebUntisTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private WebUntis _untis;

        public MainWindow() {
            InitializeComponent();

        }

        private void Ok_Click(object sender, RoutedEventArgs e) {
            try {
                _untis = new WebUntis(UsernameBox.Text, PasswordBox.Password, SchoolUrlBox.Text, "WebUntisSharp API");

                if(WebUntisSharp.WebUnitsJsonSchemes.LastError.Message != null) {
                    MessageBox.Show(WebUntisSharp.WebUnitsJsonSchemes.LastError.Message);
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
