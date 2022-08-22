using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using WpfApp3.core;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.CRUD
{
    public class DeletarPessoaCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            var viewModel = parameter as PessoaViewModel;
            return viewModel != null && viewModel.PessoasSelecionado != null;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (PessoaViewModel)parameter;

            long idPessoaSelecionada = viewModel.PessoasSelecionado.Id;

            if (MessageBox.Show("Você tem certeza que quer deletar esse item?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                viewModel.Pessoas.Remove(viewModel.PessoasSelecionado);
                viewModel.PessoasSelecionado = viewModel.Pessoas.FirstOrDefault();
            }
            string jsonString = JsonSerializer.Serialize(viewModel.Pessoas, new JsonSerializerOptions() { WriteIndented = true });
            using (StreamWriter outputFile = new StreamWriter("pessoa.json"))
            {
                outputFile.WriteLine(jsonString);
            }

            
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


            //Linq - Filtrando os pedidos da pessoa excluída
            IEnumerable<Pedido> pedidos = from pedido in todosPedidos
                                          where pedido.Pessoa.Id != idPessoaSelecionada
                                          select pedido;


            jsonString = JsonSerializer.Serialize(pedidos, new JsonSerializerOptions() { WriteIndented = true });
            using (StreamWriter outputFile = new StreamWriter("pedido.json"))
            {
                outputFile.WriteLine(jsonString);
            }

        }
    }
}