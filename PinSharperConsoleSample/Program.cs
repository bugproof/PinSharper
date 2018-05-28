using System;
using System.Threading.Tasks;

using PinSharper;
using System.Text;
using System.Net.Http;

namespace PinSharperConsoleSample
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var pinterestAPI = new PinterestAPI();
            var loginResult = await pinterestAPI.LoginAsync("email", "password");
            if(loginResult.Succeeded)
            {
                Console.WriteLine("Logged in!");
                var myInfo = await pinterestAPI.GetMyUserAsync();
                if (myInfo.Succeeded)
                {
                    Console.WriteLine(myInfo.Data.Email);
                    Console.WriteLine(myInfo.Data.Username);
                    Console.WriteLine(myInfo.Data.Gender);

                    var myBoardsResult = await pinterestAPI.GetUserBoardsAsync(myInfo.Data.Id);
                    if (myBoardsResult.Succeeded)
                    {
                        foreach (var board in myBoardsResult.Data)
                        {
                            Console.WriteLine($"Board name: {board.Name} (ID: {board.Id})");
                        }
                    }
                    //Console.WriteLine("Attempt at creating a pinboard...");
                    //var createResult = await pinterestAPI.CreateBoardAsync("My lovely pinboard", "myboard");
                    //if (createResult.Succeeded)
                    //{
                    //    Console.WriteLine($"Pinboard created (ID: {createResult.Data.id})");
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"{createResult.Message} {createResult.MessageDetail}");
                    //}

                    //var commentResult = await pinterestAPI.CreateCommentAsync("697283954777537460", "test comment");
                    //if (commentResult.Succeeded)
                    //{
                    //    Console.WriteLine("Comment created!");
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"{createResult.Message}");
                    //}

                    //Console.WriteLine("Attempt at uploading photo...");
                    ////697284023495495038 - myboard
                    //var uploadResult = await pinterestAPI.CreatePinPhotoAsync("", "697284023495495038", "hope.jpg");
                    //if (uploadResult.Succeeded)
                    //{
                    //    Console.WriteLine($"Photo uploaded (ID: {uploadResult.Data.id})");
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"Error uploading photo: {uploadResult.Message}");
                    //}
                }
                else
                {
                    Console.WriteLine(myInfo.Message);
                }
            }
            else
            {
                Console.WriteLine($"Could not log-in: {loginResult.Message}");
            }
            //var result = await pinterestAPI.AccountExistsAsync("email");
            //if (result.Data)
            //{
            //    Console.WriteLine("Email exists!");
            //}
            //else
            //{
            //    Console.WriteLine("Email doesn't exist!");
            //}
        }
    }
}