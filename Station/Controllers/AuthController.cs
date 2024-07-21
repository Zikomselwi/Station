//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Station.dto;
//using System.Net.Http.Headers;
//using Station.Setting;
//using Microsoft.Extensions.Options;
//using Station.Models;

//namespace Station.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly WhatsAppService _setting;

//        public AuthController(IOptions<WhatsAppService> settings)
//        {
//            _setting = settings.Value;
//        }

//        [HttpPost("send-welcome-massege")]
//        public async Task<IActionResult> Sendbill(SendMessage s)
//        {
//            var language = Request.Headers["language"].ToString();
//            using HttpClient httpclient = new();
//            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _setting.Token);
//            WhatsAppRequest body = new()
//            {
//                to = s.number,
//                tamplate = new Tamplate
//                {
//                    Name = "hellow_world",
//                    language = new language
//                    {
//                        code = language
//                    }

//                }
//            };


         
//        HttpResponseMessage respon = await httpclient.PostAsJsonAsync(new Uri(_setting.ApiUrl), body);
//            if(!respon.IsSuccessStatusCode)
//            throw new Exception("somthing went wrong!");
//                return Ok(respon);
//    }
//}

//}
