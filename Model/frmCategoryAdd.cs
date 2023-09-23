using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM.Model
{
    public partial class frmCategoryAdd : SampleAdd
    {
        public frmCategoryAdd()
        {
            InitializeComponent();
        }

        public void frmCategoryAdd_Load(object sender, EventArgs e)
        {
            
        }

        public override void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text =="")
            {
                guna2MessageDialog1.Show("Please enter the categoryName.");
                return;
            }
            string qry = "";
            if (id ==0)
            {
                qry = "Insert into category Values(@Name)";
            }
            else
            {
                qry = "Update category set catName = @Name where catID = @id";
                this.Close();
            }

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);

            if(MainClass.SQL(qry, ht) > 0)
            {
                guna2MessageDialog1.Show("Saved!");
                id = 0;
                txtName.Text = "";
                txtName.Focus();
            }

        }


        public int id = 0;
    }
}
