using Microsoft.Graph;

namespace Definitiv.MicrosoftGraph.Web.Services;

public class MicrosoftGraphService
{
    private readonly GraphServiceClient graphServiceClient;

    public MicrosoftGraphService(
        GraphServiceClient graphServiceClient)
    {
        this.graphServiceClient = graphServiceClient;
    }

    /// <summary>
    /// Get the User's Azure Active Directory User Id using the email address.
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> GetActiveDirectoryUserId(
        string emailAddress, 
        CancellationToken cancellationToken)
    {
        var users = await graphServiceClient.Users
            .Request()
            .Filter($"mail eq '{emailAddress}'")
            .Select("id")
            .Top(1)
            .GetAsync(cancellationToken);

        return users.First().Id;
    }

    /// <summary>
    /// Get the Id of the user's first calendar in their Outlook 365 account using their Active Directory User Id.
    /// </summary>
    /// <param name="userId">Active Director User Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> GetUserCalendarId(
        string userId, 
        CancellationToken cancellationToken)
    {
        var calendars = await graphServiceClient
            .Users[userId]
            .Calendars
            .Request()
            .Select("id")
            .Top(1)
            .GetAsync(cancellationToken);

        return calendars.First().Id;
    }

    /// <summary>
    /// Get the Event from the Office 365.
    /// </summary>
    /// <param name="userId">Active Director User Id</param>
    /// <param name="calendarId"></param>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Event> GetUserCalendarEvent(
        string userId, 
        string calendarId, 
        string eventId, 
        CancellationToken cancellationToken)
    {
        return await graphServiceClient
            .Users[userId]
            .Calendars[calendarId]
            .Events[eventId]
            .Request()
            .GetAsync(cancellationToken);
    }

    /// <summary>
    /// Create a new event on the User's Outlook calendar.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="calendarId"></param>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Event> CreateUserCalendarEvent(
        string userId, 
        string calendarId, 
        Event @event, 
        CancellationToken cancellationToken)
    {
        return await graphServiceClient
            .Users[userId]
            .Calendars[calendarId]
            .Events
            .Request()
            .AddAsync(@event, cancellationToken);
    }

    /// <summary>
    /// Updates an existing event on the user's outlook calendar.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="calendarId"></param>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Event> UpdateUserCalendarEvent(
        string userId, 
        string calendarId, 
        Event @event, 
        CancellationToken cancellationToken)
    {
        return await graphServiceClient
            .Users[userId]
            .Calendars[calendarId]
            .Events[@event.Id]
            .Request()
            .UpdateAsync(@event, cancellationToken);
    }

    /// <summary>
    /// Delete the user's Outlook calendar event.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="calendarId"></param>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteUserCalendarEvent(
        string userId, 
        string calendarId, 
        string eventId, 
        CancellationToken cancellationToken)
    {
        await graphServiceClient
            .Users[userId]
            .Calendars[calendarId]
            .Events[eventId]
            .Request()
            .DeleteAsync(cancellationToken);
    }
}
