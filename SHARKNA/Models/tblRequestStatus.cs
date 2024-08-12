using SHARKNA.ViewModels;

namespace SHARKNA.Models
{
    public class tblRequestStatus
    {
        public Guid Id { get; set; }
        public string RequestStatusAr { get; set; }
        public string RequestStatusEn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public ICollection<EventRegistrationsViewModel> EventReg { get; set; }
        public ICollection<tblEventRequests> EventReq { get; set; }
        public ICollection<tblBoardRequests> BoardReq { get; set; }
        public ICollection<tblBoardTalRequests> BoardTalReq { get; set; }
    }
}
