namespace SHARKNA.Models //ziyad
{
    public class tblPermissions
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public bool IsDeleted { get; set; }
        public tblRoles Role { get; set; }
        public Guid RoleId { get; set; }
    }
}
