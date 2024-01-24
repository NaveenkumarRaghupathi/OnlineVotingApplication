namespace OnlineVoteApplication.Models
{
    public class VotingSystem
    {
        public int Id { get; set; }
        public string ContestentName { get; set; }
        public string ContestentEmail { get; set; }
        public string ContestentImage { get; set; }
        public string VoterName { get; set; }
        public string VoterEmail { get; set; }
        public string PartyName { get; set; }
        public string Symbol { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
