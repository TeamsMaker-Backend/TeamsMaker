﻿namespace TeamsMaker.Api.Contracts.Requests.JoinRequest
{
    public class AddJoinRequest
    {
        public string StudentId { get; set; } = null!;
        public Guid CircleId { get; set; }
        public string EntityType { get; set; } = null!;
    }
}
