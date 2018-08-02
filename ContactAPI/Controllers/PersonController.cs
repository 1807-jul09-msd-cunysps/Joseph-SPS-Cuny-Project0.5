using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using PhoneLibrary;

namespace ContactAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class PersonController : ApiController
    {
        Operations crud = new Operations();

        //READ
        [HttpGet]
        public IHttpActionResult Get()
        {
            var person = Json(crud.Read()); 
            return person;
        }

        //ADD Person
        [HttpPost]
        public IHttpActionResult Post(Person p)
        {
            if (p != null)
            {
                // Make a call to CRUD Method to insert in to DB
                crud.Add(p);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        //DELETE Person
        [HttpDelete]
        public void Delete(Person p)
        {
            if (p != null)
            {
                // Make a call to CRUD Method to insert in to DB
                crud.Delete(p);
               
            }
            
        }
        
        // to do  Put
        //UPDATE Person
        [HttpPut]
        public IHttpActionResult Put(long searchfor,string updatecategory , Person p)
        {
            if (p != null)
            {
                // Make a call to CRUD Method to insert in to DB
                crud.Update(searchfor,updatecategory,p);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        //SEARCH 



    }
}
