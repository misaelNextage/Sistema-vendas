using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using WpfApp3.core;
using WpfApp3.MVVM.Command.PedidoCommand;
using WpfApp3.MVVM.Model;

namespace WpfApp3.MVVM.ViewModel
{
    public class PedidoViewModel : ObservableCollection<Produto>
    {
        public PedidoViewModel(Pessoa pessoa)
        {
            this.PedidoSelecionado = new Pedido(pessoa);
        }

        private Pedido _pedidoSelecionado;

        public IncluirPedidoCommand Novo { get; private set; } = new IncluirPedidoCommand();

        public DeletarPedidoCommand Deletar { get; private set; } = new DeletarPedidoCommand();

        public EditarPedidoCommand Editar { get; private set; } = new EditarPedidoCommand();

        private Pedido _pedidoEdit = new Pedido();

        public Pedido Pedido { get; internal set; }

        public ObservableCollection<Pedido> Pedidos { get; set; }

        public Pedido PedidoSelecionado
        {
            get { return _pedidoSelecionado; }
            set
            {
                _pedidoSelecionado = value;
                Editar.RaiseCanExecuteChanged();
            }
        }

        public Pedido PedidoEdit
        {
            get { return _pedidoEdit; }
            set { _pedidoEdit = value; }
        }

        private string _pesquisaText = "";
        public string PesquisaText
        {
            get { return _pesquisaText; }
            set { _pesquisaText = value; }
        }

        public PesquisaPedido Pesquisa { get; private set; } = new PesquisaPedido();

        public PedidoViewModel()
        {
            Pedidos = new ObservableCollection<Pedido>();
            PreparaPedidoCollection();
        }
        public void PreparaPedidoCollection()
        {
            List<Pedido> source = new List<Pedido>();

            using (StreamReader r = new StreamReader("pedido.json"))
            {
                string json = r.ReadToEnd();
                source = JsonSerializer.Deserialize<List<Pedido>>(json);
            }

            source.ForEach(p =>
            {
                Pedidos.Add(p);
            });
        }
    }

    public class PesquisaPedido : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return parameter is PedidoViewModel;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (PedidoViewModel)parameter;

            string text = viewModel.PesquisaText;

            List<Pedido> json = new List<Pedido>();

            using (StreamReader r = new StreamReader("pedido.json"))
            {
                string jsonStr = r.ReadToEnd();
                json = JsonSerializer.Deserialize<List<Pedido>>(jsonStr);
            }
            viewModel.Pedidos.Clear();

            json.ForEach(p =>
            {
                if (p.ItemsPedido != null && p.Pessoa != null)
                {
                    viewModel.Pedidos.Add(p);
                }
            });

            if (viewModel.Pedidos.Count > 0)
            {
                viewModel.PedidoSelecionado = viewModel.Pedidos.FirstOrDefault();
            }
        }
    }
}