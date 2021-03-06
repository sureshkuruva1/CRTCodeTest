﻿using CRT.CodeTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.CodeTest.Validators
{
    public class BacsPaymentValidator : PaymentValidatorBase
    {
        public override MakePaymentResult IsValid(Account account, MakePaymentRequest paymentRequest)
        {
            if (!IsValidAccount(account))
            {
                return new MakePaymentResult { Success = false };
            }

            return new MakePaymentResult() { Success = account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs) };
        }
    }
}
