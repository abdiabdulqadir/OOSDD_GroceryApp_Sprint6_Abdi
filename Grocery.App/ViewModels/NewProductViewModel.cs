using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class NewProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;
        private readonly GlobalViewModel _globalViewModel;

        [ObservableProperty]
        private string productName = "";

        [ObservableProperty]
        private string stock = "";

        [ObservableProperty]
        private string shelfLife = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");

        [ObservableProperty]
        private string price = "";

        [ObservableProperty]
        private string errorMessage = "";

        [ObservableProperty]
        private bool isAdmin = false;

        public NewProductViewModel(IProductService productService, GlobalViewModel globalViewModel)
        {
            _productService = productService;
            _globalViewModel = globalViewModel;
            IsAdmin = _globalViewModel.Client?.Role == Role.Admin;
        }

        [RelayCommand]
        public async Task CreateProduct()
        {
            ErrorMessage = "";

            // Validatie
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                ErrorMessage = "Productnaam is verplicht.";
                return;
            }

            if (!int.TryParse(Stock, out int stockAmount) || stockAmount < 0)
            {
                ErrorMessage = "Voorraadhoeveelheid moet een positief getal zijn.";
                return;
            }

            if (!DateOnly.TryParse(ShelfLife, out DateOnly shelfLifeDate))
            {
                ErrorMessage = "Houdbaarheidsdatum is ongeldig.";
                return;
            }

            if (!decimal.TryParse(Price, out decimal priceAmount) || priceAmount < 0)
            {
                ErrorMessage = "Prijs moet een positief getal zijn.";
                return;
            }

            try
            {
                Product newProduct = new(0, ProductName, stockAmount, shelfLifeDate, priceAmount);
                _productService.Add(newProduct);

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij het aanmaken van product: {ex.Message}";
            }
        }

        [RelayCommand]
        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}