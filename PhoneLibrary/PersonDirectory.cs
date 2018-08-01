using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace PhoneLibrary
{
    /* This part of the class contains all the methods to access/manipulate the data in the collections */
    public partial class PersonDirectory
    {
        int count = 100;
        public  static List<Person> Library;

        public PersonDirectory()
        {
            Library = new List<Person>();
        }

        //Output all the current contacts in the library
        public void Read()
        {
            #region ReadingAllContacts
            Console.Clear();
            Console.WriteLine();
            string message = "*****Showing all " + Library.Count() + " People in Directory*****";
            Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
            foreach (Person p in Library)
            {
                Print(p);
            }
            #endregion
            Console.WriteLine("___________________________________________________________________________________________________________________________________");
        }

        //Print Entire Person Object
        public void Print(Person p)
        {
            Console.WriteLine("___________________________________________________________________________________________________________________________________");
            Console.WriteLine($"| PersonID:{p.Pid} | Name: {p.FirstName} , {p.LastName} | Address: {p.address.houseNum} {p.address.street} st " +
            $" {p.address.city} , {p.address.State} , {p.address.zipcode} , {p.address.Country} |" +
            $" PhoneNumber: {p.phone.countryCode}-{p.phone.areaCode}-{p.phone.number} | Ext: {p.phone.ext} ");
        }

        //Add People to the library
        public void Add()
        {
            string y = "yes";
            while (y == "yes")
            {
                Console.WriteLine();
                Person p = new Person();
                Random random = new Random();
                count++;
                p.Pid = count + random.Next(1, 500);
                p.address.Aid = count+1;
                p.phone.PID = count+2;
                Console.WriteLine(" Enter a new Contact::");
                Console.WriteLine(" Type the information then click enter!");

                #region PersonInfoaddition
                GetName(p);
                GetAddress(p);
                GetPhone(p);
                Library.Add(p);
                #endregion
                
                AppendOutside(p);
                Console.WriteLine("Add another person?");
                Console.Write("::");
                y = Console.ReadLine().ToLower();
            }//end while

        }

        //part of add , just for a persons name
        void GetName(Person p)
        {
            #region GetName
            string N = null;
            string L = null;
            Console.WriteLine("\n***Name***");
            while (N == null)
            {
                Console.Write("FirstName:");
                p.FirstName = Console.ReadLine();
                N = p.FirstName;
            }
            while (L == null)
            {
                Console.Write("LastName:");
                p.LastName = Console.ReadLine();
                L = p.LastName;
            }
            #endregion
                
        }

        //part of add , just for a persons address
        void GetAddress(Person p)
        {
            #region GetAddress
            Console.WriteLine("\n***Address***");
            Console.Write("HouseNum:");
            p.address.houseNum = Console.ReadLine();
            Console.Write("Street:");
            p.address.street = Console.ReadLine();
            Console.Write("City:");
            p.address.city = Console.ReadLine();

            #region getState
            Console.Write("State Enter One of the following:NY, FL, VA, MD, MA, CA, OH, TX ,OT(Other):   ");
            string input = Console.ReadLine().ToString().ToLower();
            switch (input)
            {
                case "ny":
                    p.address.State = State.NY;
                    break;
                case "fl":
                    p.address.State = State.FL;
                    break;
                case "va":
                    p.address.State = State.VA;
                    break;
                case "md":
                    p.address.State = State.MD;
                    break;
                case "ma":
                    p.address.State = State.MA;
                    break;
                case "tx":
                    p.address.State = State.TX;
                    break;
                case "ca":
                    p.address.State = State.CA;
                    break;
                case "oh":
                    p.address.State = State.OH;
                    break;
                case "other":
                case "ot":
                    p.address.State = State.OT;
                    break;
                default:
                    p.address.State = State.OT;
                    break;
            }
            #endregion
            #region GetCounty
            Console.Write("Country Enter One of the following:US, UK, India, Pakistan, Australia, Other:");
            input = Console.ReadLine().ToString().ToLower();
            switch (input)
            {
                case "us":
                    p.address.Country = Country.US;
                    int x = (int)Country.US;
                    p.phone.countryCode = x.ToString();
                    break;
                case "uk":
                    p.address.Country = Country.UK;
                     x = (int)Country.UK;
                    p.phone.countryCode = x.ToString();
                    break;
                case "india":
                    p.address.Country = Country.India;
                     x = (int)Country.India;
                    p.phone.countryCode = x.ToString();
                    break;
                case "pakistan":
                    p.address.Country = Country.Pakistan;
                    x = (int)Country.Pakistan;
                    p.phone.countryCode = x.ToString();
                    break;
                case "australia":
                    p.address.Country = Country.Australia;
                    x = (int)Country.Australia;
                    p.phone.countryCode = x.ToString();
                    break;
                case "other":
                default:
                    p.address.Country = Country.Other;
                    x = (int)Country.Other;
                    p.phone.countryCode = x.ToString();
                    break;
            }
            #endregion
   
            Console.Write("ZipCode:");
            p.address.zipcode = Console.ReadLine(); ;
            #endregion
        }

        //part of add , just for a persons phone
        void GetPhone(Person p)
        {
            #region GetPhone#
            Console.WriteLine("\n***PhoneNumber***");
            Console.Write("AreaCode:");
            p.phone.areaCode = Console.ReadLine(); ;
            Console.Write("Number:");
            p.phone.number = Console.ReadLine();
            Console.Write("Ext:");
            p.phone.ext = Console.ReadLine();
            #endregion

        }

        //Delete People from the library
        public void Delete()
        {
            string ID;
            Console.WriteLine("Enter PID of a person to delete:");
            Console.Write("::");
            ID = Console.ReadLine();
            #region DeletePerson
            var item = Library.SingleOrDefault(x => x.Pid.ToString() == ID);
            if (item != null)
            {
                Library.Remove(item);
                DeleteOutside(item);
                Console.Clear();
                Console.WriteLine();
                string message = "*****The Selected record has been deleted*****";
                Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
                
            }
            else
                Console.WriteLine($"No such person exits with ID:{ID}");
            #endregion
        }

        //Update Records of people in the library
        public void Update()
        {
            long id = 0;
            Person p = new Person();

            #region Queryperson
            Console.WriteLine("Enter the ID of the Contact you would like to update:");
            Console.Write("::");

            try{   id = Convert.ToInt64(Console.ReadLine()); }
            catch (Exception ex) { Console.WriteLine("\n     " + ex.Message);  return; } 
            var query = from p1 in Library
                        where p1.Pid == id
                        select p1;

            if (query.Count() < 1)
            {
                Console.WriteLine();
                Console.WriteLine("          No such ID found try again later.:}          ");
                Console.WriteLine();
                return;
            }
            foreach (Person x in query)
            {
                p = x;
            }
            #endregion
            string again="yes";
            while(again == "yes")
            {

                Console.WriteLine();
                Console.WriteLine("***** Enter the field you woud like to update: *****");
                Console.WriteLine("***** Your choices are:");
                Console.WriteLine("         1.Name     2.Address    3.Phone    ");
                Console.Write("::");

                #region UserInputOptions
                string input = Console.ReadLine().ToString().ToLower();
                switch (input)
                {
                    case "1":
                    case "name":
                        GetName(p);
                        UpdateOutside(p,"name");
                        break;

                    case "2":
                    case "address":
                        GetAddress(p);
                        UpdateOutside(p,"address");
                        break;

                    case "3":
                    case "phone":
                        GetPhone(p);
                        UpdateOutside(p,"phone");
                        break;

                    default:
                        Console.WriteLine("*******You did not enter one of the choices so goodbye XD*******");
                        break;
                }//end switch 
                #endregion

                Console.WriteLine();
                Console.WriteLine("***** Would you like to Update another feild of the current Contact? *****");
                Console.WriteLine("***** Enter Yes or No *****");
                Console.Write("::");
                again = Console.ReadLine().ToString().ToLower();
            }//end while
        }//end update

        //Update for records of people in the library
        public void Search()
        { 
            #region Setup
            string type ;
            string message1 = "\n   ******Please enter Who or what you would like to search for:******";
            string search;
            Console.Clear();
            Console.WriteLine();
            string message = "****Please Select A Search Catagory:****";
            Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
            Console.WriteLine("         1.FirstName ".PadLeft((Console.WindowWidth + message.Length-25) / 2));
            Console.WriteLine("         2.LastName  ".PadLeft((Console.WindowWidth + message.Length-25) / 2));
            Console.WriteLine("         3.ZipCode   ".PadLeft((Console.WindowWidth + message.Length-25) / 2));
            Console.WriteLine("         4.City      ".PadLeft((Console.WindowWidth + message.Length-25) / 2));
            Console.WriteLine("         5.Phone     ".PadLeft((Console.WindowWidth + message.Length-25) / 2));

            Console.Write("::");
            type = Console.ReadLine();
            string message2 = "***Results shown will be anything equaling or containg what you enter:***";
            #endregion

            switch (type.ToLower())
            {
                #region Search_FirstName
                case "1":
                case "firstname":
                    Console.WriteLine(message1);
                    Console.WriteLine(message2);
                    Console.Write("::");
                    search = Console.ReadLine();
                    try
                    {
                        var result = Library.Where(p => p.FirstName.ToLower().Contains(search.ToLower()));
                        foreach (Person p in result)
                        {
                            Print(p);
                        }
                    }
                    catch (Exception e) { Console.WriteLine("{0} Exception caught.", e); }
                    break;
                #endregion
                #region Search_LastName
                case "2":
                case "lastname":
                    Console.WriteLine(message1);
                    Console.WriteLine(message2);
                    Console.Write("::");
                    search = Console.ReadLine();
                    try
                    {
                        var result = Library.Where(p => p.LastName.ToLower().Contains(search.ToLower()));
                        foreach (Person p in result)
                        {
                            Print(p);
                        }
                    }
                    catch (Exception e) { Console.WriteLine("{0} Exception caught.", e); }
                    break;
                #endregion
                #region Search_Zipcode
                case "3":
                case "zipcode":
                    Console.WriteLine(message1);
                    Console.WriteLine(message2);
                    Console.Write("::");
                    search = Console.ReadLine();
                    try
                    {
                        var result = Library.Where(p => p.address.zipcode.Contains(search));
                        foreach (Person p in result)
                        {
                            Print(p);
                        }
                    }
                    catch (Exception e) { Console.WriteLine("{0} Exception caught.", e); }
                    break;
                #endregion
                #region Search_City
                case "4":
                case "city":
                    Console.WriteLine(message1);
                    Console.WriteLine(message2);
                    Console.Write("::");
                    search = Console.ReadLine();
                    try
                    {
                        var result = Library.Where(p => p.address.city.ToLower().Contains(search.ToLower()));
                        foreach (Person p in result)
                        {
                            Print(p);
                        }
                    }
                    catch (Exception e) { Console.WriteLine("{0} Exception caught.", e); }
                    break;
                #endregion
                #region Search_Phone
                case "5":
                case "phone":
                    Console.WriteLine(message1);
                    Console.WriteLine(message2);
                    Console.Write("::");
                    search = Console.ReadLine();
                    try
                    {
                        var result = Library.Where(p => p.phone.String().Contains(search));
                        foreach (Person p in result)
                        {
                            Print(p);
                        }
                    }
                    catch (Exception e) { Console.WriteLine("{0} Exception caught.", e); }
                    break;
                #endregion
                default:
                    Console.WriteLine("*******You did not enter one of the choices so goodbye XD*******");
                    break;
            }
 
        }


    }//end class
}
