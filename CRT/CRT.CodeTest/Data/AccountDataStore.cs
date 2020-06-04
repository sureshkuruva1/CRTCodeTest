using CRT.CodeTest.Types;

namespace CRT.CodeTest.Data
{
    public class AccountDataStore : IAccountDataStore
    {
        public Account GetAccount(string accountNumber)
        {
            // Access database to retrieve account, code removed for brevity 
            return new Account { 
                AccountNumber = "1234567890",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Balance = 5000,
                Status = AccountStatus.Live
            };
        }

        public void UpdateAccount(Account account)
        {
            // Update account in database, code removed for brevity
        }
    }
}
