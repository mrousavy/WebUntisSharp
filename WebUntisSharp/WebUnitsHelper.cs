using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using WebUntisSharp.WebUnitsJsonSchemes.Classes;
using WebUntisSharp.WebUnitsJsonSchemes.ClassregEvents;
using WebUntisSharp.WebUnitsJsonSchemes.CurrentSchoolyear;
using WebUntisSharp.WebUnitsJsonSchemes.Departments;
using WebUntisSharp.WebUnitsJsonSchemes.Exams;
using WebUntisSharp.WebUnitsJsonSchemes.Holidays;
using WebUntisSharp.WebUnitsJsonSchemes.LastImportTime;
using WebUntisSharp.WebUnitsJsonSchemes.PersonIdSearch;
using WebUntisSharp.WebUnitsJsonSchemes.Rooms;
using WebUntisSharp.WebUnitsJsonSchemes.SchoolYears;
using WebUntisSharp.WebUnitsJsonSchemes.Sessions;
using WebUntisSharp.WebUnitsJsonSchemes.StatusData;
using WebUntisSharp.WebUnitsJsonSchemes.Students;
using WebUntisSharp.WebUnitsJsonSchemes.Subjects;
using WebUntisSharp.WebUnitsJsonSchemes.Substitutions;
using WebUntisSharp.WebUnitsJsonSchemes.Teachers;
using WebUntisSharp.WebUnitsJsonSchemes.Timegrid;
using WebUntisSharp.WebUnitsJsonSchemes.TimetableForElement;
using Schoolyear = WebUntisSharp.WebUnitsJsonSchemes.CurrentSchoolyear.Schoolyear;
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

            //Return all the Subjects
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

            //Return all the Rooms
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

            //Return all the Departments
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

            //Return all the Holidays
            return new List<Holiday>(result.result);
        }

        /// <summary>
        /// Get the Timegrid
        /// </summary>
        /// <returns>The returned Timegrid</returns>
        public Timegrid GetTimegrid() {
            //Get the JSON
            GetTimegrid timegrid = new GetTimegrid();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(timegrid);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            Timegrid result = JsonConvert.DeserializeObject<Timegrid>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return the Timegrid
            return result;
        }

        /// <summary>
        /// Get the StatusData
        /// </summary>
        /// <returns>The returned StatusData</returns>
        public StatusData GetStatusData() {
            //Get the JSON
            GetStatusData statusData = new GetStatusData();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(statusData);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            StatusData result = JsonConvert.DeserializeObject<StatusData>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return the Status Data
            return result;
        }

        /// <summary>
        /// Get the Current Schoolyear
        /// </summary>
        /// <returns>The current Schoolyear</returns>
        public Schoolyear GetSchoolyear() {
            //Get the JSON
            CurrentSchoolyear schoolyear = new CurrentSchoolyear();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(schoolyear);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            Schoolyear result = JsonConvert.DeserializeObject<Schoolyear>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return the Schoolyear
            return result;
        }

        /// <summary>
        /// Get all Schoolyears
        /// </summary>
        /// <returns>The returned Schoolyears</returns>
        public List<Schoolyear> GetSchoolyears() {
            //Get the JSON
            Schoolyears schoolyears = new Schoolyears();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(schoolyears);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            wus.SchoolYears.SchoolyearResult result = JsonConvert.DeserializeObject<wus.SchoolYears.SchoolyearResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return the Schoolyears
            return new List<Schoolyear>(result.result);
        }

        /// <summary>
        /// Get a Timetable for an Element
        /// </summary>
        /// <param name="elementId">The ID of the Element</param>
        /// <param name="elementType">The type of the Element (1 = klasse, 2 = teacher, 3 = subject, 4 = room, 5 = student)</param>
        /// <param name="startDate">The Start Date of the Timetable</param>
        /// <param name="endDate">The End Date of the Timetable</param>
        /// <returns>The returned Timetable</returns>
        public TimetableResult GetTimetableForElement(int elementId, int elementType, long startDate, long endDate) {
            //Get the JSON
            TimetableForElement timetable = new TimetableForElement() {
                @params = new TimetableForElement.Params() {
                    id = elementId,
                    type = elementType,
                    startDate = startDate,
                    endDate = endDate
                }
            };

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(timetable);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            TimetableResult result = JsonConvert.DeserializeObject<TimetableResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return the Timetable for the Element
            return result;
        }

        /// <summary>
        /// Get the Last Import Time
        /// </summary>
        /// <returns>The returned Import Time</returns>
        public DateTime GetLastImportTime() {
            //Get the JSON
            LastImportTime lastImport = new LastImportTime();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(lastImport);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            LastImportTimeResult result = JsonConvert.DeserializeObject<LastImportTimeResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return the Last Imported Time (DateTime)
            return result.result;
        }

        /// <summary>
        /// Get the ID of a Person by Parameters
        /// </summary>
        /// <param name="personType">The Type of Person | 2 = Teacher, 5 = Student</param>
        /// <param name="surname">The Surname of the Person to query</param>
        /// <param name="forename">The Forename of the Person to query</param>
        /// <param name="birthdata">The Birthdata of the Person (Default is 0)</param>
        /// <returns>The Person's ID</returns>
        public int GetPersonId(int personType, string surname, string forename, int birthdata = 0) {
            //Get the JSON
            SearchPersonId personId = new SearchPersonId {
                @params = new SearchPersonId.Params() {
                    dob = birthdata,
                    fn = forename,
                    sn = surname,
                    type = personType
                }
            };

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(personId);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            SearchPersonIdResult result = JsonConvert.DeserializeObject<SearchPersonIdResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return the Person ID
            return result.result;
        }

        /// <summary>
        /// Get Substitutions
        /// </summary>
        /// <param name="startDate">The Begin Date of the Substitutions to filter</param>
        /// <param name="endDate">The End Date of the Substitutions to filter</param>
        /// <param name="departmentId">The ID of the Department (default = 0)</param>
        /// <returns>The Substitution(s)</returns>
        public Substitution[] GetSubstitution(long startDate, long endDate, int departmentId = 0) {
            //Get the JSON
            Substitutions substitutions = new Substitutions {
                @params = new Substitutions.Params {
                    startDate = startDate,
                    endDate = endDate,
                    departmentId = departmentId
                }
            };

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(substitutions);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            SubstitutionResult result = JsonConvert.DeserializeObject<SubstitutionResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return the Substitutions
            return result.result;
        }

        /// <summary>
        /// Get ClassregEvents
        /// </summary>
        /// <param name="startDate">The Begin Date of the ClassregEvents to filter</param>
        /// <param name="endDate">The End Date of the ClassregEvents to filter</param>
        /// <returns>The Events(s)</returns>
        public Event[] GetClassRegEvents(long startDate, long endDate) {
            //Get the JSON
            ClassregEvents classreg = new ClassregEvents {
                @params = new ClassregEvents.Params {
                    startDate = startDate,
                    endDate = endDate
                }
            };

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(classreg);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            ClassregEventsResult result = JsonConvert.DeserializeObject<ClassregEventsResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return the ClassregEvent(s)
            return result.result;
        }

        /// <summary>
        /// Get ClassregEvents
        /// </summary>
        /// <param name="startDate">The Begin Date of the ClassregEvents to filter</param>
        /// <param name="endDate">The End Date of the ClassregEvents to filter</param>
        /// <param name="examTypeId">The Exam Type ID</param>
        /// <returns>The Exam(s)</returns>
        public Exam[] GetExams(long startDate, long endDate, int examTypeId) {
            //Get the JSON
            RequestExams requestExams = new RequestExams {
                @params = new RequestExams.Params {
                    startDate = startDate,
                    endDate = endDate,
                    examTypeId = examTypeId
                }
            };

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(requestExams);
            string responseJson = SendJsonAndWait(requestJson, _url);

            //Parse JSON to Class
            ExamResult result = JsonConvert.DeserializeObject<ExamResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return the Exams(s)
            return result.result;
        }

        /// <summary>
        /// Get Exam Types (Not yet Implemented)
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns>The Exam Types(s)</returns>
        public Exam[] GetExamTypes() {
            throw new NotImplementedException();

            ////Get the JSON
            //ExamTypes requestExams = new ExamTypes();

            ////Send and receive JSON from WebUntis
            //string requestJson = JsonConvert.SerializeObject(requestExams);
            //string responseJson = SendJsonAndWait(requestJson, _url);

            ////Parse JSON to Class
            //ExamResult result = JsonConvert.DeserializeObject<ExamResult>(responseJson);

            //if(wus.LastError.Message != null)
            //    throw new Exception(wus.LastError.Message);

            ////Return the Exams Types(s)
            //return result.result;
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
            Stream responseStream = httpResponse.GetResponseStream();
            if(responseStream == null)
                throw new Exception("Response Stream was null!");

            using(StreamReader streamReader = new StreamReader(responseStream)) {
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
