using sqlite_Example;
using System;
using System.Data;

namespace Email_Person_Generator
{
    class Sub_tasks
    {
        static public string generate_sub_tasks(SQLiteDatabase mydatabase, bool html, string assignee, string release, bool PM, string TEAM)
        {
            string blob = "";
            DataTable ds = null;
            try
            {
                string sql = string.Format("SELECT DISTINCT(`Issue Key`) FROM SUB_TASKS WHERE ASSIGNEE ='{0}'", assignee);
                ds = mydatabase.GetDataTable(sql);
                foreach (DataRow dr in ds.Rows)
                {
                    string story = dr["Issue Key"].ToString();
                    blob = blob + check_sub_tasks(story, release, html, mydatabase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(4);
            }
            if (PM && Functionality.Enabled("Sub_Task_Missing_Assignee", mydatabase))
            {
                try
                {
                    string sql = string.Format("SELECT DISTINCT(`Issue Key`) FROM SUB_TASKS WHERE ASSIGNEE IS NULL AND `Custom field (Team)` = '{0}'", TEAM);
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

        static private string check_sub_tasks(string sub_task_id, string release, bool html, SQLiteDatabase mydatabase)
        {
            string blob = "";
            blob = blob + Missing_Field.check("Sub_Task_Missing_Summary",
                                              "SUB_TASKS",
                                              "\"Issue Key\"",
                                              html,
                                              "Summary",
                                              string.Format("`Issue Key` = '{0}'", sub_task_id),
                                              "a missing summary",
                                              mydatabase);
            blob = blob + Missing_Field.check("Sub_Task_Missing_Status",
                                              "SUB_TASKS",
                                              "\"Issue Key\"",
                                              html,
                                              "Status",
                                              string.Format("`Issue Key` = '{0}'", sub_task_id),
                                              "a missing status",
                                              mydatabase);
            blob = blob + Missing_Field.check("Sub_Task_Missing_Sprint",
                                              "SUB_TASKS",
                                              "\"Issue Key\"",
                                              html,
                                              "Sprint",
                                              string.Format("`Issue Key` = '{0}'", sub_task_id),
                                              "no assigned sprint",
                                              mydatabase);
            blob = blob + Missing_Field.check("Sub_Task_Missing_Parent",
                                              "SUB_TASKS",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Parent id\"",
                                              string.Format("`Issue Key` = '{0}'", sub_task_id),
                                              "no assigned parent Issue",
                                              mydatabase);
            blob = blob + Missing_Field.check("Sub_Task_Missing_Fix_Version",
                                              "SUB_TASKS",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Fix Version/s\"",
                                              string.Format("`Issue Key` = '{0}'", sub_task_id),
                                              "no assigned Fix Version",
                                              mydatabase);
            blob = blob + Missing_Field.check("Sub_Task_Missing_Source",
                                              "SUB_TASKS",
                                              "\"Issue Key\"",
                                              html,
                                              "Custom field (Source)",
                                              string.Format("`Issue Key` = '{0}'", sub_task_id),
                                              "no assigned Source",
                                              mydatabase);
            blob = blob + Missing_Field.check("Sub_Task_Missing_Assignee",
                                              "SUB_TASKS",
                                              "\"Issue Key\"",
                                              html,
                                              "Assignee",
                                              string.Format("`Issue Key` = '{0}'", sub_task_id),
                                              "no assigned Assignee",
                                              mydatabase);
            blob = blob + Missing_Field.check("Sub_Task_Missing_Team",
                                              "SUB_TASKS",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (Team)\"",
                                              string.Format("`Issue Key` = '{0}'", sub_task_id),
                                              "no assigned team",
                                              mydatabase);
            blob = blob + Missing_Field.check("Sub_Task_Missing_Activity_Type",
                                              "SUB_TASKS",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (Activity Type)\"",
                                              string.Format("`Issue Key` = '{0}'", sub_task_id),
                                              "no assigned Activity Type",
                                              mydatabase);
            blob = blob + Missing_Field.check("Sub_Task_Missing_Resolution",
                                              "SUB_TASKS",
                                              "\"Issue Key\"",
                                              html,
                                              "Resolution",
                                              string.Format("`Issue Key` = '{0}'", sub_task_id),
                                              "no assigned Resolution",
                                              mydatabase);
            blob = blob + Incorrect_Fix_Version.check("Sub_Task_Incorrect_Fix_Version", "SUB_TASKS", string.Format("`Issue Key` = '{0}'", sub_task_id), release, html, mydatabase);

            if (0 != blob.Length)
            {
                if (html) blob = string.Format("<span>Sub Task {0} has</span>\n", sub_task_id) + blob + "<br/>\n";
                else blob = string.Format("Sub Task {0} has\n", sub_task_id) + blob + "\n\n";
            }
            return blob;

        }
    }
}
