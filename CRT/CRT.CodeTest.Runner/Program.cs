using CRT.CodeTest.Data;
using CRT.CodeTest.Services;
using CRT.CodeTest.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CRT.CodeTest.Runner
{
    class Program
    {
        static void Main()
        {
            //Console.WriteLine("Hello World!");
            //setup DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IAccountDataStore, AccountDataStore>()
                .AddSingleton<IValidationService, ValidationService>()
                .AddSingleton<IPaymentService, PaymentService>()
                .BuildServiceProvider();

            MakePaymentRequest paymentRequest = new MakePaymentRequest
            {
                Amount = 2000,
                DebtorAccountNumber = "1234567890",
                PaymentScheme = PaymentScheme.Chaps
            };

            var paymentService = serviceProvider.GetService<IPaymentService>();
            var result = paymentService.MakePayment(paymentRequest);

            Console.WriteLine(result.Success);
            Console.ReadLine();
        }
    }
}
