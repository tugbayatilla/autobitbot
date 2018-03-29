using ArchPM.Core.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AutoBitBot.BittrexProxy
{
    [DataContract]
    public class BittrexApiResponse<T> : ApiResponse<T>
    {
        [DataMember(Name = "message")]
        public override string Message { get; set; }
        [DataMember(Name = "success")]
        public override bool Result { get; set; }
        [DataMember(Name = "result")]
        public override T Data { get; set; }

        [IgnoreDataMember]
        public override string Source => "Bittrex";

        [IgnoreDataMember]
        public override string Code { get; set; }
        [IgnoreDataMember]
        public override long ET { get; set; }
        [IgnoreDataMember]
        public override List<ApiError> Errors { get => base.Errors; set => base.Errors = value; }
    }
}