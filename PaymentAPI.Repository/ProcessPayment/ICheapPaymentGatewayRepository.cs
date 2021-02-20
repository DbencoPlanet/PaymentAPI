using PaymentAPI.Core.OperationReturns;
using PaymentAPI.Core.Payment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Repository.ProcessPayment
{
    public interface ICheapPaymentGatewayRepository
    {
        /// <summary>
        /// Return true if entity presented exists
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Exists(PaymentCardModel entity);

        /// <summary>
        /// Returns true if the tracking collection contains any element of entity
        /// </summary>
        /// <returns></returns>
        bool Any();

        /// <summary>
        /// Add related field to the tracking entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<OperationResult> Insert(PaymentCardModel entity);

        /// <summary>
        /// Returns true if the tracking  collection contains any element of entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PaymentCardModel> GetById(long? id);

        /// <summary>
        /// Deletes entity from the tracking collection if found.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(PaymentCardModel entity);

        /// <summary>
        /// Updtaes entity from the tracking collection if found.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<OperationResult> Update(PaymentCardModel entity);

        /// <summary>
        /// Check if Credit Card Number is valid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IsValid(object value);
    }
}
