using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using WebUntisSharp.WebUnitsJsonSchemes.Classes;
using WebUntisSharp.WebUnitsJsonSchemes.Departments;
using WebUntisSharp.WebUnitsJsonSchemes.Holidays;
using WebUntisSharp.WebUnitsJsonSchemes.Rooms;
using WebUntisSharp.WebUnitsJsonSchemes.Sessions;
using WebUntisSharp.WebUnitsJsonSchemes.Students;
using WebUntisSharp.WebUnitsJsonSchemes.Subjects;
using WebUntisSharp.WebUnitsJsonSchemes.Teachers;
using wus = WebUntisSharp.WebUnitsJsonSchemes;

namespace WebUntisSharp {

    //Helper Class for WebUntis Requests/Responses (JSON/POST)
    public class WebUntis : IDisposable {
        #region Privates
        private readonly string _url;
        #endregion

        #region Publics
        public string SessionId;
        #endregion

        #region Statics
        public static string LastErrorMessage => wus.LastError.Message;
        public static int LastErrorCode => wus.LastError.Code;
        #endregion

        /// <summary>
        /// Create a new <seealso cref="WebUntis"/> Object and start a new Session
        /// </summary>
        /// <param name="user">The Username of the User to Login</param>
        /// <param name="password">The Password of the User to Login</param>
        /// <param name="schoolUrl">The URL of the WebUntis JSON URL (e.g. http(s)://&lt;SERVER&gt;/WebUntis/jsonrpc.do)</param>
        /// <param name="client">The Client (e.g. "ANDROID")</param>
        public WebUntis(string user, string password, string schoolUrl, string client) {
            _url = schoolUrl;

            //Login to WebUntis
            //Get the JSON
            Authentication auth = new Authentication {
                id = "1",
                @params = new Authentication.Params {
                    user = user,
                    password = password,
                    client = client
                }
            };

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(auth);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            AuthenticationResult result = JsonConvert.DeserializeObject<AuthenticationResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Get Session ID
            SessionId = result.sessionId;
        }

        /// <summary>
        /// Logout/End the current Session.
        /// An application should always logout as soon as possible to free system resources on the server
        /// </summary>
        public void Logout() {
            //Get the JSON
            Logout logout = new Logout {
                id = SessionId
            };

            //Send JSON to WebUntis
            string requestJson = JsonConvert.SerializeObject(logout);
            SendJson(requestJson, _url);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);
        }

        /// <summary>
        /// Get a List of all Teachers
        /// </summary>
        /// <returns>The <see cref="List{Teacher}"/> of all returned Teachers.</returns>
        public List<Teacher> GetTeachers() {
            //Get the JSON
            GetTeachers teachers = new GetTeachers();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(teachers);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            TeachersResult result = JsonConvert.DeserializeObject<TeachersResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return all the Teachers
            return new List<Teacher>(result.result);
        }

        /// <summary>
        /// Get a List of all Students
        /// </summary>
        /// <returns>The <see cref="List{Student}"/> of all returned Students.</returns>
        public List<Student> GetStudents() {
            //Get the JSON
            GetStudents students = new GetStudents();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(students);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            StudentsResult result = JsonConvert.DeserializeObject<StudentsResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return all the Students
            return new List<Student>(result.result);
        }

        /// <summary>
        /// Get a List of all Classes
        /// </summary>
        /// <returns>The <see cref="List{Class}"/> of all returned Classes.</returns>
        public List<Class> GetClasses() {
            //Get the JSON
            GetClasses classes = new GetClasses();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(classes);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            ClassesResult result = JsonConvert.DeserializeObject<ClassesResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return all the Classes
            return new List<Class>(result.result);
        }

        /// <summary>
        /// Get a List of all Subjects
        /// </summary>
        /// <returns>The <see cref="List{Subject}"/> of all returned Subjects.</returns>
        public List<Subject> GetSubjects() {
            //Get the JSON
            GetSubjects subjects = new GetSubjects();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(subjects);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            SubjectsResult result = JsonConvert.DeserializeObject<SubjectsResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return all the Students
            return new List<Subject>(result.result);
        }

        /// <summary>
        /// Get a List of all Rooms
        /// </summary>
        /// <returns>The <see cref="List{Room}"/> of all returned Rooms.</returns>
        public List<Room> GetRooms() {
            //Get the JSON
            GetRooms rooms = new GetRooms();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(rooms);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            RoomsResult result = JsonConvert.DeserializeObject<RoomsResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return all the Students
            return new List<Room>(result.result);
        }

        /// <summary>
        /// Get a List of all Departments
        /// </summary>
        /// <returns>The <see cref="List{Department}"/> of all returned Departments.</returns>
        public List<Department> GetDepartments() {
            //Get the JSON
            GetDepartments department = new GetDepartments();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(department);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            DepartmentsResult result = JsonConvert.DeserializeObject<DepartmentsResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return all the Students
            return new List<Department>(result.result);
        }

        /// <summary>
        /// Get a List of all Holidays
        /// </summary>
        /// <returns>The <see cref="List{Holiday}"/> of all returned Holidays.</returns>
        public List<Holiday> GetHolidays() {
            //Get the JSON
            GetHolidays holidays = new GetHolidays();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(holidays);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            HolidaysResult result = JsonConvert.DeserializeObject<HolidaysResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return all the Students
            return new List<Holiday>(result.result);
        }

        #region Private Methods
        //Send JSON
        private static void SendJson(string json, string url) {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using(StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }

        //Send JSON and wait for response
        private static string SendJsonAndWait(string json, string url) {
            string result;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using(StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using(StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                result = streamReader.ReadToEnd();
            }

            return result;
        }
        #endregion

        #region IDisposable Support
        ~WebUntis() {
            Dispose();
        }

        public void Dispose() {
            Logout();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
