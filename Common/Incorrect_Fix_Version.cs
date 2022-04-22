using sqlite_Example;
using System;
using System.Data;

namespace Email_Person_Generator
{
    class Incorrect_Fix_Version
    {
        static public string check(string feature, string table, string key, string release, bool HTML, SQLiteDatabase mydatabase)
        {
            if (!Functionality.Enabled(feature, mydatabase)) return "";
            string block = "";
            DataTable ds = null;
            try
            {
                string sql = string.Format("SELECT DISTINCT `Fix Version/s` FROM {0} WHERE {1}", table, key);
                ds = mydatabase.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(4);
            }

            foreach(DataRow dr in ds.Rows)
            {
                try
                {
                    string content = dr["Fix Version/s"].ToString().Trim();
                    if (content.Equals("None"))
                    {
                        if (HTML) block = block + string.Format("<span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;no Fix Version, please use: {0}</span>\n", release);
                        else block = block + string.Format("\tno Fix Version, please use: {0}\n", release);
                    }
                    else if(!release.Equals(content))
                    {
                        if (HTML) block = block + string.Format("<span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;incorrect Fix Version, please use: {0} instead</span>\n", release);
                        else block = block + string.Format("\tincorrect Fix Version, please use: {0} instead\n", release);
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
