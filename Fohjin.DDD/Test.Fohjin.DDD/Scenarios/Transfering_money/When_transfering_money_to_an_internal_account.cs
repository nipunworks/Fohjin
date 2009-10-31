﻿using System;
using System.Collections.Generic;
using Fohjin.DDD.Bus;
using Fohjin.DDD.Commands;
using Fohjin.DDD.Contracts;
using Fohjin.DDD.Domain;
using Fohjin.DDD.Reporting.Dto;
using Fohjin.DDD.Services;
using Moq;

namespace Test.Fohjin.DDD.Scenarios.Transfering_money
{
    public class When_transfering_money_to_an_internal_account : BaseTestFixture<MoneyTransferService>
    {
        protected override void SetupDependencies()
        {
            OnDependency<IReportingRepository>()
                .Setup(x => x.GetByExample<AccountReport>(It.IsAny<object>()))
                .Returns(new List<AccountReport> { new AccountReport(Guid.NewGuid(), Guid.NewGuid(), "AccountName", "target account number") });
        }

        protected override void Given()
        {
            // !!! This is DEMO code !!!
            // Setup the SystemRandom class to return the value where the account is found
            SystemRandom.Next = (min, max) => 0;
            SystemTimer.ByPassTimer();
        }

        protected override void When()
        {
            SubjectUnderTest.Send(new MoneyTransfer("source account number", "target account number", 123.45M));
        }

        [Then]
        public void Then_the_newly_created_account_will_be_saved()
        {
            OnDependency<ICommandBus>().Verify(x => x.Publish(It.IsAny<ReceiveMoneyTransferCommand>()));
        }

        protected override void Finally()
        {
            SystemTimer.Reset();
            SystemRandom.Reset();
        }
    }
}