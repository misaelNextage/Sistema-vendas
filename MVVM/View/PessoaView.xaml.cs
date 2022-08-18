using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using WpfApp3.MVVM.CRUD;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.View
{
    /// <summary>
    /// Interaction logic for CadastrarPessoa.xaml
    /// </summary>
    public partial class CadastrarPessoa : UserControl
    {
        public static string nomeBotaoFiltroPedido = "";

        public CadastrarPessoa()
        {
            InitializeComponent();
        }

        private void Pesquisar_TextChanged(object sender, TextChangedEventArgs e)
        {

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
            e.Handled = System.Text.RegularExpressions.Regex.IsMatch(e.Text, "[^0-9]+"); //permite só números
        }

        private void FiltroStatusPedido(object sender, RoutedEventArgs e)
        {
            Button status = sender as Button;
            nomeBotaoFiltroPedido = status.Name.ToUpper();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            var x = (Button)e.Source;
            var y = sender as Button;
            var dt = x.DataContext;
            var td = (Pedido)x.DataContext;

            MudarStatusPedido mudar = new MudarStatusPedido();
            mudar.alterarStatusPedido(td, y.Name.ToUpper());
        }
    }
}
