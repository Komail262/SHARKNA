namespace SHARKNA.Models
{
    public class tblBoards
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        //first
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public ICollection<tblBoardMembers> BoardMembers { get; set; }
        public ICollection<tblBoardTalRequests> BoardTalRequests { get; set; }
        public ICollection<tblBoardRequests> BoardRequests { get; set; }
        public ICollection<tblEventRequests> EventRequests { get; set; }
        //public ICollection<tblEvents> Events { get; set; }



    }
}
