using Bibliotech.Base;
using Bibliotech.Business;
using Bibliotech.Models;
using Bibliotech.Repository;
using Bibliotech.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bibliotech.Controllers
{
    public class PagamentoMultaController : Controller
    {
        public ActionResult ListarEmprestimosAPagar(Emprestimo exemplo, int page = 1)
        {
            Usuario usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil == Perfil.Padrao)
                exemplo = new Emprestimo { Usuario = new Usuario { Id = usuarioLogado.Id } };

            var listEmprestimos = EmprestimoRepository.Instance.GetListEmprestimosAtrasadosByExemplo(exemplo)
                .OrderBy(x => x.DataInicio).ThenBy(x => x.Usuario.Nome).ToList();

            var listPagamentos = PagamentoMultaRepository.Instance.GetList();
            foreach(var pagamento in listPagamentos)
            {
                listEmprestimos.RemoveAll(x => x.Id == pagamento.Emprestimo.Id);
            }

            return View(new Tuple<IPagedList<Emprestimo>, Emprestimo>(listEmprestimos.ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA), exemplo));
        }

        public JsonResult Pagar(PagamentoMulta pagamentoMulta)
        {
            var usuarioLogado = Functions.GetCurrentUser();
            if (usuarioLogado.Perfil != Perfil.Padrao)
            {
                try
                {
                    pagamentoMulta.Data = DateTime.Today;

                    PagamentoMultaRepository.Instance.SaveOrUpdate(pagamentoMulta);

                    return Json(new { Status = BEmprestimo.Instance.Status(), Message = Mensagens.PAGAMENTO_SUCESSO }, JsonRequestBehavior.AllowGet);
                }
                catch (NHibernate.StaleStateException dbcx)
                {
                    return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_CONCORRENCIA }, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_GENERICO }, JsonRequestBehavior.AllowGet);
        }
    }
}