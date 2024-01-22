namespace PuppyLearn.Models;

public partial class AccountType
{
    public int Id { get; set; }

    public string AccountName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
