using SignalRChat.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class UserInMemory
    {
        private ConcurrentDictionary<string, AppUser> _onlineUser { get; set; } = new ConcurrentDictionary<string, AppUser>();

        public bool AddUpdate(string name, string connectionId)
        {
            var userAlreadyExists = _onlineUser.ContainsKey(name);

            var appUser = new AppUser
            {
                Username = name,
                ConnectionId = connectionId
            };

            _onlineUser.AddOrUpdate(name, appUser, (key, value) => appUser);

            return userAlreadyExists;
        }

        public void Remove(string name)
        {
            AppUser appUser;
            _onlineUser.TryRemove(name, out appUser);
        }

        public IEnumerable<AppUser> GetAllUsers()
        {
            return _onlineUser.Values;
        }

        public AppUser GetUserInfo(string username)
        {
            AppUser user;
            _onlineUser.TryGetValue(username, out user);
            return user;
        }
    }
}
