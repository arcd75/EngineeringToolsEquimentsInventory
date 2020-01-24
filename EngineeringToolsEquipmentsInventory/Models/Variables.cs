using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringToolsEquipmentsInventory.Models
{
    public class UserSession
    {
        public static string UserID = "";
        public static string UserName = "";
        public static string UserIDTemp = "";
        public static string idScanTemp = "";
    }

    public class ItemSources
    {
        public static List<string> ConditionItems = new List<string>() {"GOOD","NO GOOD","LOST" };
        
    }

    public class TransactionSession
    {
        public static string transactionID = "";
        public static List<Tool> barrowTools = new List<Tool>();
        public static Loan newTransaction = new Loan();

        public static bool consumable = false;
        public static bool sparepart = false; 
    }

    public class ReturningSession
    {
        public static bool returnScanID = false;
        public static string borrowerID = ""; 
        public static string LoanItemLoanID = "";  
    }


    public class HistorySession
    {
        public static bool historyIDScan = false;
        public static bool historyItemLookUp = false;
        public static string queryBorrowerID = "";
        public static string queryTool = "";
        public static string toolFind = "";
    }

    public class ToolEditSession
    {
        public static string toolEditItemCode = "";
    }

    public class ConsumableSession
    {
        public static bool TransactionScan = false;
        public static Transaction NewTransaction = new Transaction();
        public static List<TransactionItem> TransItemList = new List<TransactionItem>();
    }

    public class SparePartSession
    {
        public static bool TransactionSparePartScan = false;
        public static SparePartTransaction NewTransactionSparePart = new SparePartTransaction();
        public static List<SparePartTransactionItem> TransSparePartItemList = new List<SparePartTransactionItem>();
    }

    public class DeliveryReceiving
    {
        public static Delivery newDelivery = new Delivery();
        public static List<DeliveryItems> GetDeliveryItems = new List<DeliveryItems>();
        public static List<CreateToolDelivery> GetDeliveryToolItems = new List<CreateToolDelivery>();
        public static System.Windows.Forms.BindingSource ItemBinding = new System.Windows.Forms.BindingSource();
        public static System.Windows.Forms.BindingSource ItemToolBinding = new System.Windows.Forms.BindingSource();
        
    }

    public class JigsSession
    {
        public static bool JgsTransactionScan = false;
        public static string TransactionMode = "";
        public static JigTransaction NewJigTransaction = new JigTransaction();
        public static List<JigTransactionItem> JigTransItemList = new List<JigTransactionItem>();

    }

}
