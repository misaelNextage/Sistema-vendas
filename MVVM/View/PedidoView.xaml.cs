﻿using System;
using System.Collections.Generic;
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

        public void salvar(object sender, RoutedEventArgs e)
        {

            Pedido pedido = new Pedido();

            List<Pedido> source = new List<Pedido>();

            using (System.IO.StreamReader r = new System.IO.StreamReader("pedido.json"))
            {
                string json = r.ReadToEnd();
                source = JsonSerializer.Deserialize<List<Pedido>>(json);
            }

            source.Add(pedido);

            string jsonString = JsonSerializer.Serialize(source, new JsonSerializerOptions() { WriteIndented = true });
            using (StreamWriter outputFile = new StreamWriter("pedido.json"))
            {
                outputFile.WriteLine(jsonString);
            }
        }

        private void DatagridPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                int index = datagridNovoPedidos.SelectedIndex;
                object a = e.Source;

                object b = e.AddedItems;

            }
        }

        private void Btn_Selecionar_Pessoa(object sender, RoutedEventArgs e)
        {
            PessoaViewModel param = new PessoaViewModel();
            param.CarregarTela();

            ModalCliente janela = new ModalCliente(param);
            janela.ShowDialog();
            if (param.PessoaSelecionada != null)
            {
                pedidoEmCriacao.Pessoa = param.PessoaSelecionada;
                nomePessoa.Text = pedidoEmCriacao.Pessoa.Nome;
                idPessoa.Text = pedidoEmCriacao.Pessoa.Id.ToString();
                cpfPessoa.Text = pedidoEmCriacao.Pessoa.Cpf;
                enderecoPessoa.Text = pedidoEmCriacao.Pessoa.Endereco;
            }
        }

        private void Btn_Selecionar_Produtos(object sender, RoutedEventArgs e)
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

        private void Btn_Finalizar_Pedido(object sender, RoutedEventArgs e)
        {
            PedidoViewModel viewModel = new PedidoViewModel();
            var pedido = pedidoEmCriacao;
            bool possuiPedidoPendente = this.verificaPedidoMesmoClientePedidoPendete(viewModel);
            if (!possuiPedidoPendente)
            {
                pedido.Id = viewModel.Pedidos.Count + 1;
                pedido.Status = Pedido.StatusEnum.Pendente.ToString();
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
                if (pedidoDaLista.Pessoa.Id.Equals(pedidoEmCriacao.Pessoa.Id) &&
                    pedidoDaLista.Status != null && pedidoDaLista.Status.Equals(Pedido.StatusEnum.Pendente.ToString()))
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
            TelaPedidos tp = new TelaPedidos();
            tp.Show();
        }
    }
}
