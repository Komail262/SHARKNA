using SHARKNA.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SHARKNA.Domain
{
    public class RequestStatusDomain
    {
        private readonly SHARKNAContext _context;

        public RequestStatusDomain(SHARKNAContext context)
        {
            _context = context;
        }

        public List<tblRequestStatus> GettblRequestStatuses()
        {
            return _context.tblRequestStatus.Where(rs => !rs.IsDeleted && rs.IsActive).ToList();
        }

        public tblRequestStatus GettblRequestStatusById(Guid id)
        {
            return _context.tblRequestStatus.FirstOrDefault(rs => rs.Id == id && !rs.IsDeleted && rs.IsActive);
        }

        public void AddRequestStatus(tblRequestStatus requestStatus)
        {
            _context.tblRequestStatus.Add(requestStatus);
            _context.SaveChanges();
        }

        public void UpdateRequestStatus(tblRequestStatus requestStatus)
        {
            _context.tblRequestStatus.Update(requestStatus);
            _context.SaveChanges();
        }

        public void DeleteRequestStatus(Guid id)
        {
            var requestStatus = _context.tblRequestStatus.FirstOrDefault(rs => rs.Id == id);
            if (requestStatus != null)
            {
                requestStatus.IsDeleted = true;
                _context.tblRequestStatus.Update(requestStatus);
                _context.SaveChanges();
            }
        }
    }
}
