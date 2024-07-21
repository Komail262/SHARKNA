namespace SHARKNA.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
//TEST12
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
