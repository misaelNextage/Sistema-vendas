using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
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

            //Linq
            IEnumerable<Pedido> pedidos = from pedido in todosPedidos
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