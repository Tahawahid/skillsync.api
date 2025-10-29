using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using skillsync.api.Dtos;
using skillsync.api.Services;

namespace skillsync.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OnboardingController : ControllerBase
    {
        private readonly IOnboardingService _onboardingService;

        public OnboardingController(IOnboardingService onboardingService)
        {
            _onboardingService = onboardingService;
        }

        [HttpPost("complete")]
        public async Task<ActionResult<OnboardingResponseDto>> CompleteOnboarding([FromBody] OnboardingCompleteDto onboardingData)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _onboardingService.CompleteOnboardingAsync(userId.Value, onboardingData);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("data")]
        public async Task<ActionResult<OnboardingCompleteDto>> GetOnboardingData()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var data = await _onboardingService.GetOnboardingDataAsync(userId.Value);

            if (data == null)
            {
                return NotFound("Onboarding data not found");
            }

            return Ok(data);
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }
}