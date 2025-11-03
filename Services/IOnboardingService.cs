using skillsync.api.Dtos;

namespace skillsync.api.Services
{
    public interface IOnboardingService
    {
        Task<OnboardingResponseDto> CompleteOnboardingAsync(int userId, OnboardingCompleteDto onboardingData);
        Task<OnboardingCompleteDto?> GetOnboardingDataAsync(int userId);
    }
}