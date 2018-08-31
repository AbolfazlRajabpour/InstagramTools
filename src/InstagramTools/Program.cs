using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InstagramTools.Commands;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Logger;

namespace InstagramTools
{
    class Program
    {
        /// <summary>
        ///     Api instance (one instance per Instagram user)
        /// </summary>
        private static IInstaApi _instaApi;

        private static void Main(string[] args)
        {
            MainAsync().Wait();
            // var result = Task.Run(MainAsync).GetAwaiter().GetResult();
            // if (result)
            //     return;
            // Console.ReadKey();
        }

        public static async Task<bool> MainAsync()
        {
            try
            {
                Console.WriteLine("Starting project");
                // create user session data and provide login details

                Console.Write("please Enter UserName:");
                var userName = Console.ReadLine();

                Console.Write("please Enter Password:");
                var password = Console.ReadLine();
              
                var userSession = new UserSessionData
                {
                    UserName = userName,
                    Password = password
                };

                // create new InstaApi instance using Builder
                _instaApi = InstaApiBuilder.CreateBuilder()
                    .SetUser(userSession)
                    .UseLogger(new DebugLogger(LogLevel.Exceptions)) // use logger for requests and debug messages
                    .SetRequestDelay(TimeSpan.FromSeconds(2))
                    .Build();

                //// create account
                //var username = "kajokoleha";
                //var password = "ramtinjokar";
                //var email = "ramtinak@live.com";
                //var firstName = "Ramtin";
                //var accountCreation = await _instaApi.CreateNewAccount(username, password, email, firstName);

                //const string stateFile = "state.bin";
                //try
                //{
                //    if (File.Exists(stateFile))
                //    {
                //        Console.WriteLine("Loading state from file");
                //        using (var fs = File.OpenRead(stateFile))
                //        {
                //            _instaApi.LoadStateDataFromStream(fs);
                //        }
                //    }
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e);
                //}

                if (!_instaApi.IsUserAuthenticated)
                {
                    // login
                    Console.WriteLine($"Logging in as {userSession.UserName}");
                    //delay.Disable();
                    var logInResult = await _instaApi.LoginAsync();
                    //delay.Enable();
                    if (!logInResult.Succeeded)
                    {
                        Console.WriteLine($"Unable to login: {logInResult.Info.Message}");
                        return false;
                    }
                }

                var state = _instaApi.GetStateDataAsStream();
                //using (var fileStream = File.Create(stateFile))
                //{
                //    state.Seek(0, SeekOrigin.Begin);
                //    state.CopyTo(fileStream);
                //}

                var commands = new Dictionary<ConsoleKey, ICommand>
                {
                    [ConsoleKey.D1] = new Overview(_instaApi),
                    [ConsoleKey.D2] = new UnFollowNonFollower(_instaApi),
                    [ConsoleKey.D3] = new UnFollowAll(_instaApi),
                };


                ConsoleKeyInfo key;
                do
                {
                    Console.WriteLine("Press 1 to Show User Overview");
                    Console.WriteLine("Press 2 to UnFollow NonFollower");
                    Console.WriteLine("Press 3 to UnFollow All");
                    Console.WriteLine("Press esc key to exit");

                    key = Console.ReadKey();
                    Console.WriteLine(Environment.NewLine);
                    if (commands.ContainsKey(key.Key))
                        await commands[key.Key].Do();

                } while (key.Key != ConsoleKey.Escape);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                // perform that if user needs to logged out
                // var logoutResult = Task.Run(() => _instaApi.LogoutAsync()).GetAwaiter().GetResult();
                // if (logoutResult.Succeeded) Console.WriteLine("Logout succeed");
            }

            return false;
        }
    }
}
