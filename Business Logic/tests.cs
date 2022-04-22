using sqlite_Example;
using System;
using System.Data;

namespace Email_Person_Generator
{
    class Tests
    {
        static public string generate_tests(SQLiteDatabase mydatabase, bool html, string assignee, string release, bool PM, string TEAM)
        {
            string blob = "";
            DataTable ds = null;
            try
            {
                string sql = string.Format("SELECT DISTINCT(`Issue Key`) FROM TEST_CASES WHERE ASSIGNEE ='{0}'", assignee);
                ds = mydatabase.GetDataTable(sql);
                foreach (DataRow dr in ds.Rows)
                {
                    string story = dr["Issue Key"].ToString();
                    blob = blob + check_tests(story, release, html, mydatabase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(4);
            }

            if (PM && Functionality.Enabled("Test_Missing_Assignee", mydatabase))
            {
                try
                {
                    string sql = string.Format("SELECT DISTINCT(`Issue Key`) FROM TEST_CASES WHERE ASSIGNEE IS NULL AND `Custom field (Team)` = '{0}'", TEAM);
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

        static private string check_tests(string test_id, string release, bool html, SQLiteDatabase mydatabase)
        {
            string blob = "";
            blob = blob + Missing_Field.check("Test_Missing_Summary",
                                              "TEST_CASES",
                                              "\"Issue Key\"",
                                              html,
                                              "Summary",
                                              string.Format("`Issue Key` = '{0}'", test_id),
                                              "a missing summary",
                                              mydatabase);
            blob = blob + Missing_Field.check("Test_Missing_Status",
                                              "TEST_CASES",
                                              "\"Issue Key\"",
                                              html,
                                              "Status",
                                              string.Format("`Issue Key` = '{0}'", test_id),
                                              "a missing status",
                                              mydatabase);
            blob = blob + Missing_Field.check("Test_Missing_Sprint",
                                              "TEST_CASES",
                                              "\"Issue Key\"",
                                              html,
                                              "Sprint",
                                              string.Format("`Issue Key` = '{0}'", test_id),
                                              "no assigned sprint",
                                              mydatabase);
            blob = blob + Missing_Field.check("Test_Missing_Tester_Group",
                                              "TEST_CASES",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (Tester Group)\"",
                                              string.Format("`Issue Key` = '{0}'", test_id),
                                              "no assigned Tester Group",
                                              mydatabase);
            blob = blob + Missing_Field.check("Test_Missing_Fix_Version",
                                              "TEST_CASES",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Fix Version/s\"",
                                              string.Format("`Issue Key` = '{0}'", test_id),
                                              "no assigned Fix Version",
                                              mydatabase);
            blob = blob + Missing_Field.check("Test_Missing_Release",
                                              "TEST_CASES",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (Release)\"",
                                              string.Format("`Issue Key` = '{0}'", test_id),
                                              "no assigned release",
                                              mydatabase);
            blob = blob + Missing_Field.check("Test_Missing_Assignee",
                                              "TEST_CASES",
                                              "\"Issue Key\"",
                                              html,
                                              "Assignee",
                                              string.Format("`Issue Key` = '{0}'", test_id),
                                              "no assigned Assignee",
                                              mydatabase);
            blob = blob + Missing_Field.check("Test_Missing_Team",
                                              "TEST_CASES",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (Team)\"",
                                              string.Format("`Issue Key` = '{0}'", test_id),
                                              "no assigned team",
                                              mydatabase);
            blob = blob + Missing_Field.check("Test_Missing_Environment",
                                              "TEST_CASES",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (Environment)\"",
                                              string.Format("`Issue Key` = '{0}'", test_id),
                                              "no assigned Environment",
                                              mydatabase);
            blob = blob + Incorrect_Fix_Version.check("Test_Incorrect_Fix_Version", "TEST_CASES", string.Format("`Issue Key` = '{0}'", test_id), release, html, mydatabase);

            if (0 != blob.Length)
            {
                if (html) blob = string.Format("<span>Test Case {0} has</span>\n", test_id) + blob + "<br/>\n";
                else blob = string.Format("Test Case {0} has\n", test_id) + blob + "\n\n";
            }
            return blob;

        }
    }
}
