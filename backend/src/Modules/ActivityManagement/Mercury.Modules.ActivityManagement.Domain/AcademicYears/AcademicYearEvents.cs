using Mercury.Shared.Kernel.Events;

namespace Mercury.Modules.ActivityManagement.Domain.AcademicYears;

public static class AcademicYearEvents
{
    public sealed class Created : IDomainEvent;
    public sealed class Updated : IDomainEvent;
    public sealed class Deleted : IDomainEvent;
}