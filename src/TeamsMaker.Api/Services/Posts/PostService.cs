using TeamsMaker.Api.Services.Posts.Interfaces;

namespace TeamsMaker.Api.Services.Posts;

public class PostService : IPostService
{

}


/*
Post Service

Post: add post -> Posts table && add author -> Authors
Update: patch
Delete: parent with childs Post&Comments  Comment&Replies
Get: circle, user, get feed   pagination order by desc (CreationDate & CreatedBy)

Get circle posts (circle id)
Get user posts (user id)
Get feed (split 2 tabs get all with pagination order by desc (CreationDate & CreatedBy))
Get Post (post id)
-----
Include Author data

*/