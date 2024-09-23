using Microsoft.EntityFrameworkCore;

namespace SHARKNA.Models
{
    public class SHARKNAContext : DbContext
    {
        public DbSet<tblRoles> tblRole { get; set; }
        public DbSet<tblPermissions> tblPermission { get; set; }
        public SHARKNAContext()
        {

        }

        public SHARKNAContext(DbContextOptions<SHARKNAContext> options)
                   : base(options)
        {
        }
        public DbSet<tblBoardMembers> tblBoardMembers { get; set; }
        public DbSet<tblBoardMembersLogs> tblBoardMembersLogs { get; set; }
        public DbSet<tblBoardRequestLogs> tblBoardRequestLogs { get; set; }
        public DbSet<tblBoardRequests> tblBoardRequests { get; set; }
        public DbSet<tblBoardRoles> tblBoardRoles { get; set; }
        public DbSet<tblBoards> tblBoards { get; set; }
        public DbSet<tblBoardLogs> tblBoardLogs { get; set; }
        public DbSet<tblBoardTalRequestLogs> tblBoardTalRequestLogs { get; set; }
        public DbSet<tblBoardTalRequests> tblBoardTalRequests { get; set; }
        public DbSet<tblEventAttendence> tblEventAttendence { get; set; }
        public DbSet<tblEventLogs> tblEventLogs { get; set; }
        public DbSet<tblEventRegistrations> tblEventRegistrations { get; set; }
        public DbSet<tblEventRegLogs> tblEventRegLogs { get; set; }
        public DbSet<tblEventRequestLogs> tblEventRequestLogs { get; set; }
        public DbSet<tblEventRequests> tblEventRequests { get; set; }
        public DbSet<tblEvents> tblEvents { get; set; }
        public DbSet<tblPermissions> tblPermissions { get; set; }
        public DbSet<tblPermmisionLogs> tblPermmisionLogs { get; set; }
        public DbSet<tblRequestStatus> tblRequestStatus { get; set; }
        public DbSet<tblRoles> tblRoles { get; set; }
        public DbSet<tblUsers> tblUsers { get; set; }
        public DbSet<tblEventAttendenceLogs> tblEventAttendenceLogs { get; set; }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBCS"));
        }
    }


    

    
}
