using CRT.CodeTest.Data;
using CRT.CodeTest.Types;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

namespace CRT.CodeTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IValidationService _validationService;
        public PaymentService(IAccountDataStore accountDataStore, IValidationService validationService)
        {
            _accountDataStore = accountDataStore;
            _validationService = validationService;
        }
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            Account account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

            MakePaymentResult result = _validationService.Validate(request.PaymentScheme, account, request);
           
            if (result.Success)
            {
                account.Balance -= request.Amount;
                _accountDataStore.UpdateAccount(account);
            }
            return result;
        }
    }
}
