using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp3.MVVM.Model;

namespace WpfApp3.MVVM.View
{
    /// <summary>
    /// Lógica interna para CatalagoProdutos.xaml
    /// </summary>
    public partial class CatalagoProdutos : Window
    {
        public Pedido param;
        public List<Produto> produtosDisponiveis = new List<Produto>();
        public List<ItemPedido> produtosAdicionados;
        public int numItens;
        public double totalGeral;

        public CatalagoProdutos(Pedido pedido)
        {
            this.param = pedido;
            InitializeComponent();
            this.carregarProdutos();
            totalGeral = 0;
            numItens = 0;
            totalPedido.Text = totalGeral.ToString();
            totalItens.Text = numItens.ToString();

            if (pedido.ItemsPedido != null && pedido.ItemsPedido.Count > 0)
            {
                this.produtosAdicionados = pedido.ItemsPedido;

                pedido.ItemsPedido.ForEach(p =>
                {
                    var valorItem = p.Produto.Valor * p.Quantidade;
                    totalGeral += valorItem;
                    numItens += p.Quantidade;
                });
            }
            else
            {
                produtosAdicionados = new List<ItemPedido>();
            }
            Quantidade.Text = "0";
            ValorUnitario.Text = "0";
            totalPedido.Text = "R$ " + totalGeral.ToString();
        }


        private void carregarProdutos()
        {
            List<Produto> source = new List<Produto>();

            using (System.IO.StreamReader r = new System.IO.StreamReader("produto.json"))
            {
                string json = r.ReadToEnd();
                source = JsonSerializer.Deserialize<List<Produto>>(json);
            }
            source.ForEach(p =>
            {
                produtosDisponiveis.Add(p);
            });
            datagridProdutos.ItemsSource = null;
            datagridProdutos.ItemsSource = produtosDisponiveis;
        }

        private void selecionarProduto(object sender, RoutedEventArgs e)
        {
            Produto produtoChange = (Produto)this.datagridProdutos.SelectedItem;
            bool achou = false;
            if (param.ItemsPedido != null)
            {
                param.ItemsPedido.ForEach(p =>
                {
                    if (p.Produto.Id == produtoChange.Id)
                    {
                        Quantidade.Text = p.Quantidade.ToString();

                        achou = true;
                    }
                });
            }

            if (!achou)
            {
                Quantidade.Text = "0";
            }
            try
            {
                nomeProduto.Text = produtoChange.Nome;
                ValorUnitario.Text = "R$ " + produtoChange.Valor.ToString();
            }
            catch (Exception except)
            {
                return;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.datagridProdutos.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Selecione um produto antes de adicionar! Ativo", "Produto obrigatório", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
                return;
            }

            if (Quantidade.Text == "0")
            {
                System.Windows.MessageBox.Show("O Produto precisa de uma quantidade", "Quantidade obrigatória", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
                return;
            }
            Produto produtoChange = (Produto)this.datagridProdutos.SelectedItem;
            double valorDesseItem = 0;
            bool encontrado = false;
            if (param.ItemsPedido != null)
            {
                param.ItemsPedido.ForEach(p =>
                {
                    if (p.Produto.Id == produtoChange.Id)
                    {
                        p.Quantidade = Convert.ToInt32(Quantidade.Text);
                        valorDesseItem = p.Produto.Valor * p.Quantidade;
                        //numitens
                        totalGeral += valorDesseItem;
                        totalPedido.Text = "R$ " + totalGeral.ToString();
                        encontrado = true;
                    }
                });
            }

            if (!encontrado)
            {
                valorDesseItem = produtoChange.Valor * Convert.ToDouble(Quantidade.Text);
                numItens += Convert.ToInt32(Quantidade.Text);
                totalGeral += valorDesseItem;
                totalPedido.Text = "R$ " + totalGeral.ToString();
                ItemPedido itemNovo = new ItemPedido();
                itemNovo.Quantidade = Convert.ToInt32(Quantidade.Text);
                itemNovo.Produto = produtoChange;
                itemNovo.Valor = valorDesseItem;
                produtosAdicionados.Add(itemNovo);
            }

            totalItens.Text = numItens.ToString();
            Quantidade.Text = "0";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.param.ItemsPedido = produtosAdicionados;
            double somaTotal = 0;
            produtosAdicionados.ForEach(p => {
                somaTotal += ((double)p.Quantidade * p.Produto.Valor);
            });
            this.param.ValorTotal = somaTotal;
            this.Close();
        }
    }
}
