﻿namespace TeamsMaker.Api.Contracts.Requests.Proposal;


//TODO: Patch -> add default value
public class UpdateProposalRequest
{
    public string? Overview { get; init; }
    public string? Objectives { get; init; }
    public string? TechStack { get; init; }
}
