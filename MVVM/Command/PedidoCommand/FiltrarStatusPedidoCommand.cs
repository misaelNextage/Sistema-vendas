using WpfApp3.core;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;
using WpfApp3.MVVM.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

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

            List<Pedido> source = new List<Pedido>();

            ObservableCollection<Pedido> todosPedidos = new ObservableCollection<Pedido>();

            using (StreamReader r = new StreamReader("pedido.json"))
            {
                string json = r.ReadToEnd();
                source = JsonSerializer.Deserialize<List<Pedido>>(json);
            }

            source.ForEach(p =>
            {
                todosPedidos.Add(p);
            });

            foreach (Pedido pedido in todosPedidos)
            {
                if ((CadastrarPessoa.nomeBotaoFiltroPedido.ToLower().Equals("todos") && viewModel.PessoasSelecionado.Id == pedido.Pessoa.Id) || 
                    (pedido.Status.ToLower().Equals(CadastrarPessoa.nomeBotaoFiltroPedido.ToLower()) && viewModel.PessoasSelecionado.Id == pedido.Pessoa.Id))
                {
                    viewModel.PedidosFiltrados.Add(pedido);
                }
            }
        }
    }
}