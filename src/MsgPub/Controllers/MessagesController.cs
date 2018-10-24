using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MsgPub.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IMsgPubSvc _msgPubSvc;

        public MessagesController(IMsgPubSvc msgPubSvc)
        {
            _msgPubSvc = msgPubSvc;
        }

        [HttpPost]
        public void Post([FromBody]Dictionary<string,object> value)
        {
            _msgPubSvc.PubMsg(JsonConvert.SerializeObject(value));
        }
    }
}
