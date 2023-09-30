using AutoMapper;
using crudSegnalR.Infrastructure.httpclient.Http;
using crudSegnalR.Infrastructure.Identity.Service;
using crudSignalR.Core.Application.Common;
using crudSignalR.Core.Application.Dtos.Account;
using crudSignalR.Core.Application.Interface.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.ConstrainedExecution;

namespace signalRApiStudent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IAccountService acountService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public AccountController(IAccountService acountService, IConfiguration configuration, HttpClient httpClient)

        {
            this.acountService = acountService;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        [HttpGet ("GetAllUserExectMe")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericApiResponse<List<AuthenticationResponse>>))]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllUserExectMe(string id)
        {
            var response = await acountService.GetAllUserExectMe(id);

            return Ok(
                response
                );
        }

        [HttpPatch ("UpdateStatusOnline")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdateStatusOnline(bool isOnline,string id)
        {
           var response= await acountService.UpdateStatusOnline(isOnline,id);

            return Ok(
                response
                );
        }
        
        [HttpPost ("LoginUser")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericApiResponse<AuthenticationResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult>Login ( [FromBody] AuthenticationRequest registerRequest)
        {
            var origin = Request.Headers["origin"];
            var response = await acountService.AuthenticateAsync(registerRequest);

            return Ok(
                response
                );
        }
        [HttpPost ("CreateBasicUser")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericApiResponse<RegisterResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> RegisterBasicUser( [FromBody] RegisterRequest registerRequest)
        {
            var origin = Request.Headers["origin"];
            var response = await acountService.RegsterBasicUserAsync(registerRequest, origin);

            return Ok(
                response
                );
        }


        [HttpPost("ForgotPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericApiResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword(ForgorPasswordRequest registerRequest)
        {

            var origin = Request.Headers["origin"];
            var response = await acountService.ForgotPasswordAsync(registerRequest, origin);

            return Ok(
                response
                );
        }
        [HttpPost("ResetPasswordAsyn")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericApiResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPasswordAsyn(ResetPasswordRequest resetPasswordRequest)
        {

            var response = await acountService.ResetPasswordAsyn(resetPasswordRequest);

            return Ok(
                response
                );
        }

        #region Authenticate with google Account
        [HttpPost("AuthenticateWithGoogle")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericApiResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AuthenticateWithGoogleAsyn([FromBody]ExternalAuthDto externalAuth)
        {
            try
            {
                var response = await acountService.AuthenticateWithGoogleAsyn(externalAuth);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString() );
                throw;
            }
           
        }

        #endregion
        #region Authenticate with facebook Account
        [HttpPost("AuthenticateWithFacebook")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericApiResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AuthenticateWithFacebookAsyn([FromBody]string credential)
        {
            try
            {
                var response = await acountService.AuthenticateWithFacebookAsyn(credential);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString() );
                throw;
            }
           
        }

        #endregion

        #region Update userProfile
        [HttpPut("UpdateImageUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericApiResponse<byte[]>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateImageUser([FromForm] UpdateImageUserDTO updateImageUser)
        {
            var response = await acountService.UpdateImageUser(updateImageUser);

            return Ok(
                response
                );
        }
        #endregion

        #region ChangingStatus
        [HttpPatch("ConfirmEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestDTO confirmEmailRequestDTO)
        {
            try
            {
                var response = await acountService.ConfirmEmailAsync(confirmEmailRequestDTO);               
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new GenericApiResponse<string>()
                {
                    success = false,
                    statuscode = StatusCodes.Status500InternalServerError,
                    messages = new() { "Hemos tenido problemas al procesar su solicitud, contacte con el soporte si esto persiste." }
                });
            }
        }
        #endregion

    }
}
