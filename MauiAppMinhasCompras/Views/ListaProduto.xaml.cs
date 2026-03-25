using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            lista.Clear();

            var tmp = await App.Db.GetAll();

            foreach (var item in tmp)
                lista.Add(item);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue ?? "";

            lista.Clear();

            var tmp = await App.Db.Search(q);

            foreach (var item in tmp)
                lista.Add(item);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            double soma = lista.Sum(i => i.Total);

            string msg = $"O total é {soma:C}";
            await DisplayAlert("Total dos Produtos", msg, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is MenuItem selecionado && selecionado.BindingContext is Produto p)
            {
                bool confirm = await DisplayAlert(
                    "Tem certeza?",
                    $"Remover {p.Descricao}?",
                    "Sim",
                    "Não");

                if (confirm)
                {
                    await App.Db.Delete(p.Id);
                    lista.Remove(p);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            if (e.SelectedItem is Produto p)
            {
                await Navigation.PushAsync(new Views.EditarProduto
                {
                    BindingContext = p
                });

                lst_produtos.SelectedItem = null;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
