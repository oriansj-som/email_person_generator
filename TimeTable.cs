using System;

namespace Email_Person_Generator
{
    class TimeTable
    {
        static public string timely_header(DateTime release)
        {
            DateTime T = DateTime.Today;
            TimeSpan delta = release - T;
            if (delta.TotalDays > 21) return "";
            else if (delta.TotalDays > 14) return "There's less than three weeks until implementation.\n\n";
            else if (delta.TotalDays > 7) return "There's less than two weeks until implementation.\n\n";
            else if (delta.TotalDays > 2) return "There's less than a week left until implementation.\n\n";
            else if (delta.TotalDays > 1) return "Implementation is tomorrow.\n\n";
            //else if (delta.TotalDays < 0) return "Missed implementation Window";
            else return "Implementation is tonight.\n\n";
        }
    }
}
