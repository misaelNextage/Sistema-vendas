using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfApp3.core;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.Command.PedidoCommand
{
    public class EditarPedidoCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            var viewModel = parameter as PedidoViewModel;
            return viewModel != null && viewModel.PedidoSelecionado != null;
        }

        public override void Execute(object parameter)
        {

            var viewModel = (PedidoViewModel)parameter;

            var pedidoSelecionado = viewModel.PedidoSelecionado;

            var clonePedido = (Model.Pedido)viewModel.PedidoSelecionado.Clone();

            viewModel.PedidoEdit.Id = clonePedido.Id;
            viewModel.PedidoEdit.Pessoa = clonePedido.Pessoa;
            viewModel.PedidoEdit.ItemsPedido = clonePedido.ItemsPedido;
            viewModel.PedidoEdit.ValorTotal = clonePedido.ValorTotal;
            viewModel.PedidoEdit.DataVenda = clonePedido.DataVenda;
            viewModel.PedidoEdit.FormaPagamento = clonePedido.FormaPagamento;
            viewModel.PedidoEdit.Status = clonePedido.Status;
        }
    
    }
}
