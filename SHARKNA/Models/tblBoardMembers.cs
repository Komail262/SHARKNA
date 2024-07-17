namespace SHARKNA.Models
{
    public class tblBoardMembers
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
        public string MobileNumber { get; set; }

        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public string Username { get; set}
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public tblBoardRoles BoardRole { get; set; }
        public Guid BoardRoleId { get; set; }

        public tblBoards Board { get; set; }
        public Guid BoardId { get; set; }


    }
}
