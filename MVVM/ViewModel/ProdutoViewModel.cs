using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using WpfApp3.core;
using WpfApp3.MVVM.crud;
using WpfApp3.MVVM.Model;

namespace WpfApp3.MVVM.ViewModel
{
    class ProdutoViewModel : ObservableCollection<Produto>
    {
        private Produto _produtoSelecionado;

        public NovoProdutoCommand Novo { get; private set; } = new NovoProdutoCommand();

        public DeletarProdutoCommand Deletar { get; private set; } = new DeletarProdutoCommand();

        public EditarProdutoCommand Editar { get; private set; } = new EditarProdutoCommand();

        private Produto _produtoEdit = new Produto();

        public Produto Produto { get; internal set; }

        public ObservableCollection<Produto> Produtos { get; private set; }

        public Produto ProdutoSelecionado
        {
            get { return _produtoSelecionado; }
            set
            {
                _produtoSelecionado = value;
                Editar.RaiseCanExecuteChanged();
            }
        }

        public Produto ProdutoEdit
        {
            get { return _produtoEdit; }
            set { _produtoEdit = value; }
        }

        private string _pesquisaText = "";
        public string PesquisaText
        {
            get { return _pesquisaText; }
            set { _pesquisaText = value; }
        }

        public PesquisaProduto Pesquisa { get; private set; } = new PesquisaProduto();

        public ProdutoViewModel()
        {
            Produtos = new ObservableCollection<Produto>();
            PreparaProdutoCollection();
        }

        public void PreparaProdutoCollection()
        {
            List<Produto> source = new List<Produto>();

            using (StreamReader r = new StreamReader("produto.json"))
            {
                string json = r.ReadToEnd();
                source = JsonSerializer.Deserialize<List<Produto>>(json);
            }

            source.ForEach(p =>
            {
                Produtos.Add(p);
            });

            if (Produtos.Count > 0)
            {
                ProdutoSelecionado = Produtos.FirstOrDefault();
            }
        }
    }

    class PesquisaProduto : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return parameter is ProdutoViewModel;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (ProdutoViewModel)parameter;

            string text = viewModel.PesquisaText;

            List<Produto> json = new List<Produto>();

            using (StreamReader r = new StreamReader("produto.json"))
            {
                string jsonStr = r.ReadToEnd();
                json = JsonSerializer.Deserialize<List<Produto>>(jsonStr);
            }
            viewModel.Produtos.Clear();

            json.ForEach(p =>
            {
                if (p.Nome != null && p.Nome.ToLower().Contains(text.ToLower()))
                {
                    viewModel.Produtos.Add(p);
                }
            });

            if (viewModel.Produtos.Count > 0)
            {
                viewModel.ProdutoSelecionado = viewModel.Produtos.FirstOrDefault();
            }
        }
    }
}