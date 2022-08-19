using System.IO;
using System.Text.Json;
using WpfApp3.core;
using WpfApp3.MVVM.Model;
using WpfApp3.MVVM.ViewModel;

namespace WpfApp3.MVVM.CRUD
{
    public class EditarPessoaCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            var viewModel = parameter as PessoaViewModel;
            return viewModel != null && viewModel.PessoasSelecionado != null;
        }

        public override void Execute(object parameter)
        {
            var viewModel = (PessoaViewModel)parameter;

            var clonePessoa = (Model.Pessoa)viewModel.PessoasSelecionado.Clone();

            viewModel.PessoaEdit.Id = clonePessoa.Id;
            viewModel.PessoaEdit.Nome = clonePessoa.Nome;
            viewModel.PessoaEdit.Cpf = clonePessoa.Cpf != null ? System.Text.RegularExpressions.Regex.Replace(clonePessoa.Cpf, "[^0-9]", "") : clonePessoa.Cpf;
            viewModel.PessoaEdit.Endereco = clonePessoa.Endereco;

            viewModel.Pessoas.Clear();
            viewModel.PreparaPessoaCollection();

            int contador = 0;
            int posicao = -1;
            foreach (Pessoa pessoa in viewModel.Pessoas)
            {
                if (pessoa.Id == viewModel.PessoaEdit.Id)
                {
                    posicao = contador;
                }
                contador++;
            }

            viewModel.Pessoas[posicao] = viewModel.PessoaEdit;

            viewModel.Edicao = true;

            string jsonString = JsonSerializer.Serialize(viewModel.Pessoas, new JsonSerializerOptions() { WriteIndented = true });
            using (StreamWriter outputFile = new StreamWriter("pessoa.json"))
            {
                outputFile.WriteLine(jsonString);
            }

        }
    }
}