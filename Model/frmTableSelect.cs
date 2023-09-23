﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM.Model
{
    public partial class frmTableSelect : Form
    {
        public frmTableSelect()
        {
            InitializeComponent();
        }
        public string TableName;

        private void frmTableSelect_Load(object sender, EventArgs e)
        {
            string qry = "Select * from tables";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            foreach (DataRow row in dt.Rows) 
            {
                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.Text = row["tname"].ToString();
                b.Width = 350;
                b.Height = 50;
                b.FillColor = Color.CornflowerBlue;
                b.HoverState.FillColor = Color.White;
                b.Font = new Font("Segoe UI", 14, FontStyle.Bold);

                // 이벤트 클릭
                b.Click += new EventHandler(_Click);
                flowLayoutPanel1.Controls.Add(b);

            }
        }

        private void _Click(object sender, EventArgs e)
        {
            TableName = (sender as Guna.UI2.WinForms.Guna2Button).Text.ToString();
            this.Close();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            TableName = "";
            
        }
    }
}
