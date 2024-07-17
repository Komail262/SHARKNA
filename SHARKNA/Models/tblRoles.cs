namespace SHARKNA.Models
{
    public class tblRoles
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public ICollection<tblPermissions> Permissions { get; set; }
    }
}
