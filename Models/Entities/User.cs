namespace WebServer.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public string? Email { get; set; }
        public UserStatus Status { get; set; }  // Options: Active, Blocked, Unverified
        public DateTime LastLoginTime { get; set; }
        public string? EmailConfirmationToken { get; set; }

    }
}
