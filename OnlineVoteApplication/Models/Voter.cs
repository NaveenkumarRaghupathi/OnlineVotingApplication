﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OnlineVoteApplication.Models
{
    public partial class Voter
    {
        public int Id { get; set; }
        public string VoterName { get; set; }
        public int Age { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public bool? IsValidUser { get; set; }
        public bool? IsApprovedUser { get; set; }
        public string Password { get; set; }
        public string IDProofImage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}