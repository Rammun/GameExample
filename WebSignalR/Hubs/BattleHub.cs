using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSignalR.Models;

namespace WebSignalR.Hubs
{
    public class BattleHub : Hub
    {
        static List<UserParam> Users = new List<UserParam>();

        // Подключение нового пользователя
        public void Connect(string userName)
        {
            var connectionId = Context.ConnectionId;

            if (!Users.Any(x => x.Id == connectionId))
            {
                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(connectionId, userName, Users);

                Users.Add(new UserParam { Id = connectionId, Name = userName });

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(connectionId).onNewUserConnected(connectionId, userName);
            }
        }

        public void AddHit(UserParam user1, UserParam user2)
        {
            Clients.Client(user2.Id).addHit(user1, user2);
        }
    }
}