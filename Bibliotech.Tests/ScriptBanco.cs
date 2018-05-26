using Bibliotech.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Tool.hbm2ddl;

namespace Bibliotech.Test
{
    [TestClass]
    public class ScriptBanco
    {
        [TestMethod]
        public void CriarScriptsBanco()
        {
            string conectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Bibliotech;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            FluentConfiguration configuration = Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2008.ConnectionString(conectionString))
            .ExposeConfiguration(cfg => new SchemaExport(cfg).SetOutputFile("ScriptBanco.sql").Create(true, false))
            .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Usuario>());

            configuration.BuildSessionFactory();
        }
    }
}
