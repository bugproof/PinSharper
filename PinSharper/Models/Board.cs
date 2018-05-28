using Newtonsoft.Json;
using System.Collections.Generic;

namespace PinSharper.Models
{
    public class Image
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("dominant_color")]
        public string DominantColor { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }

    public class Images
    {
        [JsonProperty("236x")]
        public IList<Image> List { get; set; }
    }

    public class Owner
    {

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("image_medium_url")]
        public string ImageMediumUrl { get; set; }

        [JsonProperty("image_xlarge_url")]
        public string ImageXlargeUrl { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("image_small_url")]
        public string ImageSmallUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("image_large_url")]
        public string ImageLargeUrl { get; set; }
    }

    public class Board
    {
        [JsonProperty("category")]
        public object Category { get; set; }

        [JsonProperty("image_cover_hd_url")]
        public string ImageCoverHdUrl { get; set; }

        [JsonProperty("followed_by_me")]
        public bool FollowedByMe { get; set; }

        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("collaborator_invites_enabled")]
        public bool CollaboratorInvitesEnabled { get; set; }

        [JsonProperty("privacy")]
        public string Privacy { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("collaborating_users")]
        public IList<object> CollaboratingUsers { get; set; }

        [JsonProperty("is_collaborative")]
        public bool IsCollaborative { get; set; }

        [JsonProperty("has_custom_cover")]
        public bool HasCustomCover { get; set; }

        [JsonProperty("board_order_modified_at")]
        public string BoardOrderModifiedAt { get; set; }

        [JsonProperty("conversation")]
        public object Conversation { get; set; }

        [JsonProperty("collaborated_by_me")]
        public bool CollaboratedByMe { get; set; }

        [JsonProperty("images")]
        public Images Images { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("pin_count")]
        public int PinCount { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
