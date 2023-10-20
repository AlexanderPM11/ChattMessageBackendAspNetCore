using crudSegnalR.Infrastructure.Identity.Entities;
using crudSignalR.Core.Application.Common;
using crudSignalR.Core.Application.Dtos.Account;
using crudSignalR.Core.Application.Dtos.EmailService;
using crudSignalR.Core.Application.Dtos.ExternalLogin.Facabook;
using crudSignalR.Core.Application.Enums.Users;
using crudSignalR.Core.Application.Helper;
using crudSignalR.Core.Application.Interface.Http;
using crudSignalR.Core.Application.Interface.Services;
using crudSignalR.Core.Domain.Settings;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace crudSegnalR.Infrastructure.Identity.Service
{
    public class AccountService : IAccountService
    {
        #region  Contructor and dependency
        private readonly UserManager<ApplicantionUser> _userManager;
        private readonly SignInManager<ApplicantionUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JwtSettings _jwtSettings;
        private readonly IConfiguration _configuration;
        private readonly IFacebookHttp _facebookHttp;

        public AccountService(
            IOptions<JwtSettings> jwtSettings,
            UserManager<ApplicantionUser> userManager, SignInManager<ApplicantionUser> signInManager, IEmailService emailService, IConfiguration configuration,IFacebookHttp facebookHttp)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;
            _configuration = configuration;
            _facebookHttp = facebookHttp;
        }
        #endregion

        #region AuthenticateAsync
        public async Task<GenericApiResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            GenericApiResponse<AuthenticationResponse> response = new GenericApiResponse<AuthenticationResponse>();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.statuscode = 404;
                response.success = false;
                response.messages = new List<string>() { $"No Account registered with this email {request.Email}" };
                return response;

            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);
            if (!result.Succeeded)
            {
                response.statuscode = 404;
                response.success = false;
                response.messages = new List<string>() { $"Invalid credantials for this email {request.Email}" };
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.statuscode = 401;
                response.success = false;
                response.messages = new List<string>() { $"Account no confirmed for this email {request.Email}" };
                return response;
            }
            AuthenticationResponse? authenticationResponse = await JwTokenResponse(user.Email);
            response.payload = authenticationResponse;
            return response;
        }

        #endregion

        #region AuthenticateWithGoogleAsyn
        public async Task<GenericApiResponse<AuthenticationResponse>> AuthenticateWithGoogleAsyn(ExternalAuthDto externalAuth)
        {
            GenericApiResponse<AuthenticationResponse> response = new GenericApiResponse<AuthenticationResponse>();
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _configuration.GetSection("GoogleAuthSettings:clientId").Value ?? "" }
                };

                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                
                if (payload != null)
                {
                    var user = await _userManager.FindByEmailAsync(payload.Email);
                    if (user == null)
                    {
                        response.statuscode = 404;
                        response.success = false;
                        response.messages = new List<string>() { $"No Account registered with this email {payload.Email}" };
                        return response;

                    }
                    if (!user.EmailConfirmed)
                    {
                        response.statuscode = 401;
                        response.success = false;
                        response.messages = new List<string>() { $"Account no confirmed for this email {user.Email}" };
                        return response;
                    }
                    AuthenticationResponse? authenticationResponse = await JwTokenResponse(payload.Email); 
                    response.payload = authenticationResponse;
                }
                return response;

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages = new List<string>() { ex.Message };
                return response;
            }
        }

        #endregion

        #region AuthenticateWithFacebookAsyn
        public async Task<GenericApiResponse<AuthenticationResponse>> AuthenticateWithFacebookAsyn(string credential)
        {
            GenericApiResponse<AuthenticationResponse> response = new GenericApiResponse<AuthenticationResponse>();
            try
            {
                string urlValidToken = "https://graph.facebook.com/debug_token?input_token=" +
                   credential + $"&access_token={_configuration.GetSection("FacebookAuth:AppId").Value}|{_configuration.GetSection("FacebookAuth:AppSecret").Value}";

                var validUser = await _facebookHttp.Get(urlValidToken);

                if (validUser.success)
                {
                    var responsevalidUser = Newtonsoft.Json.JsonConvert.DeserializeObject<UserValidFacebookDto>(validUser.payload!);
                    if (responsevalidUser!.Data.Is_Valid)
                    {
                        string urlGetUserInformation = "https://graph.facebook.com/me?fields=first_name,last_name,email,picture.width(200),id&access_token=" + credential;
                        var userData = await _facebookHttp.Get(urlGetUserInformation);
                        var responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDataFacebookDTO>(userData.payload!);
                        if (responseObj?.Email != null)
                        {
                            var user = await _userManager.FindByEmailAsync(responseObj.Email);
                            if (user == null)
                            {
                                response.statuscode = 404;
                                response.success = false;
                                response.messages = new List<string>() { $"No Account registered with this email {responseObj.Email}" };
                                return response;
                            }
                            if (!user.EmailConfirmed)
                            {
                                response.statuscode = 401;
                                response.success = false;
                                response.messages = new List<string>() { $"Account no confirmed for this email {user.Email}" };
                                return response;
                            }
                            AuthenticationResponse? authenticationResponse = await JwTokenResponse(responseObj.Email);
                            response.payload = authenticationResponse;
                        }
                        return response;
                    }
                    else
                    {
                        response.success = false;
                        response.messages = new List<string>() { "Ocurrió un error al momento de hacer el login" };
                        return response;
                    }
                }
                else
                {
                    response.success = false;
                    response.messages = new List<string>() { "Ocurrió un error al momento de hacer el login" };
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages = new List<string>() { ex.Message };
                return response;
            }
        }

        #endregion

        #region SingOutAsync
        public async Task SingOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        #endregion        

        #region UpdateStatusOnline
        public async Task<GenericApiResponse<bool>> UpdateStatusOnline(bool isOnline, string id)
        {
            GenericApiResponse<bool> response = new();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                response.statuscode = 500;
                response.success = false;
                response.messages = new List<string>() { $"An error ocurred while change status Online user" };
                return response;
            }
            user.IsOnline = !user.IsOnline;
            var result = await _userManager.UpdateAsync(user);
            response.payload = result.Succeeded;
            if (!result.Succeeded)
            {
                response.statuscode = 500;
                response.success = false;
                response.messages = new List<string>() { $"An error ocurred while change status Online user" };
                return response;
            }
            return response;
        }

        #endregion

        #region GetAllUserExectMe
        public async Task<GenericApiResponse<List<AuthenticationResponse>>> GetAllUserExectMe(string id)
        {
            GenericApiResponse<List<AuthenticationResponse>> listAuthenticationResponse = new();
            listAuthenticationResponse.payload = new();
            var users = _userManager.Users.Where(user=>user.Id != id).ToList();
            foreach (var user in users)
            {
                AuthenticationResponse authenticationResponse = new()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsOnline = user.IsOnline,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsVerified = user.EmailConfirmed,
                    PictureBytes = user.BytesImageUsery
                };

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationResponse.Roles = rolesList.ToList();
                listAuthenticationResponse.payload.Add(authenticationResponse);
            }
            return listAuthenticationResponse;
        }


        #endregion

        #region RegsterBasicUserAsync
        public async Task<GenericApiResponse<RegisterResponse>> RegsterBasicUserAsync(RegisterRequest request, string origin)
        {
            GenericApiResponse<RegisterResponse> response = new();
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                response.statuscode = 409;
                response.success = false;
                response.messages = new List<string>() { $"Username {request.UserName} is already taken." };
                return response;
            }
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.statuscode = 409;
                response.success = false;
                response.messages = new List<string>() { $"Email {request.Email} is already taken." };
                return response;
            }
            var userToCreate = new ApplicantionUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                UserName = request.UserName,
            };

            var result = await _userManager.CreateAsync(userToCreate, request.Password);
            if (result.Succeeded)
            {
                var responseUser = new RegisterResponse
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    UserName = request.UserName,

                };
                response.payload = responseUser;
                response.success = true;
                response.messages = new List<string>() { GeneralMessageResponse.Success };
                await _userManager.AddToRoleAsync(userToCreate, UserRoles.Basic.ToString());
                string verificantionUri = await SendVerificantionEmailUrl(userToCreate, origin);

                string htmlBody = $@"
                        <!DOCTYPE html>
                        <html>

                        <head>
                            <title>Confirmación de Correo Electrónico</title>
                        </head>

                        <body style='font-family: Poppins, sans-serif;'>
                            <div style='text-align: center; width: 100%; display: flex; justify-content: center;'>
                                <div style='width: 50%; text-align: center'>
                                    <h2>Confirmación de Correo Electrónico</h2>
                                    <p>Estimado(a) <b>{request.Email}</b></p>
                                    <p>Gracias por registrarte en nuestra aplicación. Para completar el proceso de registro y activar su cuenta,
                                        por favor haga clic en el botón de abajo para confirmar su dirección de correo electrónico:</p>

                                    <a href={verificantionUri}
                                        style='display: inline-block; padding: 10px 20px; background-color: #007BFF; color: white; text-decoration: none; border-radius: 5px;'>Confirmar
                                        Correo Electrónico</a>

                                    <p>Una vez que hayas confirmado su correo electrónico, tendrás acceso completo a nuestra aplicación y podrás
                                        disfrutar de todos nuestros servicios.</p>

                                    <p>¡Gracias nuevamente por unirse a nosotros!</p>
                                    <img src='https://i.postimg.cc/MTSz4TNm/Bing-Image-Of-The-Day.jpg' alt='Logo de la Compañía'
                                        style='max-width: 400px; width: 85%;  margin-top: 20px; border-radius: 10px;'>
                                </div>
                            </div>
                        </body>

                        </html>
                        ";

                await _emailService.SendAsync(
                    new EmailRequest
                    {
                        To = userToCreate.Email,
                        Body = htmlBody,
                        Subject = "Confirm registration"
                    });
                return response;
            }
            else
            {
                List<string> messages = new List<string>();
                foreach (var error in result.Errors)
                {
                    messages.Add(error.Description);
                }
                response.statuscode = 500;
                response.success = false;
                response.messages = messages;
                return response;
            }
        }

        #endregion

        #region ForgotPasswordAsync
        public async Task<GenericApiResponse<string>> ForgotPasswordAsync(ForgorPasswordRequest request, string origen)
        {
            GenericApiResponse<string> response = new();
            var account = await _userManager.FindByEmailAsync(request.Email);
            if (account == null)
            {
                return response;
            }
            var verificantionUri = await SendForgotPasswordlUrl(account, origen);
            await _emailService.SendAsync(
                  new EmailRequest
                  {
                      To = account.Email,
                      Body = $"Please reset your password visiting this url: {verificantionUri}",
                      Subject = "Reset registration"
                  });
            response.success = true;
            return response;
        }

        #endregion

        #region ConfirmEmailAsync
        public async Task<GenericApiResponse<string>> ConfirmEmailAsync(ConfirmEmailRequestDTO confirmEmailRequestDTO)
        {
            GenericApiResponse<string> response = new();
            var user = await _userManager.FindByIdAsync(confirmEmailRequestDTO.UserId);
            if (user == null)
            {
                response.messages = new List<string>() { "No account register with this user" };
                response.success = false;
                response.payload = "No account register with this user";
                return response;
            }
            if (user.EmailConfirmed)
            {
                response.messages = new List<string>() { "El correo ya fue confirmado" };
                response.success = false;
                response.payload = "El correo ya fue confirmado";
                return response;
            }
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmailRequestDTO.Token));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                response.messages = new List<string>() { $"Account confirmed for {user.Email}. You can now use the app" };
                response.success = true;
                response.payload = $"Account confirmed for {user.Email}. You can now use the app";
                return response;
            }
            response.messages = new List<string>() { $"An error ocurred while confiming {user.Email}" };
            response.success = false;
            response.payload = $"An error ocurred while confiming {user.Email} --> {result.Errors.FirstOrDefault().Description}";
            return response;
        }

        #endregion

        #region ResetPasswordAsyn
        public async Task<GenericApiResponse<string>> ResetPasswordAsyn(ResetPasswordRequest resetPasswordRequest)
        {
            GenericApiResponse<string> response = new();
            var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);
            if (user == null)
            {
                response.success = false;
                response.messages.Add("No account register with this user");
                return response;
            }
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordRequest.Token));
            var result = await _userManager.ResetPasswordAsync(user, code, resetPasswordRequest.Password);

            if (result.Succeeded)
            {
                response.success = true;
                response.messages.Add("Password reset successfully");
                response.payload = "Password reset successfully";
                return response;
            }
            response.success = false;
            foreach (var error in result.Errors)
            {
                response.messages.Add(error.Description);
            }
            return response;
        }

        #endregion 

        #region private method
        private async Task<AuthenticationResponse?> JwTokenResponse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            if(user!= null)
            {
                JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
                AuthenticationResponse authenticationResponse = new AuthenticationResponse();
                authenticationResponse.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationResponse.Id = user.Id;
                authenticationResponse.Email = user.Email;
                authenticationResponse.IsOnline = user.IsOnline;
                authenticationResponse.UserName = user.UserName;
                authenticationResponse.FirstName = user.FirstName;
                authenticationResponse.LastName = user.LastName;
                authenticationResponse.PictureBytes = user.BytesImageUsery;
                authenticationResponse.IsVerified = user.EmailConfirmed;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationResponse.Roles = rolesList.ToList();
                return authenticationResponse;
            }
            return null;
           
        }
        private async Task<string> SendVerificantionEmailUrl(ApplicantionUser applicantion, string origin)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(applicantion);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string route = "User/ConfirmEmail";
            //Uri Uri = new Uri(string.Concat($"https://confirm-emailapp.netlify.app/index.html/", route));
            Uri Uri = new Uri("https://appchattt.netlify.app/index.html/");
            string verificantionUrl = QueryHelpers.AddQueryString(Uri.ToString(), "userId", applicantion.Id);
            verificantionUrl = QueryHelpers.AddQueryString(verificantionUrl, "token", code);
            return verificantionUrl;
        }             

        private async Task<string> SendForgotPasswordlUrl(ApplicantionUser applicantion, string origin)
        {
            string code = await _userManager.GeneratePasswordResetTokenAsync(applicantion);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string route = "User/ResetPassword";
            //Uri Uri = new Uri(string.Concat($"{origin}/", route));
            //Uri Uri = new Uri(string.Concat($"https://192.168.8.193:45458/", route));
            Uri Uri = new Uri("https://appchattt.netlify.app/changePassWord.html/");
            string verificantionUrl = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);
            return verificantionUrl;
        }        
        //jwt 
        private async Task<JwtSecurityToken> GenerateJWToken(ApplicantionUser applicantion)
        {
           var userClamis=await _userManager.GetClaimsAsync(applicantion);
           var userRoles=await _userManager.GetRolesAsync(applicantion);

            var rolesClaims = new List<Claim>();

            foreach (var role in userRoles)
            {
                rolesClaims.Add(new Claim("roles",role));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,applicantion.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,applicantion.Email),
                new Claim("uid",applicantion.Id)
            }.Union(rolesClaims)
            .Union(userClamis);

            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes)
                );
            return jwtSecurityToken;
        }

        #endregion

        #region UpdateImageUser

        public async Task<GenericApiResponse<byte[]>> UpdateImageUser(UpdateImageUserDTO updateImageUser)
        {
            
            GenericApiResponse<byte[]> response = new();

            var user = await _userManager.FindByIdAsync(updateImageUser.userId);
            if (user!=null)
            {
                var imageBytes = await EncondeImageData.EncondeImage(updateImageUser.file);
                if(imageBytes != null)
                {
                user.BytesImageUsery = imageBytes;
                               IdentityResult  identityResult=  await _userManager.UpdateAsync(user);
                                if (identityResult.Succeeded)
                                {
                                    response.messages = new List<string>() { GeneralMessageResponse.Success };
                                    response.success = identityResult.Succeeded;
                        response.payload = imageBytes;

                                }
                                else
                                {
                                    response.messages = new List<string>() { GeneralMessageResponse.Error };
                                    response.success = identityResult.Succeeded;
                                }
                }
                else
                {
                    response.messages = new List<string>() { GeneralMessageResponse.Error };
                    response.success = false;
                }

            }
            else
            {
                response.success = false;
                response.messages = new List<string>() { GeneralMessageResponse.NotFoundMesage };
            }
            return response;
        }

        #endregion
    }
}
