namespace PharmaStock.Core.DTO.Common
{
    public static class ApiErrorResponse
    {
        public static object Build(int status, string error, string message) => new
        {
            timestamp = DateTime.UtcNow,
            status,
            error,
            message
        };
    }
}
