using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace RM.Model
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }

        public int MainID = 0;
        public string OrderType="";
        public int driverID = 0;
        public string customerName = "";
        public string customerPhone = "";
        public bool checking = false;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            ProductPanel.Controls.Clear();
            LoadProducts();

        }

        private void AddCategory()
        {
            string qry = "select * from category";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            CategoryPanel.Controls.Clear();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows) {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                    b.FillColor = Color.Black;
                    b.Size = new Size(134, 45);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = row["catName"].ToString();
                    b.BorderRadius = 5;
                    b.Margin = new Padding(50, 5, 5, 5);

                    // 이벤트 클릭
                    b.Click += new EventHandler(_Click);

                    CategoryPanel.Controls.Add(b);
                } 
            }
        }

        bool checkItem = false;
        private void _Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains(b.Text.Trim().ToLower());
            }
        }

        private void AddItems(string id, string proID , string name, string cat, string price, Image pimage)
        {
            var w = new ucProduct()
            {
                PName = name,
                PPrice = price,
                PCategory = cat,
                PImage = pimage,
                id = Convert.ToInt32(proID)
            };
            ProductPanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;

                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    if (Convert.ToInt32(item.Cells["dgvproID"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1 ;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                        double.Parse(item.Cells["dgvPrice"].Value.ToString());
                        GetTotal();
                        return;
                    }

                }
                // 새로운 제품 선택
                guna2DataGridView1.Rows.Add(new object[] { 0,0, wdg.id, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                GetTotal();
                checkItem = true;
            };
        }

        //데이터베이스 가져오기
        private void LoadProducts()
        {
            string qry = "Select * from products inner join category on catID = CategoryID ";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                Byte[] imagearray = (byte[])item["pImage"];
                byte[] imagebytearray = imagearray;

                AddItems("0",item["pID"].ToString(), item["pName"].ToString(), item["catName"].ToString(),
                    item["pPrice"].ToString(), Image.FromStream(new MemoryStream(imagearray)));
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach(var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());

            }
        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // 시리얼 넘버

            int count = 0;

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        private void GetTotal()
        {
            double tot = 0;
            lblTotal.Text = "";
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                tot += double.Parse(item.Cells["dgvAmount"].Value.ToString());
            }
            lblTotal.Text = tot.ToString("N2");
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            MainID = 0;
            lblTotal.Text = "00";
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            btnDin.FillColor = Color.Black;
            btnTake.FillColor = Color.Black;

            lblTable.Text = "";
            lblWaiter.Text = "";
            lblDriverName.Text = "";
            lblDriverName.Visible = false;
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Delivery";

            frmAddCustomer frm = new frmAddCustomer();
            frm.mainID = MainID;
            frm.orderType = OrderType;
            MainClass.BlurBackgroud(frm);

            if (frm.txtName.Text != "" && frm.txtPhone.Text != "" && frm.cbDriver.SelectedItem != null)
            // 드라이버가 없음
            {
                driverID = frm.driverID;
                lblDriverName.Text = "Customer Name: " + frm.txtName.Text + "Phone: " + frm.txtPhone.Text + "Driver: " + frm.cbDriver.Text;
                lblDriverName.Visible = true;
                customerName = frm.txtName.Text;
                customerPhone = frm.txtPhone.Text;
                btnDelivery.FillColor = Color.Gray;
            }
        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            btnDelivery.FillColor = Color.Black;
            btnDin.FillColor = Color.Black;

            lblTable.Text = "";
            lblWaiter.Text = "";
            lblDriverName.Text = "";
            lblDriverName.Visible = false;
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Take Away";

            frmAddCustomer frm = new frmAddCustomer();
            frm.mainID = MainID;
            frm.orderType = OrderType;
            MainClass.BlurBackgroud(frm);

            if (frm.txtName.Text  !="" && frm.txtPhone.Text != "")
                // 드라이버가 없음
            {
                driverID = frm.driverID;
                lblDriverName.Text ="Customer Name: " +frm.txtName.Text + " Phone: " + frm.txtPhone.Text;
                lblDriverName.Visible = true;
                customerName = frm.txtName.Text;
                customerPhone = frm.txtPhone.Text;
                btnTake.FillColor = Color.Gray;
            }
        }

        private void btnDin_Click(object sender, EventArgs e)
        {
            btnDelivery.FillColor = Color.Black;
            btnTake.FillColor = Color.Black;

            // 테이블 선택 필요

            OrderType = "Eat In";
            lblDriverName.Text = "";
            lblDriverName.Visible = false;

            frmTableSelect frm = new frmTableSelect();
            MainClass.BlurBackgroud(frm);
            if (frm.TableName !="")
            {
                lblTable.Text = frm.TableName;
                lblTable.Visible = true;
                btnDin.FillColor = Color.Gray;
            }
            else
            {
                lblTable.Text = "";
                lblTable.Visible = false;
            }

            // 웨이터 시스템은 필요에 따라 사용
            /*            frmWaiterSelect frm2 = new frmWaiterSelect();
                        MainClass.BlurBackgroud(frm2);
                        if (frm2.waiterName != "")
                        {
                            lblWaiter.Text = frm2.waiterName;
                            lblWaiter.Visible = true;
                        }
                        else
                        {
                            lblWaiter.Text = "";
                            lblWaiter.Visible = false;
                        }*/
        }

        private void btnKot_Click(object sender, EventArgs e)
        {
            checkOut();
            if (checking==false)
            {
                return;
            }
            //save database
            if (OrderType=="" && lblTable.Text=="" && lblDriverName.Text=="")
            {
                guna2MessageDialog1.Show("Please select order_type");
                return;
            }

            if (checkItem==true)
            {
                checkItem = false;
            }
            else
            {
                guna2MessageDialog1.Show("Please select item");
                return;
            }

            string qry1 = ""; // Main Table
            string qry2 = ""; // Detail Table

            int detailID = 0;

            if (MainID ==0)
            {
                qry1 = @"Insert into tblMain Values(@aDate,@aTime,@TableName, @WaiterName, @status, 
                        @orderType, @total, @received, @change, @driverID, @CustName, @CustPhone);
                            Select SCOPE_IDENTITY()";
                // 마지막 인서트 키 값 반환

            }
            else
            {
                qry1 = @"Update tblMain set status = @status, total= @total,
                    received = @received, change = @change where MainID = @ID";
            }

            SqlCommand cmd = new SqlCommand(qry1, MainClass.con);
            cmd.Parameters.AddWithValue("@ID", MainID);
            cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
            cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@TableName", lblTable.Text);
            cmd.Parameters.AddWithValue("@WaiterName", lblWaiter.Text);
            cmd.Parameters.AddWithValue("@status", "Pending");
            cmd.Parameters.AddWithValue("@orderType", OrderType);
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lblTotal.Text)); // 키친용. 결제되면 업데이트
            cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@driverID", driverID);
            cmd.Parameters.AddWithValue("@CustName", customerName);
            cmd.Parameters.AddWithValue("@CustPhone", customerPhone);


            if (MainClass.con.State == ConnectionState.Closed) {MainClass.con.Open();}
            if (MainID == 0) { MainID = Convert.ToInt32(cmd.ExecuteScalar()); } else {  cmd.ExecuteNonQuery(); }
            if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close();}

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detailID = Convert.ToInt32(row.Cells["dgvid"].Value);

                if (detailID == 0)
                {
                    qry2 = @"Insert into tblDetails Values(@MainID, @proID, @qty, @price, @amount)";
                }
                else
                {
                    qry2 = @"Update tblDetails Set proID = @proID, qty = @qty, price = @price, amount =  @amount
                where DetailID = @ID";
                }

                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                cmd2.Parameters.AddWithValue("@ID", detailID);
                cmd2.Parameters.AddWithValue("@MainID", MainID);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvproID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmount"].Value));

                if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
                cmd2.ExecuteNonQuery();
                if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }
            }

            guna2MessageDialog1.Show("Saved");
            MainID = 0;
            detailID = 0;
            guna2DataGridView1.Rows.Clear();
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            lblTotal.Text = "00";
            lblDriverName.Text = "";

            // 타입 초기화
            btnDin.FillColor = Color.Black;
            btnDelivery.FillColor = Color.Black;
            btnTake.FillColor = Color.Black;
            OrderType = "";

        }

        private void checkOut()
        {
            frmCheckout frm = new frmCheckout();
            frm.MainID = id;
            frm.amt = Convert.ToDouble(lblTotal.Text);
            MainClass.BlurBackgroud(frm);

            checking = frm.checking;
        }

        public int id = 0;
        private void btnBill_Click(object sender, EventArgs e)
        {
            frmBillList frm = new frmBillList();
            MainClass.BlurBackgroud(frm);

            if (frm.MainID >0)
            {
                id = frm.MainID;
                MainID = frm.MainID;
                LoadEntries();
            }

        }

        private void LoadEntries()
        {
            string qry = @"Select * from tblMain m 
                                inner join tblDetails d on m.MainID = d.MainID
                                inner join products p on p.pID = d.proID
                                    Where m.MainID = " + id + "";
            SqlCommand cmd2 = new SqlCommand(qry, MainClass.con);
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);

            if (dt2.Rows[0]["orderType"].ToString() == "Delivery")
            {
                btnDelivery.Checked = true;
                lblTable.Visible=false;
                lblWaiter.Visible = false;
            }
            else if (dt2.Rows[0]["orderType"].ToString() == "Take away")
            {
                btnTake.Checked = true;
                lblTable.Visible = false;
                lblWaiter.Visible = false;
            }
            else
            {
                btnDin.Checked = true;
                lblTable.Visible = true;
                lblWaiter.Visible = true;
            }

            guna2DataGridView1.Rows.Clear();

            foreach (DataRow item in dt2.Rows)
            {
                lblTable.Text = item["TableName"].ToString();
                lblWaiter.Text = item["WaiterName"].ToString();

                string detailid = item["DetailId"].ToString();
                string proName = item["pName"].ToString();
                string proid = item["proID"].ToString();
                string qty = item["qty"].ToString();
                string price = item["price"].ToString();
                string amount = item["amount"].ToString();

                object[] obj = { 0, detailid, proid,proName, qty, price, amount };
                guna2DataGridView1.Rows.Add(obj);
            }
            GetTotal();
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            //홀드시키는 기능 필요시 사용.
            /*string qry1 = ""; // Main Table
            string qry2 = ""; // Detail Table

            int detailID = 0;

            if (OrderType =="")
            {
                guna2MessageDialog1.Show("Please select order type");
                return;
            }

            if (MainID == 0)
            {
                qry1 = @"Insert into tblMain Values(@aDate,@aTime,@TableName, @WaiterName, @status, 
                        @orderType, @total, @received, @change, @driverID, @CustName, @CustPhone);
                            Select SCOPE_IDENTITY()";
                // 마지막 인서트 키 값 반환

            }
            else
            {
                qry1 = @"Update tblMain set status = @status, total= @total,
                    received = @received, change = @change where MainID = @ID";
            }

            SqlCommand cmd = new SqlCommand(qry1, MainClass.con);
            cmd.Parameters.AddWithValue("@ID", MainID);
            cmd.Parameters.AddWithValue("@aDate", Convert.ToDateTime(DateTime.Now.Date));
            cmd.Parameters.AddWithValue("@aTime", DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@TableName", lblTable.Text);
            cmd.Parameters.AddWithValue("@WaiterName", lblWaiter.Text);
            cmd.Parameters.AddWithValue("@status", "Hold");
            cmd.Parameters.AddWithValue("@orderType", OrderType);
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(lblTotal.Text)); // 키친용. 결제되면 업데이트
            cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@driverID", driverID);
            cmd.Parameters.AddWithValue("@CustName", customerName);
            cmd.Parameters.AddWithValue("@CustPhone", customerPhone);

            if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
            if (MainID == 0) { MainID = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
            if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detailID = Convert.ToInt32(row.Cells["dgvid"].Value);

                if (detailID == 0)
                {
                    qry2 = @"Insert into tblDetails Values(@MainID, @proID, @qty, @price, @amount)";
                }
                else
                {
                    qry2 = @"Update tblDetails Set proID = @proID, qty = @qty, price = @price, amount =  @amount
                where DetailID = @ID";
                }

                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                cmd2.Parameters.AddWithValue("@ID", detailID);
                cmd2.Parameters.AddWithValue("@MainID", MainID);
                cmd2.Parameters.AddWithValue("@proID", Convert.ToInt32(row.Cells["dgvproID"].Value));
                cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(row.Cells["dgvQty"].Value));
                cmd2.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells["dgvPrice"].Value));
                cmd2.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells["dgvAmount"].Value));

                if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
                cmd2.ExecuteNonQuery();
                if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }
            }

            guna2MessageDialog1.Show("Saved");
            MainID = 0;
            detailID = 0;
            guna2DataGridView1.Rows.Clear();
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            lblTotal.Text = "00";
            lblDriverName.Text = "";*/
        }
    }
}
