namespace TeamsMaker.Api.DataAccess.Models;

public class Staff : User
{
    public StaffClassificationsEnum Classification { get; set; }
    public virtual ICollection<DepartmentStaff> DepartmentStaff { get; set; } = [];

    //public static Staff Create(string firstName, string lastName, string email, string userName)
    //{
    //    return new Staff
    //    {
    //        FirstName = firstName,
    //        LastName = lastName,
    //        Email = email,
    //        UserName = userName
    //    };
    //}

    //public Staff WithClassification(StaffClassificationsEnum classification)
    //{
    //    Classification = classification;

    //    return this;
    //}
}

//TODO: public int Creds { get; set; }
//TODO: Circles
//TODO: Applications