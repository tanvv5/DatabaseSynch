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
            string clientConnectionString = ConfigurationManager.AppSettings["Server2Connection"].ToString();
            string[] tableName= {"Products","UserInfo" }            ;
            foreach(string tbname in tableName)
            {
                DataSynchronizer.Synchronize(tbname, serverConnectionString,
                clientConnectionString);
            }            
            Console.WriteLine("Databases synchronized...");
            Console.Read();
        }
    }
}
