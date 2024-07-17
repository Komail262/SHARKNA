namespace SHARKNA.Models
{
    public class tblEventMembers
    {
        public Guid Id { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string MobileNumber { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ICollection<tblEventAttendence> EventAttend { get; set; }
 
         
    }
}
