namespace SHARKNA.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
//TEST
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
