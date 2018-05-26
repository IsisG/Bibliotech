using Bibliotech.Base;
using Bibliotech.Models;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bibliotech.Repository
{
    public class ParametroRepository : BaseRepository<Parametro>
    {
        private static ParametroRepository instance;

        private ParametroRepository() { }

        public static ParametroRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(ParametroRepository))
                        if (instance == null)
                            instance = new ParametroRepository();

                return instance;
            }
        }

        public override void BeforeCommitSaveOrUpdate(ISession session, ref Parametro entity)
        {
            if(entity != null && entity.Senha != null)
                entity.Senha = CriptografiaHelper.Encriptar(entity.Senha);
        }

        protected override void DoAfterGet(Parametro entity)
        {
            if (entity != null && entity.Senha != null)
                entity.Senha = CriptografiaHelper.Decriptar(entity.Senha);
        }

        public override void LazyProperties(Parametro entity)
        {
            
        }

        public virtual Parametro GetParametro()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Parametro));
                criteria.SetMaxResults(1);
                var result = criteria.List<Parametro>();

                Parametro obj = null;

                if (result.Count > 0)
                    obj = result[0];

                DoAfterGet(obj);

                return obj;
            }
        }
    }
}
