using Mercury.Shared.Kernel.Events;

namespace Mercury.Modules.ActivityManagement.Domain.Activities;

public static class ActivityEvents
{
    public sealed class Created : IDomainEvent;
    public sealed class Updated : IDomainEvent;
    public sealed class Deleted : IDomainEvent;
}