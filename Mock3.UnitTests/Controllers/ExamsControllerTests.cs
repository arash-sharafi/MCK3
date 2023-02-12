using FluentAssertions;
using Mock3.Controllers;
using Mock3.Core;
using Mock3.Core.Models;
using Mock3.Core.OnlinePaymentService;
using Mock3.Core.Repositories;
using Mock3.Core.ViewModels;
using Mock3.UnitTests.Extensions;
using Moq;
using NUnit.Framework;
using System;

namespace Mock3.UnitTests.Controllers
{
    [TestFixture]
    class ExamsControllerTests
    {
        private ExamsController _controller;
        private Mock<IVoucherRepository> _mockVouchersRepository;

        public ExamsControllerTests()
        {
            _mockVouchersRepository = new Mock<IVoucherRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(v => v.Vouchers).Returns(_mockVouchersRepository.Object);

            var mockOnlinePayment = new Mock<IOnlinePayment>();

            _controller = new ExamsController(mockUnitOfWork.Object, mockOnlinePayment.Object);

            _controller.MockCurrentUser("1", "user1@domain.com");
        }

        [Test]
        public void Register_InvalidVoucher_ThrowsNullReferenceException()
        {
            var examId = 1;

            _controller.Invoking(c => c.Register(examId, new RegisterExamViewModel()))
                .Should().ThrowAsync<NullReferenceException>();
        }

        [Test]
        public void Register_ExpiredVoucher_ThrowsInvalidOperationException()
        {
            var voucher = Voucher.Create("1", "user1@domain.com", "1400/01/01");
            _mockVouchersRepository.Setup(r => r.GetVoucherByVoucherNumber("1")).Returns(voucher);
            var examId = 1;

            _controller.Invoking(c => c.Register(examId, 
                    new RegisterExamViewModel() { VoucherNo = "1" }))
                .Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Expired Voucher");
        }
    }
}
