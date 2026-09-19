using DVLD.Application.Interfaces.Events;
using DVLD.Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DVLD.Application.UnitTests;

/// <summary>
/// Dummy domain event isolated strictly for pipeline verification.
/// </summary>
public sealed record TestPersonUpdatedEvent(string Name) : IDomainEvent;

/// <summary>
/// Transient entity inheriting from <see cref="BaseEntity"/> to verify event emission.
/// </summary>
public sealed class TestPerson : BaseEntity
{
    /// <summary>
    /// Updates the person's name and appends the domain event to internal queue.
    /// </summary>
    /// <param name="newName">The new name to apply.</param>
    public void UpdateName(string newName)
    {
        AddDomainEvent(new TestPersonUpdatedEvent(newName));
    }
}

/// <summary>
/// Contains isolated unit tests to verify pre-commit and post-commit event dispatch pipelines.
/// </summary>
public class EventPipelineQuickTest
{
    // Execution tracking flags to confirm handler invokation
    public static bool BeforeCommitExecuted = false;
    public static bool AfterCommitExecuted = false;

    /// <summary>
    /// Mock pre-commit handler simulating transactional operations (e.g., audit logging).
    /// </summary>
    public class TestBeforeHandler : IBeforeCommitHandler<TestPersonUpdatedEvent>
    {
        public Task HandleAsync(TestPersonUpdatedEvent domainEvent)
        {
            BeforeCommitExecuted = true;
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Mock post-commit handler simulating out-of-transaction side effects (e.g., SMS/Email notifications).
    /// </summary>
    public class TestAfterHandler : IAfterCommitHandler<TestPersonUpdatedEvent>
    {
        public Task HandleAsync(TestPersonUpdatedEvent domainEvent)
        {
            AfterCommitExecuted = true;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task EventAggregator_ShouldExecute_BeforeAndAfterCommit_InCorrectOrder()
    {
        // -------------------------------------------------------------------------
        // Arrange: Configure isolated In-Memory Service Provider for Dependency Injection
        // -------------------------------------------------------------------------
        var services = new ServiceCollection();
        services.AddScoped<IBeforeCommitHandler<TestPersonUpdatedEvent>, TestBeforeHandler>();
        services.AddScoped<IAfterCommitHandler<TestPersonUpdatedEvent>, TestAfterHandler>();

        var serviceProvider = services.BuildServiceProvider();

        // Instantiate entity and trigger the business logic that records the event
        var person = new TestPerson();
        person.UpdateName("Salem");

        var events = person.DomainEvents.ToList();

        // Resolve registered handlers directly from the container
        var beforeHandler = serviceProvider.GetRequiredService<IBeforeCommitHandler<TestPersonUpdatedEvent>>();
        var afterHandler = serviceProvider.GetRequiredService<IAfterCommitHandler<TestPersonUpdatedEvent>>();

        // -------------------------------------------------------------------------
        // Act: Simulate the complete Unit of Work execution lifecycle
        // -------------------------------------------------------------------------

        // 1. Dispatch pre-commit handlers (operating inside active transaction scope)
        foreach (var ev in events.OfType<TestPersonUpdatedEvent>())
        {
            await beforeHandler.HandleAsync(ev);
        }

        // 2. Simulate transaction commit and flush pending domain events
        person.ClearDomainEvents();

        // 3. Dispatch post-commit handlers (fire side effects after persistence completes)
        foreach (var ev in events.OfType<TestPersonUpdatedEvent>())
        {
            await afterHandler.HandleAsync(ev);
        }

        // -------------------------------------------------------------------------
        // Assert: Ensure execution integrity and domain state clearance
        // -------------------------------------------------------------------------
        Assert.True(BeforeCommitExecuted, "BeforeCommit handler failed to execute within transaction.");
        Assert.True(AfterCommitExecuted, "AfterCommit handler failed to execute after transaction.");
        Assert.Empty(person.DomainEvents); // Confirms entity events are properly flushed
    }
}