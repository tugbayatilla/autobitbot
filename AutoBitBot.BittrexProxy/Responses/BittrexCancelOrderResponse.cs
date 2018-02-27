using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AutoBitBot.BittrexProxy.Responses
{
    [DataContract]
    public class BittrexCancelOrderResponse
    {
        [DataMember(Name ="success")]
        public bool Success { get; set; }

        [DataMember(Name ="message")]
        public String Message { get; set; }

        [DataMember(Name ="result")]
        public Object Result { get; set; }

    }
}