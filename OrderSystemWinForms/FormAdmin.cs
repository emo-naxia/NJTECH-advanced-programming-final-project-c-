using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OrderSystemWinForms
{
    public partial class FormAdmin : Form
    {
        private List<MenuItem> menuList = new List<MenuItem>();
        public FormAdmin()
        {
            InitializeComponent();
           
            LoadMenuFromFile();
        
        }

        // 加载菜单文件
        private void LoadMenuFromFile()
        {
            string path = "menu.txt";
            menuList.Clear();
            listBoxMenu.Items.Clear();

            if (!File.Exists(path))
            {
                lblStatus.Text = "未找到 menu.txt，将创建新菜单。";
                return;
            }

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length >= 3 && decimal.TryParse(parts[2].Trim(), out decimal price))
                {
                    string number = parts[0].Trim();
                    string name = parts[1].Trim();
                    var item = new MenuItem { Number = number, Name = name, Price = price };
                    menuList.Add(item);
                    listBoxMenu.Items.Add($"{number} - {name} - ￥{price}");
                }
            }

            lblStatus.Text = "菜单加载完成。";
        }
        // 添加菜品按钮点击事件
        private void btnAddDish_Click(object sender, EventArgs e)
        {
            string number = txtNumber.Text.Trim();
            string name = txtName.Text.Trim();
            string priceStr = txtPrice.Text.Trim();

            if (number == "" || name == "" || !decimal.TryParse(priceStr, out decimal price))
            {
                lblStatus.Text = "请输入有效的编号、名称和价格。";
                return;
            }

            if (menuList.Exists(item => item.Number == number))
            {
                lblStatus.Text = "菜品编号已存在，不能重复添加。";
                return;
            }

            var newItem = new MenuItem { Number = number, Name = name, Price = price };
            menuList.Add(newItem);
            listBoxMenu.Items.Add($"{number} - {name} - ￥{price}");

            txtNumber.Clear();
            txtName.Clear();
            txtPrice.Clear();
            lblStatus.Text = "添加成功。";
        }
        // 保存菜单到文件
        private void btnSave_Click(object sender, EventArgs e)
        {
            string path = "menu.txt";
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                foreach (var item in menuList)
                {
                    sw.WriteLine($"{item.Number},{item.Name},{item.Price}");
                }
            }
            lblStatus.Text = "菜单已保存到文件。";
        }
        // 加载事件（防止 Designer 报错）
        private void FormAdmin_Load(object sender, EventArgs e)
        {
            // 可选重新加载菜单
            // LoadMenuFromFile();
        }

        private void btnDeleteDish_Click(object sender, EventArgs e)
        {
            if (listBoxMenu.SelectedIndex == -1)
            {
                MessageBox.Show("请先选择要删除的菜品！");
                return;
            }

            // 获取选中项
            string selectedItem = listBoxMenu.SelectedItem.ToString();

            // 从 listBoxMenu 中移除
            listBoxMenu.Items.RemoveAt(listBoxMenu.SelectedIndex);

            // 从 menuList 中移除匹配项（Number + Name 同时匹配）
            menuList.RemoveAll(item => selectedItem.Contains(item.Number + " ") && selectedItem.Contains(item.Name));

            // 重新写入文件（直接调用已有的保存代码）
            string path = "menu.txt";
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                foreach (var item in menuList)
                {
                    sw.WriteLine($"{item.Number},{item.Name},{item.Price}");
                }
            }

            lblStatus.Text = "菜品已删除并保存。";
        }


    }
}
