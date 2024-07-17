namespace SHARKNA.Models
{
    public class tblEventAttendence
    {
        public Guid Id { get; set; }
        public DateTime EventDate { get; set; }
        
        public bool IsAttend { get; set; }
        public tblEvents Events { get; set; }
        public int Day { get; set; }
        public Guid EventstId { get; set; }
        public tblEventMembers EventMember { get; set; }
        public Guid EventMemId { get; set; }





    }
}
