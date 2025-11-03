using Microsoft.AspNetCore.Mvc;
using skillsync.api.Dtos;
using skillsync.api.Services;

namespace skillsync.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalInfoController : ControllerBase
    {
        private readonly IPersonalInfoService _personalInfoService;
        private readonly ILogger<PersonalInfoController> _logger;

        public PersonalInfoController(IPersonalInfoService personalInfoService, ILogger<PersonalInfoController> logger)
        {
            _personalInfoService = personalInfoService;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<PersonalInfoDto>> GetUserInfo(int userId)
        {
            _logger.LogInformation("Getting personal info for user {UserId}", userId);

            if (userId <= 0)
            {
                return BadRequest("Valid user ID is required");
            }

            try
            {
                var userInfo = await _personalInfoService.GetUserInfoAsync(userId);

                if (userInfo == null)
                {
                    _logger.LogInformation("No personal info found for user {UserId}", userId);
                    return NotFound("User information not found");
                }

                _logger.LogInformation("Personal info retrieved successfully for user {UserId}", userId);
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while getting personal info for user {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving user information");
            }
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<UpdatePersonalInfoResponseDto>> UpdateUserInfo(int userId, [FromBody] UpdatePersonalInfoDto updateData)
        {
            _logger.LogInformation("Updating personal info for user {UserId}", userId);
            _logger.LogInformation("Update data: {@UpdateData}", updateData);

            if (userId <= 0)
            {
                return BadRequest("Valid user ID is required");
            }

            if (updateData == null)
            {
                return BadRequest("Update data is required");
            }

            try
            {
                var result = await _personalInfoService.UpdateUserInfoAsync(userId, updateData);

                if (!result.Success)
                {
                    _logger.LogWarning("Personal info update failed for user {UserId}: {Message}", userId, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Personal info updated successfully for user {UserId}", userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while updating personal info for user {UserId}", userId);
                return StatusCode(500, new UpdatePersonalInfoResponseDto
                {
                    Success = false,
                    Message = "An error occurred while updating user information"
                });
            }
        }
    }
}