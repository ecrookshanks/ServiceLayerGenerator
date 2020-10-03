using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NokelServices.DALGenLib.Configuration;

using System.Configuration;

namespace DALUnitTests
{
    [TestClass]
    public class SettingsClassTests
    {
        DALSettingsSection _dssDefault;
        DALSettingsSection _dssConfigured;

        DALSettings _settingsDefault;
        DALSettings _settingsConfigured;

        [TestInitialize]
        public void SetupObjects()
        {
            // Read the settings from the config file.
            // Assume "App" file exists in the output folder
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            _dssConfigured = (DALSettingsSection)config.Sections["dalSettings"];
            _settingsConfigured = new DALSettings(_dssConfigured);

            _dssDefault = new DALSettingsSection();
            _settingsDefault = new DALSettings(_dssDefault);

        }

        [TestMethod]
        public void TestSettingsCreation()
        {
            Assert.IsNotNull(_settingsDefault);
            Assert.IsNotNull(_settingsDefault.Settings);

            Assert.IsNotNull(_settingsConfigured);
            Assert.IsNotNull(_settingsConfigured.Settings);
        }

        [TestMethod]
        public void TestFullTemplatePaths()
        {
            String fullDefaultServicePath = _settingsDefault.GetServiceTemplatePath();
            String fullDefaultRepoPath = _settingsDefault.GetRepositoryTemplatePath();
            String fullDefaultSvcExtPath = _settingsDefault.GetServiceExtensionPath();

            Assert.AreEqual(@"C:\templates\Services\", fullDefaultServicePath);
            Assert.AreEqual(@"C:\templates\Repositories\", fullDefaultRepoPath);
            Assert.AreEqual(@"C:\templates\Services\extensions\", fullDefaultSvcExtPath);
        }

        [TestMethod]
        public void TestFullPathsConf()
        {
            String fullServicePathConf = _settingsConfigured.GetServiceTemplatePath();
            String fullRepoPathConf = _settingsConfigured.GetRepositoryTemplatePath();
            String fullExtPathConf = _settingsConfigured.GetServiceExtensionPath();

            Assert.AreEqual(@"c:\projects\test\DAL\templates\Services\", fullServicePathConf);
            Assert.AreEqual(@"c:\projects\test\DAL\templates\Repository\", fullRepoPathConf);
            Assert.AreEqual(@"c:\projects\test\DAL\templates\Services\svcExtensions\", fullExtPathConf);
        }

        [TestMethod]
        public void TestFullTemplateFileNames()
        {
            String fullSvcTemplateFile = _settingsDefault.GetServiceInterfaceTemplateFQFileName();
            String fullSvcClassFile = _settingsDefault.GetServiceClassTemplateFQFileName();
            String fullrepoTemplateFile = _settingsDefault.GetRepoInterfaceTemplateFQFileName();
            String fullRepoClassFile = _settingsDefault.GetRepoClassTemplateFQFileName();
            

            Assert.AreEqual(@"C:\templates\Services\ITemplateService.cs", fullSvcTemplateFile);
            Assert.AreEqual(@"C:\templates\Services\TemplateService.cs", fullSvcClassFile);
            Assert.AreEqual(@"C:\templates\Repositories\IRepository.cs", fullrepoTemplateFile);
            Assert.AreEqual(@"C:\templates\Repositories\TemplateRepository.cs", fullRepoClassFile);

        }
    }
}
