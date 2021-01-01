namespace API.Errors
{
    public class APIException
    {
        public APIException(int statusCode, string msg = null, string details = null)
        {
            this.StatusCode = statusCode;
            this.Msg = msg;
            this.Details = details;
        }

        public int StatusCode { get; set; }
        public string Msg { get; set; }
        public string Details { get; set; }
    }
}