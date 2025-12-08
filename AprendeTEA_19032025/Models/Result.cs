namespace AprendeTEA_19032025.Models
{
    public class Result
    {
        public bool Correct { get; set; }
        public string ErrorMessage { get; set; }
        public object Object { get; set; }
        public List<object> Objects { get; set; }
        public Exception Ex { get; set; }
        public string ErrorCode { get; set; }
    }
}
