using sqlite_Example;
using System;
using System.Collections.Generic;
using System.Data;

namespace Email_Person_Generator
{
    class Build_Message
    {
        static private bool PM;
        static private string TEAM;

        static public void build_message(string database, string assignee, DateTime release_date, string release, bool verbose, bool html, string signature, string email_tool, string email_sender, bool fire, List<string> extra_cc_people, List<string> extra_bcc_people)
        {
            PM = false;
            TEAM = null;
            SQLiteDatabase mydatabase = new SQLiteDatabase(database);
            isPM(mydatabase, assignee);

            string blob = Stories.generate_story(mydatabase, html, assignee, release, PM, TEAM);
            blob = blob + Tests.generate_tests(mydatabase, html, assignee, release, PM, TEAM);
            blob = blob + Sub_tasks.generate_sub_tasks(mydatabase, html, assignee, release, PM, TEAM);
            blob = blob + Features.generate_features(mydatabase, html, assignee, release, PM, TEAM);


            if (0 == blob.Length)
            {
                Console.WriteLine(string.Format("Nothing of interest found for: {0}", assignee));
                return;
            }

            // Final trim
            blob = TimeTable.timely_header(release_date) + blob;
            if (html) blob = "<html><body>" + blob;
            if (null == signature) blob = blob + "</body></html>";

            // Deal with all the people being contacted
            List<string> tos = Send_Emails.team(assignee, mydatabase);
            
            // Now send the "message"
            Send_Emails.send(release, blob, email_tool, email_sender, tos, fire, extra_cc_people, extra_bcc_people, signature);

            mydatabase.SQLiteDatabase_Close();
            Console.WriteLine(blob);
        }

        static private void isPM(SQLiteDatabase mydatabase, string assignee)
        {
            try
            {
                string sql = string.Format("SELECT ROLE, Team FROM Agile_Teams WHERE Username = '{0}'", assignee);
                DataTable ds = mydatabase.GetDataTable(sql);
                if(ds.Rows.Count != 1)
                {
                    Console.WriteLine(string.Format("{0} is a member of more than 1 team, results will be wrong if a Scrum Master", assignee));
                }
                foreach(DataRow dr in ds.Rows)
                {
                    TEAM = dr["Team"].ToString();
                    string role = dr["ROLE"].ToString().Trim();
                    if (role.Equals("Scrum Master")) PM = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(4);
            }
        }
    }
}
