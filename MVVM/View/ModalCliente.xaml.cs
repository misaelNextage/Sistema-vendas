using System.Windows;
using WpfApp3.MVVM.Model;

namespace WpfApp3.MVVM.View
{
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
    }
}
