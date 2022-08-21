using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using WpfApp3.core;
using WpfApp3.MVVM.Model;

namespace WpfApp3.MVVM.ViewModel
{
    public class IncluirPedidoCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return parameter is PedidoViewModel;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (PedidoViewModel)parameter;

            bool valido = this.validar(viewModel);

            if (!valido)
            {
                MessageBox.Show("O nome do Produto precisa ser preenchido", "Campo obrigatório", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
                return;
            }
            if (viewModel.PedidoEdit.Id > 0)
            {

                var pedidoSelecionado = viewModel.PedidoSelecionado;

                var clonePedido = (Model.Pedido)viewModel.PedidoSelecionado.Clone();

                pedidoSelecionado.Id = viewModel.PedidoEdit.Id;
                pedidoSelecionado.Pessoa = viewModel.PedidoEdit.Pessoa;
                pedidoSelecionado.ItemsPedido = viewModel.PedidoEdit.ItemsPedido;
                pedidoSelecionado.ValorTotal = viewModel.PedidoEdit.ValorTotal;
                pedidoSelecionado.DataVenda = viewModel.PedidoEdit.DataVenda;
                pedidoSelecionado.FormaPagamento = viewModel.PedidoEdit.FormaPagamento;
                pedidoSelecionado.Status = viewModel.PedidoEdit.Status;

                bool pedidoId = false;

                foreach (Pedido pedido in viewModel.Pedidos)
                {
                    if (pedido.Id == viewModel.PedidoEdit.Id)
                    {
                        pedidoId = true;
                        break;
                    }
                }
                if (pedidoId)
                {
                    viewModel.Pedidos.Remove(pedidoSelecionado);

                    var pedidoClone = (Model.Pedido)viewModel.PedidoEdit.Clone();
                    viewModel.Pedidos.Add(pedidoClone);
                }

                string jsonString = JsonSerializer.Serialize(viewModel.Pedidos, new JsonSerializerOptions() { WriteIndented = true });
                using (StreamWriter outputFile = new StreamWriter("pedido.json"))
                {
                    outputFile.WriteLine(jsonString);
                }
                viewModel.PedidoEdit.Id = 0;
                viewModel.PedidoEdit.Pessoa = null;
                viewModel.PedidoEdit.ItemsPedido = null;
                viewModel.PedidoEdit.ValorTotal = 0D; ;
                viewModel.PedidoEdit.DataVenda = DateTime.Now;
                viewModel.PedidoEdit.FormaPagamento = Pedido.FormaPagamentoEnum.Dinheiro;
                viewModel.PedidoEdit.Status = Pedido.StatusEnum.Pendente.ToString();
            }
            else
            {

                var pedido = new Model.Pedido();
                long maxId = 0;
                if (viewModel.Pedidos.Any())
                {
                    maxId = viewModel.Pedidos.Max(f => f.Id);
                }
                pedido.Id = maxId + 1;
                pedido.Pessoa = viewModel.PedidoEdit.Pessoa;
                pedido.ItemsPedido = viewModel.PedidoEdit.ItemsPedido;
                pedido.ValorTotal = viewModel.PedidoEdit.ValorTotal;
                pedido.DataVenda = viewModel.PedidoEdit.DataVenda;
                pedido.FormaPagamento = viewModel.PedidoEdit.FormaPagamento;
                pedido.Status = viewModel.PedidoEdit.Status;

                viewModel.Pedidos.Add(pedido);
                viewModel.PedidoSelecionado = pedido;

                string jsonString = JsonSerializer.Serialize(viewModel.Pedidos, new JsonSerializerOptions() { WriteIndented = true });
                using (StreamWriter outputFile = new StreamWriter("pedido.json"))
                {
                    outputFile.WriteLine(jsonString);
                }
            }
        }

        private bool validar(PedidoViewModel viewModel)
        {
            if (viewModel.PedidoEdit.Pessoa == null)
            {
                return false;
            }
            if (viewModel.PedidoEdit.ItemsPedido == null && viewModel.PedidoEdit.ItemsPedido.Count < 1)
            {
                return false;
            }
            return true;
        }

    }
}
