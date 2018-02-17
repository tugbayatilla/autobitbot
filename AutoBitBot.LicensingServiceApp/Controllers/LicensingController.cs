using ArchPM.Core.Api;
using AutoBitBot.LicensingServiceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutoBitBot.LicensingServiceApp.Controllers
{
    public class LicensingController : ApiController
    {
        // GET: api/Default
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public ApiResponse<Licensing> Get(Guid? id)
        {
            return ApiResponse<Licensing>.CreateSuccessResponse(new Licensing() { OwnerId = Guid.Empty });
        }

        // POST: api/Default
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }
    }
}
