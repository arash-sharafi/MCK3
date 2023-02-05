using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mock3.Core.OnlinePaymentService;
using Mock3.Core.OnlinePaymentService.Contracts;

namespace Mock3.Infrastructure.Services.OnlinePayment
{
    public class OnlinePayment:IOnlinePayment
    {
        public PaymentResponse Payment(PaymentRequest request)
        {
            return new PaymentResponse()
            {
                Status = "ACCEPTED",
                Error = "",
                ReferenceNumber = "44588779654"
            };
        }

        public PaymentVerifyResponse VerifyPayment(PaymentVerifyRequest request)
        {
            return new PaymentVerifyResponse()
            {
                Error = "",
                Status = "SUCCESSFUL",
                AmountPaid = request.Price,
                Message = "خرید با موفقیت انجام شد"
            };
        }
    }
}