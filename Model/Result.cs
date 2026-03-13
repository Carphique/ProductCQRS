namespace ProductCQRS.Model
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public static Result<T> Success (T data, string message = "Success")
        {
            return new Result<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
        }

        public static Result<T> Fail(string message)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default
            };
        }
    }
}
