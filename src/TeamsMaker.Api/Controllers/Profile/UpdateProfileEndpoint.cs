using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.ProfileService.Interface;

namespace TeamsMaker.Api.Controllers.Profile
{
    public class UpdateProfileEndpoint : BaseApiController
    {
        private readonly IProfileService _profileService;

        public UpdateProfileEndpoint(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPut("profiles/{id}")]
        public async Task<IActionResult> Profile(Guid id, [FromForm] UpdateProfileRequest request, CancellationToken ct)
        {
            try
            {
                await _profileService.UpdateProfileAsync(id, request, ct);
            }
            catch (ArgumentException)
            {
                return NotFound(_response.FailureResponse("Invalid Data"));
            }
            return Ok(_response.SuccessResponse(null));
        }
    }
}
