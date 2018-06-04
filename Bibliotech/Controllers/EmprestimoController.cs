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
using ZXing;

namespace Bibliotech.Controllers
{
    [Authorize]
    public class EmprestimoController : Controller
    {
        public ActionResult Emprestimo()
        {
            var usuarioLogado = Functions.GetCurrentUser();

            if(usuarioLogado.Perfil != Perfil.Padrao)
                return View();
            else
                return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public JsonResult EmprestimoPost(Emprestimo emprestimo)
        {
            var usuarioLogado = Functions.GetCurrentUser();
            if (usuarioLogado.Perfil != Perfil.Padrao)
            {
                if (ModelState.IsValid)
                {
                    Exemplar exemplar = ExemplarRepository.Instance.GetById(emprestimo.Exemplar.Id);

                    if (!BEmprestimo.Instance.ValidarEmprestimo(ref emprestimo, ref exemplar))
                    {
                        return Json(new { Status = BEmprestimo.Instance.Status(), Message = BEmprestimo.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                    }
                    try
                    {
                        EmprestimoRepository.Instance.SaveOrUpdateEmprestimoUpdateExemplar(emprestimo, exemplar);

                        BEmprestimo.Instance.EnvioEmailEmprestimo(emprestimo);

                        return Json(new { Status = BEmprestimo.Instance.Status(), Message = Mensagens.EMPRESTIMO_SUCESSO }, JsonRequestBehavior.AllowGet);
                    }
                    catch (NHibernate.StaleStateException dbcx)
                    {
                        return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_CONCORRENCIA }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                    return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_GENERICO }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.USUARIO_SEM_PERMISSAO }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Devolucao()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DevolucaoPost(Emprestimo emprestimo)
        {
            if (ModelState.IsValid)
            {
                Exemplar exemplar = ExemplarRepository.Instance.GetById(emprestimo.Exemplar.Id);
                Emprestimo emprestimoRetorno = EmprestimoRepository.Instance.GetById(emprestimo.Id);

                if (!BEmprestimo.Instance.ValidarDevolucao(ref emprestimoRetorno, ref exemplar))
                {
                    return Json(new { Status = BEmprestimo.Instance.Status(), Message = BEmprestimo.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    EmprestimoRepository.Instance.SaveOrUpdateEmprestimoUpdateExemplar(emprestimoRetorno, exemplar);

                    BEmprestimo.Instance.EnvioEmailDevolucao(emprestimo);

                    return Json(new { Status = BEmprestimo.Instance.Status(), Message = Mensagens.DEVOLUCAO_SUCESSO }, JsonRequestBehavior.AllowGet);
                }
                catch (NHibernate.StaleStateException dbcx)
                {
                    return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_CONCORRENCIA }, JsonRequestBehavior.AllowGet);
                }

            }
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_GENERICO }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RenovacaoPost(Emprestimo emprestimo)
        {
            if (ModelState.IsValid)
            {
                Emprestimo emprestimoRetorno = EmprestimoRepository.Instance.GetById(emprestimo.Id);

                if (!BEmprestimo.Instance.ValidarRenovacao(ref emprestimoRetorno))
                {
                    return Json(new { Status = BEmprestimo.Instance.Status(), Message = BEmprestimo.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    EmprestimoRepository.Instance.SaveOrUpdate(emprestimoRetorno);

                    BEmprestimo.Instance.EnvioEmailRenovacao(emprestimo);

                    return Json(new { Status = BEmprestimo.Instance.Status(), Message = Mensagens.RENOVACAO_SUCESSO }, JsonRequestBehavior.AllowGet);
                }
                catch (NHibernate.StaleStateException dbcx)
                {
                    return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_CONCORRENCIA }, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_GENERICO }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarUsuario(string id)
        {
            Guid usuarioId;
            string nomeUsuario = "", mensagem = "";
            int status;

            try
            {
                usuarioId = new Guid(id);
                nomeUsuario = UsuarioRepository.Instance.GetById(usuarioId).Nome;
                status = Constantes.STATUS_SUCESSO;
            }
            catch
            {
                usuarioId = Guid.Empty;
                mensagem = Mensagens.CODIGO_ESCANEADO_INVALIDO;
                status = Constantes.STATUS_ERRO;
            }

            return Json(new { Status = status, Texto = nomeUsuario, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarExemplar(string id)
        {
            Guid exemplarId;
            string tituloExemplar = "", mensagem = "";
            int status;

            try
            {
                exemplarId = new Guid(id);
                tituloExemplar = ExemplarRepository.Instance.GetById(exemplarId).Livro.Titulo;
                status = Constantes.STATUS_SUCESSO;
            }
            catch
            {
                exemplarId = Guid.Empty;
                mensagem = Mensagens.CODIGO_ESCANEADO_INVALIDO;
                status = Constantes.STATUS_ERRO;
            }

            return Json(new { Status = status, Texto = tituloExemplar, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarEmprestimosAtivos(Emprestimo exemplo, int page = 1)
        {
            Usuario usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil == Perfil.Padrao)
                exemplo = new Emprestimo { Usuario = new Usuario { Id = usuarioLogado.Id } };

            var list = EmprestimoRepository.Instance.GetListEmprestimosAtivosByExemplo(exemplo)
                .OrderBy(x => x.DataInicio).ThenBy(x => x.Usuario.Nome)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Emprestimo>, Emprestimo>(list, exemplo));
        }

        public ActionResult ListarEmprestimosAtrasados(Emprestimo exemplo, int page = 1)
        {
            Usuario usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil == Perfil.Padrao)
                exemplo = new Emprestimo { Usuario = new Usuario { Id = usuarioLogado.Id } };

            var list = EmprestimoRepository.Instance.GetListEmprestimosAtrasadosByExemplo(exemplo)
                .OrderBy(x => x.DataInicio).ThenBy(x => x.Usuario.Nome)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Emprestimo>, Emprestimo>(list, exemplo));
        }

        public ActionResult ListarEmprestimosAnteriores(Emprestimo exemplo, int page = 1)
        {
            Usuario usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil == Perfil.Padrao)
                exemplo = new Emprestimo { Usuario = new Usuario { Id = usuarioLogado.Id } };

            var list = EmprestimoRepository.Instance.GetListEmprestimosAnterioresByExemplo(exemplo)
                .OrderBy(x => x.DataInicio).ThenBy(x => x.Usuario.Nome)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Emprestimo>, Emprestimo>(list, exemplo));
        }
    }
}
