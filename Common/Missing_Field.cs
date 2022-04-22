using sqlite_Example;
using System;
using System.Data;

namespace Email_Person_Generator
{
    class Missing_Field
    {
        static public string check(string feature, string table, string primary_key, bool html, string col, string key, string message, SQLiteDatabase mydatabase)
        {
            if (!Functionality.Enabled(feature, mydatabase)) return "";
            string block = "";
            string sql = "";

            DataTable ds = null;
            try
            {
                sql = string.Format("SELECT DISTINCT {0}, {1} FROM {2} WHERE {3}", primary_key, col, table, key);
                ds = mydatabase.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(sql);
                Environment.Exit(4);
            }

            // Clean up behave difference etween C# and sql
            col = col.Replace("\"", string.Empty);
            primary_key = primary_key.Replace("\"", string.Empty);

            foreach (DataRow dr in ds.Rows)
            {
                try
                {
                    string content = dr[col].ToString();
                    if(0 == content.Length)
                    {
                        string hold = dr[primary_key].ToString();
                        if(html) block = block + "<span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + string.Format(message, hold) + "</span>\n";
                        else block = block + string.Format("\t" + message + "\n", hold);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
            return block;
        }
    }
}
