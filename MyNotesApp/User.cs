namespace MyNotesApp
{
    public class User
    {
        public int id { get; set; }
        public string UserName { get; set; }

        public string UserPassword { get; set; }
        public string Role { get; set; }
        public string RefreshToken { get; set; } = String.Empty;

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }
    }
}
