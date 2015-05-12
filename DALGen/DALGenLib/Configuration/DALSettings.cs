using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NokelServices.DALGenLib.Configuration
{
    /// <summary>
    /// Class to encapsulate the custom config and provide
    /// convenience methods for verification, joining paths, etc.
    /// </summary>
    public class DALSettings
    {
        public DALSettingsSection Settings { get; set; }

        public DALSettings(DALSettingsSection sect)
        {
            this.Settings = sect;
        }

        /// <summary>
        /// Builds the full repository template path
        /// </summary>
        /// <returns></returns>
        public string GetRepositoryTemplatePath()
        {
            String basePath = AddTrailingSlash(Settings.BaseTemplateFolder);
            String repoTemplateFolder = AddTrailingSlash(Settings.RepositoryTemplateFolder);

            return basePath + repoTemplateFolder;
        }

        /// <summary>
        /// Builds and returns the fully qualified name of the repository class template file
        /// </summary>
        /// <returns></returns>
        public string GetRepoClassTemplateFQFileName()
        {
            String baseSvcFolder = AddTrailingSlash(GetRepositoryTemplatePath());
            String repoClassTemplate = AddFileExtension(Settings.RepositoryTemplateClassFile);

            return baseSvcFolder + repoClassTemplate;
        }

        /// <summary>
        /// Builds the fully qualified name of the repository interface template file
        /// </summary>
        /// <returns></returns>
        public string GetRepoInterfaceTemplateFQFileName()
        {
            String baseSvcFolder = AddTrailingSlash(GetRepositoryTemplatePath());
            String repoIFaceTemplate = AddFileExtension(Settings.RepositoryTemplateInterfaceFile);

            return baseSvcFolder + repoIFaceTemplate;
        }

        /// <summary>
        /// Builds the fully qualified Service class template file name
        /// </summary>
        /// <returns></returns>
        public string GetServiceClassTemplateFQFileName()
        {
            String baseSvcFolder = AddTrailingSlash(GetServiceTemplatePath());
            String svcClassFile = AddFileExtension(Settings.ServiceTemplateClassFile);

            return baseSvcFolder + svcClassFile;
        }

        public string GetServiceClassExtensionFQFileName()
        {
            String baseSvc = AddTrailingSlash(GetServiceExtensionPath());
            String svcClassFile = AddFileExtension(Settings.ServiceTemplateClassFile);

            return baseSvc + svcClassFile;
        }

        /// <summary>
        /// Builds the full path for the service extension folder
        /// </summary>
        /// <returns></returns>
        public string GetServiceExtensionPath()
        {
            String svcTemplate = AddTrailingSlash(GetServiceTemplatePath());
            String extFolder = AddTrailingSlash(Settings.ServiceExtensionTemplateFolder);

            return svcTemplate + extFolder;
        }

        /// <summary>
        /// Builds fully qualified file name for the Service Interface Template file
        /// </summary>
        /// <returns></returns>
        public string GetServiceInterfaceTemplateFQFileName()
        {
            String baseSvcFolder = AddTrailingSlash(GetServiceTemplatePath());
            String svcInterfaceFile = AddFileExtension(Settings.ServiceTemplateInterfaceFile);

            return baseSvcFolder + svcInterfaceFile;
        }

        /// <summary>
        /// Get full service template path
        /// </summary>
        /// <returns></returns>
        public string GetServiceTemplatePath()
        {
            String basePath = AddTrailingSlash(Settings.BaseTemplateFolder);
            String svcTemplateFolder = AddTrailingSlash(Settings.ServiceTemplateFolder);

            return basePath + svcTemplateFolder;
        }


        /// <summary>
        /// Helper method to add trailing slash to path/name if not present.
        /// 
        /// TODO: Make generic CompineIntoPath(string[] names) to accept 
        /// arbitrary number of components and properly deal with both
        /// leading and trailing slashes?
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private String AddTrailingSlash(String path)
        {
            if (!path.EndsWith(@"\"))
            {
                path += @"\";
            }

            // removed "empty" path parts - if a previous component 
            // contained a leading slash as was added to an ending slash.
            if (path.Contains(@"\\"))
            {
                path.Replace(@"\\", @"\");
            }

            return path;
        }

        /// <summary>
        /// Helper method to verify/add the file extension (.cs) to the
        /// given filename.
        /// 
        /// TODO: Make this a setting?
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string AddFileExtension(string fileName)
        {
            if (!fileName.EndsWith(".cs"))
            {
                fileName += ".cs";
            }

            return fileName;
        }



        
    }
}
