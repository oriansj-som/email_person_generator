using common;
using System;
using System.Collections.Generic;

namespace Email_Person_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting up Email_Person_Generator");
            string email_tool = null;
            string email_sender = null;
            if (Glue_Factory.sanity_check())
            {
                email_tool = Glue_Factory.email_tool();
                email_sender = Glue_Factory.email_sender();
            }
            string database = "example.db";
            string release = null;
            DateTime release_date = DateTime.MinValue;
            bool verbose = false;
            bool fire_email = false;
            string assignee = null;
            string signature = null;
            bool html = false;
            List<string> extra_cc = new List<string>();
            List<string> extra_bcc = new List<string>();

            int i = 0;
            while (i < args.Length)
            {
                if (match("--release", args[i]))
                {
                    // Should look like: "22.03.26"
                    release = args[i + 1];
                    string[] hold = release.Split('.');
                    try
                    {
                        string temp = hold[1] + "/" + hold[2] + "/20" + hold[0];
                        release_date = DateTime.Parse(temp);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Console.WriteLine(string.Format("You passed: {0} which doesn't match the expected YY.MM.DD format", args[i+1]));
                        Environment.Exit(1);
                    }
                    i = i + 2;
                }
                else if (match("--assignee", args[i]))
                {
                    assignee = args[i + 1];
                    i = i + 2;
                }
                else if (match("--sig-file", args[i]))
                {
                    signature = args[i + 1];
                    html = true;
                    i = i + 2;
                }
                else if (match("--database", args[i]))
                {
                    database = args[i + 1];
                    i = i + 2;
                }
                else if (match("--tool", args[i]))
                {
                    email_tool = args[i + 1];
                    i = i + 2;
                }
                else if (match("--from", args[i]))
                {
                    email_sender = args[i + 1];
                    i = i + 2;
                }
                else if (match("--cc", args[i]))
                {
                    extra_cc.Add(args[i + 1]);
                    i = i + 2;
                }
                else if (match("--bcc", args[i]))
                {
                    extra_bcc.Add(args[i + 1]);
                    if (!fire_email) Console.WriteLine("Warning bcc does not survive writing to a file");
                    i = i + 2;
                }
                else if (match("--fire-email", args[i]))
                {
                    fire_email = true;
                    i = i + 1;
                }
                else if (match("--html-email", args[i]))
                {
                    html = true;
                    i = i + 1;
                }
                else if (match("--verbose", args[i]))
                {
                    int index = 0;
                    verbose = true;
                    foreach (string s in args)
                    {
                        Console.WriteLine(string.Format("argument {0}: {1}", index, s));
                        index = index + 1;
                    }
                    i = i + 1;
                }
                else
                {
                    Console.WriteLine(string.Format("Unknown argument: {0} received", args[i]));
                    i = i + 1;
                }
            }

            if(null == release)
            {
                Console.WriteLine("Have have to provide a release date\n");
                Environment.Exit(2);
            }

            if(null == email_tool)
            {
                Console.WriteLine("We need --tool $path to work around C# not loading my config");
                Environment.Exit(3);
            }

            if(null == email_sender)
            {
                Console.WriteLine("We need --from sender@domain to work around C# not loading my config");
                Environment.Exit(4);
            }

            Build_Message.build_message(database, assignee, release_date, release, verbose, html, signature, email_tool, email_sender, fire_email, extra_cc, extra_bcc);
            
        }

        static bool match(string a, string b)
        {
            return a.Equals(b, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
