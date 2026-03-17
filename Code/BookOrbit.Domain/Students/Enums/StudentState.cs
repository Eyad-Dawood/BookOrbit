namespace BookOrbit.Domain.Students.Enums;
public enum StudentState
{
    Pending,
    Approved,
    Active,
    Rejected,
    Banned,
    Suspended,    // temporary state, used when a student is banned for a specific period of time, after which they can be reactivated
    UnVerified
}
