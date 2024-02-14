using System.Text.Json;

namespace HRM.DTOs
{
    public class MessageHelper
    {
        public string Message { get; set; } = "";
        public int StatusCode { get; set; }
        public long? AutoId { get; set; }
    }
    public class MessageHelperCreate
    {
        public string Message { get; set; } = "Created Successfully";
        public int StatusCode { get; set; } = StatusCodes.Status201Created;
        public long? AutoId { get; set; }
    }
    public class MessageHelperUpdate
    {
        public string Message { get; set; } = "Update Successfully";
        public int StatusCode { get; set; } = StatusCodes.Status202Accepted;
        public long? AutoId { get; set; }
    }
    public class MessageHelperError
    {
        public string Message { get; set; } = "Internal Server Error";
        public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;
        public long? AutoId { get; set; }
    }
}
