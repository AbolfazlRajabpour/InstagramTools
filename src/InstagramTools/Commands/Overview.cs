using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaSharper.API;
using InstaSharper.Classes;

namespace InstagramTools.Commands
{
    public class Overview : Command
    {
        public Overview(IInstaApi instaApi) : base(instaApi)
        {

        }

        public override async Task Do()
        {
            // get currently logged in user
            var currentUser = await InstaApi.GetCurrentUserAsync();
            Console.WriteLine($"Logged in: username - {currentUser.Value.UserName}, full name - {currentUser.Value.FullName}");

            // get self folling
            var following = await InstaApi.GetUserFollowingAsync(currentUser.Value.UserName, PaginationParameters.MaxPagesToLoad(5));
            var followers = await InstaApi.GetUserFollowersAsync(currentUser.Value.UserName, PaginationParameters.MaxPagesToLoad(5));
            Console.WriteLine($"Count of following {following.Value.Count}");
            Console.WriteLine($"Count of followers {followers.Value.Count}");

            // get self user's media, latest 5 pages
            var currentUserMedia = await InstaApi.GetUserMediaAsync(currentUser.Value.UserName, PaginationParameters.MaxPagesToLoad(5));
            if (currentUserMedia.Succeeded)
            {
                Console.WriteLine($"Media count : {currentUserMedia.Value.Count}");
                foreach (var media in currentUserMedia.Value)
                    Console.WriteLine($"{media.Code} likes:{media.LikesCount:D4} comments:{media.CommentsCount:D4} views:{media.ViewCount:D4}");
            }
        }
    }
}
