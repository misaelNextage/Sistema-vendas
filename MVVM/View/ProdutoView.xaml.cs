using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using WpfApp3.MVVM.Model;

namespace WpfApp3.MVVM.View
{
    public partial class CadastroProduto : UserControl
    {
        public CadastroProduto()
        {
            InitializeComponent();
            DataContext = new ViewModel.ProdutoViewModel();
        }

        public void salvar(object sender, RoutedEventArgs e)
        {

            Produto produto = new Produto();

            produto.Nome = nomeProduto.Text;
            produto.Id = 1;
            produto.Codigo = int.Parse(codigoProduto.Text);
            produto.Valor = double.Parse(valorProduto.Text);


            List<Produto> source = new List<Produto>();

            using (StreamReader r = new StreamReader("produto.json"))
            {
                string json = r.ReadToEnd();
                source = JsonSerializer.Deserialize<List<Produto>>(json);
            }

            source.Add(produto);

            string jsonString = JsonSerializer.Serialize(source, new JsonSerializerOptions() { WriteIndented = true });
            using (StreamWriter outputFile = new StreamWriter("produto.json"))
            {
                outputFile.WriteLine(jsonString);
            }
        }
        public void editar(object sender, RoutedEventArgs e)
        {
            string teste = "teste";

        }

        private void DatagridProdutos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                int index = datagridProdutos.SelectedIndex;
                object a = e.Source;

                object b = e.AddedItems;

            }
        }
    }
}
