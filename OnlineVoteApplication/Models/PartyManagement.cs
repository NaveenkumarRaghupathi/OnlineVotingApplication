﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace OnlineVoteApplication.Models
{
    public partial class PartyManagement
    {
        public int Id { get; set; }
        public string PartyName { get; set; }
        public string Symbol { get; set; }
        public string PartyImage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}