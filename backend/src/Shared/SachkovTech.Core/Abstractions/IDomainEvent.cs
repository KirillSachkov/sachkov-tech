using MediatR;

namespace SachkovTech.Core.Abstractions;

// Интерфейс для всех наших событий. Наследуется от типа из MediatR
public interface IDomainEvent : INotification
{
    public DateTime Timestamp { get; }
}

// Интерфейс обработчика событий. Также наследуется от типа из MediatR
public interface IDomainEventHandler<in T> : INotificationHandler<T>
    where T : IDomainEvent;

// Интерфейс издателя событий
public interface IDomainEventPublisher
{
    IReadOnlyCollection<IDomainEvent> Events { get; }

    void AddEvent(IDomainEvent @event);
    void AddEventRange(IEnumerable<IDomainEvent> events);
    Task HandleEvents();
}

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;
    private readonly List<IDomainEvent> _events = [];

    public DomainEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Коллекция событий, возникших за время выполнения операции
    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

    // Добавление нового события
    public void AddEvent(IDomainEvent @event) => _events.Add(@event);

    // Добавление набора событий
    public void AddEventRange(IEnumerable<IDomainEvent> events) => _events.AddRange(events);

    // Обработка всех событий через публикацию в MediatR.
    // MediatR вызывает все существующие обработчики 
    // для конкретного типа события
    public async Task HandleEvents()
    {
        foreach (var @event in _events)
        {
            await _mediator.Publish(@event);
        }
    }
}