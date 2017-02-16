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
        //Parameter Class for JSON Query Method Parameters
        //public class @params {
        //    public string user = "ANDROID";
        //    public string password;
        //    public string client;
        //}
        #endregion

        #region Individual Queries
        //Authenticate the given user and start a session
        public class Authentication : WebUntisQuery {
            public string id;
            public readonly string method = "authenticate";

            public class @params {
                public string user = "ANDROID";
                public string password;
                public string client;
            }
        }

        //End the session
        public class Logout : WebUntisQuery {
            public string id;
            public readonly string method = "logout";
        }

        //Get list of teachers
        public class Teachers : WebUntisQuery {
            public string id;
            public readonly string method = "getTeachers";
        }

        //Get list of students
        public class Students : WebUntisQuery {
            public string id;
            public readonly string method = "getStudents";
        }
        #endregion
    }
}
