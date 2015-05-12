using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;


using NokelServices.DALGenLib.Configuration;

using System.Configuration;

namespace DALUnitTests
{
    [TestClass]
    public class ConfigFileTests
    {
        DALSettingsSection dss = null;

        [TestInitialize]
        public void SetupSettings()
        {
            // HACK to be able to load the config section.
            //using (var sw = File.CreateText(".\\App"))
            //{
            //    sw.Close();
            //}
            // Assuming the file "App" exists in the output folder.
            var config = ConfigurationManager.OpenExeConfiguration(".\\App");
            dss = (DALSettingsSection)config.Sections["dalSettings"];

        }


        [TestMethod]
        public void TestConfiguredNamespaces()
        {
            String modelsns = dss.ModelNameSpace;
            String repons = dss.RepositoryNameSpace;
            String servicens = dss.ServiceNameSpace;

            Assert.AreEqual("com.someothernamespace.test", modelsns);
            Assert.AreEqual("com.someOtherRepo.test", repons);
            Assert.AreEqual("com.someOtherService.test", servicens);
        }

        [TestMethod]
        public void TestConfiguredFolderPaths()
        {
            String baseTemplates = dss.BaseTemplateFolder;
            String serviceTemplates = dss.ServiceTemplateFolder;
            String repositoryTemplates = dss.RepositoryTemplateFolder;
            String extFolder = dss.ServiceExtensionTemplateFolder;
            String baseFolderOut = dss.BaseOutputFolder;
            Boolean addExtensions = dss.AddServiceExtensions;

            Assert.AreEqual(@"c:\projects\test\DAL\templates", baseTemplates);
            Assert.AreEqual("Services", serviceTemplates);
            Assert.AreEqual("Repository", repositoryTemplates);
            Assert.AreEqual("svcExtensions", extFolder);
            Assert.AreEqual(@"C:\templates\test\DAL\output", baseFolderOut);
            Assert.IsFalse(addExtensions);

        }

        [TestMethod]
        public void TestConfiguredFileNames()
        {
            String svcTemplateFile = dss.ServiceTemplateInterfaceFile;
            String svcSvcClassFile = dss.ServiceTemplateClassFile;
            String repoIFaceFile = dss.RepositoryTemplateInterfaceFile;
            String repoClassFIle = dss.RepositoryTemplateClassFile;


            Assert.AreEqual("ITemplateServiceTest", svcTemplateFile);
            Assert.AreEqual("TemplateServiceTest", svcSvcClassFile);
            Assert.AreEqual("IRepositoryTest", repoIFaceFile);
            Assert.AreEqual("TemplateRepositoryTest", repoClassFIle);

        }


    }
}
