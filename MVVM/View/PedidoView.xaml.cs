using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfApp3.MVVM.Model;
using System.IO;
using System.Text.Json;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.View
{
    /// <summary>
    /// Interaction logic for PedidoView.xaml
    /// </summary>
    public partial class PedidoView : UserControl
    {
        public PedidoView()
        {
            pedidoEmCriacao = new Pedido();
            InitializeComponent();
            DataContext = new ViewModel.PedidoViewModel();
            pedidoEmCriacao.DataVenda = "2022-08-18";
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

        private void Button_Click(object sender, RoutedEventArgs e)
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

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            if (pedidoEmCriacao.Pessoa == null)
            {
                System.Windows.MessageBox.Show("Selecione o cliente primeiro Boy!!!", "Cliente obrigatório", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
                return;
            }

            CatalagoProdutos cp = new CatalagoProdutos(pedidoEmCriacao);
            cp.ShowDialog();
            datagridNovoPedidos.ItemsSource = null;
            datagridNovoPedidos.ItemsSource = pedidoEmCriacao.ItemsPedido;
            datagridNovoPedidos.Items.Refresh();
            totalDoPedido.Content = "R$ " + pedidoEmCriacao.ValorTotal;
        }

        private void Calendar_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

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
                    pedidoEmCriacao.FormaPagamentoPedido = "Boleto";
                    break;

                case "Cartao":
                    pedidoEmCriacao.FormaPagamentoPedido = "Cartao";
                    break;

                default:
                    pedidoEmCriacao.FormaPagamentoPedido = "Dinheiro";
                    break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
                System.Windows.MessageBox.Show("Pedido finalizado com suecsso, va para area do cliente e prossiga no pedido", "Pedido salvo", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                pedidoEmCriacao = new Pedido();
                this.limparTela();
            }
            else
            {
                var response = System.Windows.MessageBox.Show("Encontramos outro pedido para este cliente, ainda pendente, para gerar um novo altere o status do pedido pendente!", "Cliente ja possui pedido pendente", MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
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
            bool encontrou = false;
            foreach (Pedido pedidoDaLista in view.Pedidos)
            {
                if (pedidoDaLista.Pessoa.Id.Equals(pedidoEmCriacao.Pessoa.Id) && pedidoDaLista.StatusPedido.Equals(Pedido._status.PENDENTE))
                {
                    encontrou = true;
                    break;
                }
            }

            if (encontrou)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            {
                var aff = checkBox;
                //foreach (DataGridCheckBoxColumn ck in checkBox)
                //{
                //    //valos exibir a linha da [0](Cells[0]) pois ela representa a coluna checkbox
                //    //que foi selecionada
                //    if (dp.Cells[0].Value != null)
                //    {
                //        MessageBox.Show("Linha " + dr.Index + " foi selecionada");
                //    }
                //}

                var teste = datagridNovoPedidos;
                int a = 0;
            }
        }
    }
}
