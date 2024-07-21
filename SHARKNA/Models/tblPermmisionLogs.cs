namespace SHARKNA.Models
{
    public class tblPermmisionLogs
    {
        public Guid Id { get; set; }

        public string OpType { get; set; }

        public DateTime DateTime { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string CreatedTo { get; set; }

        public string AdditionalInfo { get; set; }

        public int PermId { get; set; }//see the change
    }
}
