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
    public class EstanteRepository : BaseRepository<Estante>
    {
        private static EstanteRepository instance;

        private EstanteRepository() { }

        public static EstanteRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(EstanteRepository))
                        if (instance == null)
                            instance = new EstanteRepository();

                return instance;
            }
        }
        public List<Estante> GetListEstanteByExample(Estante entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Estante));

                if (entity.Id != null)
                    criteria.Add(Restrictions.Eq("Id", entity.Id));

                if (entity.Descricao != null)
                    criteria.Add(Restrictions.InsensitiveLike("Descricao", "%" + entity.Descricao + "%"));

                return criteria.List<Estante>().ToList();
            }
        }

        public override void LazyProperties(Estante entity)
        {
            if(entity.Prateleiras != null)
            {
                foreach (var item in entity.Prateleiras)
                    item.ToString();
            }
        }

        public override void BeforeCommitSaveOrUpdate(ISession session, ref Estante entity)
        {
            if (entity.Prateleiras != null)
            {
                for (int i = 0; i < entity.Prateleiras.Count; i++)
                {
                    if (Functions.IsNullExcludingProperties(entity.Prateleiras[i], "Id"))
                    {
                        if (entity.Prateleiras[i].Id != null)
                        {
                            entity.Prateleiras[i] = (Prateleira)session.Get("Prateleira", entity.Prateleiras[i].Id);
                            session.Delete(entity.Prateleiras[i]);
                        }
                        entity.Prateleiras.Remove(entity.Prateleiras[i]);
                        i--;
                        continue;
                    }
                    else
                    {
                        entity.Prateleiras[i].Estante = entity;
                    }

                }
            }
        }
    }
}