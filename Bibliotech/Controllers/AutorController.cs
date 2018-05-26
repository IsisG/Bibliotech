using Bibliotech.Models;
using Bibliotech.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bibliotech.Controllers
{
    [Authorize]
    public class AutorController : Controller
    {
        public ActionResult Listar(int? page)
        {
            return View(AutorRepository.Instance.GetPagedList(page, 10));
        }

        public ActionResult Adicionar()
        {
            return View();
        }

        public ActionResult Alterar(Guid id)
        {
            Autor Autor = AutorRepository.Instance.GetById(id);

            return View(Autor);
        }

        [HttpPost]
        public JsonResult Salvar(Autor autor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AutorRepository.Instance.SaveOrUpdate(autor);
                    return Json(new { Status = 0, Message = "salvo com sucesso" }, JsonRequestBehavior.AllowGet);
                }
                catch (NHibernate.StaleStateException dbcx)
                {
                    return Json(new { Status = 0, Message = "erro." }, JsonRequestBehavior.AllowGet);
                }

            }
            else
                return Json(new { Status = 0, Message = "erro." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remover(Guid id)
        {
            Autor Autor = AutorRepository.Instance.GetById(id);

            AutorRepository.Instance.Delete(Autor);

            return Json(new { Status = 0, Message = "removido." }, JsonRequestBehavior.AllowGet);
        }
    }

}
