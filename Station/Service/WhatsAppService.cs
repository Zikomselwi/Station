//using Microsoft.Extensions.Options;
//using Infobip.Api.Sdk.Model.Whatsapp;
//using Station.Models;
//using Amazon.SQS.Model;
//using Infobip.Api.SDK.WhatsApp.Models;

//namespace Station.Service
//{
//    public class WhatsAppService
//    {
//        private readonly InfobipSettings _settings;
//        private readonly IWhatsAppClient _whatsAppClient;

//        public WhatsAppService(IOptions<InfobipSettings> settings, IWhatsAppClient whatsAppClient)
//        {
//            _settings = settings.Value;
//            _whatsAppClient = whatsAppClient;
//        }

//        public async Task<bool> SendWhatsAppMessage(Bill bill)
//        {
//            var messageText = $"Hello {bill.SubscriberName}, your bill details: \n" +
//                              $"Current Reading: {bill.CurrentReading}\n" +
//                              $"Previous Reading: {bill.PreviousReading}\n" +
//                              $"Consumption Difference: {bill.ConsumptionDifference}\n" +
//                              $"Consumption Cost: {bill.ConsumptionCost:C}\n" +
//                              $"Bill Date: {bill.BillDate}\n" +
//                              $"Is Paid: {bill.IsPaid}\n" +
//                              $"Subscription Type: {bill.SubscriptionType}";

//            var request = new SendMessageRequest
//            {
//                To = bill.WhatsAppNumber, // Assuming you have WhatsAppNumber property for recipient's WhatsApp number
//                From = "7774949",
//                Content = new WhatsAppTextContent { Text = messageText }
//            };

//            var response = await _whatsAppClient.SendMessageAsync(request);

//            return response.Status == SendMessageStatus.OK;
//        }
//    }
//}