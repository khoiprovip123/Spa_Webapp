using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface ITreatmentService
    {
        Task CreateTreatmentCard(TreatmentCard treatmentCard);
        Task<IEnumerable<TreatmentCard>> GetTreatmentCardAsyncByCustomer(long customerId);
        Task<TreatmentCard> GetTreatmentCardDetailAsyncByID(long treatmendID);
        Task<bool> UpdateTreatment(long treatmendID, TreatmentCard treatmentCard);
        bool UpdateStatusSession(long id, bool status);
        Task<string> GetLastCodeAsync();
        Task<bool> DeleteTreatmentDetail(long id);
        Task<bool> UpdateStatusTreatmentCard(long treatmendID, string status);
        }
}
