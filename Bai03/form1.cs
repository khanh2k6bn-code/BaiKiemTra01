using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TechMartManager
{
    public partial class Form1 : Form
    {
        // Khai báo BindingList và BindingSource làm trung gian Data Binding
        private BindingList<Product> productList = new BindingList<Product>();
        private BindingSource bindingSource = new BindingSource();
        private string selectedImagePath = "";

        public Form1()
        {
            InitializeComponent();
            SetupDataBinding();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Nạp danh mục vào ComboBox
            List<Category> categories = new List<Category>
            {
                new Category(1, "Điện thoại"),
                new Category(2, "Laptop"),
                new Category(3, "Phụ kiện")
            };

            cboCategory.DataSource = categories;
            cboCategory.DisplayMember = "CategoryName";
            cboCategory.ValueMember = "CategoryId";

            // Dữ liệu mẫu ban đầu
            productList.Add(new Product("SP01", "iPhone 15 Pro", "Điện thoại", 28000000, 10, ""));
            productList.Add(new Product("SP02", "Laptop Dell XPS", "Laptop", 35000000, 5, ""));

            UpdateStatus();
        }

        private void SetupDataBinding()
        {
            bindingSource.DataSource = productList;
            dgvProducts.DataSource = bindingSource;
        }

        private void UpdateStatus()
        {
            lblStatusText.Text = $"Tổng số sản phẩm: {productList.Count}";
        }

        // 1. VALIDATION ĐẦU VÀO DÙNG ERRORPROVIDER
        private bool ValidateInput()
        {
            bool isValid = true;
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtProductId.Text))
            {
                errorProvider1.SetError(txtProductId, "Mã sản phẩm không được để trống!");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorProvider1.SetError(txtProductName, "Tên sản phẩm không được để trống!");
                isValid = false;
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out decimal price) || price <= 0)
            {
                errorProvider1.SetError(txtUnitPrice, "Đơn giá phải là số lớn hơn 0!");
                isValid = false;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty < 0)
            {
                errorProvider1.SetError(txtQuantity, "Số lượng phải là số nguyên ≥ 0!");
                isValid = false;
            }

            return isValid;
        }

        // 2. CHỌN ẢNH ĐẠI DIỆN
        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image Files (*.jpg; *.png; *.jpeg)|*.jpg;*.png;*.jpeg";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = dialog.FileName;
                    picAvatar.ImageLocation = selectedImagePath;
                }
            }
        }

        // 3. CHỨC NĂNG THÊM MỚI SẢN PHẨM
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            if (productList.Any(item => item.ProductId.Equals(txtProductId.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm này đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Product newProd = new Product(
                txtProductId.Text.Trim(),
                txtProductName.Text.Trim(),
                cboCategory.Text,
                decimal.Parse(txtUnitPrice.Text.Trim()),
                int.Parse(txtQuantity.Text.Trim()),
                selectedImagePath
            );

            productList.Add(newProd);
            UpdateStatus();
            ClearForm();
            MessageBox.Show("Thêm mới sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 4. BẤM VÀO DATAGRIDVIEW ĐỂ NẠP DỮ LIỆU LÊN FORM
        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null && dgvProducts.CurrentRow.DataBoundItem is Product selectedProd)
            {
                txtProductId.Text = selectedProd.ProductId;
                txtProductName.Text = selectedProd.ProductName;
                cboCategory.Text = selectedProd.CategoryName;
                txtUnitPrice.Text = selectedProd.UnitPrice.ToString("0");
                txtQuantity.Text = selectedProd.Quantity.ToString();
                selectedImagePath = selectedProd.ImagePath;

                if (!string.IsNullOrEmpty(selectedProd.ImagePath) && File.Exists(selectedProd.ImagePath))
                {
                    picAvatar.ImageLocation = selectedProd.ImagePath;
                }
                else
                {
                    picAvatar.Image = null;
                }
            }
        }

        // 5. CHỨC NĂNG CẬP NHẬT (SỬA)
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            var targetProd = productList.FirstOrDefault(item => item.ProductId == txtProductId.Text.Trim());
            if (targetProd != null)
            {
                targetProd.ProductName = txtProductName.Text.Trim();
                targetProd.CategoryName = cboCategory.Text;
                targetProd.UnitPrice = decimal.Parse(txtUnitPrice.Text.Trim());
                targetProd.Quantity = int.Parse(txtQuantity.Text.Trim());
                targetProd.ImagePath = selectedImagePath;

                bindingSource.ResetBindings(false);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // 6. CHỨC NĂNG XÓA
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null && dgvProducts.CurrentRow.DataBoundItem is Product deleteProd)
            {
                var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{deleteProd.ProductName}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    productList.Remove(deleteProd);
                    UpdateStatus();
                    ClearForm();
                    MessageBox.Show("Đã xóa sản phẩm khỏi hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // 7. TÌM KIẾM THỜI GIAN THỰC (LIVE SEARCH)
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            var filtered = productList.Where(item => item.ProductName.ToLower().Contains(keyword) || item.ProductId.ToLower().Contains(keyword)).ToList();
            bindingSource.DataSource = new BindingList<Product>(filtered);
        }

        // 8. XUẤT FILE CSV
        private void menuExportCsv_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV File (*.csv)|*.csv";
                dialog.FileName = "DanhSachSanPham.csv";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Mã SP,Tên Sản Phẩm,Danh Mục,Đơn Giá,Số Lượng");

                    foreach (var item in productList)
                    {
                        sb.AppendLine($"\"{item.ProductId}\",\"{item.ProductName}\",\"{item.CategoryName}\",{item.UnitPrice},{item.Quantity}");
                    }

                    File.WriteAllText(dialog.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất dữ liệu ra file CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // 9. THOÁT ỨNG DỤNG
        private void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // 10. LÀM MỚI FORM (CLEAR)
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            picAvatar.Image = null;
            selectedImagePath = "";
            errorProvider1.Clear();
        }

        // =========================================================
        // CÁC HÀM XỬ LÝ SỰ KIỆN PHỤ (TRIỆT HẠ TẤT CẢ LỖI CS1061)
        // =========================================================
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
    }
}
