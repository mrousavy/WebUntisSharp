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
                } else {
                    var timegrid = _untis.GetTimegrid();
                    var classregevents = _untis.GetClassRegEvents(01012015, 01012016);
                    var classes = _untis.GetClasses();
                    var departments = _untis.GetDepartments();
                    var exams = _untis.GetExams(01012015, 01012016, 1);
                    var holidays = _untis.GetHolidays();
                    var lastimporttime = _untis.GetLastImportTime();
                    var person = _untis.GetPersonId(5, "Rousavy", "Marc");
                    var rooms = _untis.GetRooms();
                    var schoolyear = _untis.GetSchoolyear();
                    var schoolyears = _untis.GetSchoolyears();
                    var statusdata = _untis.GetStatusData();
                    var students = _untis.GetStudents();
                    var teacher = _untis.GetTeachers();
                    var subjects = _untis.GetSubjects();
                    var substitution = _untis.GetSubstitution(01012015, 01012016);
                    var timetable = _untis.GetTimetableForElement(0, 3, 01012015, 01012016);
                    var sessionId = _untis.SessionId;
                    Console.Write("Done.");
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
