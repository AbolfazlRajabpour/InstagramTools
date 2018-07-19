using System;
using System.Threading.Tasks;
using InstagramTools.Utils;
using InstaSharper.API;
using InstaSharper.Classes.Models;

namespace InstagramTools
{
    public abstract class Command : ICommand
    {
        protected readonly IInstaApi InstaApi;
        /// <summary>
        ///     Config values
        /// </summary>
        protected static readonly int MaxDescriptionLength = 20;

        protected Command(IInstaApi instaApi)
        {
            InstaApi = instaApi;
        }

        public abstract Task Do();

        protected void PrintMedia(string header, InstaMedia media, int maxDescriptionLength)
        {
            Console.WriteLine($"{header} [{media.User.UserName}]: {media.Caption?.Text.Truncate(maxDescriptionLength)}, {media.Code}, likes: {media.LikesCount}, multipost: {media.IsMultiPost}");
        }
    }
}