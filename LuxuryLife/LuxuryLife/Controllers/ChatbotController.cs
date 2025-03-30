using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LuxuryLife.Models;

namespace LuxuryLife.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly ChatbotService _chatbotService;
        private readonly TourBookingContext _context;

        public ChatbotController(ChatbotService chatbotService, TourBookingContext context)
        {
            _chatbotService = chatbotService;
            _context = context;
        }

        [HttpPost]
        public IActionResult SendMessage(string message)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để sử dụng chatbot." });
            }

            if (string.IsNullOrEmpty(message))
            {
                return Json(new { success = false, message = "Vui lòng nhập tin nhắn." });
            }

            var response = _chatbotService.ProcessMessage(message, customerId.Value);
            return Json(new { success = true, response });
        }

        [HttpGet]
        public IActionResult GetChatHistory()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
                return Json(new { success = false, chats = new List<ChatHistory>() });
            }

            var chatHistories = _context.ChatHistories
                .Where(ch => ch.CustomerId == customerId.Value)
                .OrderBy(ch => ch.CreatedDate)
                .Select(ch => new { ch.Message, ch.Response, ch.CreatedDate })
                .ToList();

            return Json(new { success = true, chats = chatHistories });
        }
    }
}