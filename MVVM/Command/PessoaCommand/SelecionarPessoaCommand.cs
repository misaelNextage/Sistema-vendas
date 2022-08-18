using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfApp3.core;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.CRUD
{
    class SelecionarPessoaCommad : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            var viewModel = parameter as PessoaViewModel;
            return viewModel != null && viewModel.PessoasSelecionado != null;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (PessoaViewModel)parameter;

            var clonePessoa = (Model.Pessoa)viewModel.PessoasSelecionado.Clone();

            IEnumerable<Pedido> pedidos = from pedido in viewModel.TodosPedidos
                                          where pedido.Pessoa.Id == clonePessoa.Id
                                          select pedido;
            //if (pedidos.Any() == false)
            //{
            //    MessageBox.Show("Esse cliente não possui pedidos.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
            viewModel.PedidosFiltrados.Clear();
            foreach (Pedido elm in pedidos)
            {
                viewModel.PedidosFiltrados.Add(elm);
            }

        }
    }
}