using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.Classes;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.ClassregEvents;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.CurrentSchoolyear;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.Departments;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.Exams;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.ExamTypes;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.Holidays;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.LastImportTime;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.PersonIdSearch;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.Rooms;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.SchoolYears;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.Sessions;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.StatusData;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.Students;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.Subjects;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.Substitutions;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.Teachers;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.Timegrid;
using mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.TimetableForElement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WebUntisSharp;
using Schoolyear = mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes.CurrentSchoolyear.Schoolyear;
using wus = mrousavy.APIs.WebUntisSharp.WebUnitsJsonSchemes;

namespace mrousavy.APIs.WebUntisSharp {

    //Helper Class for WebUntis Requests/Responses (JSON/POST)
    public class WebUntis : IDisposable {
        #region Privates
        private readonly string _url;
        private bool _loggedIn;
        #endregion

        #region Publics
        public string SessionId;
        public bool SuppressErrors;
        public static Logger Logger = new Logger(Logger.LogLevel.Debug);
        #endregion

        #region Statics
        public static string LastErrorMessage => wus.LastError.Message;
        public static int LastErrorCode => wus.LastError.Code;
        #endregion

        #region Constructor

        /// <summary>
        /// Create a new <seealso cref="WebUntis"/> Object and start a new Session (use <seealso cref="WebUntis.New(..)"/> for async login)
        /// </summary>
        /// <param name="user">The Username of the User to Login</param>
        /// <param name="password">The Password of the User to Login</param>
        /// <param name="schoolUrl">The URL of the WebUntis JSON URL (e.g. http(s)://&lt;SERVER&gt;/WebUntis/jsonrpc.do)</param>
        public WebUntis(string user, string password, string schoolUrl) : this(user, password, schoolUrl, "WebUntisSharp") {

        }

        /// <summary>
        /// Create a new <seealso cref="WebUntis"/> Object and start a new Session (use <seealso cref="WebUntis.New(..)"/> for async login)
        /// </summary>
        /// <param name="user">The Username of the User to Login</param>
        /// <param name="password">The Password of the User to Login</param>
        /// <param name="schoolUrl">The URL of the WebUntis JSON URL (e.g. http(s)://&lt;SERVER&gt;/WebUntis/jsonrpc.do)</param>
        /// <param name="client">The Client (e.g. "ANDROID")</param>
        public WebUntis(string user, string password, string schoolUrl, string client) {
            _url = schoolUrl;

            Login(user, password, client).GetAwaiter().GetResult();
        }


        /// <summary>
        /// Create a new <seealso cref="WebUntis"/> Object and do not login yet (for WebUntis.New(..))
        /// </summary>
        private WebUntis(string schoolUrl) {
            _url = schoolUrl;
        }

        /// <summary>
        /// Create a new <seealso cref="WebUntis"/> Object and start a new Session asynchronously
        /// </summary>
        /// <param name="user">The Username of the User to Login</param>
        /// <param name="password">The Password of the User to Login</param>
        /// <param name="schoolUrl">The URL of the WebUntis JSON URL (e.g. http(s)://&lt;SERVER&gt;/WebUntis/jsonrpc.do)</param>
        public static async Task<WebUntis> New(string user, string password, string schoolUrl) {
            return await New(user, password, schoolUrl, "WebUntisSharp");
        }

        /// <summary>
        /// Create a new <seealso cref="WebUntis"/> Object and start a new Session asynchronously
        /// </summary>
        /// <param name="user">The Username of the User to Login</param>
        /// <param name="password">The Password of the User to Login</param>
        /// <param name="schoolUrl">The URL of the WebUntis JSON URL (e.g. http(s)://&lt;SERVER&gt;/WebUntis/jsonrpc.do)</param>
        /// <param name="client">The Client (e.g. "ANDROID")</param>
        public static async Task<WebUntis> New(string user, string password, string schoolUrl, string client) {
            WebUntis untis = new WebUntis(schoolUrl);
            await untis.Login(user, password, client);
            return untis;
        }

        #endregion

        #region Session Management

        /// <summary>
        /// Create a new session and login with given credentials
        /// </summary>
        /// <param name="user">Username for the WebUntis account</param>
        /// <param name="password">Password for the WebUntis account</param>
        /// <param name="client">The Client (e.g. "ANDROID")</param>
        /// <returns></returns>
        public async Task Login(string user, string password, string client) {
            if (_loggedIn) {
                Logger.Append(Logger.LogLevel.Error, "Tried to login on a logged in object!");
                throw new WebUntisException("This object is already logged in!");
            }

            //Login to WebUntis
            //Get the JSON
            Authentication auth = new Authentication {
                @params = new Authentication.Params {
                    user = user,
                    password = password,
                    client = client
                }
            };

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(auth);
            string responseJson = await SendJsonAndWait(requestJson, _url, null);

            if (responseJson.Contains("<!DOCTYPE html>")) {
                Logger.Append(Logger.LogLevel.Error, "The school url is invalid, server responded with HTML!");
                throw new WebUntisException("The school url is invalid, server responded with HTML!");
            }

            //Parse JSON to Class
            AuthenticationResult result = JsonConvert.DeserializeObject<AuthenticationResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (/*!SuppressErrors &&*/ errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Get Session ID
            SessionId = result.result.sessionId;

            _loggedIn = true;
        }


        /// <summary>
        /// Logout/End the current Session.
        /// An application should always logout as soon as possible to free system resources on the server
        /// </summary>
        public async Task Logout() {
            if (!_loggedIn) {
                Logger.Append(Logger.LogLevel.Error, "Tried to log out a not logged in object!");
                throw new WebUntisException("This object is not logged in!");
            }

            //Get the JSON
            Logout logout = new Logout();

            //Send JSON to WebUntis
            string requestJson = JsonConvert.SerializeObject(logout);
            await SendJson(requestJson, _url, SessionId);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }
        }

        #endregion

        #region Untis calls

        /// <summary>
        /// Get a List of all Teachers
        /// </summary>
        /// <returns>The <see cref="List{Teacher}"/> of all returned Teachers.</returns>
        public async Task<List<Teacher>> GetTeachers() {
            //Get the JSON
            GetTeachers teachers = new GetTeachers();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(teachers);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            TeachersResult result = JsonConvert.DeserializeObject<TeachersResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return all the Teachers
            return new List<Teacher>(result.result);
        }

        /// <summary>
        /// Get a List of all Students
        /// </summary>
        /// <returns>The <see cref="List{Student}"/> of all returned Students.</returns>
        public async Task<List<Student>> GetStudents() {
            //Get the JSON
            GetStudents students = new GetStudents();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(students);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            StudentsResult result = JsonConvert.DeserializeObject<StudentsResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            return result.result != null ? new List<Student>(result.result) : new List<Student>();
        }

        /// <summary>
        /// Get a List of all Classes
        /// </summary>
        /// <param name="schoolyearId">The ID of the school year to query Classes</param>
        /// <returns>The <see cref="List{Class}"/> of all returned Classes.</returns>
        public async Task<List<Class>> GetClasses(string schoolyearId) {
            //Get the JSON
            GetClasses classes = new GetClasses {
                @params = new GetClasses.Params() {
                    schoolyearId = schoolyearId
                }
            };

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(classes);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            ClassesResult result = JsonConvert.DeserializeObject<ClassesResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return all the Classes
            return new List<Class>(result.result);
        }

        /// <summary>
        /// Get a List of all Subjects
        /// </summary>
        /// <returns>The <see cref="List{Subject}"/> of all returned Subjects.</returns>
        public async Task<List<Subject>> GetSubjects() {
            //Get the JSON
            GetSubjects subjects = new GetSubjects();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(subjects);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            SubjectsResult result = JsonConvert.DeserializeObject<SubjectsResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return all the Subjects
            return new List<Subject>(result.result);
        }

        /// <summary>
        /// Get a List of all Rooms
        /// </summary>
        /// <returns>The <see cref="List{Room}"/> of all returned Rooms.</returns>
        public async Task<List<Room>> GetRooms() {
            //Get the JSON
            GetRooms rooms = new GetRooms();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(rooms);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            RoomsResult result = JsonConvert.DeserializeObject<RoomsResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return all the Rooms
            return new List<Room>(result.result);
        }

        /// <summary>
        /// Get a List of all Departments
        /// </summary>
        /// <returns>The <see cref="List{Department}"/> of all returned Departments.</returns>
        public async Task<List<Department>> GetDepartments() {
            //Get the JSON
            GetDepartments department = new GetDepartments();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(department);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            DepartmentsResult result = JsonConvert.DeserializeObject<DepartmentsResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return all the Departments
            return new List<Department>(result.result);
        }

        /// <summary>
        /// Get a List of all Holidays
        /// </summary>
        /// <returns>The <see cref="List{Holiday}"/> of all returned Holidays.</returns>
        public async Task<List<Holiday>> GetHolidays() {
            //Get the JSON
            GetHolidays holidays = new GetHolidays();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(holidays);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            HolidaysResult result = JsonConvert.DeserializeObject<HolidaysResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return all the Holidays
            return new List<Holiday>(result.result);
        }

        /// <summary>
        /// Get the current Timegrid
        /// </summary>
        /// <returns>The returned Timegrid object</returns>
        public async Task<Timegrid> GetTimegrid() {
            //Get the JSON
            GetTimegrid timegrid = new GetTimegrid();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(timegrid);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            Timegrid result = JsonConvert.DeserializeObject<Timegrid>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return the Timegrid
            return result;
        }

        /// <summary>
        /// Information about lesson types and period codes and their colors
        /// </summary>
        /// <returns>The returned StatusData</returns>
        public async Task<StatusData> GetStatusData() {
            //Get the JSON
            GetStatusData statusData = new GetStatusData();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(statusData);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            StatusData result = JsonConvert.DeserializeObject<StatusData>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return the Status Data
            return result;
        }

        /// <summary>
        /// Get the current schoolyear
        /// </summary>
        /// <returns>The current Schoolyear</returns>
        public async Task<Schoolyear> GetSchoolyear() {
            //Get the JSON
            CurrentSchoolyear schoolyear = new CurrentSchoolyear();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(schoolyear);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            Schoolyear result = JsonConvert.DeserializeObject<Schoolyear>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return the Schoolyear
            return result;
        }

        /// <summary>
        /// Get a list of all Schoolyears
        /// </summary>
        /// <returns>The returned Schoolyears</returns>
        public async Task<List<Schoolyear>> GetSchoolyears() {
            //Get the JSON
            Schoolyears schoolyears = new Schoolyears();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(schoolyears);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            wus.SchoolYears.SchoolyearResult result = JsonConvert.DeserializeObject<wus.SchoolYears.SchoolyearResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return the Schoolyears
            return new List<Schoolyear>(result.result);
        }

        /// <summary>
        /// Get a Timetable for a given Element
        /// </summary>
        /// <param name="elementId">The ID of the Element</param>
        /// <param name="elementType">The type of the Element (1 = klasse, 2 = teacher, 3 = subject, 4 = room, 5 = student)</param>
        /// <param name="startDate">The Start Date of the Timetable</param>
        /// <param name="endDate">The End Date of the Timetable</param>
        /// <returns>The returned Timetable</returns>
        public async Task<TimetableResult> GetTimetableForElement(int elementId, int elementType, long startDate, long endDate) {
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
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            TimetableResult result = JsonConvert.DeserializeObject<TimetableResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return the Timetable for the Element
            return result;
        }

        /// <summary>
        /// Get the import time of the last lesson/timetable or substitution import from Untis
        /// </summary>
        /// <returns>The returned Import Time</returns>
        public async Task<DateTime> GetLastImportTime() {
            //Get the JSON
            LastImportTime lastImport = new LastImportTime();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(lastImport);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            LastImportTimeResult result = JsonConvert.DeserializeObject<LastImportTimeResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return the Last Imported Time (DateTime)
            return result.Result;
        }

        /// <summary>
        /// Get the ID of a Person (teacher/student) by Parameters
        /// </summary>
        /// <param name="personType">The Type of Person | 2 = Teacher, 5 = Student</param>
        /// <param name="surname">The Surname of the Person to query</param>
        /// <param name="forename">The Forename of the Person to query</param>
        /// <param name="birthdata">The Birthdata of the Person (Default is 0)</param>
        /// <returns>The Person's ID</returns>
        public async Task<int> GetPersonId(int personType, string surname, string forename, int birthdata = 0) {
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
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            SearchPersonIdResult result = JsonConvert.DeserializeObject<SearchPersonIdResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return the Person ID
            return result.result;
        }

        /// <summary>
        /// Get Substitutions for the given data range
        /// </summary>
        /// <param name="startDate">The Begin Date of the Substitutions to filter</param>
        /// <param name="endDate">The End Date of the Substitutions to filter</param>
        /// <param name="departmentId">The ID of the Department (default = 0)</param>
        /// <returns>The Substitution(s)</returns>
        public async Task<Substitution[]> GetSubstitution(long startDate, long endDate, int departmentId = 0) {
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
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            SubstitutionResult result = JsonConvert.DeserializeObject<SubstitutionResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return the Substitutions
            return result.result;
        }

        /// <summary>
        /// Get ClassregEvents for the given range (requires permissions)
        /// </summary>
        /// <param name="startDate">The Begin Date of the ClassregEvents to filter (unix time)</param>
        /// <param name="endDate">The End Date of the ClassregEvents to filter (unix time)</param>
        /// <returns>The Events(s)</returns>
        public async Task<Event[]> GetClassRegEvents(long startDate, long endDate) {
            //Get the JSON
            ClassregEvents classreg = new ClassregEvents {
                @params = new ClassregEvents.Params {
                    startDate = startDate,
                    endDate = endDate
                }
            };

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(classreg);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            ClassregEventsResult result = JsonConvert.DeserializeObject<ClassregEventsResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return the ClassregEvent(s)
            return result.result;
        }

        /// <summary>
        /// Get Exams
        /// </summary>
        /// <param name="startDate">The Begin Date of the ClassregEvents to filter</param>
        /// <param name="endDate">The End Date of the ClassregEvents to filter</param>
        /// <param name="examTypeId">The Exam Type ID</param>
        /// <returns>The Exam(s)</returns>
        public async Task<Exam[]> GetExams(long startDate, long endDate, int examTypeId) {
            //Get the JSON
            RequestExams requestExams = new RequestExams {
                @params = {
                    [0] = new RequestExams.Params {
                        startDate = startDate,
                        endDate = endDate,
                        examTypeId = examTypeId
                    }
                }
            };

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(requestExams);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            ExamResult result = JsonConvert.DeserializeObject<ExamResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return the Exams(s)
            return result.result;
        }


        /// <summary>
        /// Get all Exam Types
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns>The Exam Types(s)</returns>
        public async Task<Exam[]> GetExamTypes() {
            //Get the JSON
            ExamTypes requestExams = new ExamTypes();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(requestExams);
            string responseJson = await SendJsonAndWait(requestJson, _url, SessionId);

            //Parse JSON to Class
            ExamResult result = JsonConvert.DeserializeObject<ExamResult>(responseJson);

            string errorMsg = wus.LastError.Message;
            if (!SuppressErrors && errorMsg != null) {
                Logger.Append(Logger.LogLevel.Error, errorMsg);
                throw new WebUntisException(errorMsg);
            }

            //Return the Exams Types(s)
            return result.result;
        }

        #endregion

        #region Private Methods
        //Send JSON
        private static async Task SendJson(string json, string url, string sessionId) {
            Uri uri = new Uri(url);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            //Add JSESSION ID Cookie
            if (httpWebRequest.CookieContainer == null)
                httpWebRequest.CookieContainer = new CookieContainer();

            if (!string.IsNullOrWhiteSpace(sessionId))
                httpWebRequest.CookieContainer.Add(new Cookie("JSESSIONID", sessionId, "/", uri.Host));

            using (StreamWriter streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync())) {
                await streamWriter.WriteAsync(json);
                Logger.Append(Logger.LogLevel.Info, $"Sent json: {json}");
                streamWriter.Flush();
                streamWriter.Close();
            }
        }

        //Send JSON and wait for response
        private static async Task<string> SendJsonAndWait(string json, string url, string sessionId) {
            string result;
            Uri uri = new Uri(url);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            //Add JSESSION ID Cookie
            if (httpWebRequest.CookieContainer == null)
                httpWebRequest.CookieContainer = new CookieContainer();

            if (!string.IsNullOrWhiteSpace(sessionId))
                httpWebRequest.CookieContainer.Add(new Cookie("JSESSIONID", sessionId, "/", uri.Host));

            using (StreamWriter streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync())) {
                await streamWriter.WriteAsync(json);
                Logger.Append(Logger.LogLevel.Info, $"Sent json: {json}");
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
            Stream responseStream = httpResponse.GetResponseStream();
            if (responseStream == null)
                throw new WebUntisException("Response Stream was null!");

            using (StreamReader streamReader = new StreamReader(responseStream)) {
                result = await streamReader.ReadToEndAsync();
                Logger.Append(Logger.LogLevel.Info, $"Received json: {result}");
            }

            return result;
        }

        //Send JSON and wait for response (not async) (only for Login/Constructor Method)
        private static string SendJsonAndWaitSynchronous(string json, string url) {
            string result;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {
                streamWriter.Write(json);
                Logger.Append(Logger.LogLevel.Info, $"Sent json: {json}");
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpResponse.GetResponseStream();
            if (responseStream == null)
                throw new WebUntisException("Response Stream was null!");

            using (StreamReader streamReader = new StreamReader(responseStream)) {
                result = streamReader.ReadToEnd();
                Logger.Append(Logger.LogLevel.Info, $"Received json: {result}");
            }

            return result;
        }
        #endregion

        #region IDisposable Support
        ~WebUntis() {
            Dispose();
        }

        public void Dispose() {
            SuppressErrors = true;

            try {
                //sync logout
                Logout().GetAwaiter().GetResult();
            } catch {
                //already logged out
            }

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}