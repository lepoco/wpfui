using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Gallery.Models;

namespace Wpf.Ui.Gallery.ViewModels.Pages.Collections;

public partial class DataGridViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Product> _productsCollection;

    public DataGridViewModel()
    {
        _productsCollection = GenerateProducts();
    }

    private ObservableCollection<Product> GenerateProducts()
    {
        var random = new Random();
        var products = new ObservableCollection<Product> { };

        var adjectives = new[] { "Red", "Blueberry" };
        var names = new[] { "Marmalade", "Dumplings", "Soup" };
        var units = new[] { "grams", "kilograms", "milliliters" };

        for (int i = 0; i < 50; i++)
        {
            products.Add(new Product
            {
                ProductId = i,
                ProductCode = i,
                ProductName = adjectives[random.Next(0, adjectives.Length)] + " " + names[random.Next(0, names.Length)],
                UnitPrice = Math.Round(random.NextDouble() * 20.0, 3),
                UnitsInStock = random.Next(0, 100),
                IsVirtual = random.Next(0, 2) == 1
            });
        }

        return products;
    }
}
