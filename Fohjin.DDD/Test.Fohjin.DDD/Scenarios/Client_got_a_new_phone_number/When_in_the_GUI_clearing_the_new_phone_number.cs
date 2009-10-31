﻿using System;
using System.Collections.Generic;
using Fohjin.DDD.BankApplication.Presenters;
using Fohjin.DDD.BankApplication.Views;
using Fohjin.DDD.Contracts;
using Fohjin.DDD.Reporting.Dto;
using Moq;

namespace Test.Fohjin.DDD.Scenarios.Client_got_a_new_phone_number
{
    public class When_in_the_GUI_clearing_the_new_phone_number : PresenterTestFixture<ClientDetailsPresenter>
    {
        private readonly Guid _clientId = Guid.NewGuid();
        private ClientDetailsReport _clientDetailsReport;
        private List<ClientDetailsReport> _clientDetailsReports;

        protected override void SetupDependencies()
        {
            _clientDetailsReport = new ClientDetailsReport(_clientId, "Client Name", "Street", "123", "5000", "Bergen", "1234567890");
            _clientDetailsReports = new List<ClientDetailsReport> { _clientDetailsReport };
            OnDependency<IReportingRepository>()
                .Setup(x => x.GetByExample<ClientDetailsReport>(It.IsAny<object>()))
                .Returns(_clientDetailsReports);
        }

        protected override void Given()
        {
            Presenter.SetClient(new ClientReport(_clientId, "Client Name"));
            Presenter.Display();
            On<IClientDetailsView>().ValueFor(x => x.ClientName).IsSetTo("Client name");
            On<IClientDetailsView>().ValueFor(x => x.PhoneNumber).IsSetTo("1234567890");
            On<IClientDetailsView>().ValueFor(x => x.Street).IsSetTo("Street");
            On<IClientDetailsView>().ValueFor(x => x.StreetNumber).IsSetTo("123");
            On<IClientDetailsView>().ValueFor(x => x.PostalCode).IsSetTo("5000");
            On<IClientDetailsView>().ValueFor(x => x.City).IsSetTo("Bergen");
            On<IClientDetailsView>().FireEvent(x => x.OnFormElementGotChanged += null);
            On<IClientDetailsView>().FireEvent(x => x.OnInitiateClientPhoneNumberChanged += null);
        }

        protected override void When()
        {
            On<IClientDetailsView>().ValueFor(x => x.PhoneNumber).IsSetTo("1234567890");
            On<IClientDetailsView>().FireEvent(x => x.OnFormElementGotChanged += null);
        }

        [Then]
        public void Then_the_save_button_will_be_enabled()
        {
            On<IClientDetailsView>().VerifyThat.Method(x => x.DisableSaveButton()).WasCalled();
        }
    }
}