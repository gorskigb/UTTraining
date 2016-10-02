using System;
using System.Linq;
using UTTraining.Infrastructure;
using UTTraining.Interface;
using UTTraining.Model;

namespace UTTraining.Logic
{
    public class SageIntegrationLogic : ISageIntegrationLogic
    {
        IInvoiceDataValidator invoiceDataValidator;

        public SageIntegrationLogic(IInvoiceDataValidator dataValidator)
        {
            invoiceDataValidator = dataValidator;
        }

        public LogicResult<ProcessedData> ProcessFormData(Invoice data)
        {
            if (data == null)
                throw new ArgumentException();

            var validationResult = invoiceDataValidator.ValidateFormData(data);

            if (validationResult.Any())
                return new LogicResult<ProcessedData>(validationResult);

            var processedData = new ProcessedData();

            SetAmounts(processedData, data);

            return new LogicResult<ProcessedData>(processedData);
        }

        private void SetAmounts(ProcessedData processedData, Invoice data)
        {
            processedData.VatAmount = data.VatAmount;
            processedData.Gross = data.Gross;
            processedData.Net = data.Gross - data.Gross;
        }
    }

}
