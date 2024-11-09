using System.Linq;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class ProductManagementControl : UserControl
    {
        private readonly QlcuahangContext _context;

        public ProductManagementControl()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadProducts();
        }

        private void LoadProducts()
        {
            ProductsGrid.ItemsSource = _context.Products.ToList();
        }
    }
}