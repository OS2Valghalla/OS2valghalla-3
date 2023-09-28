namespace Valghalla.Application.Exceptions
{
    public class FileException : Exception
    {
        public Guid Id { get; init; }

        public FileException(Guid id) => Id = id;

        public FileException(Guid id, Exception innerException) : base("File storage error occurred", innerException)
        {
            Id = id;
        }
    }

    public class FileExistException : FileException
    {
        public FileExistException(Guid id) : base(id)
        {
        }
    }

    public class FileNotFoundException : FileException
    {
        public FileNotFoundException(Guid id) : base(id)
        {
        }
    }
}
