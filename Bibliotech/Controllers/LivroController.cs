using Bibliotech.Business;
using Bibliotech.Models;
using Bibliotech.Repository;
using Bibliotech.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bibliotech.Controllers
{
    [Authorize]
    public class LivroController : Controller
    {
        public ActionResult Listar(Livro exemplo, int page = 1)
        {
            var list = LivroRepository.Instance.GetListLivroByExample(exemplo)
                .OrderBy(x => x.Titulo)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Livro>,Livro>(list,exemplo));
        }

        public ActionResult Adicionar()
        {
            return View();
        }

        public ActionResult Alterar(Guid id)
        {
            Livro livro = LivroRepository.Instance.GetById(id);

            return View(livro);
        }

        public ActionResult Acervo(Livro exemplo, int page = 1)
        {
            var list = LivroRepository.Instance.GetListLivroByExample(exemplo)
                .OrderBy(x => x.Titulo)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Livro>, Livro>(list, exemplo));
        }

        [HttpPost]
        public JsonResult Salvar(Livro livro)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    livro.NomeFoto = Request.Files[0].FileName;
                }

                if (livro.Id != null)
                {
                    if (System.IO.Directory.Exists(Server.MapPath("~/LivroFiles/" + livro.Id.ToString())))
                    {
                        System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Server.MapPath("~/LivroFiles/" + livro.Id.ToString()));
                        foreach (System.IO.FileInfo file in directory.GetFiles())
                        {
                            if (file.Name != livro.NomeFoto)
                                file.Delete();
                        }
                    }
                }

                if (!BLivro.Instance.ValidarSalvar(ref livro))
                {
                    return Json(new { Status = BLivro.Instance.Status(), Message = BLivro.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    LivroRepository.Instance.SaveOrUpdate(livro);

                    if (Request.Files.Count > 0)
                    {
                        System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Server.MapPath("~/LivroFiles/" + livro.Id.ToString()));
                        if (!System.IO.Directory.Exists(directory.ToString()))
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/LivroFiles/" + livro.Id.ToString()));
                        Request.Files[0].SaveAs(Server.MapPath("~/LivroFiles/" + livro.Id.ToString() + "/" + Request.Files[0].FileName));
                    }

                    return Json(new { Status = BLivro.Instance.Status(), Message = BLivro.Instance.MensagemSalvar() }, JsonRequestBehavior.AllowGet);
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
            Livro livro = LivroRepository.Instance.GetById(id);
            if (!BLivro.Instance.ValidarRemover(ref livro))
            {
                return Json(new { Status = BLivro.Instance.Status(), Message = BLivro.Instance.MensagemRemover() }, JsonRequestBehavior.AllowGet);
            }

            LivroRepository.Instance.Delete(livro);
            if (System.IO.Directory.Exists(Server.MapPath("~/LivroFiles/" + id.ToString())))
            {
                System.IO.Directory.Delete(Server.MapPath("~/LivroFiles/" + id.ToString()), true);
            }

            return Json(new { Status = Constantes.STATUS_SUCESSO, Message = Mensagens.REMOVIDO_SUCESSO }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetImagemCapa(Guid id)
        {
            byte[] imagemArray = null;
            var livro = LivroRepository.Instance.GetById(id);
            string filePath = Server.MapPath("~/LivroFiles/" + id + "/" + livro.NomeFoto);
            imagemArray = System.IO.File.ReadAllBytes(filePath);
            if (imagemArray == null)
            {
                return null;
            }
            else
            {
                return new FileContentResult(imagemArray, "image/jpeg");
            }
        }
    }
}