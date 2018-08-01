using System;
using System.Data;// ADO.Net lib
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace PhoneLibrary
{  /* This part of the class contains all the methods to access/manipulate the data outside of the collection*/
    public partial class PersonDirectory
    {
       //Start The Phone App
       public void Start()
        {
            Console.SetWindowSize(145,38);
            #region setup
            string input = "0";
            SQLStart();
            //ReadToCollection();
            string message = "******* Welcome to the Phone Directory App *******";
            string padding = "**********************************************************";
            Console.WriteLine(message.PadLeft((Console.WindowWidth+message.Length)/2));
            Console.WriteLine(padding.PadLeft((Console.WindowWidth + padding.Length) / 2));
            Console.WriteLine(padding.PadLeft((Console.WindowWidth + padding.Length) / 2));
            System.Threading.Thread.Sleep(900);
            #endregion
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try { 
                while (input != "end")
                {
                    Console.WriteLine();
                    message="*****Enter one of the options below *****";
                    Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
                    message="1.Read     2.Add    3.Delete ";
                    Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
                    message="4.Update   5.Search    6.END  ";
                    Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
                    Console.Write("::");

                    #region UserInputOptions
                    input = Console.ReadLine().ToString().ToLower();
                    switch (input)
                    {
                        case "1":
                        case "read" :
                            this.Read();
                            break;

                        case "2":
                        case "add":
                            this.Add();
                            break;

                        case "3":
                        case "delete":
                            Console.WriteLine();
                            this.Delete();
                            break;

                        case "4":
                        case "update":
                            Console.WriteLine();
                            this.Update();
                            break;

                        case "5":
                        case "search":
                            Console.WriteLine();
                            this.Search();
                            break;

                        case "6":
                        case "end":
                            #region End 
                                input = "end";
                                Console.Clear();
                                Console.WriteLine();
                                message = "*** Have A Nice Day! :} ***";
                                Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
                                System.Threading.Thread.Sleep(1300);
                                break;
                           #endregion

                        default:
                            #region Retry
                                Console.Clear();
                                Console.WriteLine();
                                message="*** Please Try Again ***";
                                Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
                                System.Threading.Thread.Sleep(1000);
                                break;
                            #endregion

                    }//end switch
                    #endregion

                }//end while
            }
            catch (Exception e) { logger.Error(e.Message); }
        }//end start

        //Serialize user object list send it to a JSON stream and then into a file.
        public void WriteFromObject()
        {
            string json = "";
            foreach (Person p in Library)
            {
                json += JsonConvert.SerializeObject(p, Formatting.Indented) + Environment.NewLine;

            }
            File.WriteAllText(@"D:\squir\Revature_repo\PhoneDirectoryApp\PhoneDirectoryApp\People.json", json);
        }

        // Deserialize a JSON stream to a User object.  
        public void ReadToCollection()
        {
            Person p = new Person();
            string filepath = @"D:\squir\Revature_repo\PhoneDirectoryApp\PhoneDirectoryApp\People.json";
            var serializer = new JsonSerializer();
            using (var streamReader = new StreamReader(File.Open(filepath, FileMode.Open), new UTF8Encoding()))
            {
                using (var reader = new JsonTextReader(streamReader))
                {
                    reader.CloseInput = false;
                    reader.SupportMultipleContent = true;
                    while (reader.Read()){
                        count++;
                        Library.Add(serializer.Deserialize<Person>(reader));
                    }

                }//end using reader
            }//end using streamreader

        }

        //Start the app by reading in data from SQL Server DB
        public void SQLStart()
        {
            SqlConnection con = null;
            string conStr = "Data Source=rev-cuny-joe-server.database.windows.net;Initial Catalog=PhoneDirectory;Persist Security Info=True;User ID=jrusso;Password=Nazarick1993";
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                con = new SqlConnection(conStr);
                con.Open();
                string getAllPeople = "SELECT * FROM Person";
                SqlCommand myCommand = new SqlCommand(getAllPeople, con);
                var allPeople = myCommand.ExecuteReader();

                #region GetNamesfromDB
                using (allPeople)
                {
                    while (allPeople.Read())
                    {
                        Person p1 = new Person();
                        p1.Pid = Convert.ToInt64( allPeople.GetValue(0));
                        p1.FirstName = allPeople.GetString(1);
                        p1.LastName = allPeople.GetString(2);;
                        p1.address.Aid = Convert.ToInt64(allPeople.GetValue(3));
                        p1.phone.PID = Convert.ToInt64(allPeople.GetValue(4));
                        count++;
                        Library.Add(p1);
                    }
                }
                #endregion

                foreach (Person p2 in Library)
                {
                    SqlgetAddressandPhone(p2,con);
                }
                    
            }

            catch (SqlException ex) { logger.Error(ex.Message); }
            catch (Exception e) { logger.Error(e.Message); }
            finally { con.Close(); }

        }//end sql

        //Get The Address and Phone part of a Person from the DB
        private void SqlgetAddressandPhone(Person p1, SqlConnection con)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                //Address_ID, HouseNum, Street, City, A_State, Country, ZipCode
                string address = "SELECT * FROM P_Address WHERE Address_ID = @a";

                //Phone_ID,CountryCode, AreaCode, Number, Ext
                string phone = "SELECT * FROM Phone WHERE Phone_ID = @p";

                SqlCommand getAddress = new SqlCommand(address, con);
                SqlCommand getPhone = new SqlCommand(phone, con);

                getAddress.Parameters.AddWithValue("@a", p1.address.Aid);
                getPhone.Parameters.AddWithValue("@p", p1.phone.PID);

                #region getAddressfromDB
                var addressReader = getAddress.ExecuteReader();
                using (addressReader)
                {
                    while (addressReader.Read())
                    {
                        try
                        {
                            p1.address.houseNum = addressReader.GetString(1);
                            p1.address.street = addressReader.GetString(2);
                            p1.address.city = addressReader.GetString(3);
                            p1.address.State = (State)Enum.Parse(typeof(State),addressReader.GetString(4));
                            p1.address.Country = (Country)Enum.Parse(typeof(Country), addressReader.GetString(5));
                            p1.address.zipcode = addressReader.GetString(6);
                        }
                        catch (Exception e){ logger.Error($"Could not read address from DB. Error: {e.Message}");}
                    }// end while
                }
                #endregion
                #region getPhonefromDB
                var phoneReader = getPhone.ExecuteReader();

                using (phoneReader)
                {
                    while (phoneReader.Read())
                    {
                        try
                        {

                            p1.phone.countryCode = phoneReader.GetString(1);
                            p1.phone.areaCode = phoneReader.GetString(2);
                            p1.phone.number = phoneReader.GetString(3);
                            p1.phone.ext = phoneReader.GetString(4);


                        }
                        catch (Exception e) { logger.Error($"Could not read address from DB. Error: {e.Message}"); }
                    }
                }
                #endregion
            }
            catch (SqlException e) { logger.Error(e); }
            catch (Exception e) { logger.Error(e.Message);  }
        }//end rest

        //serialize to  db and json
        void AppendOutside(Person p)
        {
            #region Insert to DataBase
            SqlConnection con = null;
            SqlCommand myCommand = null;
            string conStr = "Data Source=rev-cuny-joe-server.database.windows.net;Initial Catalog=PhoneDirectory;Persist Security Info=True;User ID=jrusso;Password=Nazarick1993";
            //1. SQL Connection
            try
            {
                con = new SqlConnection(conStr);
                con.Open();
                string insertaddress = "INSERT INTO P_Address values(@id, @housenum, @street,  @city, @state, @country, @zip)";
                //string insertperson = "INSERT INTO Person values(@id, @firstname, @lastname, @phone, @address)";

                //myCommand = new SqlCommand($"INSERT INTO  P_Address(Address_ID, HouseNum, Street, City, A_State, Country, ZipCode) " +
                //                           $"Values ({p.address.Aid},'{p.address.houseNum}','{p.address.street}','{p.address.city}'," +
                //                           $"'{p.address.State.ToString()}','{p.address.Country.ToString()}',{p.address.zipcode})", con);
                myCommand = new SqlCommand(insertaddress,con);
                myCommand.Parameters.AddWithValue("@id",       p.address.Aid);
                myCommand.Parameters.AddWithValue("@street",   p.address.street);
                myCommand.Parameters.AddWithValue("@housenum", p.address.houseNum);
                myCommand.Parameters.AddWithValue("@city",     p.address.city);
                myCommand.Parameters.AddWithValue("@zip",      p.address.zipcode);
                myCommand.Parameters.AddWithValue("@state",    p.address.State.ToString());
                myCommand.Parameters.AddWithValue("@country",  p.address.Country.ToString());
                myCommand.ExecuteNonQuery();

                myCommand = new SqlCommand($"INSERT INTO  Phone(Phone_ID,CountryCode, AreaCode, Number, Ext) " +
                                           $"Values ({p.phone.PID},{p.phone.countryCode},'{p.phone.areaCode}','{p.phone.number}','{p.phone.ext}')", con);
                myCommand.ExecuteNonQuery();

                myCommand = new SqlCommand($"INSERT INTO  Person(Person_ID, FirstName, LastName, Addr_ID, Ph_ID) " +
                                           $"Values ({p.Pid},'{p.FirstName}','{p.LastName}',{p.address.Aid},{p.phone.PID})", con);
                myCommand.ExecuteNonQuery();
            }
            catch (SqlException ex) { Console.WriteLine(ex.Message); }
            catch (Exception ex) {Console.WriteLine(ex.Message); }
            finally { con.Close(); }
            #endregion
            #region Insert to Json
            string json = JsonConvert.SerializeObject(p, Formatting.Indented) + Environment.NewLine;
            File.AppendAllText(@"D:\squir\Revature_repo\PhoneDirectoryApp\PhoneDirectoryApp\People.json", json);
            #endregion
        }

        //Update to db and json
        void UpdateOutside(Person p, string type)
        {
            #region Update to DataBase
            SqlConnection con = null;
            SqlCommand myCommand = null;
            string conStr = "Data Source=rev-cuny-joe-server.database.windows.net;Initial Catalog=PhoneDirectory;Persist Security Info=True;User ID=jrusso;Password=Nazarick1993";
            string person=  "UPDATE Person SET FirstName=@FN , LastName=@LN WHERE Person_ID=@id";
            string address= "UPDATE P_Address SET HouseNum=@house , Street=@st , City=@city , A_State=@state , Country=@country , ZipCode=@zip WHERE Address_ID=@id";
            string phone=   "UPDATE Phone SET CountryCode=@cc , AreaCode=@ac , Number=@num , Ext=@ex WHERE Phone_ID=@id";

            //1. SQL Connection
            try
            {   con = new SqlConnection(conStr);
                con.Open();
                switch (type)
                {
                    case "name":
                        myCommand = new SqlCommand(person , con);
                        myCommand.Parameters.AddWithValue("@FN",p.FirstName);
                        myCommand.Parameters.AddWithValue("@LN",p.LastName);
                        myCommand.Parameters.AddWithValue("@id",p.Pid);
                        myCommand.ExecuteNonQuery();
                        break;

                    
                    case "address":
                        myCommand = new SqlCommand(address, con);
                        myCommand.Parameters.AddWithValue("@house", p.address.houseNum);
                        myCommand.Parameters.AddWithValue("@st", p.address.street);
                        myCommand.Parameters.AddWithValue("@city", p.address.city);
                        myCommand.Parameters.AddWithValue("@state", p.address.State.ToString());
                        myCommand.Parameters.AddWithValue("@country", p.address.Country.ToString());
                        myCommand.Parameters.AddWithValue("@zip", p.address.zipcode);
                        myCommand.Parameters.AddWithValue("@id", p.address.Aid);
                        myCommand.ExecuteNonQuery(); 
                        break;

                    
                    case "phone":
                        myCommand = new SqlCommand(phone, con);
                        myCommand.Parameters.AddWithValue("@cc", p.phone.countryCode);
                        myCommand.Parameters.AddWithValue("@ac", p.phone.areaCode);
                        myCommand.Parameters.AddWithValue("@num", p.phone.number);
                        myCommand.Parameters.AddWithValue("@ex", p.phone.ext);
                        myCommand.Parameters.AddWithValue("@id", p.phone.PID);
                        myCommand.ExecuteNonQuery();
                        break;

                    default:
                        break;
                }//end switch 
                
            }//end tyr
            catch (SqlException ex) { Console.WriteLine(ex.Message); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { con.Close(); }
            #endregion
            #region Update to Json
            WriteFromObject();
            #endregion
        }

        //delete from db and json
        void DeleteOutside(Person p)
        {
            #region Delete From DataBase
            SqlConnection con = null;
            SqlCommand myCommand = null;
            string conStr = "Data Source=rev-cuny-joe-server.database.windows.net;Initial Catalog=PhoneDirectory;Persist Security Info=True;User ID=jrusso;Password=Nazarick1993";
            //1. SQL Connection
            try
            {
                con = new SqlConnection(conStr);
                con.Open();

                myCommand = new SqlCommand($"DELETE FROM  Person WHERE (Person_ID={p.Pid} )", con);
                myCommand.ExecuteNonQuery();

                myCommand = new SqlCommand($"DELETE FROM  P_Address WHERE (Address_ID={p.address.Aid} )", con);
                myCommand.ExecuteNonQuery();

                myCommand = new SqlCommand($"DELETE FROM  Phone WHERE (Phone_ID={p.phone.PID} )", con);
                myCommand.ExecuteNonQuery();

            }
            catch (SqlException ex) { Console.WriteLine(ex.Message); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { con.Close(); }
            #endregion
            #region Delete from Json
            WriteFromObject();
            #endregion
        }

        //Used to insert the whole collection into the database -- one time use 
        void SqlInsertLibrary(SqlConnection con, string table)
        {

            foreach (Person per in Library)
            {
                SqlCommand myCommand = new SqlCommand($"INSERT INTO  P_Address(Address_ID, HouseNum, Street, City, A_State, Country, ZipCode) " +
                                                      $"Values ({per.address.Aid},'{per.address.houseNum}','{per.address.street}','{per.address.city}'," +
                                                      $"'{per.address.State.ToString()}','{per.address.Country.ToString()}',{per.address.zipcode})", con);
                myCommand.ExecuteNonQuery();

                myCommand = new SqlCommand($"INSERT INTO  Phone(Phone_ID,CountryCode, AreaCode, Number, Ext) " +
                                           $"Values ({per.phone.PID},{per.phone.countryCode},'{per.phone.areaCode}','{per.phone.number}','{per.phone.ext}')", con);
                myCommand.ExecuteNonQuery();

                myCommand = new SqlCommand($"INSERT INTO  Person(Person_ID, FirstName, LastName, Addr_ID, Ph_ID) " +
                                           $"Values ({per.Pid},'{per.FirstName}','{per.LastName}',{per.address.Aid},{per.phone.PID})", con);
                myCommand.ExecuteNonQuery();
            }

        }


    }//end class
}
