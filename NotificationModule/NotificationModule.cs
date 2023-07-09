using Microsoft.EntityFrameworkCore;
using NotificationModule.Configuration;
using NotificationModule.Entities;
using ProjectNomad.Shared.Interfaces;
using System.Text.Json;

namespace NotificationModule
{
    internal class NotificationModule : INotificationModule
    {
        readonly NotificationModuleDbContext _dbContext;
        public NotificationModule(NotificationModuleDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<ITribeNotification>> GetAndRemoveTribeNotifications(int tribeId)
        {
            var model = await _dbContext.TribeNotifications.SingleOrDefaultAsync(x => x.TribeId == tribeId);
            var notifications = JsonSerializer.Deserialize<List<TribeNotificationContentItem>>(model.Content);
            model.Content = null;

            throw new NotImplementedException();
        }

        public async Task InsertTribeNotification(int tribeId, ITribeNotification notificationDTo)
        {
            var model = _dbContext.TribeNotifications.SingleOrDefault(x => x.TribeId == tribeId);

            if (model == null)
                model = new TribeNotification { TribeId = tribeId  };

            var notificationToAdd = new TribeNotificationContentItem 
            { 
                HumanObjectId = notificationDTo.HumanUnitId, 
                Type = notificationDTo.Type, 
                Added = DateTime.UtcNow 
            };

            var notifications = JsonSerializer.Deserialize<List<TribeNotificationContentItem>>(model.Content);

            //todo test if needed
            //if (notifications != null) 
            //    notifications = new List<ContentItem>();

            notifications.Add(notificationToAdd);

            model.Content = JsonSerializer.Serialize(notifications);
            model.Updated = DateTime.UtcNow;

            await _dbContext.AddAsync(model);
            await _dbContext.SaveChangesAsync();
        }
    }

}