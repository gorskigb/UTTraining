using System.Collections.Generic;
using UTTraining.Model;

namespace UTTraining.Infrastructure
{
    public interface IInvoiceDataValidator
    {
        IList<string> ValidateFormData(Invoice invoiceData);
    }
}
