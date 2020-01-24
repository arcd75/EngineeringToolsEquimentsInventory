using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EngineeringToolsEquipmentsInventory.Models
{

    public class User
    {
        [Key]
        public string UserKey { get; set; }

        public string UserID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Type { get; set; }
    }

    public class Tool
    {
        [Key]
        public string ItemCode { get; set; }

        public string Type { get; set; }
        public string Item { get; set; }
        public string ItemDescription { get; set; }
        public string Brand { get; set; }
        public string Condition { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string PECode { get; set; }
        public DateTime DateDelivered { get; set; }
        public DateTime LastUpdate { get; set; }
    }
    public class Loan
    {
        [Key]
        public string LoanID { get; set; }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public string LoanQuantity { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string LoginName { get; set; }
    }

    public class LoanedTool
    {
        [Key]
        public string LoanToolID { get; set; }

        public string LoanID { get; set; }
        public string ItemCode { get; set; }
        public string PECode { get; set; }
        public string Item { get; set; }
        public string Status { get; set; }
        public string Condition { get; set; }
        public string LoginName { get; set; }
        public string BorrowerName { get; set; }
        public string ReturnLoginName { get; set; }
        public DateTime DateBorrowed { get; set; }
        public DateTime DateReturned { get; set; }

    }

    public class Transaction
    {
        [Key]
        public string TransactionID { get; set; }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public Int32 TotalQuantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string LoginName { get; set; }
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public Int32 NumberofItem { get; set; }
    }

    public class TransactionItem
    {
        [Key]
        public string TransactionItemID { get; set; }

        public string TransactionID { get; set; }
        public string ItemCode { get; set; }
        public string Group { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public Int32 Quantity { get; set; }
        public DateTime Date { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
    }

    public class Consumable
    {
        [Key]
        public string ItemCode { get; set; }

        public string Group { get; set; }
        public string ItemDescription { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public Int32 RemainingQuantity { get; set; }
        public Int32 MaintainingQuantity { get; set; }
    }

    public class Delivery
    {
        [Key]
        public string DeliveryID { get; set; }

        public string DRNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime Date { get; set; }
        public string PONumber { get; set; }
        public string TotalItem { get; set; }
    }

    public class DeliveryItems
    {
        [Key]
        public string DeliveryItemID { get; set; }

        public string DRNumber { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
    public class DeliveryToolItems
    {
        [Key]
        public string DeliveryItemID { get; set; }

        public string DRNumber { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string Brand { get; set; }
        public string PECode { get; set; }
        public DateTime Date { get; set; }
    }

    public class CreateDelivery
    {
        [Key]
        public string DeliveryItemID { get; set; }

        public string DRNumber { get; set; }
        public string ItemCode { get; set; }
        public string ItemCode_Tools { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public int Quantity { get; set; }
        public string PECode { get; set; }
        public DateTime Date { get; set; }
    }

    public class CreateToolDelivery
    {
        [Key]
        public string DeliveryItemID { get; set; }

        public string DRNumber { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string Brand { get; set; }
        public string PECode { get; set; }
        public DateTime Date { get; set; }
    }

    public class ItemType
    {
        [Key]
        public string ID { get; set; }

        public string Type { get; set; }
    }

    public class ProductGroup
    {
        [Key]
        public string GroupID { get; set; }

        public string GroupName { get; set; }
    }

    public class Type
    {
        [Key]
        public string ID { get; set; }

        public string ToolType { get; set; }
    }


    public class SparePart
    {
        [Key]
        public string PartID { get; set; }

        public string Location { get; set; }
        public string Type { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public float Cost { get; set; }
        public float TotalCost { get; set; }
        public int MaintainingQuantity { get; set; }
        public int AvailableQuantity { get; set; }
    }

    public class SparePartTransactionItem
    {
        [Key]
        public string TransactionItemID { get; set; }

        public string TransactionID { get; set; }
        public string ItemCode { get; set; }
        public string PartID { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string ItemName { get; set; }
        public float Cost { get; set; }
        public float TotalCost { get; set; }
        public int Quantity { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string PIC { get; set; }
        public DateTime Date { get; set; }
    }

    public class SparePartTransaction
    {
        [Key]
        public string TransactionID { get; set; }

        public string UserName { get; set; }
        public string UserID { get; set; }
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public int TotalItems { get; set; }
        public int TotalQuantity { get; set; }
        public float TotalCost { get; set; }
        public string PIC { get; set; }
        public DateTime Date { get; set; }
    }

    public class Jig
    {
        [Key]
        public string ItemCode { get; set; }

        public string JigCode { get; set; }
        public string JigName { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Quantity { get; set; }
        public string Balance { get; set; }
        public string UnitCost { get; set; }
        public string TotalCost { get; set; }
        public string Specification { get; set; }
        public string RefNo { get; set; }
        public string PIC { get; set; }
        public DateTime DateDelivered { get; set; }
    }

    public class JigTransaction
    {
        [Key]
        public string TransactionID { get; set; }

        public string UserName { get; set; }
        public string UserID { get; set; }
        public string LineCode { get; set; }
        public string LineName { get; set; }
        public int TotalItem { get; set; }
        public float TotalCost { get; set; }
        public DateTime Date { get; set; }
        public string PIC { get; set; }
        public string Type { get; set; }
    }

    public class JigTransactionItem
    {
        [Key]
        public string JigTransactionItemID { get; set; }

        public string TransactionID { get; set; }
        public string ItemCode { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public int TotalItem { get; set; }
        public float Cost { get; set; }
        public float TotalCost { get; set; }
        public string JigCode { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Specification { get; set; }
        public string RefNo { get; set; }
        public string PIC { get; set; }
        public string WarehousePIC { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateWithdrawn { get; set; }
    }

    public class Location
    {
        [Key]
        public string LocationID { get; set; }

        public string Name { get; set; }
    }

    public class JigTransactionType
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; } 
    }

}