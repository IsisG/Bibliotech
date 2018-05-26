using Bibliotech.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Bibliotech.Base
{
    public class NHibernateHelper
    {
        public static ISession OpenSession()
        {
            string conexao = @"Data Source=bibliotech.database.windows.net,1433;Initial Catalog=Bibliotech;User ID=andre-eliaas;Password=B1BL10tech@.2018;Integrated Security=False;Min Pool Size=5;Max Pool Size=250; Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //string conexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Bibliotech;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                  .ConnectionString(conexao)
                              .ShowSql()
                )
               .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<Usuario>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                                                .Create(false, false))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}