using Microsoft.Extensions.Caching.Distributed;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.DTOs.Request;
using Personal_Finance_System_BE.PersonalFinanceSys.Application.Interfaces;
using System.Text.Json;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Services
{
    public class ChatHistoryService : IChatHistoryService
    {
        private readonly IDistributedCache _cache;
        private readonly JsonSerializerOptions _jsonOptions;

        public ChatHistoryService(IDistributedCache cache)
        {
            _cache = cache;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }

        private string GetCacheKey(Guid idUser) => $"chat_history:{idUser}";

        public async Task DeleteHistoryAsync(Guid idUser)
        {
            await _cache.RemoveAsync(GetCacheKey(idUser));
        }

        public async Task<List<MessageHistoryItem>> GetHistoryAsync(Guid idUser)
        {
            var key = GetCacheKey(idUser);
            var jsonData = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(jsonData))
                return new List<MessageHistoryItem>();

            return JsonSerializer.Deserialize<List<MessageHistoryItem>>(jsonData, _jsonOptions)!;
        }

        public async Task SaveMessageAsync(Guid idUser, MessageHistoryItem message)
        {
            var key = GetCacheKey(idUser);
            var existingData = await _cache.GetStringAsync(key);

            List<MessageHistoryItem> history;
            if (!string.IsNullOrEmpty(existingData)){
                history = JsonSerializer.Deserialize<List<MessageHistoryItem>>(existingData, _jsonOptions)!;
            }
            else{
                history = new List<MessageHistoryItem>();
            }

            history.Add(message);

            var jsonData = JsonSerializer.Serialize(history, _jsonOptions);

            await _cache.SetStringAsync(
                key,
                jsonData,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });
        }
    }
}
