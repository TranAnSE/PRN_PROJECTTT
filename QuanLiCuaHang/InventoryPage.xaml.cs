using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using QuanLiCuaHang.Models;

namespace QuanLiCuaHang
{
    public partial class InventoryPage : UserControl
    {
        private readonly QlcuahangContext _context;

        public InventoryPage()
        {
            InitializeComponent();
            _context = new QlcuahangContext();
            LoadInventory();
        }

        private void LoadInventory()
        {
            var inventory = _context.Consignments
                .Include(c => c.Product)
                .GroupBy(c => c.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Title,
                    InStock = g.Sum(c => c.InStock),
                    InputPrice = g.OrderByDescending(c => c.InputInfoId).First().InputPrice,
                    OutputPrice = g.OrderByDescending(c => c.InputInfoId).First().OutputPrice
                })
                .ToList();

            InventoryDataGrid.ItemsSource = inventory;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchTerm = SearchTextBox.Text.ToLower();
            var filteredInventory = _context.Consignments
                .Include(c => c.Product)
                .Where(c => c.Product.Title.ToLower().Contains(searchTerm) || c.ProductId.ToLower().Contains(searchTerm))
                .GroupBy(c => c.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Title,
                    InStock = g.Sum(c => c.InStock),
                    InputPrice = g.OrderByDescending(c => c.InputInfoId).First().InputPrice,
                    OutputPrice = g.OrderByDescending(c => c.InputInfoId).First().OutputPrice
                })
                .ToList();

            InventoryDataGrid.ItemsSource = filteredInventory;
        }

        private void UpdateInventory_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext as dynamic;
            if (selectedItem != null)
            {
                var updateInventoryWindow = new UpdateInventoryWindow(selectedItem.ProductId);
                if (updateInventoryWindow.ShowDialog() == true)
                {
                    LoadInventory();
                }
            }
        }
    }
}