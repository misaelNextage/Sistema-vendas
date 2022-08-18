using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Lógica interna para ModalCliente.xaml
    /// </summary>
    public partial class ModalCliente : Window
    {
        public ModalCliente(ViewModel.PessoaViewModel listParam)
        {
            InitializeComponent();
            datagridPessoasModal.ItemsSource = listParam.Pessoas;
            this.param = listParam;
        }

        ViewModel.PessoaViewModel param;

        private string _pesquisaText = "";
        public string PesquisaText
        {
            get { return _pesquisaText; }
            set { _pesquisaText = value; }
        }

        public void Confirmar(object sender, RoutedEventArgs e)
        {
            param.PessoaSelecionada = (Pessoa)datagridPessoasModal.SelectedItem;
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
