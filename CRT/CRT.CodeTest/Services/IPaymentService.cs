using CRT.CodeTest.Types;

namespace CRT.CodeTest.Services
{
    public interface IPaymentService
    {
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
