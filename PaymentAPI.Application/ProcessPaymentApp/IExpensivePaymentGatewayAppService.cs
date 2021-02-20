using PaymentAPI.Application.ProcessPaymentApp.Dtos;
using PaymentAPI.Core.OperationReturns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.ProcessPaymentApp
{
    public interface IExpensivePaymentGatewayAppService
    {

        /// <summary>
        /// Add related field to the tracking entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<OperationResult> Insert(PaymentCardModelDto entity);

        /// <summary>
        /// Returns true if the tracking  collection contains any element of entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PaymentCardModelDto> GetById(long? id);

        /// <summary>
        /// Deletes entity from the tracking collection if found.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(PaymentCardModelDto entity);

        /// <summary>
        /// Updtaes entity from the tracking collection if found.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<OperationResult> Update(PaymentCardModelDto entity);
    }
}
