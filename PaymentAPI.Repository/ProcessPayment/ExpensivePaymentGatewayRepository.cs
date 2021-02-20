using Microsoft.EntityFrameworkCore;
using PaymentAPI.Core.OperationReturns;
using PaymentAPI.Core.Payment;
using PaymentAPI.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Repository.ProcessPayment
{
    public class ExpensivePaymentGatewayRepository : IExpensivePaymentGatewayRepository
    {
        private readonly PaymentAPIDbContext _context;

        /// <summary>
        /// Instatiates a new object for the context.
        /// </summary>
        /// <param name="context"></param>
        public ExpensivePaymentGatewayRepository(PaymentAPIDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Returns true if the tracking collection contains any element of entity
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return _context.PaymentCardModels.Any();
        }

        /// <summary>
        /// Deletes entity from the tracking collection if found.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<OperationResult> Delete(PaymentCardModel entity)
        {
            if (Any())
            {
                if (Exists(entity))
                {
                    var data = await _context.PaymentCardModels.FindAsync(entity.Id);
                    _context.PaymentCardModels.Remove(data);
                    await _context.SaveChangesAsync();
                    return new OperationResult()
                    {
                        Message = "Payment Card is deleted successfully!",
                        Status = OperationStatus.Deleted,
                        Succeeded = true,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else if ((int)HttpStatusCode.BadRequest == 400)
                {
                    return new OperationResult()
                    {
                        Message = "The request is invalid",
                        Status = OperationStatus.Unknown,
                        Succeeded = false,
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }


            }

            if ((int)HttpStatusCode.InternalServerError == 500)
            {
                return new OperationResult()
                {
                    Message = "Internal server error",
                    Status = OperationStatus.NotFound,
                    Succeeded = false,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

        }

        /// <summary>
        /// Return true if entity presented exists
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Exists(PaymentCardModel entity)
        {
            return _context.PaymentCardModels.Find(entity.Id) == null ? false : true;
        }

        /// <summary>
        /// Returns true if the tracking  collection contains any element of entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PaymentCardModel> GetById(long? id)
        {
            var data = await _context.PaymentCardModels.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
                return null;
            else return data;
        }

        /// <summary>
        /// Add related field to the tracking entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<OperationResult> Insert(PaymentCardModel entity)
        {
            if (!Exists(entity))
            {
                if (IsValid(entity.CreditCardNumber))
                {

                    _context.PaymentCardModels.Add(entity);
                    await _context.SaveChangesAsync();
                    if (entity.CreditCardNumber != null)
                    {
                        return new OperationResult()
                        {
                            Message = "Payment is processed: 200 OK",
                            Status = OperationStatus.Created,
                            Succeeded = true,
                            StatusCode = HttpStatusCode.OK,
                            Payment = entity
                        };
                    }
                    else
                    {
                        return new OperationResult()
                        {
                            Message = "The request is invalid: 400 Bad request",
                            Status = OperationStatus.NotFound,
                            Succeeded = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            Payment = entity
                        };

                    }

                }

            }

            return new OperationResult()
            {
                Message = "Any error: 500 internal server error",
                Status = OperationStatus.Unknown,
                Succeeded = false,
                StatusCode = HttpStatusCode.BadRequest
            };


        }

        /// <summary>
        /// Updtaes entity from the tracking collection if found.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<OperationResult> Update(PaymentCardModel entity)
        {
            if (Exists(entity))
            {
                if (IsValid(entity.CreditCardNumber))
                {

                    var update = await _context.PaymentCardModels.FirstOrDefaultAsync(x => x.Id == entity.Id);
                    update.Amount = entity.Amount;
                    update.CardHolder = entity.CardHolder;
                    update.CreditCardNumber = entity.CreditCardNumber;
                    update.ExpirationDate = entity.ExpirationDate;
                    update.SecurityCode = entity.SecurityCode;
                    update.Status = entity.Status;

                    _context.PaymentCardModels.Update(update);
                    await _context.SaveChangesAsync();
                    if (update.CreditCardNumber != null)
                    {
                        return new OperationResult()
                        {
                            Message = "Payment is processed: 200 OK",
                            Status = OperationStatus.Updated,
                            Succeeded = true,
                            StatusCode = HttpStatusCode.OK,
                            Payment = entity
                        };
                    }
                    else
                    {
                        return new OperationResult()
                        {
                            Message = "The request is invalid: 400 Bad request",
                            Status = OperationStatus.NotFound,
                            Succeeded = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            Payment = entity
                        };
                    }

                }

            }

            return new OperationResult()
            {
                Message = "Any error: 500 internal server error",
                Status = OperationStatus.Unknown,
                Succeeded = false,
                StatusCode = HttpStatusCode.BadRequest
            };

        }

        /// <summary>
        /// Check if Credit Card Number is valid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            string text = value as string;
            if (text == null)
            {
                return false;
            }
            text = text.Replace("-", "");
            text = text.Replace(" ", "");
            int num = 0;
            bool flag = false;
            foreach (char current in text.Reverse<char>())
            {
                if (current < '0' || current > '9')
                {
                    return false;
                }
                int i = (int)((current - '0') * (flag ? '\u0002' : '\u0001'));
                flag = !flag;
                while (i > 0)
                {
                    num += i % 10;
                    i /= 10;
                }
            }
            return num % 10 == 0;
        }

    }
}
