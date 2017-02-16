using Newtonsoft.Json;
using System.Collections.Generic;
using wus = WebUntisSharp.WebUnitsJsonSchemes;

namespace WebUntisSharp {

    //Helper Class for WebUntis Requests/Responses (JSON/POST)
    public class WebUnitsHelper {
        //Get List of Teachers
        public static List<wus.Teachers.Teacher> GetTeachers(int id) {
            wus.Teachers.GetTeachers teachers = new wus.Teachers.GetTeachers() { id = id.ToString() };
            string queryJson = JsonConvert.SerializeObject(teachers);

            //TODO: Send queryJson

            //TODO: Get response
            string responseJson = "";
            wus.Teachers.TeachersResult teacherResult = JsonConvert.DeserializeObject<wus.Teachers.TeachersResult>(responseJson);

            List<wus.Teachers.Teacher> result = new List<wus.Teachers.Teacher>(teacherResult.result);
            return result;
        }
    }
}
