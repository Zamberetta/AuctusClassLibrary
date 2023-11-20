namespace Auctus.DataMiner.Library.Auctus.Common
{
    /// <summary>
    ///   Enum used in conjunction with the GetUserInfo method.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// The users account name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The users full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The users email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The users telephone number.
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// The users pager number.
        /// </summary>
        public string Pager { get; set; }

        /// <summary>
        /// The name of the groups the user is assigned to.
        /// </summary>
        public string[] Groups { get; set; }
    }
}