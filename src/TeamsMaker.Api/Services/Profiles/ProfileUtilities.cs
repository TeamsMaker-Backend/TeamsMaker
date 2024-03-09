﻿using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Storage.Interfacecs;
using TeamsMaker.Core.Enums;
namespace TeamsMaker.Api.Services.Profiles;

public class ProfileUtilities // [Refactor] remove dublicates - Urgent
    (AppDBContext db, IWebHostEnvironment host, IUserInfo userInfo, IStorageService storageService, IServiceProvider serviceProvider)
{
    private readonly AppDBContext _db = db;
    private readonly IWebHostEnvironment _host = host;
    private readonly IUserInfo _userInfo = userInfo;
    private readonly IStorageService _storageService = storageService;
    private readonly IServiceProvider _serviceProvider = serviceProvider;


    public void GetUserData(User user, GetProfileResponse response)
    {
        response.Id = user.Id;
        response.FirstName = user.FirstName;
        response.LastName = user.LastName;
        response.SSN = user.SSN;
        response.Email = user.Email!;
        response.Bio = user.Bio;
        response.About = user.About;
        response.Gender = user.Gender;
        response.City = user.City;
        response.EmailConfirmed = user.EmailConfirmed;
        response.Phone = user.PhoneNumber;
        response.OrganizationId = user.OrganizationId;
        response.Links = user.Links.Select(l => new LinkInfo { Url = l.Url, Type = l.Type }).ToList();
    }

    public void GetOtherUserData(User user, GetOtherProfileResponse response)
    {
        response.Id = user.Id;
        response.FirstName = user.FirstName;
        response.LastName = user.LastName;
        response.Email = user.Email!;
        response.Bio = user.Bio;
        response.About = user.About;
        response.Gender = user.Gender;
        response.City = user.City;
        response.Phone = user.PhoneNumber;
        response.Links = user.Links.Select(l => new LinkInfo { Url = l.Url, Type = l.Type }).ToList();
    }

    public void GetStudentData(Student student, GetProfileResponse response)
    {
        var fileService = _serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);

        var studentInfo = new StudentInfo
        {
            CollegeId = student.CollegeId,
            GPA = student.GPA,
            GraduationYear = student.GraduationYear,
            Level = student.Level,
            DepartmentName = student.Department?.Name,
            CV = fileService.GetFileUrl(student.Id, FileTypes.CV),

            Experiences =
                student.Experiences.Select(ex => new ExperienceInfo
                {
                    Id = ex.Id,
                    Organization = ex.Organization,
                    Role = ex.Role,
                    StartDate = ex.StartDate,
                    EndDate = ex.EndDate,
                    Description = ex.Description
                }).ToList(),

            Projects =
                student.Projects.Select(prj => new ProjectInfo
                {
                    Id = prj.Id,
                    Name = prj.Name,
                    Url = prj.Url,
                    Description = prj.Description,
                    Skills = prj.Skills.Select(s => s.Name).ToList()
                }).ToList()
        };

        response.StudentInfo = studentInfo;
        response.Avatar = fileService.GetFileUrl(student.Id, FileTypes.Avatar);
        response.Header = fileService.GetFileUrl(student.Id, FileTypes.Header);
    }

    public void GetOtherStudentData(Student student, GetOtherProfileResponse response)
    {
        var fileService = _serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);

        var otherStudentInfo = new OtherStudentInfo
        {
            GPA = student.GPA,
            GraduationYear = student.GraduationYear,
            Level = student.Level,
            DepartmentName = student.Department?.Name,
            CV = fileService.GetFileUrl(student.Id, FileTypes.CV),

            Experiences =
                student.Experiences.Select(ex => new ExperienceInfo
                {
                    Id = ex.Id,
                    Organization = ex.Organization,
                    Role = ex.Role,
                    StartDate = ex.StartDate,
                    EndDate = ex.EndDate,
                    Description = ex.Description
                }).ToList(),

            Projects =
                student.Projects.Select(prj => new ProjectInfo
                {
                    Id = prj.Id,
                    Name = prj.Name,
                    Url = prj.Url,
                    Description = prj.Description,
                    Skills = prj.Skills.Select(s => s.Name).ToList()
                }).ToList()
        };

        response.OtherStudentInfo = otherStudentInfo;
        response.Avatar = fileService.GetFileUrl(student.Id, FileTypes.Avatar);
        response.Header = fileService.GetFileUrl(student.Id, FileTypes.Header);
    }

    public void GetStaffData(Staff staff, GetProfileResponse response)
    {
        var fileService = _serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);

        var staffInfo = new StaffInfo
        {
            Classification = staff.Classification
        };

        response.StaffInfo = staffInfo;
        response.Avatar = fileService.GetFileUrl(staff.Id, FileTypes.Avatar);
        response.Header = fileService.GetFileUrl(staff.Id, FileTypes.Header);
    }

    public void GetOtherStaffData(Staff staff, GetOtherProfileResponse response)
    {
        var otherStaffInfo = new OtherStaffInfo
        {
            Classification = staff.Classification
        };
        response.OtherStaffInfo = otherStaffInfo;
    }

    public async void UpdateUserDataAsync(User user, UpdateProfileRequest request, string folder, CancellationToken ct)
    {
        var links = _db.Links.Where(l => l.UserId == _userInfo.UserId);
        _db.Links.RemoveRange(links);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Bio = request.Bio;
        user.About = request.About;
        user.Gender = request.Gender ?? GenderEnum.Unknown;
        user.City = request.City;
        user.PhoneNumber = request.Phone;
        user.Links = request.Links?.Select(l => new Link { UserId = user.Id, Url = l.Url, Type = l.Type }).ToList() ?? [];

        user.Avatar = await _storageService.UpdateFileAsync(user.Avatar?.Name, request.Avatar, CreateName(FileTypes.Avatar, request.Avatar?.FileName),
            Path.Combine(folder, user.Id), ct);

        user.Header = await _storageService.UpdateFileAsync(user.Header?.Name, request.Header, CreateName(FileTypes.Header, request.Header?.FileName),
            Path.Combine(folder, user.Id), ct);

        if (user is Student student)
            student.CV = await _storageService.UpdateFileAsync(student.CV?.Name, request.CV, CreateName(FileTypes.CV, request.CV?.FileName),
                Path.Combine(folder, student.Id), ct);
    }

    private static string CreateName(string fileType, string? file)
        => $"{fileType}{Path.GetExtension(file)}";

    private static string? GetBaseType(User user)
        => user is Student ? BaseTypes.Student : BaseTypes.Staff;
}