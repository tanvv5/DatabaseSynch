using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DatabaseSynch
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverConnectionString = ConfigurationManager.AppSettings["Server1Connection"].ToString();
            string clientConnectionString = ConfigurationManager.AppSettings["ClientConnection"].ToString();
            string tablenames = ConfigurationManager.AppSettings["tablename"].ToString();
            string[] tableName= tablenames.Split(',');
            foreach(string tbname in tableName)
            {
                DataSynchronizer.Synchronize(tbname, serverConnectionString,
                clientConnectionString);
            }
            Common.WriteLog(System.DateTime.Now.ToString()+": Databases synchronized Success..");
            Console.WriteLine("Databases synchronized...");
            //Console.Read();
        }
    }
}
