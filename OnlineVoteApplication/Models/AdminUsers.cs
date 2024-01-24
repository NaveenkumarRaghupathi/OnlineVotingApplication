namespace OnlineVoteApplication.Models
{
    public class AdminUsers
    {
        public int Id { get; set; }
        public string AdminName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Password { get; set; }

    }
}
