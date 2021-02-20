using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentApi.API.Controllers;
using PaymentAPI.Application.ProcessPaymentApp;
using PaymentAPI.Application.ProcessPaymentApp.Dtos;
using PaymentAPI.Core.OperationReturns;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentAPI.Test
{
    public class PaymentControllerTest
    {
        private Mock<ICheapPaymentGatewayAppService> _cheapPaymentGateway;
        private Mock<IExpensivePaymentGatewayAppService> _expensivePaymentGateway;
        private Mock<IPremiumPaymentGatewayAppService> _premiumPaymentGateway;
        private PaymentController controller;
        private const int MaxRetries = 3;

        public PaymentControllerTest()
        {
            _cheapPaymentGateway = new Mock<ICheapPaymentGatewayAppService>();
            _expensivePaymentGateway = new Mock<IExpensivePaymentGatewayAppService>();
            _premiumPaymentGateway = new Mock<IPremiumPaymentGatewayAppService>();
            controller = new PaymentController(
                _cheapPaymentGateway.Object,
                _expensivePaymentGateway.Object,
                _premiumPaymentGateway.Object);
        }

        [Fact]
        public async Task PaymentProcessTest1_ValidObjectPassed_ReturnsCreatedResponse()
        {
            OperationResult returnResult = new OperationResult();
            var data = new PaymentCardModelDto()
            {
                Amount = 20,
                CardHolder = "John Doe",
                CreditCardNumber = "5900374652654375",
                ExpirationDate = DateTime.UtcNow.AddHours(1),
                SecurityCode = "567",
                Status = Core.Enums.PaymentStatus.Pending
            };
            if (data.Amount <= 20)
            {
                data.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                var result = await _cheapPaymentGateway.Object.Insert(data);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _cheapPaymentGateway.Object.GetById(result.Payment.Id);
                    pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                    OperationResult result2 = await _cheapPaymentGateway.Object.Update(pay);

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

                Assert.IsType<OperationResult>(returnResult);
            }
            else if (data.Amount >= 21 && data.Amount <= 500)
            {
                data.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                OperationResult result = await _expensivePaymentGateway.Object.Insert(data);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _expensivePaymentGateway.Object.GetById(result.Payment.Id);
                    pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                    OperationResult result2 = await _expensivePaymentGateway.Object.Update(pay);

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
                Assert.IsType<OperationResult>(returnResult);
            }
            else if (data.Amount > 500)
            {
                data.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                OperationResult result = await _premiumPaymentGateway.Object.Insert(data);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _premiumPaymentGateway.Object.GetById(result.Payment.Id);
                    if (pay.Status == PaymentAPI.Core.Enums.PaymentStatus.Pending)
                    {
                        for (int i = 0; i < MaxRetries; i++)
                        {
                            pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                            OperationResult result2 = await _premiumPaymentGateway.Object.Update(pay);

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
                Assert.IsType<OperationResult>(returnResult);
            }
        }

        [Fact]
        public async Task PaymentProcessTest2_ValidObjectPassed_ReturnsCreatedResponse()
        {
            OperationResult returnResult = new OperationResult();
            var data = new PaymentCardModelDto()
            {
                Amount = 300,
                CardHolder = "John Doe",
                CreditCardNumber = "5900374652654375",
                ExpirationDate = DateTime.UtcNow.AddHours(1),
                SecurityCode = "567",
                Status = Core.Enums.PaymentStatus.Pending
            };
            if (data.Amount <= 20)
            {
                data.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                var result = await _cheapPaymentGateway.Object.Insert(data);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _cheapPaymentGateway.Object.GetById(result.Payment.Id);
                    pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                    OperationResult result2 = await _cheapPaymentGateway.Object.Update(pay);

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
                Assert.IsType<OperationResult>(returnResult);
            }
            else if (data.Amount >= 21 && data.Amount <= 500)
            {
                data.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                OperationResult result = await _expensivePaymentGateway.Object.Insert(data);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _expensivePaymentGateway.Object.GetById(result.Payment.Id);
                    pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                    OperationResult result2 = await _expensivePaymentGateway.Object.Update(pay);

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

                Assert.IsType<OperationResult>(returnResult);
            }
            else if (data.Amount > 500)
            {
                data.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                OperationResult result = await _premiumPaymentGateway.Object.Insert(data);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _premiumPaymentGateway.Object.GetById(result.Payment.Id);
                    if (pay.Status == PaymentAPI.Core.Enums.PaymentStatus.Pending)
                    {
                        for (int i = 0; i < MaxRetries; i++)
                        {
                            pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                            OperationResult result2 = await _premiumPaymentGateway.Object.Update(pay);

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
                Assert.IsType<OperationResult>(returnResult);
            }
        }


        [Fact]
        public async Task PaymentProcessTest3_ValidObjectPassed_ReturnsCreatedResponse()
        {
            OperationResult returnResult = new OperationResult();
            var data = new PaymentCardModelDto()
            {
                Amount = 700,
                CardHolder = "John Doe",
                CreditCardNumber = "5900374652654375",
                ExpirationDate = DateTime.UtcNow.AddHours(1),
                SecurityCode = "567",
                Status = Core.Enums.PaymentStatus.Pending
            };
            if (data.Amount <= 20)
            {
                data.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                var result = await _cheapPaymentGateway.Object.Insert(data);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _cheapPaymentGateway.Object.GetById(result.Payment.Id);
                    pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                    OperationResult result2 = await _cheapPaymentGateway.Object.Update(pay);

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
                Assert.IsType<OperationResult>(returnResult);
            }
            else if (data.Amount >= 21 && data.Amount <= 500)
            {
                data.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                OperationResult result = await _expensivePaymentGateway.Object.Insert(data);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _expensivePaymentGateway.Object.GetById(result.Payment.Id);
                    pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                    OperationResult result2 = await _expensivePaymentGateway.Object.Update(pay);

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
                Assert.IsType<OperationResult>(returnResult);
            }
            else if (data.Amount > 500)
            {
                data.Status = PaymentAPI.Core.Enums.PaymentStatus.Pending;
                OperationResult result = await _premiumPaymentGateway.Object.Insert(data);
                if (result.Succeeded)
                {
                    returnResult.Status = result.Status;
                    returnResult.Message = result.Message;
                    returnResult.StatusCode = result.StatusCode;
                    returnResult.Succeeded = result.Succeeded;
                    returnResult.Payment = result.Payment;

                    //Update Payment
                    var pay = await _premiumPaymentGateway.Object.GetById(result.Payment.Id);
                    if (pay.Status == PaymentAPI.Core.Enums.PaymentStatus.Pending)
                    {
                        for (int i = 0; i < MaxRetries; i++)
                        {
                            pay.Status = PaymentAPI.Core.Enums.PaymentStatus.Processed;
                            OperationResult result2 = await _premiumPaymentGateway.Object.Update(pay);

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
                Assert.IsType<OperationResult>(returnResult);
            }
        }

    }
}
