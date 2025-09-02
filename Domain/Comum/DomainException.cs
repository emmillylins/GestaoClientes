namespace Domain.Comum
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
