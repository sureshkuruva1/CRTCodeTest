using CRT.CodeTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.CodeTest.Validators
{
    public abstract class PaymentValidatorBase : IPaymentValidator
    {
        protected static bool IsValidAccount(Account account)
        {
            return account != null;
        }
        public abstract MakePaymentResult IsValid(Account account, MakePaymentRequest paymentRequest);
    }
}
