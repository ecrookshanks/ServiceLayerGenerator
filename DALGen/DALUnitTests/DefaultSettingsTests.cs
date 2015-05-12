using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NokelServices.DALGenLib.Configuration;

namespace DALUnitTests
{
    [TestClass]
    public class DefaultSettingsTests
    {
        DALSettingsSection dss = null;

        [TestInitialize]
        public void SetupSettings()
        {
            dss = new DALSettingsSection();
        }

        [TestMethod]
        public void TestNameSpaceDefaults()
        {
            String modelsns = dss.ModelNameSpace;
            String repons = dss.RepositoryNameSpace;
            String servicens = dss.ServiceNameSpace;

            Assert.AreEqual("Com.ReverseUrl.Models", modelsns);
            Assert.AreEqual("Com.ReverseUrl.Repositories", repons);
            Assert.AreEqual("Com.ReverseUrl.Services", servicens);
        }

        [TestMethod]
        public void TestDatabaseSettings()
        {
            String connStr = dss.ConnectionString;
            Boolean plural = dss.MakeObjectsPlural;

            Assert.AreEqual("ConnectionStringRequired", connStr);
            Assert.IsFalse(plural);
        }

        [TestMethod]
        public void TestDirectoryDefaults()
        {
            String baseFolder = dss.BaseTemplateFolder;
            String serviceFolder = dss.ServiceTemplateFolder;
            String repoFolder = dss.RepositoryTemplateFolder;
            String serviceExtFolder = dss.ServiceExtensionTemplateFolder;
            String baseFolderOut = dss.BaseOutputFolder;
            Boolean addExtensions = dss.AddServiceExtensions;


            Assert.AreEqual(@"C:\templates\", baseFolder);
            Assert.AreEqual("Services", serviceFolder);
            Assert.AreEqual("Repositories", repoFolder);
            Assert.AreEqual("extensions", serviceExtFolder);
            Assert.AreEqual(@"C:\templates\output", baseFolderOut);
            Assert.IsTrue(addExtensions);
        }

        [TestMethod]
        public void TestDefaultFileNames()
        {
            String svcTemplateFile = dss.ServiceTemplateInterfaceFile;
            String svcSvcClassFile = dss.ServiceTemplateClassFile;
            String repoIFaceFile = dss.RepositoryTemplateInterfaceFile;
            String repoClassFIle = dss.RepositoryTemplateClassFile;


            Assert.AreEqual("ITemplateService", svcTemplateFile);
            Assert.AreEqual("TemplateService", svcSvcClassFile);
            Assert.AreEqual("IRepository", repoIFaceFile);
            Assert.AreEqual("TemplateRepository", repoClassFIle);

        }

    }
}
