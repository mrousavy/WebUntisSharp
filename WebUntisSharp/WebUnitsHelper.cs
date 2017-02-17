using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using WebUntisSharp.WebUnitsJsonSchemes.Sessions;
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
            GetTeachers teacher = new GetTeachers();

            //Send and receive JSON from WebUntis
            string requestJson = JsonConvert.SerializeObject(teacher);
            string responseJson = SendJsonAndWait(requestJson, _url);

            TeachersResult result = JsonConvert.DeserializeObject<TeachersResult>(responseJson);

            if(wus.LastError.Message != null)
                throw new Exception(wus.LastError.Message);

            //Return all the Teachers
            return new List<Teacher>(result.result);
        }

        //Get List of Teachers
        public List<wus.Teachers.Teacher> GetTeachers(int id) {
            wus.Teachers.GetTeachers teachers = new wus.Teachers.GetTeachers() { id = id.ToString() };
            string queryJson = JsonConvert.SerializeObject(teachers);

            string responseJson = SendJsonAndWait(queryJson, _url);
            wus.Teachers.TeachersResult teacherResult = JsonConvert.DeserializeObject<wus.Teachers.TeachersResult>(responseJson);

            List<wus.Teachers.Teacher> result = new List<wus.Teachers.Teacher>(teacherResult.result);
            return result;
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
