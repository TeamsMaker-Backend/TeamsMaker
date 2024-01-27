namespace TeamsMaker.Api.DataAccess.Models;

public class Staff : User
{
    public StaffClassificationsEnum Classification { get; set; }
}

//TODO: public int Creds { get; set; }
//TODO: Circles
//TODO: Applications