namespace EventBuss.Messages.Events;

public class BaseIntegrationEvent
{
    // Co-Relation Id.
    public Guid Id { get; private set; }
    public DateTime CreationDate { get; private set; }

    public string CorrelationId { get; set; }
    public BaseIntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    public BaseIntegrationEvent(Guid id, DateTime creationTime)
    {
        Id = id;
        CreationDate = creationTime;
    }
}
