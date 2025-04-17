using System;
using System.Collections.Generic;
using System.Linq;
using Twilio.Rest.Api.V2010.Account;

namespace Ecomm_Project_2003.Utility
{
    public interface ISMSService
    {
        Task<MessageResource> SendAsync(string message, string to);
        Task<CallResource> SendCallAsync(string message, string to);

    }
}
