using System.Collections.Generic;
using UTTraining.Model.Form;

namespace UTTraining.Model
{
    public class Invoice
    {
        public Invoice()
        {
            Positions = new List<InvoicePositions>();
        }

        public string InvoiceNumber { get; set; }

        public decimal Gross { get; set; }

        public decimal VatAmount { get; set; }

        public IList<InvoicePositions> Positions { get; set; }
    }
}
