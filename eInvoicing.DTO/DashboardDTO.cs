using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eInvoicing.DTO.Base;

namespace eInvoicing.DTO
{
    public class DashboardDTO: BaseDTO
    {
        public List<GoodsModel> goodsModel { get; set; }
        public string SubmittedInvoiceTotalAmount { get; set; }
        public string SubmittedCreditTotalAmount { get; set; }
        public string SubmittedDebitTotalAmount { get; set; }
        public string SubmittedInvoiceTotalTax { get; set; }
        public string SubmittedCreditTotalTax { get; set; }
        public string SubmittedDebitTotalTax { get; set; }
        public int SubmittedInvoiceCount { get; set; }
        public int SubmittedCreditCount { get; set; }
        public int SubmittedDebitCount { get; set; }
        public int SubmittedDocumentsCount { get; set; }
        public int SubmittedValidDocumentsCount { get; set; }
        public double SubmittedValidDocumentsCountPercentage { get; set; }
        public int SubmittedInValidDocumentsCount { get; set; }
        public double SubmittedInValidDocumentsCountPercentage { get; set; }
        public int SubmittedCanceledDocumentsCount { get; set; }
        public double SubmittedCanceledDocumentsCountPercentage { get; set; }
        public int SubmittedRejectedDocumentsCount { get; set; }
        public double SubmittedRejectedDocumentsCountPercentage { get; set; }
        public int allSubmittedDocumentsCount { get; set; }
        public int allDraftedDocumentsCount { get; set; }
        public double SubmittedDocumentsCountPercentage { get; set; }
        public string ReceivedInvoiceTotalAmount { get; set; }
        public string ReceivedCreditTotalAmount { get; set; }
        public string ReceivedDebitTotalAmount { get; set; }
        public string ReceivedInvoiceTotalTax { get; set; }
        public string ReceivedCreditTotalTax { get; set; }
        public string ReceivedDebitTotalTax { get; set; }
        public int ReceivedInvoiceCount { get; set; }
        public int ReceivedCreditCount { get; set; }
        public int ReceivedDebitCount { get; set; }
        public int ReceivedDocumentsCount { get; set; }
        public int ReceivedValidDocumentsCount { get; set; }
        public double ReceivedValidDocumentsCountPercentage { get; set; }
        public int ReceivedInValidDocumentsCount { get; set; }
        public double ReceivedInValidDocumentsCountPercentage { get; set; }
        public int ReceivedCanceledDocumentsCount { get; set; }
        public double ReceivedCanceledDocumentsCountPercentage { get; set; }
        public int ReceivedRejectedDocumentsCount { get; set; }
        public double ReceivedRejectedDocumentsCountPercentage { get; set; }
        public int ReceivedSubmittedDocumentsCount { get; set; }
        public double ReceivedSubmittedDocumentsCountPercentage { get; set; }
    }
    public class GoodsModel
    {
        public string itemCode { get; set; }
        public string itemDesc { get; set; }
        public int month { get; set; }
        public decimal count { get; set; }
        public string totalAmount { get; set; }
        public string totalTax { get; set; }
    }
}
