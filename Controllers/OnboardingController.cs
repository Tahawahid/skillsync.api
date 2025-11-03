using Microsoft.AspNetCore.Mvc;
using skillsync.api.Dtos;
using skillsync.api.Services;

namespace skillsync.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OnboardingController : ControllerBase
    {
        private readonly IOnboardingService _onboardingService;
        private readonly ILogger<OnboardingController> _logger;

        public OnboardingController(IOnboardingService onboardingService, ILogger<OnboardingController> logger)
        {
            _onboardingService = onboardingService;
            _logger = logger;
        }

        [HttpPost("complete")]
        public async Task<ActionResult<OnboardingResponseDto>> CompleteOnboarding([FromBody] OnboardingCompleteDto onboardingData)
        {
            _logger.LogInformation("CompleteOnboarding endpoint called");
            _logger.LogInformation("Received onboarding data for user ID: {UserId}", onboardingData.UserId);

            if (onboardingData.UserId <= 0)
            {
                _logger.LogWarning("Invalid user ID in request: {UserId}", onboardingData.UserId);
                return BadRequest("Valid user ID is required");
            }

            try
            {
                var result = await _onboardingService.CompleteOnboardingAsync(onboardingData.UserId, onboardingData);

                if (!result.Success)
                {
                    _logger.LogWarning("Onboarding service returned failure: {Message}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Onboarding completed successfully for user {UserId}", onboardingData.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during onboarding completion for user {UserId}", onboardingData.UserId);
                return StatusCode(500, new OnboardingResponseDto
                {
                    Success = false,
                    Message = "An error occurred while completing onboarding"
                });
            }
        }

        [HttpGet("data/{userId}")]
        public async Task<ActionResult<OnboardingCompleteDto>> GetOnboardingData(int userId)
        {
            _logger.LogInformation("GetOnboardingData endpoint called for user {UserId}", userId);

            if (userId <= 0)
            {
                return BadRequest("Valid user ID is required");
            }

            try
            {
                var data = await _onboardingService.GetOnboardingDataAsync(userId);

                if (data == null)
                {
                    _logger.LogInformation("No onboarding data found for user {UserId}", userId);
                    return NotFound("Onboarding data not found");
                }

                _logger.LogInformation("Onboarding data retrieved successfully for user {UserId}", userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while getting onboarding data for user {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving onboarding data");
            }
        }
    }
}