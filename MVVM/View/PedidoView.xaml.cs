using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.View
{

    public partial class PedidoView : UserControl
    {
        public PedidoView()
        {
            pedidoEmCriacao = new Pedido();
            InitializeComponent();
            DataContext = new ViewModel.PedidoViewModel();
            pedidoEmCriacao.DataVenda = DateTime.Now;
            dataFaturamento.Content = pedidoEmCriacao.DataVenda;
            datagridNovoPedidos.ItemsSource = this.pedidoEmCriacao.ItemsPedido;
        }
        public Pedido pedidoEmCriacao;

        private void DatagridPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                int index = datagridNovoPedidos.SelectedIndex;
                object a = e.Source;

                object b = e.AddedItems;
            }
        }

        private void BuscarCliente(object sender, RoutedEventArgs e)
        {
            PessoaViewModel param = new PessoaViewModel();
            ModalCliente janela = new ModalCliente(param);
            janela.ShowDialog();
            pedidoEmCriacao.Pessoa = param.PessoaSelecionada;
            nomePessoa.Text = pedidoEmCriacao.Pessoa.Nome;
            idPessoa.Text = pedidoEmCriacao.Pessoa.Id.ToString();
            cpfPessoa.Text = pedidoEmCriacao.Pessoa.Cpf;
            enderecoPessoa.Text = pedidoEmCriacao.Pessoa.Endereco;
        }

        private void AdicionarProduto(object sender, RoutedEventArgs e)
        {
            if (pedidoEmCriacao.Pessoa == null)
            {
                MessageBox.Show("Selecione o cliente primeiro!", "Cliente obrigatório", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
                return;
            }

            CatalagoProdutos cp = new CatalagoProdutos(pedidoEmCriacao);
            cp.ShowDialog();
            datagridNovoPedidos.ItemsSource = null;
            datagridNovoPedidos.ItemsSource = pedidoEmCriacao.ItemsPedido;
            datagridNovoPedidos.Items.Refresh();
            totalDoPedido.Content = "R$ " + pedidoEmCriacao.ValorTotal;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selecao = (ComboBoxItem)e.AddedItems[0];
            if (selecao == null)
            {
                return;
            }
            switch (selecao.Content)
            {
                case "Boleto":
                    pedidoEmCriacao.FormaPagamento = Pedido.FormaPagamentoEnum.Boleto;
                    break;

                case "Cartao":
                    pedidoEmCriacao.FormaPagamento = Pedido.FormaPagamentoEnum.Cartao;
                    break;

                default:
                    pedidoEmCriacao.FormaPagamento = Pedido.FormaPagamentoEnum.Dinheiro;
                    break;
            }
        }

        private void FinalizarPedido(object sender, RoutedEventArgs e)
        {
            PedidoViewModel viewModel = new PedidoViewModel();
            var pedido = pedidoEmCriacao;
            bool possuiPedidoPendente = this.verificaPedidoMesmoClientePedidoPendete(viewModel);
            if (!possuiPedidoPendente)
            {
                pedido.Id = viewModel.Pedidos.Count + 1;
                viewModel.Pedidos.Add(pedido);
                string jsonString = JsonSerializer.Serialize(viewModel.Pedidos, new JsonSerializerOptions() { WriteIndented = true });
                using (StreamWriter outputFile = new StreamWriter("pedido.json"))
                {
                    outputFile.WriteLine(jsonString);
                }
                MessageBox.Show("Pedido finalizado com suecsso, va para área do cliente e prossiga no pedido", "Pedido salvo", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                pedidoEmCriacao = new Pedido();
                this.limparTela();
            }
            else
            {
                var response = MessageBox.Show("Encontramos outro pedido para este cliente, ainda pendente, para gerar um novo altere o status do pedido pendente!", "Cliente ja possui pedido pendente", MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
            }

        }

        private void limparTela()
        {
            datagridNovoPedidos.ItemsSource = null;
            idPessoa.Text = "";
            nomePessoa.Text = "";
            enderecoPessoa.Text = "";
            cpfPessoa.Text = "";
            totalDoPedido.Content = "0";
        }

        private bool verificaPedidoMesmoClientePedidoPendete(PedidoViewModel view)
        {
            bool pedidoPendendte = false;
            foreach (Pedido pedidoDaLista in view.Pedidos)
            {
                if (pedidoDaLista.Pessoa.Id.Equals(pedidoEmCriacao.Pessoa.Id) && pedidoDaLista.Status.Equals(Pedido.StatusEnum.Pendente))
                {
                    pedidoPendendte = true;
                    break;
                }
            }

            if (pedidoPendendte)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
