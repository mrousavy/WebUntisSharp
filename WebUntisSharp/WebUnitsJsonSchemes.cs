namespace WebUntisSharp {
    //All JSON Queries from the WebUntis API translated to C#
    namespace WebUnitsJsonSchemes {
        #region Special Classes
        //Base-Class for all Queries
        public class WebUntisQuery {
            public string id;
            public string method;
            public string jsonrpc = "2.0";
            //public @params @params;

            public class @params { }
        }

        //Base-Class for all Responses
        public class WebUntisResult {
            public string id;
            public object result;
            public string jsonrpc = "2.0";
        }
        //Parameter Class for JSON Query Method Parameters
        //public class @params {
        //    public string user = "ANDROID";
        //    public string password;
        //    public string client;
        //}
        #endregion

        //22 JSON Queries, see Resources/WebUntis_JSON_API.pdf
        #region Individual Queries
        //1 & 2.
        namespace Sessions {
            //Authenticate the given user and start a session
            public class Authentication : WebUntisQuery {
                public new string id;
                public readonly new string method = "authenticate";

                public new class @params {
                    public string user = "ANDROID";
                    public string password;
                    public string client;
                }
            }

            //Result of Authentication
            public class AuthenticationResult : WebUntisResult {
                public string sessionId;
                public int personType;
                public int personId;
            }

            //End the session
            public class Logout : WebUntisQuery {
                public new string id;
                public new readonly string method = "logout";
            }
        }

        //3.
        namespace Teachers {
            //Get list of teachers
            public class GetTeachers : WebUntisQuery {
                public new string id;
                public new readonly string method = "getTeachers";
            }

            //Individual Teacher
            public class Teacher {
                public int id;
                public string name;
                public string foreName;
                public string longName;
                public string foreColor;
                public string backColor;
            }

            //Result of Get Teachers Query
            public class TeachersResult : WebUntisResult {
                public new Teacher[] result;
            }
        }

        //4.
        namespace Students {
            //Get list of Students
            public class GetStudents : WebUntisQuery {
                public new string id;
                public new readonly string method = "getStudents";
            }

            //Individual Student
            public class Student {
                public int id;
                public string key;
                public string name;
                public string foreName;
                public string longName;
                //There are only 2 genders, why not use a bool
                public string gender;
            }

            //Result of Get Students Query
            public class StudentsResult : WebUntisResult {
                public new Student[] result;
            }
        }

        //5.
        namespace Classes {
            //Get Classes (Klassen) for schoolyear
            public class GetClasses : WebUntisQuery {
                public new readonly string method = "getKlassen";
                public new class @params {
                    string schoolyearId;
                }
            }

            //Individual Classes (Klassen)
            public class Class {
                public int id;
                public string name;
                public string longName;
                public string foreColor;
                public string backColor;
                public int did;
            }

            //Result of GetClasses Query
            public class ClassesResult : WebUntisResult {
                public new Class[] result;
            }
        }

        //6.
        namespace Subjects {
            //Get List of Subjects
            public class GetSubjects : WebUntisQuery {
                public new string method = "getSubjects";
            }

            //Individual Subjects
            public class Subject {
                public int id;
                public string name;
                public string longName;
                public string foreColor;
                public string backColor;
            }

            //Result of GetSubjects Query
            public class SubjectsResult : WebUntisResult {
                public new Subject[] result;
            }
        }

        //7.
        namespace Rooms {
            //Get List of Rooms
            public class GetRooms : WebUntisQuery {
                public new string method = "getRooms";
            }

            //Individual Room
            public class Room {
                public int id;
                public string name;
                public string longName;
                public string foreColor;
                public string backColor;
            }

            //Result of GetRooms Query
            public class RoomsResult : WebUntisResult {
                public new Room[] result;
            }
        }

        //8.
        namespace Departments {
            //Get List of Departments
            public class GetDepartments : WebUntisQuery {
                public new string method = "getDepartments";
            }

            //Individual Department
            public class Department {
                public int id;
                public string name;
                public string longName;
            }

            //Result of GetDepartments Query
            public class DepartmentsResult : WebUntisResult {
                public new Department[] result;
            }
        }

        //9.
        namespace Holidays {
            //Get List of Holidays
            public class GetHolidays : WebUntisQuery {
                public new string method = "getHolidays";
            }

            //Individual Holiday
            public class Holiday {
                public int id;
                public string name;
                public string longName;
                public long startDate;
                public long endDate;
            }

            //Result of GetHolidays Query
            public class HolidaysResult : WebUntisResult {
                public new Holiday[] result;
            }
        }

        //10.
        namespace Timegrid {
            //Get List of Timegrids
            public class GetTimegrids : WebUntisQuery {
                public new string method = "getTimegridUnits";
            }

            //Individual Timegrids
            public class Timegrid {
                //Day of the Week, 1 = sunday, 2 = monday, .. 7 = saturday
                public int day;
                public TimeUnit[] timeUnits;
            }

            //Individual TimeUnits
            public class TimeUnit {
                public long startTime;
                public long endTime;
            }

            //Result of Timegrids Query
            public class TimegridsResult : WebUntisResult {
                public new Timegrid[] result;
            }
        }

        //11.
        namespace StatusData { }

        //12.
        namespace CurrentSchoolyear { }

        //13.
        namespace SchoolYears { }

        //14 & 15
        namespace TimetableForElement { }

        //16
        //[removed]

        //17
        namespace StatusData { }

        //18
        namespace StatusData { }

        //19
        namespace StatusData { }

        //20
        namespace StatusData { }

        //21
        namespace StatusData { }

        //22
        namespace StatusData { }
        #endregion
    }
}
