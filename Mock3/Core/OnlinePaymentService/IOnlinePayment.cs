using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mock3.Core.OnlinePaymentService.Contracts;

namespace Mock3.Core.OnlinePaymentService
{
    public interface IOnlinePayment
    {
        PaymentResponse Payment(PaymentRequest request);
        PaymentVerifyResponse VerifyPayment(PaymentVerifyRequest request);
    }

}
