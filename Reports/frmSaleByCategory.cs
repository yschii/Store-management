using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM.Reports
{
    public partial class frmSaleByCategory : Form
    {
        public frmSaleByCategory()
        {
            InitializeComponent();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            string qry = @"select * from tblMain m
                            inner join tblDetails d on d.MainID = m.MainID
                            inner join products p on p.pID = d.proID
                            inner join category c on c.catID = p.CategoryID
                            where m.aDate between @sdate and @edate";

            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            cmd.Parameters.AddWithValue("@sdate", Convert.ToDateTime(dateTimePicker1.Value).Date.ToString().Substring(0, 10));
            cmd.Parameters.AddWithValue("@edate", Convert.ToDateTime(dateTimePicker2.Value).Date.ToString().Substring(0, 10));
            
            MainClass.con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            MainClass.con.Close();
            frmPrint frm = new frmPrint();
            rptSaleByCategory cr = new rptSaleByCategory();

            cr.SetDatabaseLogon("rm_manager", "1234");
            cr.SetDataSource(dt);
            frm.crystalReportViewer1.ReportSource = cr;
            frm.crystalReportViewer1.Refresh();
            frm.Show();
        }
    }
}
