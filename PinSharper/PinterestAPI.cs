using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PinSharper.Models;

namespace PinSharper
{
    public class PinterestAPI : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClientHandler _httpClientHandler;

        public PinterestAPI()
        {
            _httpClientHandler = new PinterestHttpHandler()
            {
                CookieContainer = new CookieContainer(),
                AutomaticDecompression = DecompressionMethods.GZip
            };

            _httpClient = new HttpClient(_httpClientHandler)
            {
                BaseAddress = new Uri("https://api.pinterest.com/v3/")
            };

            _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en_US");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", PinterestConstants.USER_AGENT);            

            _httpClient.DefaultRequestHeaders.Add("Pinterest-App-Info", "version=6.17.0;build=617040;environment=Release");
            _httpClient.DefaultRequestHeaders.Add("X-Pinterest-AppState", "active");
            _httpClient.DefaultRequestHeaders.Add("X-Pinterest-App-Type", "3");
            _httpClient.DefaultRequestHeaders.Add("X-Pinterest-Device", "SCH-I829");
            _httpClient.DefaultRequestHeaders.Add("X-Pinterest-Device-HardwareId", "a7919442c027368");
            _httpClient.DefaultRequestHeaders.Add("X-Pinterest-InstallId", PinterestHelpers.GenerateInstallId());
            
            _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            _httpClient.DefaultRequestHeaders.Add("Host", "api.pinterest.com");
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _httpClientHandler.Dispose();
        }
        
        /// <summary>
        /// Check if the specified account exists
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<IResult<bool>> AccountExistsAsync(string email)
        {
            var request = new PinterestRequestMessage(HttpMethod.Get, $"register/exists/?email={email}", isSigned: true);
            var response = await _httpClient.SendAsync(request);
            return JsonConvert.DeserializeObject<Result<bool>>(await response.Content.ReadAsStringAsync(), new ResultConverter<bool>());
        }

        /// <summary>
        /// Register an account
        /// </summary>
        /// <param name="signUp">Sign up parameters</param>
        /// <returns>Token string used by Authorization header e.g.: "Authorization: Bearer MTQzMTYwMjo2OTcyODQwOTIyMTQ5Mzk0NTM6OTIyMzM3MjAzNjg1NDc3NTgwNzoxfDE0OTQ3OTg0NjA6MC0tZDk0N2E4ZjBlODgzNDY2MmZkNDM2NDAzMjM1MDU1MjU=" </returns>
        public async Task<IResult<string>> RegisterAsync(SignUpParams signUp)
        {
            var data = new Dictionary<string, string>()
            {
                {"first_name", signUp.FirstName},
                {"username", signUp.Username},
                {"gender", signUp.Gender},
                {"last_name", signUp.Gender},
                {"email", signUp.Email},
                {"locale", signUp.Locale},
                {"password", signUp.Password}
            };
            var request = new PinterestRequestMessage(HttpMethod.Post, "register/email/", isSigned: true);
            request.Content = new FormUrlEncodedContent(data);
            var response = await _httpClient.SendAsync(request);
            var result = JsonConvert.DeserializeObject<Result<string>>(await response.Content.ReadAsStringAsync(), new ResultConverter<string>());
            if (result.Succeeded)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Data);
            }
            return result;
        }

        /// <summary>
        /// Logs you in
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>access_token and token_type</returns>
        public async Task<IResult<dynamic>> LoginAsync(string usernameOrEmail, string password)
        {
            var pinterestRequest = new PinterestRequestMessage(HttpMethod.Post, "login/", isSigned: true);
            var data = new Dictionary<string, string>()
            {
                {"password", password},
                {"username_or_email", usernameOrEmail}
            };
            pinterestRequest.Content = new FormUrlEncodedContent(data);
            var response = await _httpClient.SendAsync(pinterestRequest);
            var result = JsonConvert.DeserializeObject<Result<dynamic>>(await response.Content.ReadAsStringAsync(), new ResultConverter<dynamic>());
            if (result.Succeeded)
            {
                Console.WriteLine($"ACCESS_TOKEN: { result.Data.access_token}");
                Console.WriteLine($"TOKEN_TYPE: { result.Data.token_type}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)result.Data.access_token);
            }
            return result;
        }

        /// <summary>
        /// Logs you out
        /// </summary>
        /// <returns></returns>
        public async Task<IResult<dynamic>> LogoutAsync()
        {
            var request = new PinterestRequestMessage(HttpMethod.Put, "logout/");
            var response = await _httpClient.SendAsync(request);
            return JsonConvert.DeserializeObject<Result<dynamic>>(await response.Content.ReadAsStringAsync(), new ResultConverter<dynamic>());
        }

        /// <summary>
        /// Creates board where you can add pins
        /// </summary>
        /// <param name="description">Board description (Can be null)</param>
        /// <param name="name">Board name</param>
        /// <param name="privacy">Visibility of the board [public, secret]</param>
        public async Task<IResult<dynamic>> CreateBoardAsync(string description, string name, string privacy = "public")
        {
            var request = new PinterestRequestMessage(HttpMethod.Put, "boards/");
            var content = new Dictionary<string, string>()
            {
                {"description", description},
                {"name", name},
                {"privacy", privacy},
                {"collaborator_invites_enabled", "true"}
            };
            request.Content = new FormUrlEncodedContent(content);
            var response = await _httpClient.SendAsync(request);
            return JsonConvert.DeserializeObject<Result<dynamic>>(await response.Content.ReadAsStringAsync(), new ResultConverter<dynamic>());
        }

        /// <summary>
        /// Uploads a photo to a pinboard
        /// </summary>
        /// <param name="description"></param>
        /// <param name="boardId"></param>
        /// <param name="photoUrl"></param>
        /// <returns></returns>
        public async Task<IResult<dynamic>> CreatePinPhotoAsync(string description, string boardId, byte[] photo)
        {
            var request = new PinterestRequestMessage(HttpMethod.Put, "pins/");
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(description), "\"description\"");
            formData.Add(new StringContent("photos"), "\"method\"");
            formData.Add(new StringContent(boardId), "\"board_id\"");
            formData.Add(new StringContent("pin.images[236x,736x,136x136]"), "\"add_fields\"");
            formData.Add(new StringContent("0"), "\"share_twitter\"");
            var imageContent = new ByteArrayContent(photo);
            imageContent.Headers.Add("Content-Transfer-Encoding", "binary");
            formData.Add(imageContent, "image", $"myphoto.jpg");
            request.Content = formData;
            var response = await _httpClient.SendAsync(request);
            return JsonConvert.DeserializeObject<Result<dynamic>>(await response.Content.ReadAsStringAsync(), new ResultConverter<dynamic>());
        }

        /// <summary>
        /// Creates a comment under a pin
        /// </summary>
        /// <param name="pinId">Pin id to post comment under</param>
        /// <param name="comment">Comment text message</param>
        public async Task<IResult<dynamic>> CreateCommentAsync(string pinId, string comment)
        {
            var request = new PinterestRequestMessage(HttpMethod.Post, $"pins/{pinId}/comment/");
            var data = new Dictionary<string, string>()
            {
                {"text", comment}
            };
            request.Content = new FormUrlEncodedContent(data);
            var response = await _httpClient.SendAsync(request);
            return JsonConvert.DeserializeObject<Result<dynamic>>(await response.Content.ReadAsStringAsync(), new ResultConverter<dynamic>());
        }

        /// <summary>
        /// Get boards of the specified user
        /// </summary>
        /// <param name="userId">User id to get boards from</param>
        public async Task<IResult<List<Board>>> GetUserBoardsAsync(string userId)
        {
            var request = new PinterestRequestMessage(HttpMethod.Get, $"users/{userId}/boards/feed/?sort=last_pinned_to&filter=all&fields={Uri.EscapeDataString("pin.images[236x],board.images[236x],board.conversation(),board.followed_by_me,board.created_at,board.image_cover_hd_url,board.board_order_modified_at,board.collaborated_by_me,board.privacy,board.collaborating_users(),board.is_collaborative,board.collaborator_invites_enabled,board.url,conversation.id,board.owner(),board.category,board.has_custom_cover,conversation.unread,board.pin_count,board.id,board.name,board.layout")}");
            var response = await _httpClient.SendAsync(request);
            return JsonConvert.DeserializeObject<Result<List<Board>>>(await response.Content.ReadAsStringAsync(), new ResultConverter<List<Board>>());
        }

        public async Task<IResult<User>> GetMyUserAsync()
        {
            var fields = "user.connected_to_facebook,user.image_xlarge_url,user.like_count,user.partner(),user.gender,user.last_name,user.facebook_timeline_enabled,user.connected_to_gplus,user.last_pin_like_time,user.full_name,user.is_default_image,user.username,user.board_count,user.facebook_publish_stream_enabled,user.created_at,user.email,user.image_medium_url,user.first_name,user.secret_board_count,user.experiments,user.id,user.is_employee,user.image_large_url,user.gatekeeper_experiments";
            var request = new PinterestRequestMessage(HttpMethod.Get, $"users/me/?fields={fields}");
            var response = await _httpClient.SendAsync(request);
            return JsonConvert.DeserializeObject<Result<User>>(await response.Content.ReadAsStringAsync(), new ResultConverter<User>());
        }
    }
}
