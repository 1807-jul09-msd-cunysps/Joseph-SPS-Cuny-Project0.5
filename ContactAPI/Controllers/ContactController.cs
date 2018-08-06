﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using System.Web.Http.Results;
using PhoneLibrary;

namespace ContactAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ContactController : ApiController
    {


        EmailOps crud = new EmailOps();

        //READ
        [HttpGet]
        public IHttpActionResult Get()
        {
            var email = Json(crud.Read());
            return email;
        }

        //ADD Person
        [HttpPost]
        public IHttpActionResult Post([FromBody]Email data)
        {
            if (data != null)
            {
                // Make a call to CRUD Method to insert in to DB
                crud.Add(data);
                return Ok("Email Added");
            }
            else
            {
                return BadRequest("Bad Data supplied");
            }

        }
    }
}