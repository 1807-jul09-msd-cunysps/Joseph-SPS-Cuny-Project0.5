using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneLibrary
{
    public class EmailOps
    {
        //************** Email Operations*******************

        //Output all the current contacts in the BD
        public List<Email> Read()
        {
            List<Email> result = new List<Email>();
            SqlConnection con = null;
            SqlDataReader dr;
            string conStr = "Data Source=rev-cuny-joe-server.database.windows.net;Initial Catalog=PhoneDirectory;Persist Security Info=True;User ID=jrusso;Password=Nazarick1993";
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                con = new SqlConnection(conStr);
                con.Open();
                string getAllEmails = "SELECT * FROM Emails ";
                SqlCommand myCommand = new SqlCommand(getAllEmails, con);
                dr = myCommand.ExecuteReader();
                while (dr.Read())
                {
                    Email e = new Email();
                    e.FirstName = dr.GetValue(1).ToString();
                    e.LastName = dr.GetValue(2).ToString();
                    e.email = dr.GetValue(3).ToString();
                    e.Message = dr.GetValue(4).ToString();
                    result.Add(e);
                    e = null;
                }

            }
            catch (SqlException ex) { logger.Error(ex.Message + "" + ex.Procedure); }
            catch (Exception e) { logger.Error(e.Message + " " + e.StackTrace); }
            finally { con.Close(); }

            return result;
        }//end read
        
        //calls the methods to add email messages
        public void Add(Email e)
        {
            try
            {
                SqlConnection con = null;
                SqlDataReader dr;
                string conStr = "Data Source=rev-cuny-joe-server.database.windows.net;Initial Catalog=PhoneDirectory;Persist Security Info=True;User ID=jrusso;Password=Nazarick1993";
                var logger = NLog.LogManager.GetCurrentClassLogger();
                try
                {
                    con = new SqlConnection(conStr);
                    con.Open();
                    string getAllEmails = "INSERT INTO Emails values(@FN, @LN, @E, @M)"; ;
                    SqlCommand myCommand = new SqlCommand(getAllEmails, con);

                    myCommand.Parameters.AddWithValue("@FN", e.FirstName);
                    myCommand.Parameters.AddWithValue("@LN", e.LastName);
                    myCommand.Parameters.AddWithValue("@E",e.email);
                    myCommand.Parameters.AddWithValue("@M", e.Message);
                    myCommand.ExecuteNonQuery();

                }
                catch (SqlException ex) { logger.Error(ex.Message + "" + ex.Procedure); }
                catch (Exception ex) { logger.Error(ex.Message + " " + ex.StackTrace); }
                finally { con.Close(); }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

    }
}
