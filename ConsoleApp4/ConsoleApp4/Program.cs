using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Data.SqlClient;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {            
            string domain = System.Environment.UserDomainName;
            Console.WriteLine(domain);

            //this need to be wrapped
            string password = "bushuev";
            SecureString secureString = new SecureString();
            Array.ForEach(password.ToArray(), (z) => secureString.AppendChar(z));
            //end 

            secureString.MakeReadOnly();
            try
            {
                //use sql crederntial and password
                SqlCredential sql = new SqlCredential(Environment.UserName, secureString);
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.Add("Data Source", domain);
                builder.Add("Initial Catalog", "Example");
                SqlCredential sqlCredential = new SqlCredential("roman", secureString);
                using (SqlConnection sqlConnectio = new SqlConnection(builder.ConnectionString, sqlCredential))
                {
                    sqlConnectio.Open();
                    SqlCommand sqlCommand = new SqlCommand("select SYSTEM_USER", sqlConnectio);
                    string result = (string)sqlCommand.ExecuteScalar();
                    Console.WriteLine(result);
                    sqlConnectio.Close();
                }

                //use windows authentication 
                builder.Clear();
                builder.Add("Data Source", domain);
                builder.Add("Initial Catalog", "Example");
                builder.Add("Integrated Security", "SSPI");
                using (SqlConnection sqlConnectionSecond = new SqlConnection(builder.ConnectionString))
                {
                    sqlConnectionSecond.Open();
                    SqlCommand sqlCommand = new SqlCommand("select SYSTEM_USER", sqlConnectionSecond);
                    string result = (string)sqlCommand.ExecuteScalar();
                    Console.WriteLine(result);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                secureString.Dispose();
            }
        }
    }
}
