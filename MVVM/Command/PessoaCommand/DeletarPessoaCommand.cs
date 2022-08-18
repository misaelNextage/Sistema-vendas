using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using WpfApp3.core;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.CRUD
{
    class DeletarPessoaCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            var viewModel = parameter as PessoaViewModel;
            return viewModel != null && viewModel.PessoasSelecionado != null;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (PessoaViewModel)parameter;

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
        }
    }
}