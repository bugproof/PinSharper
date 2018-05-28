namespace PinSharper
{
    public class SignUpParams
    {
        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Username (Optional)
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gender (male, female)
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Locale e.g.: "en_US"
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
