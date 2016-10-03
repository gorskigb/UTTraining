using NUnit.Framework;
using System;
using UTTraining.Interface;
using UTTraining.Logic;
using UTTraining.Model;
using FakeItEasy;
using UTTraining.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace UTTraining.Tests.SageIntegrationLogicTests
{
    [TestFixture]
    public class ProcessFormDataTests
    {
        [TestCase]
        public void ProcessFormData_NullArgument_ThrowsArgumentException()
        {
            var invoiceDataValidationStub = CreateInvoiceDataValidatorFake();
            var logic = GetSageIntegrationLogicInstance(invoiceDataValidationStub);

            Invoice argument = null;

            Assert.Throws<ArgumentException>(() => logic.ProcessFormData(argument));
        }

        [TestCase]
        public void ProcessFormData_NotNullArgument_ExceptionWasNotThrown()
        {
            var invoiceDataValidationStub = CreateInvoiceDataValidatorFake();
            var logic = GetSageIntegrationLogicInstance(invoiceDataValidationStub);

            Invoice argument = A.Dummy<Invoice>();

            Assert.DoesNotThrow(() => logic.ProcessFormData(argument));
        }

        [TestCase]
        public void ProcessFormData_NotNullArgument_ValidateFormDataCalled()
        {
            var invoiceDataValidationMock = CreateInvoiceDataValidatorFake();
            var logic = GetSageIntegrationLogicInstance(invoiceDataValidationMock);
            Invoice argument = A.Dummy<Invoice>();

            logic.ProcessFormData(argument);

            A.CallTo(() => invoiceDataValidationMock
                .ValidateFormData(argument))
                .MustHaveHappened();
        }

        [TestCase]
        public void ProcessFormData_InvalidFormData_SuccededIsFalse()
        {
            Invoice argument = A.Dummy<Invoice>();

            var invoiceDataValidationStub = CreateInvoiceDataValidatorFake(argument, A.CollectionOfDummy<string>(1));
            var logic = GetSageIntegrationLogicInstance(invoiceDataValidationStub);

            var result = logic.ProcessFormData(argument);

            Assert.False(result.Succeded);
        }

        [TestCase]
        public void ProcessFormData_InvalidFormData_ReturnsAllMessagesFromValidationLogic()
        {
            var validationMessagesCount = 10;
            Invoice argument = A.Dummy<Invoice>();

            var invoiceDataValidationStub = CreateInvoiceDataValidatorFake(argument, A.CollectionOfDummy<string>(validationMessagesCount));
            var logic = GetSageIntegrationLogicInstance(invoiceDataValidationStub);

            var result = logic.ProcessFormData(argument);

            Assert.That(result.ErrorMessages.Count.Equals(validationMessagesCount));
        }

        [TestCase]
        public void ProcessFormData_ValidFormData_SuccededIsTrue()
        {
            Invoice argument = A.Dummy<Invoice>();

            var invoiceDataValidationStub = CreateInvoiceDataValidatorFake();
            var logic = GetSageIntegrationLogicInstance(invoiceDataValidationStub);

            var result = logic.ProcessFormData(argument);

            Assert.That(result.Succeded);
        }

        [TestCase]
        public void ProcessFormData_ValidFormData_ErrorMessagesCollectionIsEmpty()
        {
            Invoice argument = A.Dummy<Invoice>();

            var invoiceDataValidationStub = CreateInvoiceDataValidatorFake();
            var logic = GetSageIntegrationLogicInstance(invoiceDataValidationStub);

            var result = logic.ProcessFormData(argument);

            Assert.That(!result.ErrorMessages.Any());
        }

        [TestCase]
        public void ProcessFormData_ValidFormData_ResultObjectIsNotNull()
        {
            Invoice argument = A.Dummy<Invoice>();

            var invoiceDataValidationStub = CreateInvoiceDataValidatorFake();
            var logic = GetSageIntegrationLogicInstance(invoiceDataValidationStub);

            var result = logic.ProcessFormData(argument);

            Assert.NotNull(result.Result);
        }

        [TestCase]
        public void ProcessFormData_GrossAmountWasProvided_TheSameGrossAmountWasReturned()
        {
            decimal gross = 10.71m;
            Invoice argument = new Invoice { Gross = gross };

            var invoiceDataValidationStub = CreateInvoiceDataValidatorFake();
            var logic = GetSageIntegrationLogicInstance(invoiceDataValidationStub);

            var result = logic.ProcessFormData(argument);

            Assert.Equals(result.Result.Gross, gross);
        }

        [TestCase]
        public void ProcessFormData_VatAmountIsZero_GrossEqualsNet()
        {
            Invoice argument = new Invoice { Gross = 10, VatAmount = Decimal.Zero };

            var invoiceDataValidationStub = CreateInvoiceDataValidatorFake();
            var logic = GetSageIntegrationLogicInstance(invoiceDataValidationStub);

            var result = logic.ProcessFormData(argument);

            Assert.Equals(result.Result.Gross, result.Result.Net);
        }

        [TestCase]
        public void ProcessFormData_VatAmountIsGreaterThanZero_NetEqualsGrossMinusVat()
        {
            decimal netValue = 8;
            Invoice argument = new Invoice { Gross = 10, VatAmount = 2 };

            var invoiceDataValidationStub = CreateInvoiceDataValidatorFake();
            var logic = GetSageIntegrationLogicInstance(invoiceDataValidationStub);

            var result = logic.ProcessFormData(argument);

            Assert.Equals(result.Result.Net, netValue);
        }

        private IInvoiceDataValidator CreateInvoiceDataValidatorFake()
        {
            return CreateInvoiceDataValidatorFake(A.Dummy<Invoice>(), A.Dummy<IList<string>>());
        }

        private IInvoiceDataValidator CreateInvoiceDataValidatorFake(Invoice formData)
        {
            return CreateInvoiceDataValidatorFake(formData, A.Dummy<IList<string>>());
        }

        private IInvoiceDataValidator CreateInvoiceDataValidatorFake(Invoice formData,IList<string> requiredResult)
        {
            var invoiceDataValidatorStub = A.Fake<IInvoiceDataValidator>();
            A.CallTo(() => invoiceDataValidatorStub.ValidateFormData(formData)).Returns(requiredResult);

            return invoiceDataValidatorStub;
        }

        private ISageIntegrationLogic GetSageIntegrationLogicInstance(IInvoiceDataValidator invoiceDataValidatorFake)
        {
            return new SageIntegrationLogic(invoiceDataValidatorFake);
        }
    }
}
