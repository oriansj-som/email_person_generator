using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace common
{
    class Glue_Factory
    {
        static public bool sanity_check()
        {
            bool r = true;

            try
            {
                ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = "EXECONFIG_PATH" };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                NameValueCollection appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("ReadAppSettings: The AppSettings section is empty.");
                    r = false;
                }

                for (int i = 0; i < appSettings.Count; i++)
                {
                    Console.WriteLine("ReadAppSettings: {0} Key: {1} Value: {2}", i, appSettings.GetKey(i), appSettings[i]);
                }
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine("ReadAppSettings: {0}", e.ToString());
                r = false;
            }

            return r;
        }

        static public string email_tool()
        {
            try
            {
                string hold = ConfigurationManager.AppSettings["Email_Tool"].ToString();
                if (0 == hold.Length)
                {
                    Console.WriteLine("Field Email_Tool not set in configuration");
                    Environment.Exit(11);
                }
                if (!File.Exists(hold))
                {
                    Console.WriteLine("Field Email_Tool not set to an actual file");
                    Environment.Exit(12);
                }
                return hold;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(13);
            }

            // Impossible case
            Console.WriteLine("WTF Glue_Factory:email_tool");
            Environment.Exit(42);
            return "";
        }

        static public string email_sender()
        {
            try
            {
                string hold = ConfigurationManager.AppSettings["Email_Sender"].ToString();
                if (0 == hold.Length)
                {
                    Console.WriteLine("Field Email_Sender not set in configuration");
                    Environment.Exit(11);
                }

                if(!hold.Contains("@michigan.gov"))
                {
                    Console.WriteLine("Field Email_Sender needs to be set to your SOM email address");
                    Environment.Exit(12);
                }

                return hold;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(13);
            }

            // Impossible case
            Console.WriteLine("WTF Glue_Factory:email_sender");
            Environment.Exit(42);
            return "";
        }
    }
}
