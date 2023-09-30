using crudSignalR.Core.Application.Interface.Repository;
using crudSignalR.Core.Application.Interface.Services;
using crudSignalR.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application
{
    public class SiganalServer:Hub
    {
        
        private readonly IAccountService acountService;
        private readonly IMessageRepository messageRepository;
        public SiganalServer(IAccountService acountService,IMessageRepository messageRepository)
        {
            this.acountService = acountService;
            this.messageRepository = messageRepository;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            Console.WriteLine("Disconect");
        }
        public override async Task OnDisconnectedAsync(Exception e)
        {            
            Console.WriteLine("OnDisconnectedAsync");
            await base.OnDisconnectedAsync(e);
        } 
        public async Task  ValidateUserToken(object payload)
        {
            var mapa = JObject.Parse(payload.ToString());
            string id = mapa["id"].Value<string>();
            string token = mapa["token"].Value<string>();
            bool result=await acountService.ValidateUserToken(id, token);
            //if (!result)
            //{
            //    Context.Abort();
            //}          
        }

        public async Task JoinUserToRoom(string id)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, id);
            Console.WriteLine(Context.ConnectionId);
        }
        public async Task SendMessageToUser(string groupName, object message)
        {
            
            await Clients.Group(groupName).SendAsync("NewMessage", message);
           

        }

        public async Task Personalmessage(object payload)
        {
            var mapa = JObject.Parse(payload.ToString());
            string idTo = mapa["idTo"].Value<string>();
            string from = mapa["from"].Value<string>();
            string Requestmessage = mapa["message"].Value<string>();
            Message message = new Message();
            message.From =from;
            message.To = idTo;
            message.Messae = Requestmessage;
            var result=  await messageRepository.AddAsync(message);

            if(result != null)
            {
                await SendMessageToUser(idTo, payload);
            }
        }
    }
}
