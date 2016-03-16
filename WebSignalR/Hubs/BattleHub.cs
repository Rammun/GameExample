using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebSignalR.Models;

namespace WebSignalR.Hubs
{
    public class BattleHub : Hub
    {
        static List<UserParam> users = new List<UserParam>();
        static List<UsersBattle> battles = new List<UsersBattle>();

        static Random rnd = new Random();

        // Подключение нового пользователя
        public void Connect(string userName)
        {
            var connectionId = Context.ConnectionId;

            if (!users.Any(x => x.Id == connectionId))
            {
                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(connectionId, userName, users);

                users.Add(new UserParam { Id = connectionId, Name = userName });

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(connectionId).onNewUserConnected(connectionId, userName);
            }
        }

        public void AddHit(string user1Name, string user2Id, string hit)
        {
            UsersBattle battle = battles.FirstOrDefault(b => b.UserParam1.Name == user1Name && b.UserParam2.Id == user2Id);

            UserParam user1 = battle.UserParam1;
            UserParam user2 = battle.UserParam2;

            SetNewParam(user1, user2, hit);

            if(user1.HP <= 0 || user2.HP <= 0)
            {
                Clients.Client(user1.Id).battleEnd(user2.Id);
                Clients.Client(user2.Id).battleEnd(user1.Id);
            }

            Clients.Client(user1.Id).addHit(user1.HP, user1.MP, user2.HP, user2.MP);
            Clients.Client(user2.Id).addHit(user2.HP, user2.MP, user1.HP, user1.MP);
        }

        public void SetNewParam(UserParam user1, UserParam user2, string hit)
        {
            int[] hits = user1.skills[hit];
            int damage = rnd.Next(hits[0], hits[1]);

            if (user1.MP >= hits[2] && user2.HP > 0)
            {
                user2.HP -= damage;
                user1.MP -= hits[2];
            }
        }

        public void InviteSelectUser(string user1Name, string user2Id)
        {
            UserParam user1 = users.FirstOrDefault(u => u.Name == user1Name);
            UserParam user2 = users.FirstOrDefault(u => u.Id == user2Id);

            if(!user1.Busy && !user2.Busy)
            {
                user1.Busy = true;
                user2.Busy = true;

                Clients.Client(user2Id).inviteUser(user1.Id, user1Name);
            }
        }

        public void AcceptBattle(string user1Name, string user2Id)
        {
            UserParam user1 = users.FirstOrDefault(u => u.Name == user1Name);
            UserParam user2 = users.FirstOrDefault(u => u.Id == user2Id);

            battles.Add(new UsersBattle
            {
                UserParam1 = user1.Clone(),
                UserParam2 = user2.Clone()
            });

            Clients.Client(user1.Id).acceptBattle(user2.Name, user1.HP, user1.MP, user2.HP, user2.MP);
            Clients.Client(user2.Id).acceptBattle(user1.Name, user2.HP, user2.MP, user1.HP, user1.MP);
        }

        public void CanselRequest(string user1Name, string user2Id)
        {
            UserParam user1 = users.FirstOrDefault(u => u.Name == user1Name);
            UserParam user2 = users.FirstOrDefault(u => u.Id == user2Id);

            user1.Busy = false;
            user2.Busy = false;

            Clients.Client(user2Id).canselRequest(user1Name);
        }

        public void CanselBattle(string user1Name, string user2Id)
        {
            UserParam user1 = users.FirstOrDefault(u => u.Name == user1Name);
            UserParam user2 = users.FirstOrDefault(u => u.Id == user2Id);

            user1.Busy = false;
            user2.Busy = false;

            Clients.Client(user2Id).canselBattle(user1Name);
        }

        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            var userName = Context.User.Identity.Name;

            if (!users.Any(x => x.Id == connectionId))
            {
                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(connectionId, userName, users);

                users.Add(new UserParam { Id = connectionId, Name = userName });

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(connectionId).onNewUserConnected(connectionId, userName);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var id = Context.ConnectionId;

            users.Remove(users.FirstOrDefault(u => u.Id == id));
            Clients.Caller.onUserDisconnected(id);

            return base.OnDisconnected(stopCalled);
        }
    }
}