using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using NokelServices.DALGenLib.Configuration;
using NokelServices.DALGenLib;

namespace ConsoleSettings
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Simple test of constructing the objects...");
            DALSettingsSection dss = null;
            
            var config = ConfigurationManager.OpenExeConfiguration(null);
            dss = (DALSettingsSection)config.Sections["dalSettings"];

            Console.WriteLine("Looking for templates in this folder: " + dss.BaseTemplateFolder);

            DALSettings settings = new DALSettings(dss);
            ProcessDAL pd = new ProcessDAL(settings);

            if (pd.GenerateAllFiles())
            {
                Console.WriteLine("Files generated successfully.");
                Console.WriteLine("Please check the " + dss.BaseOutputFolder + " folder for results.");
            }
            else
            {
                Console.WriteLine("There was some sort of processing error.");
                Console.WriteLine("Check the log files.");
            }

            Console.Write("End of test.  Press Enter...");
            Console.ReadLine();

        }
    }
}
