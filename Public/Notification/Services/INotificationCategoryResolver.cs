public interface INotificationCategoryResolver
{
    Task ProcessEventAsync<TEvent>(TEvent evt, string triggerEvent);
}
