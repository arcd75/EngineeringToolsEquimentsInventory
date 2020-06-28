using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringToolsEquipmentsInventory.Models
{
  public class JigPriceList
    {
        [Key]
        public string Jig_Code { get; set; }

        public string Jig_Name { get; set; }
        public string Classification { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public decimal Unit_Price { get; set; }
        public int EntryId { get; set; }
        public DateTime EntryDate { get; set; }
        public int ModifyId { get; set; }
        public DateTime ModifyDate { get; set; }
    }

    public class Receiving
    {
        [Key]
        public int TranNo { get; set; }

        public string ReceivingNo { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string DeliveryNo { get; set; }
        public string PONo { get; set; }
        public string Jig_Code { get; set; }
        public string Jig_Name { get; set; }
        public int POQty { get; set; }
        public int DeliveryQty { get; set; }
        public int ReceivedQty { get; set; }
        public string Remarks { get; set; }
        public Int64 DetailedId { get; set; }
        public int EntryId { get; set; }
        public DateTime? EntryDate { get; set; }
        public int ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool pause_flag { get; set; }
        public bool transfer_flag { get; set; }
        public string transfer_by { get; set; }
        public DateTime? transfer_date { get; set; }
    }

    public class PurchaseOrder
    {
        [Key]
        public Int64 TranNo { get; set; }

        public string PONo { get; set; }
        public DateTime PODate { get; set; }
        public string CompanyCode { get; set; }
        public string ReferenceNo { get; set; }
        public string IssuerName { get; set; }
        public string OrderType { get; set; }
        public string Location { get; set; }
        public string Customer { get; set; }
        public DateTime DateNeeded { get; set; }
        public string Purpose { get; set; }
        public bool CancelFlag { get; set; }
        public int? CancelBy { get; set; }
        public DateTime? CancelDate { get; set; }
        public bool SDEFlag { get; set; }
        public int? SDEBy { get; set; }
        public DateTime SDEDate { get; set; }
        public Int64 DetailedId { get; set; }
        public string Jig_Code { get; set; }
        public string Jig_Name { get; set; }
        public string Classification { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Remarks { get; set; }
        public Int64 Quantity { get; set; }
        public decimal Unit_Price { get; set; }
        public decimal TotalCost { get; set; }
        public string TranId { get; set; }
        public int EntryId { get; set; }
        public DateTime EntryDate { get; set; }
        public int ModifyId { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool pause_flag { get; set; }
        public bool transfer_flag { get; set; }
        public string transfer_by { get; set; }
        public DateTime? transfer_date { get; set; }
    }

    public class PO
    {
        [Key]
        public Int64 TranNo { get; set; }
        public string PONo { get; set; }
        public DateTime PODate { get; set; }
        public string ReferenceNo { get; set; }
        public string IssuerName { get; set; }
        public Int64 Quantity { get; set; }
        public decimal TotalCost { get; set; }
    }

    public class JigClassification
    {
        [Key]
        public int TranNo { get; set; }

        public string Classification { get; set; }
        public int EntryId { get; set; }
        public DateTime Entrydate { get; set; }
        public int ModifyId { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}