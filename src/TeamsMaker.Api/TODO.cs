/**Tasks

    * *Task#1: 
        * TODO: sreach about record class
        * ? done
    
    * *Task#2:
        * TODO: handle connection string
        * ? done
    
    * *Task#3
        * TODO: add new entities to erd diagram
        * ?  waiting
        
    * *Task#4
        * TODO: study the following courses
            1- Rest API Authorization with JWT (Roles Vs Claims Vs Policy) - Step by Step https://youtu.be/eVxzuOxWEiY?si=xiWBfE5MixFgNIPe
            2- Web API Create JSON Web Tokens (JWT) - User Registration / Login / Authentication https://youtu.be/Y-MjCw6thao?si=GyaA7D8cMGyxY8vO
            3- Build CRUD with .NET 6 Web API & Entity Framework Core https://youtu.be/wtFs4356xp4?si=71oLpOH2jJWmHyeY
            4- .NET 6 Web API Create Refresh Tokens - JSON Web Tokens (JWT) https://www.youtube.com/watch?v=2_H0Zj-C8EM
            5- .NET 8 .🚀🔥: Understanding CORS with ASP.NET https://youtu.be/WAKsZwzXhf4?si=Rmywucg_N3kbOq4W
            6- 
        * ? done

    * *Task#5
        * TODO: Add Organization service and complete seeding functions
        * ?  waiting
    
    * *Task#5
        * TODO: Add Organization service and complete seeding functions
        * ?  waiting
*/


/*
Profile
0- seeding
1- Add avatar/header
2- Add Bio/About
3- Add links
4- Add CV
5- Add Gender/City

Post
Put
Get User Profile


circle 


Admin panel
Organization -> Get organziaiton info/ Add organization info/ Update organization info
User -> Get all users/ Add user info/ Update user info "change role"/ deactivate user
Deprtment -> Get all departments/ Add department info/ Update department info
*/

/*
url
request
response
*/

/*************************
[Post] -> api/users/login

body -> 
{
    email: string,
    password: string
}

return ->
{
    token: string,
    refreshToken: string
}
****************************/

/*****************************
[Post] -> api/users/register

body ->
{
    userType: number,
    firstName: string,
    lastName: string,
    userName: string,
    email: string,
    password: string,
    ssn: string
}

return ->
{
    token: string,
    refreshToken: string
}

UserTypeEnum
{
    Student = 1,
    Staff = 2   
}
********************************/

/*************************
[Post] -> api/users/verify

body -> 
{
    ssn: string,
    collegeId: string | null,
    userType: number
}

return -> true or false

UserTypeEnum
{
    Student = 1,
    Staff = 2   
}
****************************/


/*****************************
[Post] -> api/users/refresh_token

body ->
{
    token: string,
    refreshToken: string
}

return ->
{
    token: string,
    refreshToken: string
}
********************************/