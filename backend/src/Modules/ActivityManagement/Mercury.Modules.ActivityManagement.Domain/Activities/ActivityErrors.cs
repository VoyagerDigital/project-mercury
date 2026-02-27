namespace Mercury.Modules.ActivityManagement.Domain.Activities;

public static class ActivityErrors
{
    public static readonly string NameEmpty = $"{nameof(Activity)}.{nameof(Activity.Name)}.Empty";
    public static readonly string NameTooLong = $"{nameof(Activity)}.{nameof(Activity.Name)}.TooLong";
    public static readonly string DescriptionEmpty = $"{nameof(Activity)}.{nameof(Activity.Description)}.Empty";
    public static readonly string DescriptionTooLong = $"{nameof(Activity)}.{nameof(Activity.Description)}.TooLong";
    public static readonly string StartTimeInThePast = $"{nameof(Activity)}.{nameof(Activity.StartTime)}.InThePast";
    public static readonly string EndTimeBeforeStartTime = $"{nameof(Activity)}.{nameof(Activity.EndTime)}.BeforeStartTime";
}