using System.Windows.Controls;

namespace WpfApp3.MVVM.View
{
    public partial class CadastroProduto : UserControl
    {
        public CadastroProduto()
        {
            InitializeComponent();
            DataContext = new ViewModel.ProdutoViewModel();
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
