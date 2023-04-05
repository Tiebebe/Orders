namespace Domain.Models
{
    public enum OrderStatus
    {
        Undefined = 0,
        Created = 1,
        ProcessThatAllowsEditing = 2,
        TooLatetoEdit = 3,
        Cancelled = 4, 
        Completed = 5

    }
}