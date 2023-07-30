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

            if (model is null || string.IsNullOrEmpty(model.Content))
                return new List<ITribeNotification>();

            var notifications = JsonSerializer.Deserialize<List<TribeNotificationContentItem>>(model.Content);

            if (notifications is null)
                return new List<ITribeNotification>();

            model.Content = string.Empty;
            await _dbContext.SaveChangesAsync();

            return notifications;
        }

        public async Task InsertTribeNotification(int tribeId, ITribeNotification notificationDTo)
        {
            var model = _dbContext.TribeNotifications.SingleOrDefault(x => x.TribeId == tribeId);

            if (model == null)
                model = new TribeNotification { TribeId = tribeId  };

            var notificationToAdd = new TribeNotificationContentItem 
            { 
                HumanUnitId = notificationDTo.HumanUnitId, 
                Type = notificationDTo.Type, 
                Added = DateTime.UtcNow 
            };

            List<TribeNotificationContentItem> notifications = null;

            if (!string.IsNullOrEmpty(model.Content))
                notifications = JsonSerializer.Deserialize<List<TribeNotificationContentItem>>(model.Content);

            if (notifications == null)
                    notifications = new List<TribeNotificationContentItem>();

            notifications.Add(notificationToAdd);

            model.Content = JsonSerializer.Serialize(notifications);
            model.Updated = DateTime.UtcNow;

            try
            {
                if (model.Id == 0)
                    await _dbContext.AddAsync(model);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //after few loops.. 
                //SqlException: Cannot insert explicit value for identity column in table 'TribeNotifications' when IDENTITY_INSERT is set to OFF.
                throw ex;
            }
        }
    }

}