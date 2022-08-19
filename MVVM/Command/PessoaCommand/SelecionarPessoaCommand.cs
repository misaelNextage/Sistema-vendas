using System.Collections.Generic;
using System.Linq;
using WpfApp3.core;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.CRUD
{
    public class SelecionarPessoaCommad : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            var viewModel = parameter as PessoaViewModel;
            return viewModel != null && viewModel.PessoasSelecionado != null;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (PessoaViewModel)parameter;

            var clonePessoa = (Pessoa)viewModel.PessoasSelecionado.Clone();

            IEnumerable<Pedido> pedidos = from pedido in viewModel.TodosPedidos
                                          where pedido.Pessoa.Id == clonePessoa.Id
                                          select pedido;
            
            viewModel.PedidosFiltrados.Clear();
            foreach (Pedido pedido in pedidos)
            {
                viewModel.PedidosFiltrados.Add(pedido);
            }
            
        }
    }
}