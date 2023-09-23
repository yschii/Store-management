using RM.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM.View
{
    public partial class frmKitchenView : Form
    {
        public frmKitchenView()
        {
            InitializeComponent();
        }

        private void frmKitchenView_Load(object sender, EventArgs e)
        {
            GetOrders();
        }

        private void GetOrders()
        {
            flowLayoutPanel1.Controls.Clear();
            string qry1 = @"Select * from tblMain where status = 'pending' ";
            SqlCommand cmd1 = new SqlCommand(qry1, MainClass.con);
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(dt1);

            FlowLayoutPanel p1;

            for (int i=0; i<dt1.Rows.Count; i++)
            {
                p1 = new FlowLayoutPanel();
                p1.AutoSize = true;
                p1.BackColor = Color.Black;
                p1.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                p1.Width = 400;
                p1.Height = 430;
                p1.FlowDirection = FlowDirection.TopDown;
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Margin = new Padding(10, 10, 10, 10);

                FlowLayoutPanel p2;
                p2 = new FlowLayoutPanel();
                p2.BackColor = Color.White;
                p2.AutoSize = true;
                p2.Width = 380;
                p2.Height = 200;
                //p2.MaximumSize = new Size(380,150);
                p2.FlowDirection = FlowDirection.TopDown;
                p2.Margin = new Padding(10, 10, 10, 10);

                Label lb1 = new Label();
                lb1.ForeColor = Color.Black;
                lb1.Margin = new Padding(10, 10, 3, 0);
                lb1.AutoSize = true;

                Label lb2 = new Label();
                lb2.ForeColor = Color.Black;
                lb2.Margin = new Padding(10, 5, 3, 0);
                lb2.AutoSize = true;

                Label lb3 = new Label();
                lb3.ForeColor = Color.Black;
                lb3.Margin = new Padding(10, 5, 3, 0);
                lb3.AutoSize = true;

                Label lb4 = new Label();
                lb4.ForeColor = Color.Black;
                lb4.Margin = new Padding(10, 5, 3, 10);
                lb4.AutoSize = true;

                lb1.Text = "Table :" + dt1.Rows[i]["TableName"].ToString();
                lb2.Text = "Waiter Name :" + dt1.Rows[i]["WaiterName"].ToString();
                lb3.Text = "Order Time \n:" + dt1.Rows[i]["aDate"].ToString().Substring(0,10)+"      " + dt1.Rows[i]["aTime"].ToString()+ "\n";
                lb4.Text = "Order Type :" + dt1.Rows[i]["orderType"].ToString();

                p2.Controls.Add(lb1);
                p2.Controls.Add(lb2);
                p2.Controls.Add(lb3);
                p2.Controls.Add(lb4);

                p1.Controls.Add(p2);

                //add products
                int mid = 0;
                mid = Convert.ToInt32(dt1.Rows[i]["MainID"].ToString());

                string qry2 = @"Select * from tblMain m 
                                inner join tblDetails d on m.MainID = d.MainID
                                inner join products p on p.pID = d.proID
                                    Where m.MainID = " + mid + "";

                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                DataTable dt2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(dt2);

                for (int j=0; j<dt2.Rows.Count; j++)
                {
                    Label lb5 = new Label();
                    lb5.ForeColor = Color.White;
                    lb5.Margin = new Padding(10, 10, 3, 0);
                    lb5.AutoSize = true;

                    int no = j + 1;
                    lb5.Text = "" + no + " " + dt2.Rows[j]["pName"].ToString() + " " + dt2.Rows[j]["qty"].ToString();

                    p1.Controls.Add(lb5);
                }

                //체인지 버튼

                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.AutoRoundedCorners = true;
                b.AutoSize = true;
                b.Font = new Font("Segoe UI",14, FontStyle.Bold);
                b.Size = new Size(100, 35);
                b.FillColor = Color.LightSalmon;
                b.Margin = new Padding(135, 20, 20, 20);
                b.Text = "complete";
                b.Tag = dt1.Rows[i]["MainID"].ToString();
                b.Cursor = Cursors.Hand;

                b.Click += new EventHandler(_Click);
                p1.Controls.Add(b);


                flowLayoutPanel1.Controls.Add(p1);
            }
        }

        private void _Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString());

            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            if (guna2MessageDialog1.Show("are you sure?") == DialogResult.Yes)
            {
                string qry = @"Update tblMain Set status = 'complete' where MainID = @ID";
                Hashtable ht = new Hashtable();
                ht.Add("@ID", id);

                if (MainClass.SQL(qry,ht)>0)
                {
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("saved");
                }

                GetOrders();
            }
        }
    }
}
