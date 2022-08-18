using System.IO;
using System.Linq;
using System.Text.Json;
using WpfApp3.core;

namespace WpfApp3.MVVM.ViewModel
{
    class PesquisarPessoaCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            var viewModel = parameter as PessoaViewModel;
            return viewModel != null && viewModel.PessoasSelecionado != null;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (PessoaViewModel)parameter;
            viewModel.Pessoas.Remove(viewModel.PessoasSelecionado);
            viewModel.PessoasSelecionado = viewModel.Pessoas.FirstOrDefault();

            string jsonString = JsonSerializer.Serialize(viewModel.Pessoas, new JsonSerializerOptions() { WriteIndented = true });
            using (StreamWriter outputFile = new StreamWriter("produto.json"))
            {
                outputFile.WriteLine(jsonString);
            }
        }
    }
}