using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringToolsEquipmentsInventory.Models
{
    public class single_issue_tran
    {
        [Key]
        public string issue_no { get; set; }
        public string section_c { get; set; }
        public string line_c { get; set; }
        public byte shift { get; set; }
        public DateTime issue_dt { get; set; }
        public string issueclosed_flg { get; set; }
        public string material_no { get; set; }
        public int reqd_qty { get; set; }
        public int surplus_qty { get; set; }
        public int issue_qty { get; set; }
        public int issued_qty { get; set; }
        public string issord_no { get; set; }
        public string serial_no { get; set; }
        public Int16 cut_length { get; set; }
        public string split { get; set; }
        public int unit_confac_issue { get; set; }
        public string issue_unit { get; set; }
        public string userid { get; set; }
        public DateTime ent_dt { get; set; }
        public DateTime upd_dt { get; set; }
        public string material_nm_single { get; set; }
        public string bom_unit_single { get; set; }
        public int issue_min_lot_single { get; set; }
        public string location_c_single { get; set; }
        public object comment { get; set; }
        public string trml_nm { get; set; }
    }
    public class bom_mst_dtl
    {
        [Key]
        [Column(Order = 5)]
        public string material_no { get; set; }
        [Key]
        [Column(Order = 1)]
        public string product_no { get; set; }
        [Key]
        [Column(Order = 2)]
        public string cusdesch_c1 { get; set; }
        [Key]
        [Column(Order = 3)]
        public string cusdesch_c2 { get; set; }
        [Key]
        [Column(Order = 4)]
        public string intdesch_c { get; set; }
        [Key]
        [Column(Order = 6)]
        public string sub_assy1 { get; set; }
        [Key]
        [Column(Order = 7)]
        public string sub_assy2 { get; set; }
        [Key]
        [Column(Order = 8)]
        public string sub_assy3 { get; set; }
        public decimal bom_qty { get; set; }
        public string kit_issue_flg { get; set; }
        public string mrp_flg { get; set; }
        public string userid { get; set; }
        public DateTime ent_dt { get; set; }
        public DateTime upd_dt { get; set; }
    }

    public class bom_mst_dtl_qry
    {
        [Key]
        [Column(Order = 6)]
        public string material_no { get; set; }
        [Key]
        [Column(Order = 4)]
        public string intdesch_c { get; set; }
        [Key]
        [Column(Order = 1)]
        public string product_no { get; set; }
        [Key]
        [Column(Order = 2)]
        public string cusdesch_c1 { get; set; }
        [Key]
        [Column(Order = 3)]
        public string cusdesch_c2 { get; set; }

    }

    public class product_mst
    {
        [Key]
        public string cust_product_no { get; set; }

        public string product_no { get; set; }
        public string cusdesch_c1 { get; set; }
        public string cusdesch_c2 { get; set; }
        public string intdesch_c { get; set; }
        public byte safety_od { get; set; }
        public double cc_mh { get; set; }
        public double splice_mh { get; set; }
        public double assy_mh { get; set; }
        public string mrp_flg { get; set; }
        public string price_flg { get; set; }
        public string assy_flg { get; set; }
        public object last_assy_flg_dt { get; set; }
        public decimal sales_price { get; set; }
        public decimal std_cost { get; set; }
        public decimal prd_wt { get; set; }
        public Int16 no_of_circuits { get; set; }
        public Int16 no_of_bags { get; set; }
        public string prodshrt_c { get; set; }
        public string packing_tp { get; set; }
        public decimal ins_mh { get; set; }
        public Nullable<DateTime> st_prd_dt { get; set; }
        public DateTime ed_prd_dt { get; set; }
        public string userid { get; set; }
        public DateTime ent_dt { get; set; }
        public DateTime upd_dt { get; set; }
        public string sp_flg { get; set; }
        public decimal fifo_cost { get; set; }
    }

    public class material_mst
    {
        [Key]
        public string material_no { get; set; }

        public string material_nm { get; set; }
        public string material_tp { get; set; }
        public string chk_mat_flg { get; set; }
        public decimal std_cost { get; set; }
        public string mrp_flg { get; set; }
        public string kit_issue_flg { get; set; }
        public string bom_unit { get; set; }
        public string po_unit { get; set; }
        public string issue_unit { get; set; }
        public int issue_min_lot { get; set; }
        public string pack_form { get; set; }
        public double cut_length { get; set; }
        public int bag_qty { get; set; }
        public int box_qty { get; set; }
        public int unit_confac_po { get; set; }
        public int unit_confac_issue { get; set; }
        public object rmks { get; set; }
        public string material_kd { get; set; }
        public string boxlabel_flg { get; set; }
        public string baglabel_flg { get; set; }
        public string generic_c { get; set; }
        public string sub_type { get; set; }
        public string cc_flg { get; set; }
        public decimal weight { get; set; }
        public decimal latest_po_price { get; set; }
        public string latest_curr_c { get; set; }
        public decimal old_std_cost { get; set; }
        public string gross_c { get; set; }
        public string userid { get; set; }
        public DateTime ent_dt { get; set; }
        public DateTime upd_dt { get; set; }
        public decimal fifo_cost { get; set; }
        public object box_c { get; set; }
        public string gts_material { get; set; }
        public string gts_label_print { get; set; }
        public string sur_stk_flg { get; set; }
        public string ex_use_matl { get; set; }
        public string rohs_flg { get; set; }
        public object custom_material { get; set; }
        public object CnC_short_code { get; set; }
    }

    public class line_mst
    {
        [Key, Column(Order = 1)]
        public string section_c { get; set; }
        [Key, Column(Order = 2)]
        public string line_c { get; set; }
        public string line_nm { get; set; }
        public string userid { get; set; }
        public DateTime ent_dt { get; set; }
        public DateTime upd_dt { get; set; }
        public string use_surplus { get; set; }
        public string use_wip { get; set; }
    }

}
