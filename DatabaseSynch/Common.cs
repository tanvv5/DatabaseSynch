using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSynch
{
    class Common
    {
        public static bool WriteLog(string strMessage)
        {
            try
            {
                string strFileName = "logs\\DatabaseSynch_Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!System.IO.File.Exists(strFileName))
                {
                    System.IO.File.Create(strFileName);
                }
                var Rootpath = System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

                FileStream objFilestream = new FileStream(strFileName, FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
