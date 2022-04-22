using sqlite_Example;
using System;
using System.Data;

namespace Email_Person_Generator
{
    class Functionality
    {
        static public bool Enabled(string feature, SQLiteDatabase mydatabase)
        {
            DataTable ds = null;
            try
            {
                ds = mydatabase.GetDataTable(string.Format("SELECT Enabled FROM Richard_Features WHERE Feature = '{0}'", feature));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(4);
            }

            if (1 != ds.Rows.Count)
            {
                if (0 == ds.Rows.Count) Console.WriteLine(string.Format("Feature named {0} is unknown", feature));
                else Console.WriteLine(string.Format("Duplcate features: {0} shows up {1} times", feature, ds.Rows.Count));
                return false;
            }

            try
            {
                string hold = ds.Rows[0]["Enabled"].ToString();
                if (hold.Equals("1")) return true;
                if (hold.Equals("0")) return false;
                bool r = bool.Parse(hold);
                return r;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(2);
            }

            // Impossible case
            Console.WriteLine("WTF Missing_Field:enabled");
            Environment.Exit(42);
            return false;
        }
    }
}
