using mrousavy.APIs.WebUntisSharp;
using System;
using System.Windows;
using System.Windows.Input;
using WebUntisSharp = mrousavy.APIs.WebUntisSharp;

namespace WebUntisTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private WebUntis _untis;

        public MainWindow() {
            InitializeComponent();

            PasswordBox.Focus();
        }

        private async void Submit_Event(object sender, RoutedEventArgs e) {
            try {
                _untis = new WebUntis(UsernameBox.Text, PasswordBox.Password, SchoolUrlBox.Text, "WebUntisSharp API");

                if (WebUntisSharp.WebUnitsJsonSchemes.LastError.Message != null) {
                    MessageBox.Show(WebUntisSharp.WebUnitsJsonSchemes.LastError.Message);
                } else {
                    var departments = await _untis.GetDepartments();
                    var classregevents = await _untis.GetClassRegEvents(01012015, 01012016);
                    var classes = await _untis.GetClasses("1");
                    var exams = await _untis.GetExams(01012015, 01012016, 1);
                    var holidays = await _untis.GetHolidays();
                    var lastimporttime = await _untis.GetLastImportTime();
                    var person = await _untis.GetPersonId(5, "Rousavy", "Marc");
                    var rooms = await _untis.GetRooms();
                    var schoolyear = await _untis.GetSchoolyear();
                    var schoolyears = await _untis.GetSchoolyears();
                    var statusdata = await _untis.GetStatusData();
                    var students = await _untis.GetStudents();
                    var teacher = await _untis.GetTeachers();
                    var subjects = await _untis.GetSubjects();
                    var substitution = await _untis.GetSubstitution(01012015, 01012016);
                    var timetable = await _untis.GetTimetableForElement(0, 3, 01012015, 01012016);
                    var timegrid = await _untis.GetTimegrid();
                    var sessionId = _untis.SessionId;
                    Console.Write("Done.");
                }
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void PasswordBox_OnKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter)
                Submit_Event(null, null);
        }
    }
}
