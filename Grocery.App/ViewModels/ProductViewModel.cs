using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class ProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly GlobalViewModel _globalViewModel;

        public ObservableCollection<Product> Products { get; set; }

        [ObservableProperty]
        private bool isAdmin = false;

        public ProductViewModel(IProductService productService, GlobalViewModel globalViewModel)
        {
            _productService = productService;
            _globalViewModel = globalViewModel;
            Products = [];
            IsAdmin = _globalViewModel.Client?.Role == Role.Admin;
            Load();
        }

        private void Load()
        {
            Products.Clear();
            foreach (Product p in _productService.GetAll())
            {
                Products.Add(p);
            }
        }

        [RelayCommand]
        public async Task AddNewProduct()
        {
            await Shell.Current.GoToAsync(nameof(Views.NewProductView), true);
        }

        public override void OnAppearing()
        {
            Load();
        }

        public override void OnDisappearing()
        {
            Products.Clear();
        }
    }
}