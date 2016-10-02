using System.Collections.Generic;
using System.Linq;
using UTTraining.Infrastructure;
using UTTraining.Model;

namespace UTTraining.Logic
{
    public class InvoiceDataValidator : IInvoiceDataValidator
    {
        private List<string> validationMessages;

        public InvoiceDataValidator()
        {
            validationMessages = new List<string>();
        }

        public IList<string> ValidateFormData(Invoice invoiceData)
        {
            if(!ValidateIfThereIsAtLeastOnePosition(invoiceData))
                validationMessages.Add("At least one invoice position is required!");

            if (!GrossIsGreaterThanVatAmount(invoiceData))
                validationMessages.Add("Gross must be greater than vat amount!");

            return validationMessages;
        }

        private bool ValidateIfThereIsAtLeastOnePosition(Invoice invoiceData)
        {
            var isValid = true;

            if (!invoiceData.Positions.Any())
                isValid = false;

            return isValid;
        }

        private bool GrossIsGreaterThanVatAmount(Invoice invoiceData)
        {
            var isValid = true;

            if (invoiceData.VatAmount >= invoiceData.Gross)
                isValid = false;

            return isValid;
        }

    }
}
