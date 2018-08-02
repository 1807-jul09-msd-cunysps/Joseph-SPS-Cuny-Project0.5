using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace PhoneLibrary
{
    /* This part of the class contains all the methods to access/manipulate the data in the collections */
    public partial class Operations
    {
        int count = 100;
        public   List<Person> Library;

        public Operations()
        {
            Library = new List<Person>();
        }
        
        //Output all the current contacts in the BD
        public List<Person> Read()
        {
            List<Person> result = new List<Person>();
            SqlConnection con = null;
            SqlDataReader dr;
            string conStr = "Data Source=rev-cuny-joe-server.database.windows.net;Initial Catalog=PhoneDirectory;Persist Security Info=True;User ID=jrusso;Password=Nazarick1993";
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                con = new SqlConnection(conStr);
                con.Open();
                string getAllPeople = "SELECT * FROM Person " +
                                      "INNER JOIN P_Address on Addr_ID = Address_ID " +
                                      "INNER JOIN Phone on Ph_ID = Phone_ID";
                SqlCommand myCommand = new SqlCommand(getAllPeople, con);
                dr = myCommand.ExecuteReader();
                while (dr.Read())
                {
                  
                    Person p = new Person();
                    p.Pid = Convert.ToInt64(dr["Person_ID"]);
                    p.FirstName = dr["FirstName"].ToString();
                    p.LastName = dr["LastName"].ToString();
                    p.address.Aid = Convert.ToInt64(dr.GetValue(3));
                    p.phone.PID = Convert.ToInt64(dr.GetValue(4));
                    
                    p.address.houseNum = dr.GetString(6);
                    p.address.street = dr.GetString(7);
                    p.address.city = dr.GetString(8);
                    p.address.State = (State)Enum.Parse(typeof(State), dr.GetString(9));
                    p.address.Country = (Country)Enum.Parse(typeof(Country), dr.GetString(10));
                    p.address.zipcode = dr.GetString(11);

                    p.phone.countryCode = dr.GetString(13);
                    p.phone.areaCode = dr.GetString(14);
                    p.phone.number = dr.GetString(14);
                    p.phone.ext = dr.GetString(16);

                    result.Add(p);
                    p = null;
                }

            }
            catch (SqlException ex) { logger.Error(ex.Message + ""+ ex.Procedure); }
            catch (Exception e) { logger.Error(e.Message +" "+ e.StackTrace); }
            finally { con.Close(); }



            return result;


        }//end read

        //calls the methods to add inside and outside  the collection
        public void Add(Person p)
        {
            Random random = new Random();
            count++;
            p.Pid = count + random.Next(1, 100);
            p.address.Aid = count + 1;
            p.phone.PID = count + 2;
            Library.Add(p);
            AppendOutside(p);
        }

        //calls the methods to delete inside and outsie the collection
        public void Delete(Person p)
        {
            Library.Remove(p);
            DeleteOutside(p);
        }

        //call the methods to Update Records of people in  and out of the collection
        public void Update(long updatefor, string updateby, Person update)
        {
            Person p = new Person();
            var query = from p1 in Library
                        where p1.Pid == updatefor
                        select p1;

            if (query.Count() < 1)
            {
                throw new Exception("\n          No such ID found try again later.:}         \n ");

            }
            foreach (Person x in query)
            {
                p = x;
            }
            switch (updateby)
            {
                case "1":
                case "name":
                    p.FirstName = update.FirstName;
                    p.LastName = update.LastName;
                    UpdateOutside(p, "name");
                    break;

                case "2":
                case "address":
                    updateAddress(p,update);
                    UpdateOutside(p, "address");
                    break;

                case "3":
                case "phone":
                    updatePhone(p, update);
                    UpdateOutside(p, "phone");
                    break;

                default:
                    break;
            }//end switch 

        }

        //part of update , just for a persons address
        void updateAddress(Person p, Person u)
        {
            #region GetAddress
            p.address.houseNum = u.address.houseNum;
            p.address.street = u.address.street;
            p.address.city = u.address.city;
            p.address.State = u.address.State;
            p.address.Country= u.address.Country;
            p.phone.countryCode = u.phone.countryCode;
            p.address.zipcode = u.address.zipcode;
            #endregion
        }

        //part of update , just for a persons phone
        void updatePhone(Person p, Person u)
        {
            #region GetPhone#
            p.phone.areaCode = u.phone.areaCode; 
            p.phone.number = u.phone.number;
            p.phone.ext = u.phone.ext;
            #endregion

        }

        //Search for  Contact Record
        public IEnumerable<Person> Search(string type, string search)
        {
            switch (type.ToLower())
            {
                #region Search_FirstName
                case "1":
                case "firstname":
                    try
                    {
                        var result = Library.Where(p => p.FirstName.ToLower().Contains(search.ToLower()));
                        return result;
                    }
                    catch (Exception e) { Console.WriteLine("{0} Exception caught.", e); }
                    break;
                #endregion
                #region Search_LastName
                case "2":
                case "lastname":
                    try
                    {
                        var result = Library.Where(p => p.LastName.ToLower().Contains(search.ToLower()));
                        return result;
                        
                    }
                    catch (Exception e) { Console.WriteLine("{0} Exception caught.", e); }
                    break;
                #endregion
                #region Search_Zipcode
                case "3":
                case "zipcode":
                    try
                    {
                        var result = Library.Where(p => p.address.zipcode.Contains(search));
                        return result;
                    }
                    catch (Exception e) { Console.WriteLine("{0} Exception caught.", e); }
                    break;
                #endregion
                #region Search_City
                case "4":
                case "city":
                    try
                    {
                        var result = Library.Where(p => p.address.city.ToLower().Contains(search.ToLower()));
                        return result;
                    }
                    catch (Exception e) { Console.WriteLine("{0} Exception caught.", e); }
                    break;
                #endregion
                #region Search_Phone
                case "5":
                case "phone":
                    try
                    {
                        var result = Library.Where(p => p.phone.String().Contains(search));
                        return result;
                    }
                    catch (Exception e) { Console.WriteLine("{0} Exception caught.", e); }
                    break;
                #endregion
                default:
                    throw new Exception("*******You did not enter one of the choices so goodbye XD*******");
            }
            return null;
        }

        /*    DATA SERIALIZATION SECTION      */
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
                    while (reader.Read())
                    {
                        count++;
                        Library.Add(serializer.Deserialize<Person>(reader));
                    }

                }//end using reader
            }//end using streamreader

        }

        /*    SQL SECTION      */

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
                        p1.Pid = Convert.ToInt64(allPeople.GetValue(0));
                        p1.FirstName = allPeople.GetString(1);
                        p1.LastName = allPeople.GetString(2); ;
                        p1.address.Aid = Convert.ToInt64(allPeople.GetValue(3));
                        p1.phone.PID = Convert.ToInt64(allPeople.GetValue(4));
                        count=(int)p1.Pid;
                        Library.Add(p1);
                    }
                }
                #endregion

                foreach (Person p2 in Library)
                {
                    SqlgetAddressandPhone(p2, con);
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
                            p1.address.State = (State)Enum.Parse(typeof(State), addressReader.GetString(4));
                            p1.address.Country = (Country)Enum.Parse(typeof(Country), addressReader.GetString(5));
                            p1.address.zipcode = addressReader.GetString(6);
                        }
                        catch (Exception e) { logger.Error($"Could not read address from DB. Error: {e.Message}"); }
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
            catch (Exception e) { logger.Error(e.Message); }
        }//end rest

        //serialize to  db and json   <------add nlog error handleing
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
                string insertphone = "INSERT INTO Person values(@id, @firstname, @lastname, @phone, @address)";
                string insertperson = "INSERT INTO Person values(@id, @firstname, @lastname, @phone, @address)";

               
                myCommand = new SqlCommand(insertaddress, con);
                myCommand.Parameters.AddWithValue("@id", p.address.Aid);
                myCommand.Parameters.AddWithValue("@street", p.address.street);
                myCommand.Parameters.AddWithValue("@housenum", p.address.houseNum);
                myCommand.Parameters.AddWithValue("@city", p.address.city);
                myCommand.Parameters.AddWithValue("@zip", p.address.zipcode);
                myCommand.Parameters.AddWithValue("@state", p.address.State.ToString());
                myCommand.Parameters.AddWithValue("@country", p.address.Country.ToString());
                myCommand.ExecuteNonQuery();



                myCommand = new SqlCommand($"INSERT INTO  Phone(Phone_ID,CountryCode, AreaCode, Number, Ext) " +
                                           $"Values ({p.phone.PID},{p.phone.countryCode},'{p.phone.areaCode}','{p.phone.number}','{p.phone.ext}')", con);
                myCommand.ExecuteNonQuery();

                myCommand = new SqlCommand($"INSERT INTO  Person(Person_ID, FirstName, LastName, Addr_ID, Ph_ID) " +
                                           $"Values ({p.Pid},'{p.FirstName}','{p.LastName}',{p.address.Aid},{p.phone.PID})", con);
                myCommand.ExecuteNonQuery();
            }
            catch (SqlException ex) { Console.WriteLine(ex.Message); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { con.Close(); }
            #endregion
            #region Insert to Json
            string json = JsonConvert.SerializeObject(p, Formatting.Indented) + Environment.NewLine;
            File.AppendAllText(@"D:\squir\Revature_repo\PhoneDirectoryApp\PhoneDirectoryApp\People.json", json);
            #endregion
        }

        //UIUpdate to db and json
        void UpdateOutside(Person p, string type)
        {
            #region Update to DataBase
            SqlConnection con = null;
            SqlCommand myCommand = null;
            string conStr = "Data Source=rev-cuny-joe-server.database.windows.net;Initial Catalog=PhoneDirectory;Persist Security Info=True;User ID=jrusso;Password=Nazarick1993";
            string person = "UPDATE Person SET FirstName=@FN , LastName=@LN WHERE Person_ID=@id";
            string address = "UPDATE P_Address SET HouseNum=@house , Street=@st , City=@city , A_State=@state , Country=@country , ZipCode=@zip WHERE Address_ID=@id";
            string phone = "UPDATE Phone SET CountryCode=@cc , AreaCode=@ac , Number=@num , Ext=@ex WHERE Phone_ID=@id";

            //1. SQL Connection
            try
            {
                con = new SqlConnection(conStr);
                con.Open();
                switch (type)
                {
                    case "name":
                        myCommand = new SqlCommand(person, con);
                        myCommand.Parameters.AddWithValue("@FN", p.FirstName);
                        myCommand.Parameters.AddWithValue("@LN", p.LastName);
                        myCommand.Parameters.AddWithValue("@id", p.Pid);
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

            }//end try
            catch (SqlException ex) { Console.WriteLine(ex.Message); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { con.Close(); }
            #endregion

           //Update to Json
            WriteFromObject();
            
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

            //Delete from Json
            WriteFromObject();
            
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
