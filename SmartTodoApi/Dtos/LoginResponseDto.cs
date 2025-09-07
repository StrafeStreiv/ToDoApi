namespace SmartTodoApi.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserProfileDto User { get; set; } = null!;
        public DateTime Expiry { get; set; }
    }
}