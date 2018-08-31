using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaSharper.API;
using InstaSharper.Classes;

namespace InstagramTools.Commands
{
    public class UnFollowAll : Command
    {
        public UnFollowAll(IInstaApi instaApi) : base(instaApi)
        {

        }

        public override async Task Do()
        {
            // get currently logged in user
            var currentUser = await InstaApi.GetCurrentUserAsync();

            // get self folling
            var followings = await InstaApi.GetUserFollowingAsync(currentUser.Value.UserName, PaginationParameters.MaxPagesToLoad(100));
            var countAll = followings.Value.Count;
            var current = 0;
            foreach (var following in followings.Value)
            {
                var userId = following.Pk;
                try
                {
                    var status = await InstaApi.UnFollowUserAsync(userId);
                    current++;
                    if (status.Succeeded)
                    {
                        Console.WriteLine($"{current * 100 / countAll } unfollow {DateTime.Now.ToShortTimeString()} {following.UserName}");
                    }
                    else
                    {
                        Console.WriteLine($"{current * 100 / countAll } error in unfollow {DateTime.Now.ToShortTimeString()} {following.UserName}");
                    }
                }
                catch (System.Exception)
                {
                        Console.WriteLine($"{current * 100 / countAll } fatal error in unfollow {DateTime.Now.ToShortTimeString()} {following.UserName}");
                }

            }
        }
    }
}
