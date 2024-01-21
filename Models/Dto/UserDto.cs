namespace PuppyLearn.Models.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public DateTime? SignUpTime { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public int AccountTypeId { get; set; }

        public bool IsSuspend { get; set; }

        public bool IsValid { get; set; }

        public string Email { get; set; } = null!;
    }
}
