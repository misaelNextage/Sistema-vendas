using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using WpfApp3.core;
using WpfApp3.MVVM.Model;

namespace WpfApp3.MVVM.ViewModel
{
    class NovoProdutoCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return parameter is ProdutoViewModel;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (ProdutoViewModel)parameter;

            bool valido = this.validar(viewModel);

            if (!valido)
            {
                System.Windows.MessageBox.Show("O nome do Produto precisa ser preenchido", "Campo obrigatório", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
                return;
            }
            if (viewModel.ProdutoEdit.Id != null && viewModel.ProdutoEdit.Id > 0)
            {

                var produtoSelecionado = viewModel.ProdutoSelecionado;

                var cloneProduto = (Model.Produto)viewModel.ProdutoSelecionado.Clone();

                produtoSelecionado.Nome = viewModel.ProdutoEdit.Nome;
                produtoSelecionado.Codigo = viewModel.ProdutoEdit.Codigo;
                produtoSelecionado.Valor = viewModel.ProdutoEdit.Valor;

                bool achou = false;

                foreach (Produto produto in viewModel.Produtos)
                {
                    if (produto.Id == viewModel.ProdutoEdit.Id)
                    {
                        achou = true;
                        break;
                    }
                }
                if (achou)
                {
                    viewModel.Produtos.Remove(produtoSelecionado);

                    var cloneProduto2 = (Model.Produto)viewModel.ProdutoEdit.Clone();
                    viewModel.Produtos.Add(cloneProduto2);
                }

                string jsonString = JsonSerializer.Serialize(viewModel.Produtos, new JsonSerializerOptions() { WriteIndented = true });
                using (StreamWriter outputFile = new StreamWriter("produto.json"))
                {
                    outputFile.WriteLine(jsonString);
                }
                viewModel.ProdutoEdit.Id = 0;
                viewModel.ProdutoEdit.Nome = "";
                viewModel.ProdutoEdit.Codigo = 0000000000;
                viewModel.ProdutoEdit.Valor = 0.00;
            }
            else
            {

                var produto = new Model.Produto();
                long maxId = 0;
                if (viewModel.Produtos.Any())
                {
                    maxId = viewModel.Produtos.Max(f => f.Id);
                }
                produto.Id = maxId + 1;
                produto.Nome = viewModel.ProdutoEdit.Nome;
                produto.Codigo = viewModel.ProdutoEdit.Codigo;
                produto.Valor = viewModel.ProdutoEdit.Valor;

                viewModel.Produtos.Add(produto);
                viewModel.ProdutoSelecionado = produto;

                string jsonString = JsonSerializer.Serialize(viewModel.Produtos, new JsonSerializerOptions() { WriteIndented = true });
                using (StreamWriter outputFile = new StreamWriter("produto.json"))
                {
                    outputFile.WriteLine(jsonString);
                }
            }
        }

        private bool validar(ProdutoViewModel viewModel)
        {
            if (viewModel.ProdutoEdit.Nome == null || viewModel.ProdutoEdit.Nome == "")
            {
                return false;
            }
            if (viewModel.ProdutoEdit.Valor == null || viewModel.ProdutoEdit.Valor < 1)
            {
                return false;
            }
            if (viewModel.ProdutoEdit.Codigo == null || viewModel.ProdutoEdit.Codigo < 1)
            {
                return false;
            }
            return true;
        }
    }
}