using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FinstatApi
{
    public class WebClientWithTimeout : WebClient
    {
        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientWithTimeout"/> class.
        /// </summary>
        public WebClientWithTimeout() : this(60000) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClientWithTimeout"/> class.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public WebClientWithTimeout(int timeout)
        {
            this.Timeout = timeout;
        }

        /// <summary>
        /// Returns a <see cref="T:System.Net.WebRequest" /> object for the specified resource.
        /// </summary>
        /// <param name="address">A <see cref="T:System.Uri" /> that identifies the resource to request.</param>
        /// <returns>
        /// A new <see cref="T:System.Net.WebRequest" /> object for the specified resource.
        /// </returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }
    }
}
