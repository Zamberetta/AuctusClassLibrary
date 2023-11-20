namespace Auctus.DataMiner.Library.Protocol
{
    using global::Auctus.DataMiner.Library.Auctus.Common;
    using Skyline.DataMiner.Net.Messages;
    using Skyline.DataMiner.Scripting;
    using System;
    using System.Linq;

    /// <summary>
    /// Methods that aid with retrieving user info.
    /// </summary>
    public static class User
    {
        /// <summary>Retrieves the account name for user that triggered the QAction.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        public static string GetUser(SLProtocol protocol)
        {
            try
            {
                var userCookie = protocol.UserCookie;
                var user = (string)protocol.NotifyDataMiner((int)NotifyType.GetUser, userCookie, null);
                return string.IsNullOrEmpty(user) ? string.Empty : user;
            }
            catch (Exception ex)
            {
                protocol.Logger(ex);
                return string.Empty;
            }
        }

        /// <summary>Retrieves the user information for user that triggered the QAction or the specified account.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="accountName">The account name to retrieve the user information for.</param>
        public static UserInfo GetUserInfo(SLProtocol protocol, string accountName = null)
        {
            try
            {
                accountName = accountName ?? GetUser(protocol);

                if (string.IsNullOrWhiteSpace(accountName))
                {
                    return null;
                }

                var userInformation = (object[])protocol.NotifyDataMiner((int)NotifyType.GetUserInfo, accountName, null);
                var user = (string[])userInformation.ElementAtOrDefault(0);

                if (user == null)
                {
                    return null;
                }

                var userInfo = new UserInfo
                {
                    Name = user[0],
                    FullName = user[1],
                    Email = user[2],
                    Telephone = user[3],
                    Pager = user[4]
                };

                var groupsCount = user.Length - 5;

                if (groupsCount <= 0)
                {
                    userInfo.Groups = Array.Empty<string>();
                    return userInfo;
                }

                var groups = new string[groupsCount];

                for (var i = 5; i < user.Length; i++)
                {
                    groups[i - 5] = user[i];
                }

                userInfo.Groups = groups;

                return userInfo;
            }
            catch (Exception ex)
            {
                protocol.Logger(ex);
                return null;
            }
        }
    }
}