using ArchPM.Core.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AutoBitBot.BittrexProxy
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ArchPM.Core.Api.ApiResponse{T}" />
    [DataContract]
    public class BittrexApiResponse<T> : ApiResponse<T>
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember(Name = "message")]
        public override string Message { get; set; }
        /// <summary>
        /// Gets or sets a value the requested operation whether is operated correctly or not.
        /// this is not HttpResponse result.
        /// </summary>
        /// <value>
        ///   <c>true</c> if result; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "success")]
        public override bool Result { get; set; }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [DataMember(Name = "result")]
        public override T Data { get; set; }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// Default is Core
        /// </value>
        [IgnoreDataMember]
        public override string Source => "Bittrex";

        /// <summary>
        /// Gets or sets the application specific code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [IgnoreDataMember]
        public override string Code { get; set; }
        /// <summary>
        /// Gets or sets the et.
        /// </summary>
        /// <value>
        /// The Execution Time
        /// </value>
        [IgnoreDataMember]
        public override long ET { get; set; }
        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        [IgnoreDataMember]
        public override List<ApiError> Errors { get => base.Errors; set => base.Errors = value; }
    }
}