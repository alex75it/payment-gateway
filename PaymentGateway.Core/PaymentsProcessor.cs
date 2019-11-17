﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PaymentGateway.Core.Bank;
using PaymentGateway.Core.Models;
using PaymentGateway.DataLayer;
using PaymentGateway.Models;

namespace PaymentGateway.Core
{
    public interface IPaymentsProcessor
    {
        PaymentCreationResult CreatePayment(PaymentCreationData data);
        Payment GetPayment(string paymentId);
    }

    public class PaymentsProcessor : IPaymentsProcessor
    {
        private ILogger<PaymentsProcessor> logger;
        private IPaymentsRepository paymentsRepository;
        private IBankClient bankClient;

        public PaymentsProcessor(ILogger<PaymentsProcessor> logger, IPaymentsRepository paymentsRepository, IBankClient bankClient)
        {
            this.logger = logger;
            this.paymentsRepository = paymentsRepository;
            this.bankClient = bankClient;
        }

        public PaymentCreationResult CreatePayment(PaymentCreationData data)
        {
            try
            {
                var payment = bankClient.CreatePayment(data);
                return PaymentCreationResult.Success(payment);
            }
            catch (Exception exc)
            {
                logger.LogError(exc, "Failed to create payment");
                return PaymentCreationResult.Fail("General error on creating payment");
            }
        }

        public Payment GetPayment(string paymentId)
        {
            return paymentsRepository.Get(paymentId);
        }
    }
}
