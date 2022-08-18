using WpfApp3.core;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;
using WpfApp3.MVVM.View;

namespace WpfApp3.MVVM.CRUD
{
    public class FiltrarStatusPedidosCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            var viewModel = parameter as PessoaViewModel;
            return viewModel != null && viewModel.PessoasSelecionado != null;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (PessoaViewModel)parameter;

            viewModel.PedidosFiltrados.Clear();
            foreach (Pedido pedido in viewModel.TodosPedidos)
            {
                if (pedido.StatusPedido.Equals(CadastrarPessoa.nomeBotaoFiltroPedido) && viewModel.PessoasSelecionado.Id == pedido.Pessoa.Id)
                {
                    viewModel.PedidosFiltrados.Add(pedido);
                }

            }

        }
    }
}