using System.ComponentModel.DataAnnotations;

namespace SHARKNA.Models
{
    public class tblBoardRequests
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string UserName { get; set; } //test

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string FullNameAr { get; set; }

        [Required]
        [StringLength(100)]
        public string FullNameEn { get; set; }


        public tblBoards Board { get; set; }

        [Required]
        [StringLength(100)]
        public Guid BoardId { get; set; }

        [Required]
        [StringLength(100)]
        public tblRequestStatus RequestStatus { get; set; }

        [Required]
        [StringLength(100)]
        public Guid RequestStatusId { get; set; }
        public string? RejectionReasons { get; set; }
        
    }
}
