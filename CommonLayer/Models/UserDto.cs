namespace CommonLayer.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RoleId { get; set; }
        //public string Password { get; set; } // For Add
    }
}
