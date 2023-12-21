namespace Smart_Agenda_Logic.Exceptions
{
    [Serializable]
    public class TaskException : Exception
    {
        public TaskException()
        {
        }

        public TaskException(string? message) : base(message)
        {
        }

        public TaskException(string? message, Exception innerException) : base(message, innerException)
        {
        }
        protected TaskException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {

        }
    }
}
