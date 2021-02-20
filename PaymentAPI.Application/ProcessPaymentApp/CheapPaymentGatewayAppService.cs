using PaymentAPI.Application.ProcessPaymentApp.Dtos;
using PaymentAPI.Core.OperationReturns;
using PaymentAPI.Core.Payment;
using PaymentAPI.Repository.ProcessPayment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.ProcessPaymentApp
{
    public class CheapPaymentGatewayAppService : ICheapPaymentGatewayAppService
    {
        private readonly ICheapPaymentGatewayRepository _cheapPaymentGatewayRepository;

        public CheapPaymentGatewayAppService(
            ICheapPaymentGatewayRepository cheapPaymentGatewayRepository)
        {
            _cheapPaymentGatewayRepository = cheapPaymentGatewayRepository;
        }

        /// <summary>
        /// Deletes entity from the tracking collection if found.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(PaymentCardModelDto entity)
        {
            var data = new PaymentCardModel
            {
                Id = entity.Id,
                Amount = entity.Amount,
                CardHolder = entity.CardHolder,
                CreditCardNumber = entity.CreditCardNumber,
                ExpirationDate = entity.ExpirationDate,
                SecurityCode = entity.SecurityCode,
                Status = entity.Status,

            };
            var delete = await _cheapPaymentGatewayRepository.Delete(data);
            return new OperationResult()
            {
                Message = delete.Message,
                Status = delete.Status,
                Succeeded = delete.Succeeded,
                StatusCode = delete.StatusCode,
            };
        }

        /// <summary>
        /// Returns true if the tracking  collection contains any element of entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PaymentCardModelDto> GetById(long? id)
        {
            try
            {
                var entity = await _cheapPaymentGatewayRepository.GetById(id);
                var data = new PaymentCardModelDto
                {
                    Id = entity.Id,
                    Amount = entity.Amount,
                    CardHolder = entity.CardHolder,
                    CreditCardNumber = entity.CreditCardNumber,
                    ExpirationDate = entity.ExpirationDate,
                    SecurityCode = entity.SecurityCode,
                    Status = entity.Status
                };
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add related field to the tracking entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<OperationResult> Insert(PaymentCardModelDto entity)
        {
            var data = new PaymentCardModel
            {
                Amount = entity.Amount,
                CardHolder = entity.CardHolder,
                CreditCardNumber = entity.CreditCardNumber,
                ExpirationDate = entity.ExpirationDate,
                SecurityCode = entity.SecurityCode,
                Status = entity.Status

            };
            var insert = await _cheapPaymentGatewayRepository.Insert(data);
            return new OperationResult()
            {
                Message = insert.Message,
                Status = insert.Status,
                Succeeded = insert.Succeeded,
                StatusCode = insert.StatusCode,
                Payment = insert.Payment
            };
        }

        /// <summary>
        /// Updtaes entity from the tracking collection if found.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(PaymentCardModelDto entity)
        {
            var data = new PaymentCardModel
            {
                Id = entity.Id,
                Amount = entity.Amount,
                CardHolder = entity.CardHolder,
                CreditCardNumber = entity.CreditCardNumber,
                ExpirationDate = entity.ExpirationDate,
                SecurityCode = entity.SecurityCode,
                Status = entity.Status

            };
            var update = await _cheapPaymentGatewayRepository.Update(data);
            return new OperationResult()
            {
                Message = update.Message,
                Status = update.Status,
                Succeeded = update.Succeeded,
                StatusCode = update.StatusCode,
                Payment = update.Payment
            };
        }
    }
}
