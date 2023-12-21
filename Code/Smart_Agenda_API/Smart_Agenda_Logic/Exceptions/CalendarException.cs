namespace Smart_Agenda_Logic.Exceptions
{
    [Serializable]
    public class CalendarException : Exception
    {
        public CalendarException()
        {
        }

        public CalendarException(string message) : base(message)
        {
        }

        public CalendarException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
