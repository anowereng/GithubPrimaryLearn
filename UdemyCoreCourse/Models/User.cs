namespace UdemyCoreCourse.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}