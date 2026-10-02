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
        private BindingList<Product> _products = new BindingList<Product>();
        private BindingSource _bindingSource = new BindingSource();
        private List<Category> _categories = new List<Category>();
        private string _selectedImagePath = "";

        public Form1()
        {
            InitializeComponent();

            // Cấu hình ứng dụng khi khởi chạy
            ConfigControls();
            InitData();
            SetupDataGridView();
            SetupDataBinding();
            WireUpEvents(); // Tự động đăng ký sự kiện bulletproof
            UpdateStatus();
        }

        /// <summary>
        /// Khởi tạo cấu hình ban đầu cho các Control
        /// </summary>
        private void ConfigControls()
        {
            // Cấu hình ComboBox Danh mục
            _categories = new List<Category>
            {
                new Category(1, "Điện thoại"),
                new Category(2, "Laptop"),
                new Category(3, "Phụ kiện")
            };
            cboCategory.DataSource = _categories;
            cboCategory.DisplayMember = "CategoryName";
            cboCategory.ValueMember = "CategoryId";

            // Cấu hình hiển thị PictureBox
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
        }

        /// <summary>
        /// Tạo dữ liệu mẫu ban đầu
        /// </summary>
        private void InitData()
        {
            _products.Add(new Product { ProductId = "SP01", ProductName = "iPhone 15 Pro", CategoryId = 1, CategoryName = "Điện thoại", UnitPrice = 28000000, Quantity = 10, ImagePath = "" });
            _products.Add(new Product { ProductId = "SP02", ProductName = "MacBook Air M2", CategoryId = 2, CategoryName = "Laptop", UnitPrice = 24500000, Quantity = 5, ImagePath = "" });
            _products.Add(new Product { ProductId = "SP03", ProductName = "Tai nghe AirPods Pro 2", CategoryId = 3, CategoryName = "Phụ kiện", UnitPrice = 5900000, Quantity = 15, ImagePath = "" });
        }

        /// <summary>
        /// Định nghĩa cột cho DataGridView (AutoGenerateColumns = false)
        /// </summary>
        private void SetupDataGridView()
        {
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.Columns.Clear();

            // Cột 1: Mã SP
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductId",
                HeaderText = "Mã SP",
                Width = 80
            });

            // Cột 2: Tên SP
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Tên Sản Phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Cột 3: Danh Mục
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CategoryName",
                HeaderText = "Danh Mục",
                Width = 120
            });

            // Cột 4: Đơn Giá (Định dạng N0 / VNĐ) - Đáp ứng TC03
            var colPrice = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UnitPrice",
                HeaderText = "Đơn Giá",
                Width = 140
            };
            colPrice.DefaultCellStyle.Format = "#,##0 'VNĐ'";
            dgvProducts.Columns.Add(colPrice);

            // Cột 5: Số Lượng
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "Số Lượng",
                Width = 90
            });

            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.ReadOnly = true;
        }

        /// <summary>
        /// Gán nguồn dữ liệu qua BindingSource
        /// </summary>
        private void SetupDataBinding()
        {
            _bindingSource.DataSource = _products;
            dgvProducts.DataSource = _bindingSource;
        }

        /// <summary>
        /// Đăng ký sự kiện an toàn (không lo trùng hoặc sót sự kiện từ Designer)
        /// </summary>
        private void WireUpEvents()
        {
            btnAdd.Click -= btnAdd_Click;
            btnAdd.Click += btnAdd_Click;

            btnUpdate.Click -= btnUpdate_Click;
            btnUpdate.Click += btnUpdate_Click;

            btnDelete.Click -= btnDelete_Click;
            btnDelete.Click += btnDelete_Click;

            btnClear.Click -= btnClear_Click;
            btnClear.Click += btnClear_Click;

            btnChooseImage.Click -= btnChooseImage_Click;
            btnChooseImage.Click += btnChooseImage_Click;

            txtSearch.TextChanged -= txtSearch_TextChanged;
            txtSearch.TextChanged += txtSearch_TextChanged;

            dgvProducts.SelectionChanged -= dgvProducts_SelectionChanged;
            dgvProducts.SelectionChanged += dgvProducts_SelectionChanged;

            if (exportCSVToolStripMenuItem != null)
            {
                exportCSVToolStripMenuItem.Click -= exportCSVToolStripMenuItem_Click;
                exportCSVToolStripMenuItem.Click += exportCSVToolStripMenuItem_Click;
            }

            if (exitToolStripMenuItem != null)
            {
                exitToolStripMenuItem.Click -= exitToolStripMenuItem_Click;
                exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            }
        }

        /// <summary>
        /// Cập nhật thanh trạng thái StatusStrip
        /// </summary>
        private void UpdateStatus()
        {
            lblStatus.Text = $"Tổng số sản phẩm: {_products.Count}";
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ dữ liệu nhập (Validation - Đáp ứng TC02)
        /// </summary>
        private bool ValidateInput()
        {
            bool isValid = true;
            errorProvider1.Clear();

            // 1. Tên SP không được để trống
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorProvider1.SetError(txtProductName, "Tên sản phẩm không được để trống!");
                isValid = false;
            }

            // 2. Đơn giá phải là số > 0
            if (!decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal price) || price <= 0)
            {
                errorProvider1.SetError(txtUnitPrice, "Đơn giá phải là số lớn hơn 0!");
                isValid = false;
            }

            // 3. Số lượng phải là số nguyên >= 0
            if (!int.TryParse(txtQuantity.Text.Trim(), out int qty) || qty < 0)
            {
                errorProvider1.SetError(txtQuantity, "Số lượng phải là số nguyên lớn hơn hoặc bằng 0!");
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Chọn ảnh sản phẩm (Đáp ứng TC04)
        /// </summary>
        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Tệp hình ảnh (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif";
                ofd.Title = "Chọn ảnh đại diện sản phẩm";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedImagePath = ofd.FileName;
                    picAvatar.ImageLocation = _selectedImagePath;
                }
            }
        }

        /// <summary>
        /// Thêm mới sản phẩm
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            // Tự động tạo mã SP duy nhất (ví dụ SP04, SP05...)
            int maxId = 0;
            foreach (var prod in _products)
            {
                if (prod.ProductId.StartsWith("SP") && int.TryParse(prod.ProductId.Substring(2), out int idNum))
                {
                    if (idNum > maxId) maxId = idNum;
                }
            }
            string newId = "SP" + (maxId + 1).ToString("D2");

            var category = (Category)cboCategory.SelectedItem;

            Product p = new Product
            {
                ProductId = newId,
                ProductName = txtProductName.Text.Trim(),
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                UnitPrice = decimal.Parse(txtUnitPrice.Text.Trim()),
                Quantity = int.Parse(txtQuantity.Text.Trim()),
                ImagePath = _selectedImagePath
            };

            _products.Add(p);
            UpdateStatus();
            ClearForm();
            MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm đang chọn
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || !(dgvProducts.CurrentRow.DataBoundItem is Product currentProduct))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần cập nhật từ bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            var category = (Category)cboCategory.SelectedItem;

            currentProduct.ProductName = txtProductName.Text.Trim();
            currentProduct.CategoryId = category.CategoryId;
            currentProduct.CategoryName = category.CategoryName;
            currentProduct.UnitPrice = decimal.Parse(txtUnitPrice.Text.Trim());
            currentProduct.Quantity = int.Parse(txtQuantity.Text.Trim());
            currentProduct.ImagePath = _selectedImagePath;

            _bindingSource.ResetCurrentItem();
            MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Xóa sản phẩm với Dialog xác nhận (Đáp ứng TC05)
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || !(dgvProducts.CurrentRow.DataBoundItem is Product selectedProduct))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sản phẩm \"{selectedProduct.ProductName}\" không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dr == DialogResult.Yes)
            {
                _products.Remove(selectedProduct);
                UpdateStatus();
                ClearForm();
            }
        }

        /// <summary>
        /// Nạp ngược thông tin từ DataGridView lên khung nhập liệu
        /// </summary>
        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null && dgvProducts.CurrentRow.DataBoundItem is Product p)
            {
                txtProductId.Text = p.ProductId;
                txtProductName.Text = p.ProductName;
                cboCategory.SelectedValue = p.CategoryId;
                txtUnitPrice.Text = p.UnitPrice.ToString("0.##");
                txtQuantity.Text = p.Quantity.ToString();

                _selectedImagePath = p.ImagePath;
                if (!string.IsNullOrEmpty(p.ImagePath) && File.Exists(p.ImagePath))
                {
                    picAvatar.ImageLocation = p.ImagePath;
                }
                else
                {
                    picAvatar.Image = null;
                }
            }
        }

        /// <summary>
        /// Tìm kiếm sản phẩm theo thời gian thực (Live Search TextChanged)
        /// </summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                _bindingSource.DataSource = _products;
            }
            else
            {
                var filtered = _products.Where(p => p.ProductName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                _bindingSource.DataSource = new BindingList<Product>(filtered);
            }
        }

        /// <summary>
        /// Xuất file CSV bằng SaveFileDialog (Ctrl+E / Menu File)
        /// </summary>
        private void ExportCSV()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files (*.csv)|*.csv";
                sfd.FileName = "TechMart_Products.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        // Ghi tiêu đề cột
                        sb.AppendLine("Mã SP,Tên Sản Phẩm,Danh Mục,Đơn Giá,Số Lượng");

                        // Ghi từng dòng dữ liệu
                        foreach (var p in _products)
                        {
                            sb.AppendLine($"\"{p.ProductId}\",\"{p.ProductName}\",\"{p.CategoryName}\",{p.UnitPrice},{p.Quantity}");
                        }

                        // Ghi mã hóa UTF-8 để không bị lỗi font Tiếng Việt trong Excel
                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Xuất file CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void exportCSVToolStripMenuItem_Click(object sender, EventArgs e) => ExportCSV();

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void btnClear_Click(object sender, EventArgs e) => ClearForm();

        /// <summary>
        /// Xóa sạch thông tin trên khung nhập liệu
        /// </summary>
        private void ClearForm()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            if (cboCategory.Items.Count > 0)
                cboCategory.SelectedIndex = 0;
            picAvatar.Image = null;
            _selectedImagePath = "";
            errorProvider1.Clear();
        }
    }
}
