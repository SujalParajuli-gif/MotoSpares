namespace MotoSpares.Common.Responses
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public int? TotalCount { get; set; }

        public static ApiResponse<T> Success(T data, string message = "Operation successful")
        {
            return new ApiResponse<T> { IsSuccess = true, Data = data, Message = message };
        }

        public static ApiResponse<T> Success(T data, int totalCount)
        {
            return new ApiResponse<T> { IsSuccess = true, Data = data, TotalCount = totalCount };
        }

        public static ApiResponse<T> Failure(string message)
        {
            return new ApiResponse<T> { IsSuccess = false, Message = message };
        }
    }
}
