using skillsync.api.Dtos;

namespace skillsync.api.Services
{
    public interface IPersonalInfoService
    {
        Task<PersonalInfoDto?> GetUserInfoAsync(int userId);
        Task<UpdatePersonalInfoResponseDto> UpdateUserInfoAsync(int userId, UpdatePersonalInfoDto updateData);
    }
}