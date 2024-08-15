using Microsoft.AspNetCore.SignalR;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spa.Application.Models;
using AutoMapper;
using static StackExchange.Redis.Role;
using Spa.Domain.Entities;
using Spa.Domain.Service;

namespace Spa.Application.SignalR
{
    /*    public class ChatHub : Hub
        {
            private readonly MessageService _messageService;

            public ChatHub(*//*MessageService messageService*//*)
            {
               *//* _messageService = messageService;*//*
            }

            public async Task SendMessage(string user, string message)

            {
        //       await _messageService.AddMessagesAsync(user, message);
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }

        }*/
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        // Constructor nhận IMessageService qua DI
        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(string user, string message)
        {
            // Giả sử AddMessagesAsync nhận các tham số kiểu Message
            var messageModel = new Message
            {
                UserName = user,
                Content = message
            };

            await _messageService.AddMessagesAsync(messageModel.UserName,messageModel.Content);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task<List<Message>> GetMessages()
        {
            var messages = await _messageService.GetMessagesAsync();
            return messages;
        }
    }
}
