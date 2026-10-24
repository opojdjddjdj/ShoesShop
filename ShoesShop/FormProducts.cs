using System.Drawing.Drawing2D;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using ShoesShop.Models;
using ShoesShop.Properties;

namespace ShoesShop
{
    public partial class FormProducts : Form
    {
        private const int PhotoSize = 200;
        private const string PlaceholderName = "picture";
        private static readonly CultureInfo RuCulture = new("ru-RU");

        public FormProducts()
        {
            InitializeComponent();

            Resource1.ResourceManager.IgnoreCase = true;

            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.ColumnHeadersVisible = false;

            var colPhoto = new DataGridViewImageColumn();
            colPhoto.Name = "colPhoto";
            colPhoto.ImageLayout = DataGridViewImageCellLayout.Zoom;
            colPhoto.FillWeight = 25;
            colPhoto.MinimumWidth = PhotoSize + 20;

            var colInfo = new DataGridViewTextBoxColumn();
            colInfo.Name = "colInfo";
            colInfo.FillWeight = 55;
            colInfo.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            var colPrice = new DataGridViewTextBoxColumn();
            colPrice.Name = "colPrice";
            colPrice.FillWeight = 20;
            colPrice.MinimumWidth = 160;
            colPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colPrice.DefaultCellStyle.Font = new Font(Font, FontStyle.Bold);

            dgvProducts.Columns.AddRange(
            [
                colPhoto, colInfo, colPrice
            ]);

            LoadProducts();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            dgvProducts.CurrentCell = null;
            dgvProducts.ClearSelection();
        }

        private void LoadProducts()
        {
            try
            {
                using var db = new ShoesShopContext();

                var products = db.Products
                    .AsNoTracking()
                    .Include(p => p.IdCategoryNavigation)
                    .Include(p => p.IdSubcategoryNavigation)
                    .Include(p => p.IdManufacturerNavigation)
                    .Include(p => p.ProductItems)
                    .OrderBy(p => p.IdProducts)
                    .ToList();

                dgvProducts.SuspendLayout();
                dgvProducts.Rows.Clear();

                foreach (var product in products)
                {
                    int totalStock = product.ProductItems.Sum(i => i.CountInStock);

                    int rowIndex = dgvProducts.Rows.Add();
                    var row = dgvProducts.Rows[rowIndex];

                    row.Cells["colPhoto"].Value = LoadProductImage(product.PhotoUrl);
                    row.Cells["colInfo"].Value = FormatProductInfo(product, totalStock);
                    row.Cells["colPrice"].Value = FormatPrice(product);

                    ApplyRowStyles(row, totalStock);
                }

                dgvProducts.ResumeLayout();
                dgvProducts.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

                lblCount.Text = $"Товаров: {products.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyRowStyles(DataGridViewRow row, int totalStock)
        {
            if (totalStock <= 3)
            {
                row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FF8080");
            }
            else
            {
                row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#D2F6E7");
            }
            this.BackColor = Color.White;

        }

        private string FormatProductInfo(Product product, int totalStock)
        {
            return product.ProductName + Environment.NewLine
                + $"{product.IdCategoryNavigation.CategoryName} | {product.IdSubcategoryNavigation.SubcategoryName}" + Environment.NewLine
                + $"Описание товара: {product.Description}" + Environment.NewLine
                + $"Состав: {product.Composition}" + Environment.NewLine
                + $"Производитель: {product.IdManufacturerNavigation.ManufacturerName}" + Environment.NewLine
                + $"Размеры: {product.SizeRange}" + Environment.NewLine
                + $"Количество на складе: {totalStock}";
        }

        private string FormatPrice(Product product)
        {
            if (product.ProductItems.Count == 0)
            {
                return "—";
            }

            decimal min = product.ProductItems.Min(i => i.Price);
            decimal max = product.ProductItems.Max(i => i.Price);

            string minText = min.ToString("C", RuCulture);
            return min == max ? minText : $"от {minText}";
        }
        private Image LoadProductImage(string? photoUrl)
        {
            Image? source = null;

            if (!string.IsNullOrWhiteSpace(photoUrl))
            {
                source = GetResourceImage(Path.GetFileNameWithoutExtension(photoUrl))
                      ?? GetResourceImage(Path.GetFileName(photoUrl));
            }

            source ??= GetResourceImage(PlaceholderName);

            return source == null ? new Bitmap(1, 1) : CreateThumbnail(source);
        }

        private static Image? GetResourceImage(string name)
        {
            return Resource1.ResourceManager.GetObject(name) as Image;
        }
        private static Image CreateThumbnail(Image source)
        {
            double scale = Math.Min((double)PhotoSize / source.Width, (double)PhotoSize / source.Height);
            if (scale > 1)
            {
                scale = 1;
            }

            int width = Math.Max(1, (int)(source.Width * scale));
            int height = Math.Max(1, (int)(source.Height * scale));

            var thumbnail = new Bitmap(width, height);
            using var g = Graphics.FromImage(thumbnail);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(source, 0, 0, width, height);
            return thumbnail;
        }
    }
}