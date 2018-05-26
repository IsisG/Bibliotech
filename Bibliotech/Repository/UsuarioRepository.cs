using Bibliotech.Base;
using Bibliotech.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario>
    {
        private static UsuarioRepository instance;

        private UsuarioRepository() { }

        public static UsuarioRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(UsuarioRepository))
                        if (instance == null)
                            instance = new UsuarioRepository();

                return instance;
            }
        }

        public override void BeforeCommitSaveOrUpdate(ISession session, ref Usuario entity)
        {
            entity.Senha = CriptografiaHelper.Encriptar(entity.Senha);
        }

        protected override void DoAfterGet(Usuario entity)
        {
            if (entity != null)
                entity.Senha = CriptografiaHelper.Decriptar(entity.Senha);
        }

        public List<Usuario> GetListUsuarioByExample(Usuario entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Usuario));

                if(entity.Id != null)
                    criteria.Add(Restrictions.Eq("Id", entity.Id));

                if (entity.DataNascimento != null)
                    criteria.Add(Restrictions.Eq("DataNascimento", entity.DataNascimento));

                if (entity.Perfil != null)
                    criteria.Add(Restrictions.Eq("Perfil", entity.Perfil));

                if (entity.Email != null)
                    criteria.Add(Restrictions.InsensitiveLike("Email", "%" + entity.Email + "%"));

                if (entity.Login != null)
                    criteria.Add(Restrictions.InsensitiveLike("Login", "%" + entity.Login + "%"));

                if (entity.Nome != null)
                    criteria.Add(Restrictions.InsensitiveLike("Nome", "%" + entity.Nome + "%"));

                return criteria.List<Usuario>().ToList();
            }
        }

        public override void LazyProperties(Usuario model)
        {
            
        }
    }
}