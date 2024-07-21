namespace SHARKNA.Models
{
    public class tblBoardRoles
    {
        public Guid Id { get; set; }
        //
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public ICollection<tblBoardMembers> BoardMember { get; set; }

    }
}
