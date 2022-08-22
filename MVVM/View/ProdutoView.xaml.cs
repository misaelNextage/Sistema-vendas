using System.Windows;
using System.Windows.Controls;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.View
{
    public partial class CadastroProduto : UserControl
    {
        public CadastroProduto()
        {
            InitializeComponent();
            DataContext = new ViewModel.ProdutoViewModel();
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            MainViewModel vm = (MainViewModel)main.DataContext;
            vm.PedidoVm.PedidoEdit = null;
        }

        private void DatagridProdutos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                int index = datagridProdutos.SelectedIndex;
                object a = e.Source;

                object b = e.AddedItems;

            }
        }
    }
}
