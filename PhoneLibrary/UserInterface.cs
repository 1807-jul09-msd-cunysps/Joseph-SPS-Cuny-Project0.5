using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneLibrary
{
    public class UserInterface
    {
        Operations pOperations = new Operations();
        public UserInterface()
        {
            Start();

        }
        
        //Start The Phone App
        public void Start()
        {
            Console.SetWindowSize(145, 38);
            #region setup
            string input = "0";
            pOperations.SQLStart();
            //ReadToCollection();
            string message = "******* Welcome to the Phone Directory App *******";
            string padding = "**********************************************************";
            Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
            Console.WriteLine(padding.PadLeft((Console.WindowWidth + padding.Length) / 2));
            Console.WriteLine(padding.PadLeft((Console.WindowWidth + padding.Length) / 2));
            #endregion
            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
              
                while (input != "end")
                {
                    Console.WriteLine();
                    message = "*****Enter one of the options below *****";
                    Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
                    message = "1.Read     2.Add    3.Delete ";
                    Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
                    message = "4.Update   5.Search    6.END  ";
                    Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
                    Console.Write("::");

                    #region UserInputOptions
                    input = Console.ReadLine().ToString().ToLower();
                    switch (input)
                    {
                        case "1":
                        case "read":
                            UIRead();
                            break;

                        case "2":
                        case "add":
                            UIAdd();
                            break;

                        case "3":
                        case "delete":
                            Console.WriteLine();
                            UIDelete();
                            break;

                        case "4":
                        case "update":
                            Console.WriteLine();
                            UIUpdate();
                            break;

                        case "5":
                        case "search":
                            Console.WriteLine();
                            UISearch();
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
                            message = "*** Please Try Again ***";
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

        //UI for Read
        public void UIRead()
        {
            Console.Clear();
            Console.WriteLine();
            string message = "*****Showing all " + pOperations.Library.Count() + " People in Directory*****";
            Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
            List<Person> contacts = new List<Person>();
            contacts = pOperations.Read();
            foreach (Person p in contacts)
            {
                Print(p);
            }
            Console.WriteLine("___________________________________________________________________________________________________________________________________");
        }

       //UI for Add method info of a Person  <----- change the id --ignore
        public void UIAdd()
        {
            string y = "yes";
            while (y == "yes")
            {
                Console.WriteLine();
                Person p = new Person();
                
                
                Console.WriteLine(" Enter a new Contact::");
                Console.WriteLine(" Type the information then click enter!");

                #region PersonInfoaddition
                GetName(p);
                GetAddress(p);
                GetPhone(p);
                pOperations.Add(p);
                #endregion
                
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
            Console.Write("Gender:");
            p.Gender = Console.ReadLine();
            

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
                    p.address.State = State.NY.ToString();
                    break;
                case "fl":
                    p.address.State = State.FL.ToString();
                    break;
                case "va":
                    p.address.State = State.VA.ToString();
                    break;
                case "md":
                    p.address.State = State.MD.ToString();
                    break;
                case "ma":
                    p.address.State = State.MA.ToString();
                    break;
                case "tx":
                    p.address.State = State.TX.ToString();
                    break;
                case "ca":
                    p.address.State = State.CA.ToString();
                    break;
                case "oh":
                    p.address.State = State.OH.ToString();
                    break;
                case "other":
                case "ot":
                    p.address.State = State.OT.ToString();
                    break;
                default:
                    p.address.State = State.OT.ToString();
                    break;
            }
            #endregion
            #region GetCounty
            Console.Write("Country Enter One of the following:US, UK, India, Pakistan, Australia, Other:");
            input = Console.ReadLine().ToString().ToLower();
            switch (input)
            {
                case "us":
                    p.address.Country = Country.US.ToString();
                    int x = (int)Country.US;
                    p.phone.countryCode = x.ToString();
                    break;
                case "uk":
                    p.address.Country = Country.UK.ToString();
                     x = (int)Country.UK;
                    p.phone.countryCode = x.ToString();
                    break;
                case "india":
                    p.address.Country = Country.India.ToString();
                     x = (int)Country.India;
                    p.phone.countryCode = x.ToString();
                    break;
                case "pakistan":
                    p.address.Country = Country.Pakistan.ToString();
                    x = (int)Country.Pakistan;
                    p.phone.countryCode = x.ToString();
                    break;
                case "australia":
                    p.address.Country = Country.Australia.ToString();
                    x = (int)Country.Australia;
                    p.phone.countryCode = x.ToString();
                    break;
                case "other":
                default:
                    p.address.Country = Country.Other.ToString();
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

        //UIDelete People from the library
        public void UIDelete()
        {
            string ID;
            Console.WriteLine("Enter PID of a person to delete:");
            Console.Write("::");
            ID = Console.ReadLine();
            #region DeletePerson
            var item = pOperations.Library.SingleOrDefault(x => x.Pid.ToString() == ID);
            if (item != null)
            {
                Console.Clear();
                Console.WriteLine();
                pOperations.Delete(item);
                string message = "*****The Selected record has been deleted*****";
                Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
                
            }
            else
                Console.WriteLine($"No such person exits with ID:{ID}");
            #endregion
        }
        
        //UI for Update method
        public void UIUpdate()
        {
            long id = 0;
            Person p = new Person();

            #region Queryperson
            Console.WriteLine("Enter the ID of the Contact you would like to update:");
            Console.Write("::");

            try { id = Convert.ToInt64(Console.ReadLine()); }
            catch (Exception ex) { Console.WriteLine("\n     " + ex.Message); return; }

            #endregion

            string again = "yes";
            while (again == "yes")
            {

                Console.WriteLine();
                Console.WriteLine("***** Enter the field you woud like to update: *****");
                Console.WriteLine("***** Your choices are:");
                Console.WriteLine("         1.Name     2.Address    3.Phone    ");
                Console.Write("::");

                #region UserInputOptions
                string input = Console.ReadLine().ToString().ToLower();

                try
                {

                    switch (input)
                    {
                        case "1":
                        case "name":
                            GetName(p);
                            pOperations.Update(id, input, p);
                            break;

                        case "2":
                        case "address":
                            GetAddress(p);
                            pOperations.Update(id, input, p);
                            break;

                        case "3":
                        case "phone":
                            GetPhone(p);
                            pOperations.Update(id, input, p);
                            break;

                        default:
                            Console.WriteLine("*******You did not enter one of the choices so goodbye XD*******");
                            break;
                    }


                }
                catch (Exception e) { Console.WriteLine(e.Message); }
                #endregion

                Console.WriteLine();
                Console.WriteLine("***** Would you like to Update another feild of the current Contact? *****");
                Console.WriteLine("***** Enter Yes or No *****");
                Console.Write("::");
                again = Console.ReadLine().ToString().ToLower();
            }//end while

        }//end update
        
        //UI for Search method
        public void UISearch()
        {
            #region Setup
            string type;
            string search;
            Console.Clear();
            Console.WriteLine();
            string message = "****Please Select A Search Catagory:****";
            Console.WriteLine(message.PadLeft((Console.WindowWidth + message.Length) / 2));
            Console.WriteLine("         1.FirstName ".PadLeft((Console.WindowWidth + message.Length - 25) / 2));
            Console.WriteLine("         2.LastName  ".PadLeft((Console.WindowWidth + message.Length - 25) / 2));
            Console.WriteLine("         3.ZipCode   ".PadLeft((Console.WindowWidth + message.Length - 25) / 2));
            Console.WriteLine("         4.City      ".PadLeft((Console.WindowWidth + message.Length - 25) / 2));
            Console.WriteLine("         5.Phone     ".PadLeft((Console.WindowWidth + message.Length - 25) / 2));

            Console.Write("::");
            type = Console.ReadLine();
            string message1 = "\n   ******Please enter Who or what you would like to search for:******";
            string message2 = "***Results shown will be anything equaling or containg what you enter:***";

            Console.WriteLine(message1);
            Console.WriteLine(message2);
            Console.Write("::");
            search = Console.ReadLine();
            #endregion
            try
            {
                IEnumerable<Person> result = pOperations.Search(type, search);
                foreach (Person p in result)
                {
                    Print(p);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        
        //Print Entire Person Object
        public void Print(Person p)
        {
            Console.WriteLine("___________________________________________________________________________________________________________________________________");
            Console.WriteLine($"| PersonID:{p.Pid} | Name: {p.FirstName} , {p.LastName} | Address: {p.address.houseNum} {p.address.street} st " +
            $" {p.address.city} , {p.address.State} , {p.address.zipcode} , {p.address.Country} |" +
            $" PhoneNumber: {p.phone.countryCode}-{p.phone.areaCode}-{p.phone.number} | Ext: {p.phone.ext} ");
        }

    }
}
