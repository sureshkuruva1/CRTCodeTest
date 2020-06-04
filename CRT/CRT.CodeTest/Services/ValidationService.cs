using CRT.CodeTest.Types;
using CRT.CodeTest.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.CodeTest.Services
{
    public class ValidationService : IValidationService
    {
        private readonly Dictionary<PaymentScheme, IPaymentValidator> _paymentValidations;

        public ValidationService()
        {
            _paymentValidations = new Dictionary<PaymentScheme, IPaymentValidator>()
            {
                {PaymentScheme.Chaps, new ChapsPaymentValidator()},
                {PaymentScheme.FasterPayments, new FasterPaymentValidator()},
                {PaymentScheme.Bacs, new BacsPaymentValidator()}
            };
        }

        public MakePaymentResult Validate(PaymentScheme paymentScheme, Account account, MakePaymentRequest paymentRequest)
        {
            return _paymentValidations[paymentScheme].IsValid(account, paymentRequest);
        }
    }
}
