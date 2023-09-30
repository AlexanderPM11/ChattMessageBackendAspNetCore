using AutoMapper;
using crudSegnalR.Infrastructure.Identity.ViewModels;
using crudSignalR.Core.Application.Dtos.Account;
using crudSignalR.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Text;

namespace crudSignalR.Controllers
{
    public class UserController : Controller
    {
       private readonly IAccountService acountService;
       private readonly IMapper mapper;

       public  UserController(IAccountService acountService,IMapper mapper)
       {
            this.acountService = acountService;
            this.mapper = mapper;
       }

        public IActionResult ResetPassword(ResetPasswordViewModel vm)
        {
           
            return View(vm);
        }
        public async Task<IActionResult> ResetPasswordPost(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            ResetPasswordRequest request = mapper.Map<ResetPasswordRequest>(vm);
            var res = await acountService.ResetPasswordAsyn(request);
            return View(vm);
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            ConfirmEmailResponse confirmEmailResponse = new ConfirmEmailResponse { Result = true };
            using (HttpClient httpClient = new HttpClient())
            {
               
                try
                {

                    // Serializa el objeto a formato JSON
                    ConfirmEmailRequestDTO confirmEmailRequestDTO=new ConfirmEmailRequestDTO { Token= token ,UserId =userId};
                    string patchContent = JsonConvert.SerializeObject(confirmEmailRequestDTO);
                    StringContent patchData = new StringContent(patchContent, Encoding.UTF8, "application/json-patch+json");

                    var url = "http://172.20.10.2:45455/api/Account/ConfirmEmail";
                    HttpResponseMessage response = await httpClient.PatchAsync(url, patchData);

                    // Verifica si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                        confirmEmailResponse.Result = true;
                        confirmEmailResponse.Message = responseObject.payload;                    
                    }
                    else
                    {
                        confirmEmailResponse.Result = false;
                        confirmEmailResponse.Message = $"Error en la solicitud. Código de estado: {response.StatusCode}";
                    }
                }
                catch (HttpRequestException e)
                {
                    confirmEmailResponse.Result = false;
                    confirmEmailResponse.Message =  $"Error en la solicitud: {e.Message}";
                }
            }
            return View(model: confirmEmailResponse);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest registerRequest)
        {
            var origin = Request.Headers["origin"];
            var res = await acountService.RegsterBasicUserAsync(registerRequest, origin);
            return View();
        }

    }
}
