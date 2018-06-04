using Bibliotech.Base;
using Bibliotech.Business;
using Bibliotech.Models;
using Bibliotech.Repository;
using Bibliotech.Util;
using NReco.ImageGenerator;
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
            var list = LivroRepository.Instance.GetListLivroByExample(exemplo, true)
                .OrderBy(x => x.Titulo)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Livro>,Livro>(list,exemplo));
        }

        public ActionResult Adicionar()
        {
            var usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil != Perfil.Padrao)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Alterar(Guid id)
        {
            Livro livro = LivroRepository.Instance.GetById(id);

            var usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil != Perfil.Padrao)
                return View(livro);
            else
                return RedirectToAction("Index", "Home");

        }

        public ActionResult Acervo(Livro exemplo, int page = 1)
        {
            var list = LivroRepository.Instance.GetListLivroByExample(exemplo)
                .OrderBy(x => x.Titulo)
                .ToPagedList(page, Constantes.LIMITE_REGISTROS_PAGINA);

            return View(new Tuple<IPagedList<Livro>, Livro>(list, exemplo));
        }

        public ActionResult Visualizar(Guid id)
        {
            Livro livro = LivroRepository.Instance.GetById(id);

            return View(livro);
        }

        [HttpPost]
        public JsonResult Salvar(Livro livro)
        {
            var usuarioLogado = Functions.GetCurrentUser();
            if (usuarioLogado.Perfil != Perfil.Padrao)
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
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.USUARIO_SEM_PERMISSAO }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Remover(Guid id)
        {
            Livro livro = LivroRepository.Instance.GetById(id);

            var usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil != Perfil.Padrao)
            {
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
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.USUARIO_SEM_PERMISSAO }, JsonRequestBehavior.AllowGet);
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

        public ActionResult GerarEtiquetaExemplar(Guid id)
        {
            var usuarioLogado = Functions.GetCurrentUser();

            if (usuarioLogado.Perfil != Perfil.Padrao)
            {
                string conteudo = FormatarEtiquetaExemplar(ExemplarRepository.Instance.GetById(id));

                MemoryStream ms = Functions.ConvertHtmlToImage(conteudo, ImageFormat.Png, "--width 500 --height 150");
                ViewBag.ImageSrc = Functions.GetPngImageSrc(ms);

                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        private string FormatarEtiquetaExemplar(Exemplar exemplar)
        {
            string conteudo =
                "<html>" +
                "<head>" +
                    "<meta charset='UTF-8'>" +
                "</head>" +
                "<body>" +
                    "<table border='1' width='500px' height='150px' style='margin-top: -8; margin-left: -8; border-collapse: collapse; font-family: Franklin Gothic Medium, Arial Narrow, Arial, sans-serif'>" +
                        "<tr>" +
                            "<td width='150px' rowspan='5' style='text-align: center'>" +
                                "<img src='[QRCODE]' width='130px' height='130px' />" +
                            "</td>" +
                            "<td>[TITULO][EDICAO]</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td>[AUTOR]</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td>[EDITORA]</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td>[ISBN]</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td>[CODIGO]</td>" +
                        "</tr>" +
                    "</table>"+
                "</body>" +
                "</html>";

            conteudo = conteudo.Replace("[TITULO]",exemplar.Livro.Titulo);
            conteudo = conteudo.Replace("[EDICAO]",exemplar.Livro.Edicao != null ? " - " + exemplar.Livro.Edicao.ToString() + " Ed." : string.Empty);
            conteudo = conteudo.Replace("[AUTOR]",exemplar.Livro.Autor.Nome);
            conteudo = conteudo.Replace("[EDITORA]",exemplar.Livro.Editora.Nome);
            conteudo = conteudo.Replace("[ISBN]",exemplar.Livro.Isbn);
            conteudo = conteudo.Replace("[CODIGO]",exemplar.Codigo);
            conteudo = conteudo.Replace("[QRCODE]", Functions.GetPngImageSrc(Functions.GenerateQRCode(exemplar.Id.ToString(), 130, 130)));

            return conteudo;
        }
    }
}