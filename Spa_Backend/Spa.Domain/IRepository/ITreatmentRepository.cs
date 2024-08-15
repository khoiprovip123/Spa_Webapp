using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IRepository
{
    public interface ITreatmentRepository
    {
        Task CreateTreatmentCard(TreatmentCard treatmentCard);
        Task<IEnumerable<TreatmentCard>> GetTreatmentCardAsyncByCustomer(long customerId);
        Task<TreatmentCard> GetTreatmentCardDetailAsyncByID(long treatmentlID);
        Task<TreatmentDetail> GetTreatmentDetailAsyncByID(long treatmentDetailID);
        bool UpdateTreatment(TreatmentCard treatmentCard);
        bool UpdateStatusSession(long id, bool status);
        Task<string> GetLastCodeAsync();
        Task<bool> DeleteTreatmentDetail(long id);

    }  
}
