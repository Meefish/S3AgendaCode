namespace Smart_Agenda_Logic.Exceptions
{
    [Serializable]
    public class AddTaskException : Exception
    {
        public AddTaskException() { }
        public AddTaskException(string? message) : base(message) { }
        public AddTaskException(string? message, Exception inner) : base(message, inner) { }
        protected AddTaskException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
