using CRT.CodeTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.CodeTest.Services
{
    public interface IValidationService
    {
        MakePaymentResult Validate(PaymentScheme paymentScheme, Account account, MakePaymentRequest paymentRequest);
    }
}
