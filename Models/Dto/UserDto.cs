namespace PuppyLearn.Models.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public DateTime SignUpTime { get; set; }

        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 1：普通用户；2：vip用户；3：teacher；4：admin;5：superAdmin
        /// </summary>
        public int AccountTypeId { get; set; }

        public bool IsSuspend { get; set; }

        public bool IsValid { get; set; }

        public string Email { get; set; } = null!;

        /// <summary>
        /// 上一次学习的BookId
        /// </summary>
        public int? LastLedBookId { get; set; }
    }
}
