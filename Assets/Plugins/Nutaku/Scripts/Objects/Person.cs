using System;

namespace Nutaku.Unity
{
    /// <summary>
    /// Holds profile information for use with the People API
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Nutaku UserID (Basic info)
        /// </summary>
        public string id;

        /// <summary>
        /// Nickname (Basic info)
        /// </summary>
        public string nickname;

        /// <summary>
        /// Birthday (Profile info)
        /// </summary>
        public DateTime? birthday;

        /// <summary>
        /// Country (Profile info)
        /// </summary>
        public Address addresses;

        /// <summary>
        /// Profile URL(Basic info)
        /// </summary>
        public string profileUrl;

        /// <summary>
        /// Standard thumbnail URL (Basic info)
        /// </summary>
        public string thumbnailUrl;

        /// <summary>
        /// Small thumbnail URL (Basic info)
        /// </summary>
        public string thumbnailUrlSmall;

        /// <summary>
        /// Large thumbnail URL (Basic info)
        /// </summary>
        public string thumbnailUrlLarge;

        /// <summary>
        /// Huge thumbnail URL (Basic info)
        /// </summary>
        public string thumbnailUrlHuge;

        /// <summary>
        /// Gender (Profile info)
        /// </summary>
        public string gender;

        /// <summary>
        /// Age (Profile info)
        /// </summary>
        public string age;

        /// <summary>
        /// If the user is a player of the current app (Basic info)
        /// </summary>
        public bool? hasApp;

        /// <summary>
        /// User Type (Basic info)
        /// Null or empty string represents a normal player. Anything else means it's a staff or developer account, so payments should be ignored from financial metrics
        /// </summary>
        public string userType;

        /// <summary>
        /// User Grade
        /// 0: guest, 1: registered, 2: registered and has verified the email address
        /// </summary>
        public string grade;

        /// <summary>
        /// User's language code written in ISO639-1 format (the language he is viewing the site in)
        /// </summary>
        public string languagesSpoken;

        public static class Fields
        {
            public static readonly string Id = "id";

            public static readonly string Nickname = "nickname";

            public static readonly string Birthday = "birthday";

            public static readonly string Addresses = "addresses";

            public static readonly string ProfileUrl = "profileUrl";

            public static readonly string ThumbnailUrl = "thumbnailUrl";

            public static readonly string ThumbnailUrlSmall = "thumbnailUrlSmall";

            public static readonly string ThumbnailUrlLarge = "thumbnailUrlLarge";

            public static readonly string ThumbnailUrlHuge = "thumbnailUrlHuge";

            public static readonly string Gender = "gender";

            public static readonly string Age = "age";

            public static readonly string HasApp = "hasApp";

            public static readonly string UserType = "userType";

            public static readonly string Grade = "grade";

            public static readonly string LanguagesSpoken = "languagesSpoken";
        }
    }
}
