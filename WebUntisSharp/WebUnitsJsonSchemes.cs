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

        #region Individual Queries
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

        namespace Classes {
            //Get Classes (Klassen) for schoolyear
            public class GetClasses : WebUntisQuery {
                public new readonly string method = "getKlassen";
                public new class @params {
                    string schoolyearId;
                }
            }

            //Classes (Klassen) class
            public class Class {
                string name;
                string longName;
                string foreColor;
                string backColor;
                int did;
            }

            //Result of GetClasses
            public class ClassesResult : WebUntisResult {
                public new Class result;
            }
        }

        namespace Subjects {

        }
        #endregion
    }
}
