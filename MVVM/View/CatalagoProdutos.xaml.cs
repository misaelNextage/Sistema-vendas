using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
using WpfApp3.MVVM.Model;

namespace WpfApp3.MVVM.View
{
    public partial class CatalagoProdutos : Window
    {
        public Pedido param;
        public List<Produto> produtosDisponiveis = new List<Produto>();
        public List<ItemPedido> produtosAdicionados;
        public int numeroItens;
        public double totalGeral;

        public CatalagoProdutos(Pedido pedido)
        {
            this.param = pedido;
            InitializeComponent();
            this.carregarProdutos();
            totalGeral = 0;
            numeroItens = 0;
            totalPedido.Text = totalGeral.ToString();
            totalItens.Text = numeroItens.ToString();

            if (pedido.ItemsPedido != null && pedido.ItemsPedido.Count > 0)
            {
                this.produtosAdicionados = pedido.ItemsPedido;

                pedido.ItemsPedido.ForEach(p =>
                {
                    var valorItem = p.Produto.Valor * p.Quantidade;
                    totalGeral += valorItem;
                    numeroItens += p.Quantidade;
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

            using (System.IO.StreamReader reader = new System.IO.StreamReader("produto.json"))
            {
                string json = reader.ReadToEnd();
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
            bool produto = false;
            if (param.ItemsPedido != null)
            {
                param.ItemsPedido.ForEach(p =>
                {
                    if (p.Produto.Id == produtoChange.Id)
                    {
                        Quantidade.Text = p.Quantidade.ToString();

                        produto = true;
                    }
                });
            }

            if (!produto)
            {
                Quantidade.Text = "0";
            }
            try
            {
                nomeProduto.Text = produtoChange.Nome;
                ValorUnitario.Text = "R$ " + produtoChange.Valor.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro" + ex);
            }
        }

        private void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            if (this.datagridProdutos.SelectedItem == null)
            {
                MessageBox.Show("Selecione um produto antes de adicionar! Ativo", "Produto obrigatório", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
                return;
            }

            if (Quantidade.Text == "0")
            {
                MessageBox.Show("O Produto precisa de uma quantidade", "Quantidade obrigatória", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
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
                        totalGeral += valorDesseItem;
                        totalPedido.Text = "R$ " + totalGeral.ToString();
                        encontrado = true;
                    }
                });
            }

            if (!encontrado)
            {
                valorDesseItem = produtoChange.Valor * Convert.ToDouble(Quantidade.Text);
                numeroItens += Convert.ToInt32(Quantidade.Text);
                totalGeral += valorDesseItem;
                totalPedido.Text = "R$ " + totalGeral.ToString();
                ItemPedido itemNovo = new ItemPedido();
                itemNovo.Quantidade = Convert.ToInt32(Quantidade.Text);
                itemNovo.Produto = produtoChange;
                itemNovo.Valor = valorDesseItem;
                produtosAdicionados.Add(itemNovo);
            }

            totalItens.Text = numeroItens.ToString();
            Quantidade.Text = "0";
        }

        private void SalvarProdutos(object sender, RoutedEventArgs e)
        {
            this.param.ItemsPedido = produtosAdicionados;
            double somaTotal = 0;
            produtosAdicionados.ForEach(p =>
            {
                somaTotal += ((double)p.Quantidade * p.Produto.Valor);
            });
            this.param.ValorTotal = somaTotal;
            this.Close();
        }
    }
}
