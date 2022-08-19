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

        public List<Produto> produtosDisponiveis;

        public int somaDosItens;
        public int somaDosProdutos;

        public double valorDoItem;
        public double valoreTotalPedido;


        public CatalagoProdutos(Pedido pedido)
        {
            InitializeComponent();
            this.param = pedido;
            this.carregarProdutos();
            this.inicializaVariaveis();
        }

        private void carregarProdutos()
        {
            ViewModel.ProdutoViewModel pvm = new ViewModel.ProdutoViewModel();
            this.produtosDisponiveis = new List<Produto>();
            datagridProdutos.ItemsSource = null;
            datagridProdutos.Items.Clear();
            foreach (Produto produto in pvm.Produtos)
            {
                if (produto.Id != null)
                {
                    produtosDisponiveis.Add(produto);
                }
            }
            datagridProdutos.ItemsSource = this.produtosDisponiveis;
            datagridProdutos.Items.Refresh();
        }

        private void inicializaVariaveis()
        {
            if (this.param.ItemsPedido != null && this.param.ItemsPedido.Count > 0)
            {
                this.carregarValoresEmTela();
            }
            else
            {
                this.param.ItemsPedido = new List<ItemPedido>();
                this.carregarValoresEmTela();
            }
        }

        private void carregarValoresEmTela()
        {
            Quantidade.Text = "0";
            nomeProduto.Text = "";
            ValorUnitario.Text = "R$ 0,00";
            btnRemove.Visibility = Visibility.Hidden;

            this.somaDosItens = 0;
            this.somaDosProdutos = 0;
            this.valorDoItem = 0;
            this.valoreTotalPedido = 0;

            this.param.ItemsPedido.ForEach(i => {
                this.somaDosItens += i.Quantidade;
                this.somaDosProdutos++;
                this.valoreTotalPedido += ((double)i.Quantidade * i.Produto.Valor);
            });
            this.param.ValorTotal = this.valoreTotalPedido;

            totalPedido.Text = valoreTotalPedido.ToString();
            totalItens.Text = somaDosItens.ToString();
        }
        private void selecionarProduto(object sender, RoutedEventArgs e)
        {
            Produto produtoChange = (Produto)this.datagridProdutos.SelectedItem;
            nomeProduto.Text = produtoChange.Nome;
            ValorUnitario.Text = "R$ " + ((double)produtoChange.Valor).ToString();
            bool existe = false;
            int qtd = 0;
            param.ItemsPedido.ForEach(i =>
            {
                if (i.Produto.Id == produtoChange.Id)
                {
                    Quantidade.Text = Convert.ToString(i.Quantidade);
                    btnRemove.Visibility = Visibility.Visible;
                    existe = true;
                }
            });
            if (!existe)
            {
                Quantidade.Text = "0";
                btnRemove.Visibility = Visibility.Hidden;
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.datagridProdutos.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Selecione um produto antes de adicionar!", "Seleção Obrigatória!", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
                return;
            }

            if (Quantidade.Text == "0" || Convert.ToInt32(Quantidade.Text) < 0)
            {
                System.Windows.MessageBox.Show("Para adicionar produtos é necessario informar uma quantidade", "Quantidade minima obrigatória", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
                return;
            }
            Produto produtoChange = (Produto)this.datagridProdutos.SelectedItem;
            bool jaExiste = false;
            param.ItemsPedido.ForEach(i =>
            {
                if (i.Produto.Id == produtoChange.Id)
                {
                    i.Quantidade = Convert.ToInt32(Quantidade.Text);
                    jaExiste = true;
                }
            });
            if (!jaExiste)
            {
                ItemPedido item = new ItemPedido();
                item.Produto = produtoChange;
                item.Quantidade = Convert.ToInt32(Quantidade.Text);
                item.Valor = ((double)item.Quantidade * item.Produto.Valor);
                param.ItemsPedido.Add(item);

            }
            this.carregarValoresEmTela();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            Produto produtoChange = (Produto)this.datagridProdutos.SelectedItem;
            int index = 0;
            this.param.ItemsPedido.ForEach(i => {
                if (i.Produto.Id == produtoChange.Id)
                {
                    index = this.param.ItemsPedido.IndexOf(i);
                }
            });
            this.param.ItemsPedido.RemoveAt(index);
            this.carregarValoresEmTela();
        }


    }
}
