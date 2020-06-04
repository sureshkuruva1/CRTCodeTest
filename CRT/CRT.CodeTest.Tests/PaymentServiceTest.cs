using CRT.CodeTest.Data;
using CRT.CodeTest.Services;
using CRT.CodeTest.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRT.CodeTest.Tests
{
    [TestClass]
    public class PaymentServiceTest
    {
        private readonly Mock<IAccountDataStore> _mockAccountDataStore;
        private readonly IPaymentService _paymentService;
        private const string AccountNumber = "1234567890";
        public PaymentServiceTest()
        {
            _mockAccountDataStore = new Mock<IAccountDataStore>();
            _paymentService = new PaymentService(_mockAccountDataStore.Object, new ValidationService());
        }

        [TestMethod]
        public void AccountNotFound_ShouldReturnFalse_Test()
        {
            _mockAccountDataStore.Setup(x => x.GetAccount(AccountNumber)).Returns(() => null);

            var actualResult = _paymentService.MakePayment(new MakePaymentRequest()
            {
                DebtorAccountNumber = AccountNumber,
                PaymentScheme = PaymentScheme.Bacs,
                Amount = 10.00m
            });

            Assert.IsFalse(actualResult.Success);
        }

        [TestMethod]
        [DataRow(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs,AccountStatus.Disabled)]
        [DataRow(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs,AccountStatus.InboundPaymentsOnly)]
        [DataRow(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs,AccountStatus.Live)]
        [DataRow(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps,AccountStatus.Live)]
        [DataRow(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments,AccountStatus.Disabled)]
        [DataRow(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments, AccountStatus.InboundPaymentsOnly)]
        [DataRow(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments, AccountStatus.Live)]
        public void PaymentSuccessful_ShouldReturnTrue_Test(AllowedPaymentSchemes allowedPaymentScheme, PaymentScheme paymentScheme,AccountStatus accountStatus)
        {
            var testAccount = new Account()
            {
                AccountNumber = AccountNumber,
                AllowedPaymentSchemes = allowedPaymentScheme,
                Balance = 5000,
                Status = accountStatus
            };

            _mockAccountDataStore.Setup(x => x.GetAccount(AccountNumber)).Returns(() => testAccount);

            var actualResult = _paymentService.MakePayment(new MakePaymentRequest()
            {
                DebtorAccountNumber = AccountNumber,
                PaymentScheme = paymentScheme,
                Amount = 1000
            });

            Assert.IsTrue(actualResult.Success);
        }

        [TestMethod]
        [DataRow(AllowedPaymentSchemes.Bacs, PaymentScheme.Chaps)]
        [DataRow(AllowedPaymentSchemes.Bacs, PaymentScheme.FasterPayments)]
        [DataRow(AllowedPaymentSchemes.Chaps, PaymentScheme.Bacs)]
        [DataRow(AllowedPaymentSchemes.Chaps, PaymentScheme.FasterPayments)]
        [DataRow(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Chaps)]
        [DataRow(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Bacs)]
        public void PaymentFailed_InvalidPaymentScheme_ShouldReturnFalse_Test(AllowedPaymentSchemes allowedPaymentScheme, PaymentScheme paymentScheme)
        {
            var testAccount = new Account()
            {
                AccountNumber = AccountNumber,
                AllowedPaymentSchemes = allowedPaymentScheme,
                Balance = 5000,
                Status = AccountStatus.Live
            };

            _mockAccountDataStore.Setup(x => x.GetAccount(AccountNumber)).Returns(() => testAccount);

            var actualResult = _paymentService.MakePayment(new MakePaymentRequest()
            {
                DebtorAccountNumber = AccountNumber,
                PaymentScheme = paymentScheme,
                Amount = 1000
            });

            Assert.IsFalse(actualResult.Success);
        }

        [TestMethod]
        public void PaymentSchemeFasterPayments_InsufficientBalance_ShouldReturnFalse_Test()
        {
            _mockAccountDataStore.Setup(x => x.GetAccount(AccountNumber)).Returns(() => new Account()
            {
                AccountNumber = AccountNumber,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 1000,
                Status = AccountStatus.Live
            });

            var actualResult = _paymentService.MakePayment(new MakePaymentRequest()
            {
                DebtorAccountNumber = AccountNumber,
                PaymentScheme = PaymentScheme.FasterPayments,
                Amount = 5000
            });

            Assert.IsFalse(actualResult.Success);
        }

        [TestMethod]
        public void PaymentSchemeChap_AccountStatusNotLive_ShouldReturnFalse_Test()
        {
            _mockAccountDataStore.Setup(x => x.GetAccount(AccountNumber)).Returns(() => new Account()
            {
                AccountNumber = AccountNumber,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Balance = 5000,
                Status = AccountStatus.Disabled
            });

            var actualResult = _paymentService.MakePayment(new MakePaymentRequest()
            {
                DebtorAccountNumber = AccountNumber,
                PaymentScheme = PaymentScheme.Chaps,
                Amount = 1000
            });

            Assert.IsFalse(actualResult.Success);
        }
    }
}
