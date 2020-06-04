using CRT.CodeTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.CodeTest.Validators
{
    public interface IPaymentValidator
    {
        MakePaymentResult IsValid(Account account, MakePaymentRequest paymentRequest);
    }
}
