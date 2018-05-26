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
    public class EditoraRepository : BaseRepository<Editora>
    {

        private static EditoraRepository instance;

        private EditoraRepository() { }

        public static EditoraRepository Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(EditoraRepository))
                        if (instance == null)
                            instance = new EditoraRepository();

                return instance;
            }
        }

        public List<Editora> GetListEditoraByExample(Editora entity)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Editora));

                if (entity.Id != null)
                    criteria.Add(Restrictions.Eq("Id", entity.Id));

                if (entity.Nome != null)
                    criteria.Add(Restrictions.InsensitiveLike("Nome", "%" + entity.Nome + "%"));

                return criteria.List<Editora>().ToList();
            }
        }

        public override void LazyProperties(Editora entity)
        {

        }
    }
}

