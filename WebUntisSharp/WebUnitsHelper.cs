using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using WebUntisSharp.WebUnitsJsonSchemes.Sessions;
using wus = WebUntisSharp.WebUnitsJsonSchemes;

namespace WebUntisSharp {

    //Helper Class for WebUntis Requests/Responses (JSON/POST)
    public class WebUnits {
        private readonly string _url;

        public string SessionId;


        public WebUnits(string client, string password, string schoolUrl, string user) {
            _url = schoolUrl;

            Authentication auth = new Authentication {
                id = "1",
                @params = new Authentication.Params {
                    client = client,
                    password = password,
                    user = user
                }
            };
            string requestJson = JsonConvert.SerializeObject(auth);

            string responseJson = SendJsonAndWait(requestJson, _url);

            AuthenticationResult result = JsonConvert.DeserializeObject<AuthenticationResult>(responseJson);

            SessionId = result.sessionId;
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
    }
}
