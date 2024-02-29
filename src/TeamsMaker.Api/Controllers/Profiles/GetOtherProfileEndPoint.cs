using DataAccess.Base.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Controllers.Profiles
{
    [Authorize]
    public class GetOtherProfileEndpoint(IServiceProvider serviceProvider) : BaseApiController
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        //private readonly IUserInfo _userInfo = userInfo;

        [Tags("profiles")]
        [HttpGet("profiles/{userEnum}/{id}")]
        public async Task<IActionResult> Profile(UserEnum userEnum, string id, CancellationToken ct)
        {
            var profileService = _serviceProvider.GetRequiredKeyedService<IProfileService>(userEnum);
            GetOtherProfileResponse response;

            try
            {
                response = await profileService.GetOtherProfileAsync(id,ct);
                LoadFiles(id, userEnum, response);
            }
            catch (ArgumentException)
            {
                return NotFound(_response.FailureResponse("Invalid Data!"));
            }

            return Ok(_response.SuccessResponse(response));
        }

        private void LoadFiles(string id, UserEnum userEnum, GetOtherProfileResponse response)
        {
            response.Avatar = Url.Action(nameof(GetFileEndpoint.File), nameof(GetFileEndpoint), new { userEnum, id, fileType = FileTypes.Avatar }, Request.Scheme);
            response.Header = Url.Action(nameof(GetFileEndpoint.File), nameof(GetFileEndpoint), new { userEnum, id, fileType = FileTypes.Header }, Request.Scheme);

            if (response.OtherStudentInfo != null)
                response.OtherStudentInfo!.CV = Url.Action(nameof(GetFileEndpoint.File), nameof(GetFileEndpoint), new { userEnum, id, fileType = FileTypes.CV }, Request.Scheme);
        }
    }
}
