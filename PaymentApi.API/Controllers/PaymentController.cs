using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Application.ProcessPaymentApp;
using PaymentAPI.Application.ProcessPaymentApp.Dtos;
using PaymentAPI.Core.OperationReturns;

namespace PaymentApi.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ICheapPaymentGatewayAppService _cheapPaymentGateway;
        private readonly IExpensivePaymentGatewayAppService _expensivePaymentGateway;
        private readonly IPremiumPaymentGatewayAppService _premiumPaymentGateway;
        private const int MaxRetries = 3;

        public PaymentController(
            ICheapPaymentGatewayAppService cheapPaymentGateway,
            IExpensivePaymentGatewayAppService expensivePaymentGateway,
            IPremiumPaymentGatewayAppService premiumPaymentGateway)
        {
            _cheapPaymentGateway = cheapPaymentGateway;
            _expensivePaymentGateway = expensivePaymentGateway;
            _premiumPaymentGateway = premiumPaymentGateway;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<OperationResult>> PaymentProcess(PaymentCardModelDto Payment)
        {
            OperationResult returnResult = new OperationResult();
            if (Payment.Amount <= 20)
            {
                Payment.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                OperationResult result = await _cheapPaymentGateway.Insert(Payment);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _cheapPaymentGateway.GetById(result.Payment.Id);
                    pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                    OperationResult result2 = await _cheapPaymentGateway.Update(pay);

                    if (result2.Succeeded)
                    {
                        returnResult.Status = result2.Status;
                        returnResult.Message = result2.Message;
                        returnResult.StatusCode = result2.StatusCode;
                        returnResult.Succeeded = result2.Succeeded;
                        returnResult.Payment = result2.Payment;
                    }
                    else
                    {
                        returnResult.Status = result2.Status;
                        returnResult.Message = result2.Message;
                        returnResult.StatusCode = result2.StatusCode;
                        returnResult.Succeeded = result2.Succeeded;
                        returnResult.Payment = result2.Payment;
                    }
                }
                else
                {
                    //Update Payment
                    var pay = await _cheapPaymentGateway.GetById(result.Payment.Id);
                    pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Failed;
                    OperationResult result3 = await _cheapPaymentGateway.Update(pay);
                    returnResult.Status = result3.Status;
                    returnResult.Message = result3.Message;
                    returnResult.StatusCode = result3.StatusCode;
                    returnResult.Succeeded = result3.Succeeded;
                    returnResult.Payment = result3.Payment;
                }

            }
            else if (Payment.Amount >= 21 && Payment.Amount <= 500)
            {
                Payment.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                OperationResult result = await _expensivePaymentGateway.Insert(Payment);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _expensivePaymentGateway.GetById(result.Payment.Id);
                    pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                    OperationResult result2 = await _expensivePaymentGateway.Update(pay);

                    if (result2.Succeeded)
                    {
                        returnResult.Status = result2.Status;
                        returnResult.Message = result2.Message;
                        returnResult.StatusCode = result2.StatusCode;
                        returnResult.Succeeded = result2.Succeeded;
                        returnResult.Payment = result2.Payment;
                    }
                    else
                    {
                        returnResult.Status = result2.Status;
                        returnResult.Message = result2.Message;
                        returnResult.StatusCode = result2.StatusCode;
                        returnResult.Succeeded = result2.Succeeded;
                        returnResult.Payment = result2.Payment;
                    }
                }
                else
                {
                    //Update Payment
                    var pay = await _expensivePaymentGateway.GetById(result.Payment.Id);
                    pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Failed;
                    OperationResult result3 = await _expensivePaymentGateway.Update(pay);
                    returnResult.Status = result3.Status;
                    returnResult.Message = result3.Message;
                    returnResult.StatusCode = result3.StatusCode;
                    returnResult.Succeeded = result3.Succeeded;
                    returnResult.Payment = result3.Payment;
                }
            }
            else if (Payment.Amount > 500)
            {

                Payment.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                OperationResult result = await _premiumPaymentGateway.Insert(Payment);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _premiumPaymentGateway.GetById(result.Payment.Id);
                    if (pay.Status == PaymentAPI.Core.Enums.PaymentStatus.Pending)
                    {
                        for (int i = 0; i < MaxRetries; i++)
                        {
                            pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                            OperationResult result2 = await _premiumPaymentGateway.Update(pay);

                            if (result2.Succeeded)
                            {
                                returnResult.Status = result2.Status;
                                returnResult.Message = result2.Message;
                                returnResult.StatusCode = result2.StatusCode;
                                returnResult.Succeeded = result2.Succeeded;
                                returnResult.Payment = result2.Payment;
                            }
                            else
                            {
                                returnResult.Status = result2.Status;
                                returnResult.Message = result2.Message;
                                returnResult.StatusCode = result2.StatusCode;
                                returnResult.Succeeded = result2.Succeeded;
                                returnResult.Payment = result2.Payment;
                            }
                        }

                    }

                }
                else
                {
                    //Update Payment
                    var pay = await _premiumPaymentGateway.GetById(result.Payment.Id);
                    pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Failed;
                    OperationResult result3 = await _premiumPaymentGateway.Update(pay);

                    returnResult.Status = result3.Status;
                    returnResult.Message = result3.Message;
                    returnResult.StatusCode = result3.StatusCode;
                    returnResult.Succeeded = result3.Succeeded;
                    returnResult.Payment = result3.Payment;
                }


            }
            return returnResult;

        }



    }
}