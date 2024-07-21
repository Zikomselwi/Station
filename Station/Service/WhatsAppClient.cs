//using Amazon.SQS.Model;
//using Infobip.Api.Config;
//using Infobip.Api.Sdk;
//using Infobip.Api.Sdk.Model.Whatsapp;
//using Infobip.Api.Sdk.WhatsApp;
//namespace Station.Service
//{
//    public class WhatsAppClient : IWhatsAppClient
//    {
//        private readonly Infobip.Api.Sdk.WhatsApp.WhatsAppClient _infobipWhatsAppClient;

//        public WhatsAppClient(string baseUrl, string apiKey)
//        {
//            var configuration = new BasicAuthConfiguration(baseUrl, apiKey);
//            _infobipWhatsAppClient = new Infobip.Api.Sdk.WhatsApp.WhatsAppClient(configuration);
//        }

//        public async Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request)
//        {
//            return await _infobipWhatsAppClient.SendWhatsAppMessageAsync(request);
//        }

//    }
//    public interface IWhatsAppClient
//    {
//        Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request);
//    }
//}