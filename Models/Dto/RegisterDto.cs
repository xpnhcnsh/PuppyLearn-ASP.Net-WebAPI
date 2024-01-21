namespace PuppyLearn.Models.Dto
{
    public class RegisterDto
    {
        public string UserName { get; set; } = null!;

        public int AccountTypeId { get; set; }

        public bool IsSuspend { get; set; } = false;

        public bool IsValid { get; set; } = true;

        public string Email { get; set; } = null!;

        public string Password { get; set; }
    }

}
