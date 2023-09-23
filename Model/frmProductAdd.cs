﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM.Model
{
    public partial class frmProductAdd : SampleAdd
    {
        public frmProductAdd()
        {
            InitializeComponent();
        }

        private void frmProductAdd_Load(object sender, EventArgs e)
        {
            string qry = "select catID 'id' , catName 'name' from category";
            MainClass.CBFill(qry, cbCat);

            if (cID >0 )
            {
                cbCat.SelectedValue= cID;
            }

            if (id>0)
            {
                ForUpdateLoadData();
            }
        }

        public int id = 0;
        public int cID = 0;

        string filePath;
        Byte[] imageByteArray;
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images(.jpg, .png)|* .png; *.jpg";
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                filePath = ofd.FileName;
                txtImage.Image = new Bitmap(filePath);
            }
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text =="")
            {
                guna2MessageDialog1.Show("상품명을 입력해주세요");
                return;
            }

            try
            {
                if (txtPrice.Text=="")
                {
                    guna2MessageDialog1.Show("가격을 입력해주세요");
                    return;
                }
                int price = int.Parse(txtPrice.Text);
            }
            catch (FormatException)
            {
                guna2MessageDialog1.Show("숫자만 작성해주세요");
                return;
            }

            if (cbCat.SelectedItem == null)
            {
                guna2MessageDialog1.Show("카테고리를 선택해주세요");
                return;
            }

            if (txtImage.Image == null)
            {
                guna2MessageDialog1.Show("이미지를 입력해주세요");
                return;
            }

            string qry = "";
            if (id == 0)
            {
                qry = "Insert into products Values(@Name, @Price, @cat,@img )";
            }
            else
            {
                qry = "Update products set pName = @Name, pPrice = @Price, CategoryID=@cat, pImage = @img  where pID = @id";
                this.Close();
            }

            //image
            Image temp = new Bitmap(txtImage.Image);
            MemoryStream ms = new MemoryStream();
            temp.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
            imageByteArray = ms.ToArray();

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);
            ht.Add("@Price", txtPrice.Text);
            ht.Add("@cat", Convert.ToInt32(cbCat.SelectedValue));
            ht.Add("@img", imageByteArray);


            if (MainClass.SQL(qry, ht) > 0)
            {
                guna2MessageDialog1.Show("Saved successfully..");
                id = 0;
                cID = 0;
                txtName.Text = "";
                txtPrice.Text = "";
                cbCat.SelectedIndex = 0;
                cbCat.SelectedIndex = -1;
                txtImage.Image = null;
                txtName.Focus();
            }

        }

        private void ForUpdateLoadData()
        {
            string qry = @"Select * from products where pID = " +id + "";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count>0)
            {
                txtName.Text = dt.Rows[0]["pName"].ToString();
                txtPrice.Text = dt.Rows[0]["pPrice"].ToString();

                Byte[] imageArray = (byte[])(dt.Rows[0]["pImage"]);
                byte[] imageByteArray = imageArray;
                txtImage.Image = Image.FromStream(new MemoryStream(imageArray));

            }
        }
    }
}
