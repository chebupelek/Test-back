using System.Linq;
using System.Collections.Generic;
using System;

public class UserComments
{
    public string user { get; set; }
    public int comments { get; set; }
}

public class OrderedPosts
{
    public string post { get; set; }
    public DateTime lastCommentTime { get; set; }
    public string lastComment { get; set; }
}

public static class BlogService
{
    public static List<UserComments> NumberOfCommentsPerUser(MyDbContext context)
    {
        var comment = context.BlogComments
             .GroupBy(x => x.UserName)
             .Select(x => new UserComments
                { 
                    user = x.Key, 
                    comments = x.Count() 
                })
             .ToList();

        return comment;
    }

    public static List<OrderedPosts> PostsOrderedByLastCommentDate(MyDbContext context)
    {
        var posts = context.BlogPosts
            .Select(x => new OrderedPosts
            {
                post = x.Title,
                lastCommentTime = x.Comments.OrderByDescending(comment => comment.CreatedDate).FirstOrDefault().CreatedDate,
                lastComment = x.Comments.OrderByDescending(comment => comment.CreatedDate).FirstOrDefault().Text
            })
            .OrderByDescending(x => x.lastCommentTime)
            .ToList();

        return posts;
    }

    public static List<UserComments> NumberOfLastCommentsLeftByUser(MyDbContext context)
    {
        var usersLastComments = context.BlogPosts
            .Select(x => x.Comments.OrderByDescending(comment => comment.CreatedDate).FirstOrDefault())
            .GroupBy(x => x.UserName)
            .Select(x => new UserComments
            { 
                user = x.Key, 
                comments = x.Count() 
            })
            .ToList();

        return usersLastComments;
    }
}