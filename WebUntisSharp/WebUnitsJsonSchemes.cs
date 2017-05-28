using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace mrousavy.APIs.WebUntisSharp {
    //All JSON Queries from the WebUntis API translated to C#
    namespace WebUnitsJsonSchemes {
        //Base-Classes/Schemes for Queries and Responses
        #region Special Classes

        //Last occured Error in Web Untis
        public static class LastError {
            public static string Message {
                get {
                    string ret = _message;
                    _message = null;
                    return ret;
                }
                set {
                    _message = value;
                }
            }

            public static int Code {
                get {
                    int ret = _code;
                    _code = 0;
                    return ret;
                }
                set {
                    _code = value;
                }
            }

            private static string _message;
            private static int _code;
        }

        //Base-Class for all Queries
        public class WebUntisQuery {
            public string id;
            public string method;
            public readonly string jsonrpc = "2.0";
        }

        //Base-Class for all Responses
        public class WebUntisResult {
            public string id;
            public object result;
            public readonly string jsonrpc = "2.0";

            public Error error;
        }

        //The general Error class
        public class Error {
            public string message {
                get { return _message; }
                set {
                    _message = value;
                    LastError.Message = value;
                }
            }

            public int code {
                get { return _code; }
                set {
                    _code = value;
                    LastError.Code = value;
                }
            }

            private string _message;
            private int _code;
        }
        #endregion

        //22 JSON Queries, see Resources/WebUntis_JSON_API.pdf
        #region Individual Queries
        //1 & 2.
        namespace Sessions {
            //Authenticate the given user and start a session
            public class Authentication : WebUntisQuery {
                public new string id = "1";
                public new readonly string method = "authenticate";
                public Params @params;

                public class Params {
                    public string user = "ANDROID";
                    public string password;
                    public string client;
                }
            }

            //Result of Authentication
            public class AuthenticationResult : WebUntisResult {
                public new Result result;

                public class Result {
                    public string sessionId;
                    public int personType;
                    public int personId;
                    public int klasseId;
                }
            }

            //End the session
            public class Logout : WebUntisQuery {
                public new string id = "2";
                public new readonly string method = "logout";
            }
        }

        //3.
        namespace Teachers {
            //Get list of teachers
            public class GetTeachers : WebUntisQuery {
                public new string id = "3";
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
                public new string id = "4";
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
                public new string id = "5";
                public new readonly string method = "getKlassen";
                public Params @params;

                public class Params {
                    public string schoolyearId;
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
                public new string id = "6";
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
                public new string id = "7";
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
                public new string id = "8";
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
                public new string id = "9";
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
            public class GetTimegrid : WebUntisQuery {
                public new string id = "10";
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
            public class TimegridResult : WebUntisResult {
                public new Timegrid[] result;
            }
        }

        //11.
        namespace StatusData {
            //Request Status Data
            public class GetStatusData : WebUntisQuery {
                public new string id = "11";
                public new string method = "getStatusData";
            }

            //Result from Request Status Data Query
            public class StatusData : WebUntisResult {
                public KeyValuePair<string, Colors> lstypes;
                public KeyValuePair<string, Colors> codes;
            }

            public class Colors {
                public string foreColor;
                public string backColor;
            }
        }

        //12.
        namespace CurrentSchoolyear {
            //Get Data for the current schoolyear
            public class CurrentSchoolyear : WebUntisQuery {
                public new string id = "12";
                public new readonly string method = "getCurrentSchoolyear";
            }

            //Response for Get Schoolyear
            public class SchoolyearResult : WebUntisResult {
                public new Schoolyear[] result;
            }

            //Individual Schoolyears
            public class Schoolyear {
                public int id;
                public string name;
                public long startDate;
                public long endDate;
            }
        }

        //13.
        namespace SchoolYears {
            //Get Data for the schoolyears
            public class Schoolyears : WebUntisQuery {
                public new string id = "13";
                public new readonly string method = "getSchoolyears";
            }

            //Response for Get Schoolyear
            public class SchoolyearResult : WebUntisResult {
                public new CurrentSchoolyear.Schoolyear[] result;
            }
        }

        //14 & 15
        namespace TimetableForElement {
            //Get Timetable for element
            public class TimetableForElement : WebUntisQuery {
                public new string id = "14";
                public new readonly string method = "getTimetable";
                public Params @params;

                public class Params {
                    public int id;
                    public int type;
                    public long startDate;
                    public long endDate;
                }
            }

            //Timetable Result
            public class TimetableResult : WebUntisResult {
                public new int id;
                public long date;
                public long startTime;
                public long endTime;

                //array of classes ids
                public int[] kl;
                //array of teacher ids
                public int[] te;
                //array of subject ids
                public int[] su;
                //array of room ids
                public int[] ro;

                //ls (lesson) | oh (office hour) | sb (standby) | bs (break supervision) | ex (examination)
                //omitted if lesson
                public string lstype;

                // | cancelled | irregular
                //omitted if empty
                public string code;

                //text of the lesson, omitted if empty
                public string lstext;

                //statistical flags of the lesson, omitted if empty
                public string statflags;
            }

            //Response for Get Schoolyear
            public class SchoolyearResult : WebUntisResult {
                public new Schoolyear[] result;
            }

            //Individual Schoolyears
            public class Schoolyear {
                public int id;
                public string name;
                public long startDate;
                public long endDate;
            }
        }

        //16
        //[removed]

        //17
        namespace LastImportTime {
            //Get last import time
            public class LastImportTime : WebUntisQuery {
                public new string id = "17";
                public new readonly string method = "getLatestImportTime";
            }

            //Last import time Result
            public class LastImportTimeResult {
                [JsonIgnore]
                public DateTime Result {
                    get {
                        DateTime time = new DateTime(1970, 1, 1);
                        return time.AddMilliseconds(1495788867289);
                    }
                    set {
                        result = value.ToFileTime();
                    }
                }

                internal long result;
            }
        }

        //18
        namespace PersonIdSearch {
            //Get Id of the person (teacher or student) from the name
            public class SearchPersonId : WebUntisQuery {
                public new string id = "18";
                public new readonly string method = "getPersonId";
                public Params @params;

                public class Params {
                    //Type of Person | 2 = Teacher, 5 = Student
                    public int type;
                    //Surname
                    public string sn;
                    //Forename
                    public string fn;
                    //Birthdata | default = 0
                    public int dob;
                }
            }

            //Response of Search Person ID
            public class SearchPersonIdResult : WebUntisResult {
                public new int result;
            }
        }

        //19
        namespace Substitutions {
            //Request substitutions for the given date range
            public class Substitutions : WebUntisQuery {
                public new string id = "19";
                public new readonly string method = "getSubstitutions";
                public Params @params;

                public class Params {
                    public long startDate;
                    public long endDate;
                    public int departmentId = 0;
                }
            }

            //Result of Get Substitutions
            public class SubstitutionResult : WebUntisResult {
                public new Substitution[] result;
            }

            //Individual Substitutions
            public class Substitution {
                //type of substitution:
                //cancel = cancellation | subst = teacher substitution | add = additional period | shift = shifted period | rmchg = room change
                public string type;

                public ID id;
                public long date;
                public long startTime;
                public long endTime;
                public int[] kl;
                public int[] te;
                public int[] su;
                public int[] ro;
                public int[] txt;
                public Reschedule reschedule;
            }

            public class ID {
                public int id;
                public int orgid;
            }

            public class Reschedule {
                public int date;
                public int startTime;
                public int endTime;
            }
        }

        //20
        namespace ClassregEvents {
            //Request classregevents for the given date range
            public class ClassregEvents : WebUntisQuery {
                public new string id = "20";
                public new readonly string method = "getClassregEvents";
                public Params @params;

                public class Params {
                    public long startDate;
                    public long endDate;
                }
            }

            public class Event {
                public string studentid;
                public string surname;
                public string forname;
                public long date;
                public string subject;
                public string reason;
                public string text;
            }

            //Result from ClassregEvent Query
            public class ClassregEventsResult : WebUntisResult {
                public new Event[] result;
            }
        }

        //21
        namespace Exams {
            //Request Exams
            public class RequestExams : WebUntisQuery {
                public new string id = "21";
                public new readonly string method = "getExams";
                public Params @params;

                public class Params {
                    public int examTypeId;
                    public long startDate;
                    public long endDate;
                }
            }

            //Result of Request Exams
            public class ExamResult : WebUntisResult {
                public new Exam[] result;
            }


            //Exam Class
            public class Exam : WebUntisResult {
                public int[] classes;
                public int[] teachers;
                public int[] students;
                public int subject;
                public long date;
                public long startTime;
                public long endTime;

                public string name;
                public string longName;
                public bool showInTimetable;
            }
        }

        //22
        namespace ExamTypes {
            //Request Exam Types
            public class ExamTypes : WebUntisQuery {
                public new string id = "22";
                public new readonly string method = "getExamTypes";
            }
        }
        #endregion
    }
}
