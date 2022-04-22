using sqlite_Example;
using System;
using System.Data;

namespace Email_Person_Generator
{
    class Stories
    {
        static public string generate_story(SQLiteDatabase mydatabase, bool html, string assignee, string release, bool PM, string TEAM)
        {
            string blob = "";
            DataTable ds = null;
            try
            {
                string sql = string.Format("SELECT DISTINCT(`Issue Key`) FROM STORIES WHERE ASSIGNEE ='{0}'", assignee);
                ds = mydatabase.GetDataTable(sql);
                foreach (DataRow dr in ds.Rows)
                {
                    string story = dr["Issue Key"].ToString();
                    blob = blob + check_story(story, release, html, mydatabase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(4);
            }

            if(PM && Functionality.Enabled("Story_Missing_Assignee", mydatabase))
            {
                try
                {
                    string sql = string.Format("SELECT DISTINCT(`Issue Key`) FROM STORIES WHERE ASSIGNEE IS NULL AND `Custom field (Team)` = '{0}'", TEAM);
                    ds = mydatabase.GetDataTable(sql);
                    foreach (DataRow dr in ds.Rows)
                    {
                        string story = dr["Issue Key"].ToString();
                        if (html) blob = blob + string.Format("<span>{0} has no assignee</span><br/>\n", story);
                        else blob = blob + string.Format("{0} has no assignee\n", story);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Environment.Exit(4);
                }
            }

            return blob;
        }
        static private string check_story(string story_id, string release, bool html, SQLiteDatabase mydatabase)
        {
            string blob = "";
            blob = blob + Missing_Field.check("Story_Missing_Summary",
                                              "Stories",
                                              "\"Issue Key\"",
                                              html,
                                              "Summary",
                                              string.Format("`Issue Key` = '{0}'", story_id),
                                              "a missing summary",
                                              mydatabase);
            blob = blob + Missing_Field.check("Story_Missing_Status",
                                              "Stories",
                                              "\"Issue Key\"",
                                              html,
                                              "Status",
                                              string.Format("`Issue Key` = '{0}'", story_id),
                                              "a missing status",
                                              mydatabase);
            blob = blob + Missing_Field.check("Story_Missing_Sprint",
                                              "Stories",
                                              "\"Issue Key\"",
                                              html,
                                              "Sprint",
                                              string.Format("`Issue Key` = '{0}'", story_id),
                                              "no assigned sprint",
                                              mydatabase);
            blob = blob + Missing_Field.check("Story_Missing_Fix_Version",
                                              "Stories",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Fix Version/s\"",
                                              string.Format("`Issue Key` = '{0}'", story_id),
                                              "no assigned Fix Version",
                                              mydatabase);
            blob = blob + Missing_Field.check("Story_Missing_Release",
                                              "Stories",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (Release)\"",
                                              string.Format("`Issue Key` = '{0}'", story_id),
                                              "no assigned release",
                                              mydatabase);
            blob = blob + Missing_Field.check("Story_Missing_Assignee",
                                              "Stories",
                                              "\"Issue Key\"",
                                              html,
                                              "Assignee",
                                              string.Format("`Issue Key` = '{0}'", story_id),
                                              "no assigned Assignee",
                                              mydatabase);
            blob = blob + Missing_Field.check("Story_Missing_Team",
                                              "Stories",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (Team)\"",
                                              string.Format("`Issue Key` = '{0}'", story_id),
                                              "no assigned team",
                                              mydatabase);
            blob = blob + Missing_Field.check("Story_Missing_Parent_Link_To_Feature",
                                              "Stories",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (Parent Link)\"",
                                              string.Format("`Issue Key` = '{0}'", story_id),
                                              "no Parent Link to Feature",
                                              mydatabase);
            blob = blob + hasTests(story_id, html, mydatabase);
            blob = blob + Incorrect_Fix_Version.check("Story_Incorrect_Fix_Version", "Stories", string.Format("`Issue Key` = '{0}'", story_id), release, html, mydatabase);

            if (0 != blob.Length)
            {
                if (html) blob = string.Format("<span>Story {0} has</span>\n", story_id) + blob + "<br/>\n";
                else blob = string.Format("Story {0} has\n", story_id) + blob + "\n\n";
            }
            return blob;

        }

        static private string hasTests(string story_id, bool html, SQLiteDatabase mydatabase)
        {
            string blob = "";
            try
            {
                string sql = string.Format("SELECT COUNT(*) FROM Test_Cases A INNER JOIN Outward_Relates_Link B ON A.`Issue Key` = B.`Issue Key` WHERE B.link = '{0}'", story_id);
                DataTable ds = mydatabase.GetDataTable(sql);
                foreach (DataRow dr in ds.Rows)
                {
                    int count = 0;
                    try
                    {
                        string tests = dr["COUNT(*)"].ToString();
                        count = int.Parse(tests);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    if (0 == count)
                    {
                        if (html) blob = blob + "<span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Has no tests linked</span>\n";
                        else blob = blob + "\tHas no tests linked\n";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(4);
            }

            return blob;
        }
    }
}
