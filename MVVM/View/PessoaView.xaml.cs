using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp3.MVVM.CRUD;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.View
{
    public partial class CadastrarPessoa : UserControl
    {
        public static string nomeBotaoFiltroPedido = "";

        public CadastrarPessoa()
        {
            InitializeComponent();
        }

        private void Cpf_LostFocus(object sender, RoutedEventArgs e)
        {
            string cpf = Cpf.Text;
            Cpf.MaxLength = 14;
            try
            {
                if(Cpf.Text != null && Cpf.Text != "")
                {
                    Cpf.Text = Cpf.Text.Length == 11 ? System.Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00"): cpf;
                }
            }
            catch { Cpf.Text = cpf; }
        }

        private void Cpf_GotFocus(object sender, RoutedEventArgs e)
        {
            Cpf.Text = System.Text.RegularExpressions.Regex.Replace(Cpf.Text, "[^0-9]","");
        }

        private void Cpf_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var cpfDigitado = sender as TextBox;
            Cpf.MaxLength = 11;
            e.Handled = System.Text.RegularExpressions.Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void FiltroStatusPedido(object sender, RoutedEventArgs e)
        {
            Button status = sender as Button;
            nomeBotaoFiltroPedido = status.Name.ToUpper();
        }

        private void AlterarStatus(object sender, RoutedEventArgs e)
        {
           
            var btnSource = (Button)e.Source;
            var nomeBotao = sender as Button;
            var pedido = (Pedido)btnSource.DataContext;

            MudarStatusPedido mudarStatus = new MudarStatusPedido();
            mudarStatus.alterarStatusPedido(pedido, nomeBotao.Name.ToUpper());

            datagridPed.Items.Refresh();
        }

        private void Incluir_Novo_Pedido(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btnSource = (Button)e.Source;
                Button nomeBotao = sender as Button;
                PessoaViewModel pessoaVm = (PessoaViewModel)btnSource.DataContext;
                var teste = datagridPessoas.SelectedItems;
                Pessoa pessoaSelecionada = pessoaVm.PessoaSelecionada;

                if (pessoaSelecionada != null)
                {
                    MainWindow main = (MainWindow)Application.Current.MainWindow;
                    MainViewModel vm = (MainViewModel)main.DataContext;
                    vm.PedidoVm.PedidoEdit = null;
                    PedidoViewModel pedidoVm = (PedidoViewModel)vm.PedidoVm;
                    main.Pedidos.IsChecked = true;
                    pedidoVm = new PedidoViewModel(pessoaSelecionada);
                    vm.PedidoVm.PedidoEdit = new Pedido(pessoaSelecionada);
                    pedidoVm.PedidoEdit = new Pedido(pessoaSelecionada);
                    vm.CurrentView = pedidoVm;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro" + ex);
            }
        }
    }
}
