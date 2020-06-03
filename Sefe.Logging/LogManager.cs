using Sefe.ApplicationSettings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefe.Logging
{
    public class LogManager
    {
        string Source = Settings.GetAppSetting("EventLog_Source", "");
        string AppName = Settings.GetAppSetting("EventLog_ApplicationName", "");
        public LogManager()
        {
            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, AppName);
            }
        }
        public void WriteToLog(string message, EventLogEntryType eventType)
        {
            EventLog.WriteEntry(Source, message, eventType);
        }
    }
}
