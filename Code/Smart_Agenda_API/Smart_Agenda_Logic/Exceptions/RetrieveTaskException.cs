namespace Smart_Agenda_Logic.Exceptions
{
    [Serializable]
    public class RetrieveTaskException : Exception
    {
        public RetrieveTaskException() { }
        public RetrieveTaskException(string? message) : base(message) { }
        public RetrieveTaskException(string? message, Exception inner) : base(message, inner) { }
        protected RetrieveTaskException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
