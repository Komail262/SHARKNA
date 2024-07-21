namespace SHARKNA.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
//TEST129
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
