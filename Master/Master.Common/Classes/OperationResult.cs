namespace Master.Common.Classes
{
    public class OperationResult
    {
        public bool ErrorOccured { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public object ExtraData { get; set; }

        public OperationResult()
        {
            Message = string.Empty;
            ErrorOccured = false;
            Data = string.Empty;
        }

        public OperationResult(bool errorOccured, string message = null, object data = null)
        {
            ErrorOccured = errorOccured;
            Message = message;
            Data = data;
        }

        public static OperationResult Of(bool isSuccess, string message = null, object data = null)
            => new OperationResult(!isSuccess, message, data);
    }
}
