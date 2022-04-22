using sqlite_Example;
using System;
using System.Data;

namespace Email_Person_Generator
{
    class Features
    {
        static public string generate_features(SQLiteDatabase mydatabase, bool html, string assignee, string release, bool PM, string TEAM)
        {
            string blob = "";
            DataTable ds = null;
            try
            {
                string sql = string.Format("SELECT DISTINCT(`Issue Key`) FROM Features INNER JOIN Agile_Teams ON Features.`Custom field (Team)` = Agile_Teams.Team WHERE Role = 'Scrum Master' AND Username = '{0}'", assignee);
                ds = mydatabase.GetDataTable(sql);
                foreach (DataRow dr in ds.Rows)
                {
                    string story = dr["Issue Key"].ToString();
                    blob = blob + check_features(story, release, html, mydatabase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(4);
            }
            return blob;
        }

        static private string check_features(string feature_id, string release, bool html, SQLiteDatabase mydatabase)
        {
            string blob = "";
            blob = blob + Missing_Field.check("Feature_Missing_Summary",
                                              "FEATURES",
                                              "\"Issue Key\"",
                                              html,
                                              "Summary",
                                              string.Format("`Issue Key` = '{0}'", feature_id),
                                              "a missing summary",
                                              mydatabase);
            blob = blob + Missing_Field.check("Feature_Missing_Status",
                                              "FEATURES",
                                              "\"Issue Key\"",
                                              html,
                                              "Status",
                                              string.Format("`Issue Key` = '{0}'", feature_id),
                                              "a missing status",
                                              mydatabase);
            blob = blob + Missing_Field.check("Feature_Missing_Fix_Version",
                                              "FEATURES",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Fix Version/s\"",
                                              string.Format("`Issue Key` = '{0}'", feature_id),
                                              "no assigned Fix Version",
                                              mydatabase);
            blob = blob + Missing_Field.check("Feature_Missing_Release",
                                              "FEATURES",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (Release)\"",
                                              string.Format("`Issue Key` = '{0}'", feature_id),
                                              "no assigned release",
                                              mydatabase);
            blob = blob + Missing_Field.check("Feature_Missing_Team",
                                              "FEATURES",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (Team)\"",
                                              string.Format("`Issue Key` = '{0}'", feature_id),
                                              "no assigned team",
                                              mydatabase);
            blob = blob + Missing_Field.check("Feature_Missing_ROM",
                                              "FEATURES",
                                              "\"Issue Key\"",
                                              html,
                                              "\"Custom field (ROM)\"",
                                              string.Format("`Issue Key` = '{0}'", feature_id),
                                              "no assigned ROM",
                                              mydatabase);
            blob = blob + Incorrect_Fix_Version.check("Feature_Incorrect_Fix_Version", "FEATURES", string.Format("`Issue Key` = '{0}'", feature_id), release, html, mydatabase);

            if (0 != blob.Length)
            {
                if (html) blob = string.Format("<span>Feature {0} has</span>\n", feature_id) + blob + "<br/>\n";
                else blob = string.Format("Feature {0} has\n", feature_id) + blob + "\n\n";
            }
            return blob;

        }
    }
}
