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
    [Authorize]
    public class UsuarioController : Controller
    {
        public ActionResult Listar(Usuario exemplo, int page = 1)
        {
            var list = UsuarioRepository.Instance.GetListUsuarioByExample(exemplo)
                .OrderBy(x => x.Nome)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Usuario>, Usuario>(list, exemplo));
        }

        public ActionResult Adicionar()
        {
            return View();
        }

        public ActionResult Alterar(Guid id)
        {
            Usuario usuario = UsuarioRepository.Instance.GetById(id);

            return View(usuario);
        }

        [HttpPost]
        public JsonResult Salvar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (!BUsuario.Instance.ValidarSalvar(ref usuario))
                {
                    return Json(new { Status = BUsuario.Instance.Status(), Message = BUsuario.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    UsuarioRepository.Instance.SaveOrUpdate(usuario);
                    return Json(new { Status = BUsuario.Instance.Status(), Message = BUsuario.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                }
                catch (NHibernate.StaleStateException dbcx)
                {
                    return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_CONCORRENCIA }, JsonRequestBehavior.AllowGet);
                }

            }
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_GENERICO }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remover(Guid id)
        {
            Usuario usuario = UsuarioRepository.Instance.GetById(id);
            if (!BUsuario.Instance.ValidarRemover(ref usuario))
            {
                return Json(new { Status = BUsuario.Instance.Status(), Message = BUsuario.Instance.MensagemRemover() }, JsonRequestBehavior.AllowGet);
            }

            UsuarioRepository.Instance.Delete(usuario);

            return Json(new { Status = Constantes.STATUS_SUCESSO, Message = Mensagens.REMOVIDO_SUCESSO }, JsonRequestBehavior.AllowGet);
        }
    }
}
