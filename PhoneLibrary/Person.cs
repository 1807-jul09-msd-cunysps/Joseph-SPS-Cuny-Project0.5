using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PhoneLibrary
{

    public enum State
    {
        NY, FL, VA, MD, MA, CA, OH,TX, OT
    }

    public enum Country
    {
        US = 1, UK = 44, India = 91, Pakistan = 92, Australia = 61 , Other=00
    }

    [DataContract]
    public class Person
    {
        public Person()
        {
            address = new Address();
            address2 = new Address();
            phone = new Phone();
        }

        [DataMember]
        public long Pid        { get; set; }

        [DataMember]
        public string FirstName{ get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public Address address { get; set; }

        [DataMember]
        public Address address2 { get; set; }

        [DataMember]
        public Phone phone     { get; set; }

        
    }

    public class Address
    {
        public long Aid       { get; set; }
        public string houseNum { get; set; }
        public string street   { get; set; }
        public string city     { get; set; }
        public string State     { get; set; }
        public string Country { get; set; }
        public string zipcode  { get; set; }
    }

    public class Phone
    {
        public long PID        { get; set; }
        public string countryCode { get; set; }
        public string areaCode { get; set; }
        public string number   { get; set; }
        public string ext      { get; set; }

        public string String()
        {
            return areaCode + number;
        }
    }

    public class Email
    {
        public Email()
        {

        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public string Message { get; set; }
    }



}
