using Newtonsoft.Json;

namespace PinSharper.Models
{
    public class User
    {
        [JsonProperty("is_employee")]
        public bool IsEmployee { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("image_medium_url")]
        public string ImageMediumUrl { get; set; }

        [JsonProperty("image_xlarge_url")]
        public string ImageXlargeUrl { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("partner")]
        public object Partner { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("facebook_timeline_enabled")]
        public bool FacebookTimelineEnabled { get; set; }

        [JsonProperty("secret_board_count")]
        public int SecretBoardCount { get; set; }

        [JsonProperty("is_default_image")]
        public bool IsDefaultImage { get; set; }

        [JsonProperty("connected_to_gplus")]
        public bool ConnectedToGplus { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("image_large_url")]
        public string ImageLargeUrl { get; set; }

        [JsonProperty("board_count")]
        public int BoardCount { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("last_pin_like_time")]
        public object LastPinLikeTime { get; set; }

        [JsonProperty("facebook_publish_stream_enabled")]
        public bool FacebookPublishStreamEnabled { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("connected_to_facebook")]
        public bool ConnectedToFacebook { get; set; }

        [JsonProperty("like_count")]
        public int LikeCount { get; set; }
    }
}
