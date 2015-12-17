using System;
using System.Collections.Generic;

namespace Nirvana.Models
{
    public interface INirvanaRepository
    {
        int AddLikePts(RandomActsModel act, ApplicationUser who_liked);
        RandomActsModel CreateAct(string ActTitle, string ActDescription, DateTime ActDate, ApplicationUser owner);
        bool CreateComment(Comment comm_2_add, int ActId);
        Likes CreateLike(RandomActsModel act, ApplicationUser UserWhoLiked);
        bool DeleteComment(int ActId, int comment_id);
        bool DeleteLike(int like_id);
        int GetActCount();
        int GetActCount(ApplicationUser user);
        List<RandomActsModel> GetAllActs();
        List<RandomActsModel> GetAllActs(ApplicationUser _user);
        List<Comment> GetAllComments();
        List<Comment> GetAllComments(int ActId);
        Dictionary<string, int> GetAllUsersRanks();
        int GetLikeCount(int act_id);
        int GetTotalPoints(ApplicationUser user1);
        Rank GetUserRank(ApplicationUser user);
        List<string> SearchActs(string description);
        bool UpdateComment(int comment_id, string new_text);
    }
}