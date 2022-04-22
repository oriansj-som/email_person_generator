using sqlite_Example;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Email_Person_Generator
{
    class Send_Emails
    {
        static public bool send(string release, string block, string program, string sender, List<string> target, bool fire, List<string> extra_cc_people, List<string> extra_bcc_people, string signature)
        {
            TemporaryFile t = new TemporaryFile();
            if (!Directory.Exists(".\\" + release))
            {
                Directory.CreateDirectory(".\\" + release);
            }
            string o = Path.GetFullPath(".\\" + release);
            File.WriteAllText(t.FilePath, block);
            string reciepients = string.Join(" --to ", target);
            Process proc = new Process();
            proc.StartInfo.FileName = program;
            string cmd = string.Format("--to {0} --from {1} --message-file-plain \"{2}\" --pickup-folder \"{3}\" --verbose", reciepients, sender, t.FilePath, o);
            foreach(string s in extra_cc_people)
            {
                cmd = cmd + " --cc " + s;
            }
            foreach (string s in extra_bcc_people)
            {
                cmd = cmd + " --bcc " + s;
            }
            if(null != signature)
            {
                cmd = cmd + " --sig-file " + signature;
            }
            proc.StartInfo.Arguments = cmd;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = false;
            bool success = proc.Start();
            proc.WaitForExit();
            if(success)t.Dispose();
            return true;
        }

        static public List<string> team(string target, SQLiteDatabase mydatabase)
        {
            List<string> all = new List<string>();
            all.Add(target+"@michigan.gov");
            try
            {
                // Get team central points
                string sql = string.Format("SELECT Email_Preferred FROM Agile_Teams WHERE Priority_Contact = '0' AND Team in (SELECT TEAM FROM Agile_Teams WHERE Username = '{0}' )", target);
                DataTable ds = mydatabase.GetDataTable(sql);

                foreach(DataRow dr in ds.Rows)
                {
                    string s = dr["Email_Preferred"].ToString();
                    all.Add(s);
                }

                // Get the noisy people too
                sql = string.Format("SELECT Email_Preferred FROM Agile_Teams WHERE Priority_Contact = '-1'");
                ds = mydatabase.GetDataTable(sql);

                foreach (DataRow dr in ds.Rows)
                {
                    string s = dr["Email_Preferred"].ToString();
                    all.Add(s);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(4);
            }

            // Only return non-duplicates
            return all.Distinct().ToList();
        }
    }
}
