using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using wus = WebUntisSharp.WebUnitsJsonSchemes;

namespace WebUntisSharp {

    //Helper Class for WebUntis Requests/Responses (JSON/POST)
    public class WebUnitsHelper {
        private static string url => "http://" + school + "/WebUntis/jsonrpc.do";
        private static string school;


        //Get List of Teachers
        public static List<wus.Teachers.Teacher> GetTeachers(int id) {
            wus.Teachers.GetTeachers teachers = new wus.Teachers.GetTeachers() { id = id.ToString() };
            string queryJson = JsonConvert.SerializeObject(teachers);

            SendJsonAndWait(queryJson);

            //TODO: Get response
            string responseJson = "";
            wus.Teachers.TeachersResult teacherResult = JsonConvert.DeserializeObject<wus.Teachers.TeachersResult>(responseJson);

            List<wus.Teachers.Teacher> result = new List<wus.Teachers.Teacher>(teacherResult.result);
            return result;
        }



        //Send JSON
        private static void SendJson(string json) {
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
        private static string SendJsonAndWait(string json) {
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
