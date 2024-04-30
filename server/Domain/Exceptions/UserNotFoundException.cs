namespace Domain.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(int userId)
            : base($"The user with the id {userId} was not found.")
        {
        }
    }
}
