using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaSharper.API;
using InstaSharper.Classes;

namespace InstagramTools.Commands
{
    public class UnFollowNonFollower : Command
    {
        public UnFollowNonFollower(IInstaApi instaApi) : base(instaApi)
        {

        }

        public override async Task Do()
        {
            // get currently logged in user
            var currentUser = await InstaApi.GetCurrentUserAsync();

            // get self folling
            var following = await InstaApi.GetUserFollowingAsync(currentUser.Value.UserName, PaginationParameters.MaxPagesToLoad(100));
            var followers = await InstaApi.GetUserFollowersAsync(currentUser.Value.UserName, PaginationParameters.MaxPagesToLoad(100));
            Console.WriteLine($"Count of following {following.Value.Count}");
            Console.WriteLine($"Count of followers {followers.Value.Count}");

            var followersId = followers.Value.ToList().Select(s => s.Pk);
            var followingsId = following.Value.ToList().Select(s => s.Pk);
            var nonFollowerIds = followingsId.Except(followersId);

            foreach (var nonFollowerId in nonFollowerIds)
            {
                var profile = following.Value.First(f => f.Pk == nonFollowerId);
                // double check. sometime nonfollower list is wrong!
                var friendshipStatus = await InstaApi.GetFriendshipStatusAsync(nonFollowerId);
                if (friendshipStatus.Value.FollowedBy == false)
                {
                    var status = await InstaApi.UnFollowUserAsync(nonFollowerId);
                    Console.WriteLine($"{profile.UserName} {profile.FullName} unfollow");
                }
            }
        }
    }
}
