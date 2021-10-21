using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TesteDeConhecimento.Models
{
    public class InscricaoCadastroViewModel
    {
        [Required(ErrorMessage = "Informe o Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe a Data de Nascimento")]
        public string DataNascimento { get; set; }

        [Required(ErrorMessage = "Informe o Telefone")]
        public string Telefone { get; set; }

        public List<System.Web.Mvc.SelectListItem> Pacotes { get; set; }
        public List<AtividadeViewModel> Atividades;


    }

    public class AtividadeViewModel
    {
        public string Descricao { get; set; }
        public bool Selecao { get; set; }
        public string CodAtv { get; set; }
    }
}