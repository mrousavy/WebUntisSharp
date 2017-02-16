using Newtonsoft.Json;
using System.Collections.Generic;
using WebUntisSharp.WebUnitsJsonSchemes;

namespace WebUntisSharp {

    //Helper Class for WebUntis Requests/Responses (JSON/POST)
    public class WebUnitsHelper {
        //Get List of Teachers
        public static List<string> GetTeachers(int id) {
            Teachers teachers = new Teachers() { id = id.ToString() };
            string json = JsonConvert.SerializeObject(teachers);

            List<string> result = new List<string>();
            return result;
        }


    }
}
