using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace NokelServices.DALGenLib.Configuration
{
    /// <summary>
    /// Handles the custom configuration section for the DALSettings
    /// library.  
    /// </summary>
    ///
    public class DALSettingsSection : ConfigurationSection
    {
        public static readonly DALSettingsSection Current =
            (DALSettingsSection)ConfigurationManager.GetSection("dalSettings");

        
        #region NameSpace settings
        
        [ConfigurationProperty("ModelNameSpace", DefaultValue="Com.ReverseUrl.Models")]
        public String ModelNameSpace
        {
            get { return base["ModelNameSpace"].ToString(); }
            set { base["ModelNameSpace"] = value; }
        }

        [ConfigurationProperty("RepositoryNameSpace", DefaultValue = "Com.ReverseUrl.Repositories")]
        public String RepositoryNameSpace
        {
            get { return base["RepositoryNameSpace"].ToString(); }
            set { base["RepositoryNameSpace"] = value; }
        }

        [ConfigurationProperty("ServiceNameSpace", DefaultValue = "Com.ReverseUrl.Services")] 
        public String ServiceNameSpace
        {
            get { return base["ServiceNameSpace"].ToString(); }
            set { base["ServiceNameSpace"] = value; }
        }

        [ConfigurationProperty("ContextName", DefaultValue = "DBContext")]
        public String ContextName
        {
            get { return base["ContextName"].ToString(); }
            set { base["ContextName"] = value; }
        }
        #endregion

        #region File Locations and options

        [ConfigurationProperty("BaseTemplateFolder", DefaultValue = @"C:\templates\")]
        public String BaseTemplateFolder
        {
            get { return base["BaseTemplateFolder"].ToString(); }
            set { base["BaseTemplateFolder"] = value; }
        }

        [ConfigurationProperty("ServiceTemplateFolder", DefaultValue="Services")]
        public String ServiceTemplateFolder
        {
            get { return base["ServiceTemplateFolder"].ToString(); }
            set { base["ServiceTemplateFolder"] = value; }
        }

        [ConfigurationProperty("RepositoryTemplateFolder", DefaultValue="Repositories")]
        public String RepositoryTemplateFolder
        {
            get { return base["RepositoryTemplateFolder"].ToString(); }
            set { base["RepositoryTemplateFolder"] = value; }
        }

        [ConfigurationProperty("ServiceExtensionTemplateFolder", DefaultValue="extensions")]
        public String ServiceExtensionTemplateFolder
        {
            get { return base["ServiceExtensionTemplateFolder"].ToString(); }
            set { base["ServiceExtensionTemplateFolder"] = value; }
        }

        [ConfigurationProperty("BaseOutputFolder", DefaultValue = @"C:\templates\output")]
        public String BaseOutputFolder
        {
            get { return base["BaseOutputFolder"].ToString(); }
            set { base["BaseOutputFolder"] = value; }
        }

        [ConfigurationProperty("AddServiceExtensions", DefaultValue=true)]
        public Boolean AddServiceExtensions
        {
            get { return (Boolean)base["AddServiceExtensions"]; }
            set { base["AddServiceExtensions"] = value; }
        }

        [ConfigurationProperty("PluralizeCollections", DefaultValue = true)]
        public Boolean PluralizeCollections
        {
            get { return (Boolean)base["PluralizeCollections"]; }
            set { base["PluralizeCollections"] = value; }
        }

        [ConfigurationProperty("CreateOutputFolders", DefaultValue=false)]
        public Boolean CreateOutputFolders
        {
            get { return (Boolean)base["CreateOutputFolders"]; }
            set { base["CreateOutputFolders"] = value; }
        }

        #endregion

        #region Template File Names

        [ConfigurationProperty("ServiceTemplateInterfaceFile", DefaultValue = "ITemplateService")]
        public String ServiceTemplateInterfaceFile
        {
            get { return base["ServiceTemplateInterfaceFile"].ToString(); }
            set { base["ServiceTemplateInterfaceFile"] = value; }
        }

        [ConfigurationProperty("ServiceTemplateClassFile", DefaultValue = "TemplateService")]
        public String ServiceTemplateClassFile
        {
            get { return base["ServiceTemplateClassFile"].ToString(); }
            set { base["ServiceTemplateClassFile"] = value; }
        }

        [ConfigurationProperty("RepositoryTemplateInterfaceFile", DefaultValue = "IRepository")]
        public String RepositoryTemplateInterfaceFile
        {
            get { return base["RepositoryTemplateInterfaceFile"].ToString(); }
            set { base["RepositoryTemplateInterfaceFile"] = value; }
        }

        [ConfigurationProperty("RepositoryTemplateClassFile", DefaultValue = "TemplateRepository")]
        public String RepositoryTemplateClassFile
        {
            get { return base["RepositoryTemplateClassFile"].ToString(); }
            set { base["RepositoryTemplateClassFile"] = value; }
        }

        #endregion

        #region Database settings

        [ConfigurationProperty("ConnectionString", DefaultValue = "ConnectionStringRequired")]
        public String ConnectionString
        {
            get { return base["ConnectionString"].ToString(); }
            set { base["ConnectionString"] = value; }
        }

        [ConfigurationProperty("MakeObjectsPlural", DefaultValue = false)]
        public Boolean MakeObjectsPlural
        {
            get { return (Boolean)base["MakeObjectsPlural"]; }
            set { base["MakeObjectsPlural"] = value; }
        }
        #endregion
    }
}
