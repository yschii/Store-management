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
    public partial class frmStaffAdd : SampleAdd
    {
        public frmStaffAdd()
        {
            InitializeComponent();
        }

        public int id = 0;

        private void frmStaffAdd_Load(object sender, EventArgs e)
        {

        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                guna2MessageDialog1.Show("직원 이름을 입력해주세요");
                return;
            }

            try
            {
                if (txtPhone.Text == "")
                {
                    guna2MessageDialog1.Show("전화번호를 입력해주세요");
                    return;
                }
                int price = int.Parse(txtPhone.Text);
            }
            catch (FormatException)
            {
                guna2MessageDialog1.Show("숫자만 작성해주세요");
                return;
            }

            if (cbRole.SelectedItem == null)
            {
                guna2MessageDialog1.Show("직무를 선택해주세요");
                return;
            }


            string qry = "";
            if (id == 0)
            {
                qry = "Insert into staff Values(@Name, @Phone, @Role )";
            }
            else
            {
                qry = "Update staff set sName = @Name, sPhone = @Phone, sRole=@Role where staffID = @id";
                this.Close();
            }

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@Name", txtName.Text);
            ht.Add("@Phone", txtPhone.Text);
            ht.Add("@Role", cbRole.Text);

            if (MainClass.SQL(qry, ht) > 0)
            {
                guna2MessageDialog1.Show("Saved successfully..");
                id = 0;
                txtName.Text = "";
                txtPhone.Text = "";
                cbRole.SelectedIndex = -1;
                txtName.Focus();
            }

        }
    }
}
