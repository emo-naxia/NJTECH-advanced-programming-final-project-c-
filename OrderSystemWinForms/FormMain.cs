using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace OrderSystemWinForms
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            FormCustomer customerForm = new FormCustomer();
            customerForm.Show();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("请输入管理员密码：", "管理员登录", "");

            // 如果用户点击取消或关闭输入框（返回空），则中断，不提示
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            // 如果密码不正确
            if (input != "123456")
            {
                MessageBox.Show("密码错误！");
                return;
            }

            // 正确密码，打开管理窗口
            FormAdmin admin = new FormAdmin();
            admin.Show();

        }


        private void FormMain_Load(object sender, EventArgs e)
        {

        }
    }
}
