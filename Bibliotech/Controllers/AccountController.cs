using Bibliotech.Base;
using Bibliotech.Models;
using Bibliotech.Repository;
using Bibliotech.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Bibliotech.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult SignIn()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return Redirect("/Home/Index");

            return View(new Tuple<Usuario, string>(null, Request.QueryString["ReturnUrl"]));
        }

        [HttpPost]
        public ActionResult SignIn(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Senha = CriptografiaHelper.Encriptar(usuario.Senha);
                usuario = UsuarioRepository.Instance.GetByExample(usuario);

                if(usuario == null)
                    return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.USUARIO_SENHA_INVALIDOS }, JsonRequestBehavior.AllowGet);

                FormsAuthentication.SetAuthCookie(usuario.Email, false);
                return Json(new { Status = Constantes.STATUS_SUCESSO }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = Constantes.STATUS_ERRO, Message = Mensagens.ERRO_GENERICO }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }
    }
}