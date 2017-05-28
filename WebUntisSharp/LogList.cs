using System;
using System.Collections.Generic;
using static WebUntisSharp.Logger;

namespace WebUntisSharp {
    public class LogList : List<Tuple<LogLevel, string>> {
        public void Add(LogLevel level, string message) {
            Add(new Tuple<LogLevel, string>(level, message));
        }
    }
}
