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
        static Random rnd = new Random();

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

        public void AddHit(UserParam user1, UserParam user2, string hit)
        {
            SetNewParam(user1, user2, hit);
            Clients.Client(user2.Id).addHit(user2, user1);
        }

        private void SetNewParam(UserParam user1, UserParam user2, string hit)
        {
            int[] hits = user1.skills[hit];
            int damage = rnd.Next(hits[0], hits[1]);

            if (user1.MP >= hits[2] && user2.HP > 0)
            {
                user2.HP -= damage;
                user1.MP -= hits[2];
            }
        }
    }
}