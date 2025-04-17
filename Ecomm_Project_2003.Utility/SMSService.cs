using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Ecomm_Project_2003.Utility
{
    public  class SMSService
    {
        

        private readonly SMSSettings _sMSSettings;
        public SMSService(IOptions<SMSSettings> sMSSettings)
        {
            _sMSSettings = sMSSettings.Value;
        }
        public async Task<MessageResource> SendAsync(string to, string message)
        {
            TwilioClient.Init(_sMSSettings.AccountSID, _sMSSettings.AuthToken);

            var result = await MessageResource.CreateAsync(
                 body: message,
                 from: new PhoneNumber(_sMSSettings.PhoneNumber),
                 to: new PhoneNumber(to)
                 );
            return result;
        }

        public async Task<CallResource> SendCallAsync(string to, string message)
        {
            TwilioClient.Init(_sMSSettings.AccountSID, _sMSSettings.AuthToken);

            var result = await CallResource.CreateAsync(
                 from: new PhoneNumber(_sMSSettings.PhoneNumber),
                 to: new PhoneNumber(to),
                 twiml: new Twiml($"<Response><Say>{message}</Say></Response>")
                 );
            return result;
        }
    }
}
   