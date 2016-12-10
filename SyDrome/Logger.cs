using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyDrome
{
    class Logger
    {
     public static void WriteLog(String messege,String power)
     {
            try
            {
                Stream stream;
                using (stream = File.Open("Log.log", FileMode.Append, FileAccess.Write))
                {
                    StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine("[" + DateTime.Now + "]" + "[" + power + "]" + messege);
                    writer.Close();
                }
                Console.WriteLine("[" + DateTime.Now + "]" + "[" + power + "]" + messege);
            }catch(Exception e)
            {
                File.WriteAllText("Magic.log", "[" + DateTime.Now + "]" + "[Error]" + e.Message);
               // File.Delete("Log.log");
            }
     }
    }
}
