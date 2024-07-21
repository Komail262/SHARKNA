namespace SHARKNA.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
//TEST1200
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
