using crudSignalR.Core.Application.Common;
using crudSignalR.Core.Application.Dtos.Account;

namespace crudSignalR.Core.Application.Interface.Services
{
    public interface IAccountService
    {
        Task<GenericApiResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<GenericApiResponse<RegisterResponse>> RegsterBasicUserAsync(RegisterRequest request, string origin);
        Task<GenericApiResponse<string>> ForgotPasswordAsync(ForgorPasswordRequest request, string origen);
        Task<GenericApiResponse<string>> ConfirmEmailAsync(ConfirmEmailRequestDTO confirmEmailRequestDTO);
        Task<GenericApiResponse<string>> ResetPasswordAsyn(ResetPasswordRequest resetPasswordRequest);
        Task SingOutAsync();
        Task<GenericApiResponse<bool>> UpdateStatusOnline(bool isOnline, string id);
        Task<GenericApiResponse<List<AuthenticationResponse>>> GetAllUserExectMe(string id);
        Task<GenericApiResponse<byte[]>> UpdateImageUser(UpdateImageUserDTO updateImageUser);
        Task<GenericApiResponse<AuthenticationResponse>> AuthenticateWithGoogleAsyn(ExternalAuthDto externalAuth);
        Task<GenericApiResponse<AuthenticationResponse>> AuthenticateWithFacebookAsyn(string credential);


    }
}