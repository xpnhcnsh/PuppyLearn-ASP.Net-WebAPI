namespace PuppyLearn.Models.Dto
{
    public class LoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; } = null!;

        public string Email { get; set; }
    }
}
