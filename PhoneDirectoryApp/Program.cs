using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PhoneLibrary;
using System.Data.SqlClient;


namespace PhoneDirectoryApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            #region StartApp
            PersonDirectory pDirectory = new PersonDirectory();
            pDirectory.Start();
            #endregion


        }//end main

    }//end program
}//end namespace
