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
    public partial class FormCustomer : Form
    {
        //留白
        private void lblMenu_Click(object sender, EventArgs e) { }
        private void listBoxMenu_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lblQuantity_Click(object sender, EventArgs e) { }
        private void txtQuantity_TextChanged(object sender, EventArgs e) { }
        private void lblOrder_Click(object sender, EventArgs e) { }
        private void listBoxOrder_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lblStatus_Click(object sender, EventArgs e) { }




        // 菜单列表和订单列表
        private List<MenuItem> menuList = new List<MenuItem>();
        private List<OrderItem> orderList = new List<OrderItem>();

        public FormCustomer()
        {
            InitializeComponent();
            LoadMenuFromFile();
        }

        // 加载菜单文件
        private void LoadMenuFromFile()
        {
            string path = "menu.txt";
            if (!File.Exists(path))
            {
                lblStatus.Text = "找不到 menu.txt 文件。";
                return;
            }

            menuList.Clear();
            listBoxMenu.Items.Clear();

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length >= 3)
                {
                    string number = parts[0].Trim();
                    string name = parts[1].Trim();
                    if (decimal.TryParse(parts[2].Trim(), out decimal price))
                    {
                        var item = new MenuItem { Number = number, Name = name, Price = price };
                        menuList.Add(item);
                        listBoxMenu.Items.Add($"{number} - {name} - ￥{price}");
                    }
                }
            }

            lblStatus.Text = "菜单已加载。";
        }

        // 点击“添加到订单”
        private void btnAddToOrder_Click(object sender, EventArgs e)
        {
            if (listBoxMenu.SelectedIndex == -1)
            {
                lblStatus.Text = "请先选择一个菜品。";
                return;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int qty) || qty <= 0)
            {
                lblStatus.Text = "请输入有效的数量。";
                return;
            }

            var selectedItem = menuList[listBoxMenu.SelectedIndex];
            orderList.Add(new OrderItem
            {
                DishName = selectedItem.Name,
                Price = selectedItem.Price,
                Quantity = qty
            });

            listBoxOrder.Items.Add($"{selectedItem.Name} x{qty} = ￥{selectedItem.Price * qty}");
            lblStatus.Text = "添加成功！";
        }

        // 点击“结账”
        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (orderList.Count == 0)
            {
                lblStatus.Text = "订单为空，无法结账。";
                return;
            }

            decimal total = 0;
            foreach (var item in orderList)
                total += item.Price * item.Quantity;

            // 弹窗显示总金额
            MessageBox.Show($"订单总金额为 ￥{total}。\n感谢您的点单！", "支付成功");

            // ==== 写入 TXT 记录 ====
            string orderTxtPath = "order.txt";
            using (StreamWriter sw = new StreamWriter(orderTxtPath, true)) // true 表示追加
            {
                sw.WriteLine("========= 顾客订单 =========");
                sw.WriteLine("时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                foreach (var item in orderList)
                {
                    sw.WriteLine($"{item.DishName} × {item.Quantity} @￥{item.Price} = ￥{item.Price * item.Quantity}");
                }
                sw.WriteLine($"总金额：￥{total}");
                sw.WriteLine("===========================\n");
            }

            // ==== 新增功能：保存为 CSV 文件 ====
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string csvFileName = $"order_{timestamp}.csv";
            using (StreamWriter sw = new StreamWriter(csvFileName))
            {
                sw.WriteLine("菜品名称,单价,数量,小计");

                foreach (var item in orderList)
                {
                    decimal subtotal = item.Price * item.Quantity;
                    sw.WriteLine($"{item.DishName},{item.Price},{item.Quantity},{subtotal}");
                }

                sw.WriteLine($",,,总金额,{total}");
            }

            // 清空订单
            listBoxOrder.Items.Clear();
            orderList.Clear();
            lblStatus.Text = $"订单已保存（TXT + CSV）。";
        }


        private void FormCustomer_Load(object sender, EventArgs e)
        {

        }
    }

    // 菜品类
   

    // 订单项
    public class OrderItem
    {
        public string DishName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}