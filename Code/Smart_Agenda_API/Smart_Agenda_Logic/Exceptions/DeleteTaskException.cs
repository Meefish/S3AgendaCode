namespace Smart_Agenda_Logic.Exceptions
{
    [Serializable]
    public class DeleteTaskException : Exception
    {

        public DeleteTaskException()
        {
        }

        public DeleteTaskException(string? message) : base(message)
        {
        }

        public DeleteTaskException(string? message, Exception innerException) : base(message, innerException)
        {
        }
        protected DeleteTaskException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {

        }
    }
}
