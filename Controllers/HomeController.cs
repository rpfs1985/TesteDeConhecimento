using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesteDeConhecimento.Models;

namespace TesteDeConhecimento.Controllers
{

    public class HomeController : Controller
    {
        private TESTE_DE_CONHECIMENTOEntities1 db = new TESTE_DE_CONHECIMENTOEntities1();

        public ActionResult Index()
        {
            InscricaoCadastroViewModel model = new InscricaoCadastroViewModel();

            var ativ = db.Atividades.ToList();


            List<AtividadeViewModel> atividades = new List<AtividadeViewModel>();
            model.Atividades = new List<AtividadeViewModel>();
            foreach (var item in ativ)
            {
                model.Atividades.Add(new AtividadeViewModel
                {
                    Descricao = "" + item.DescAtv + " - Preço: " + item.Preco + "",
                    Selecao = false,
                    CodAtv = item.CodAtv.ToString()

                });
            }

            
            var pacotes = db.Pacotes.ToList();

            model.Pacotes = db.Pacotes
       .Select(v => new SelectListItem
       {
           Text = "Pacote: " + v.Descricao + " Preço: " + v.Preco.ToString() + "",
           Value = v.CodPac.ToString()
       }
       ).ToList();


            return View(model);
        }

        [HttpPost]
        public ActionResult Inscricao(FormCollection col)
        {
            string nome = col["Nome"] as string;
            string dataNasc = col["DataNascimento"] as string;
            string tel = col["Telefone"] as string;
            string pacote = col["Pacotes"] as string;
            string atividades = col["Atividades"] as string;

            var dat = new DateTime();

            if (!string.IsNullOrEmpty(dataNasc))
            {
                bool res = DateTime.TryParse(dataNasc, out dat);

                if (!res)
                {
                    dat = DateTime.Now;
                }
            }

            int pac = int.Parse(pacote);
            string[] atividades_codigos = atividades.Split(',');

            var participante = new Participante
            {
                DataNascimento = dat,
                Nome = nome,
                Telefone = tel,
                Categoria = "Sócio"
            };

            db.Participantes.Add(participante);

            db.SaveChanges();

            var codPar = participante.CodPar;

           var particip = db.Participantes.Find(codPar);
            var pacote_selecionado = db.Pacotes.Find(pac);


            // NÃO ENTENDI BEM COMO FUNCIONA A PARTE DE CATEGORIA DO PARTICIPANTE
            //EM RELAÇÃO A ELE SER SÓCIO OU NÃO SÓCIO
            //POIS NA DESCRIÇÃO DO TESTE A TABELA DE PARTICIPANTES NÃO TEM UM CAMPO
            //CATEGORIA , ENTÃO EU DECIDI CRIAR O CAMPO NA TABELA E FAZER UMA REGRA
            //AQUI PARA INCLUIR UM VALOR CASO O PARTICIPANTE NÃO SÓCIO
           if (particip.Categoria == "Não Sócio")
            {
                pacote_selecionado.Preco = pacote_selecionado.Preco * 0.10;
            }

            db.Entry(pacote_selecionado).State = EntityState.Modified;
            db.SaveChanges();

            db.AxParticipantePacotes.Add(new AxParticipantePacote
            {
                CodPac = pacote_selecionado.CodPac,
                CodPar = codPar
            });
            db.SaveChanges();

            foreach (var item in atividades_codigos)
            {
                db.AxParticipanteAtividades.Add(new AxParticipanteAtividade
                {
                    CodAtv = int.Parse(item),
                    CodPar = codPar
                });
                db.SaveChanges();
            }



            return View();
        }


    }
}