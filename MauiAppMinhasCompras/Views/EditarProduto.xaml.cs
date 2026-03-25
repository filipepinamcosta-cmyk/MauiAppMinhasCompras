using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (BindingContext is not Produto produto_anexado)
                throw new Exception("Produto não encontrado");

            string descricao = txt_descricao.Text ?? "";

            if (string.IsNullOrWhiteSpace(descricao))
                throw new Exception("Preencha a descrição");

            if (!double.TryParse(txt_quantidade.Text ?? "", out double quantidade))
                throw new Exception("Quantidade inválida");

            if (!double.TryParse(txt_preco.Text ?? "", out double preco))
                throw new Exception("Preço inválido");

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = descricao,
                Quantidade = quantidade,
                Preco = preco
            };

            await App.Db.Update(p);

            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
