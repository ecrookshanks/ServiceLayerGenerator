using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using log4net;
using NokelServices.DALGenLib.Configuration;
using System.Threading;

namespace NokelServices.DALGenLib
{
    public class ProcessDAL
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ProcessDAL));

        public static readonly String TIME_TAG = "{{generatedTime}}";
        public static readonly String CONTEXT_TAG = "{{contextName}}";
        public static readonly String ENTITY_REMOVE_PLURALS = "{{entityNameRP}}";
        public static readonly String ENTITY_PLURAL_COLS = "{{entityNameP}}";
        public static readonly String ENTITY_NAME = "{{entityName}}";

        public DALSettings Settings { get; set; }

        // Holds the contents of each of the template files
        String RepoClassTemplate { get; set; }
        String RepoInterfaceTemplate { get; set; }
        String ServiceClassTemplate { get; set; }
        String ServiceInterfaceTemplate { get; set; }
        String ServiceExtClassTemplate { get; set; }

        public ProcessDAL()
        {
            // Create a Settings object with all Defaults
            DALSettingsSection dss = new DALSettingsSection();
            Settings = new DALSettings(dss);
        }

        public ProcessDAL(DALSettings settings)
        {
            this.Settings = settings;
        }

    public Boolean GenerateAllFiles(Boolean isTest)
    {
      if (isTest)
      {
        Thread.Sleep(8000);
        return true;
      }
      else
      {
        return this.GenerateAllFiles();
      }
    }

        public Boolean GenerateAllFiles()
        {
            // TODO: Create Custom SettingsExceptions class and throw?
            if (Settings == null)
            {
                _log.Error("The settings object is Empty.  Cannot proceed without settings.");
                return false;
            }

            if (LoadAllTemplates())
            {
                // output directory check
                if (!OutputFoldersExist())
                {
                    _log.Error("Output location(s) not present! Exiting.");
                    return false;
                }

                // Query the database to get list of tables
                List<String> tableList = GetTableNames();

                // for each table, generate the following files:
                // ITableSerive, TableService, extension\TableService, TableRepository 

                foreach (var table in tableList)
                {
                    GenerateRepositoryClass(table);
                    GenerateServiceInterface(table);
                    GenerateServiceClass(table);
                    // GenerateRepositoryInterface(table);
                }

                // TODO: code option for generating individual Repository 
                // interfaces as well? If so include in loop above, otherwise
                // the below call only generates one file.
                GenerateRepositoryInterface();
            }
            else
            {
                _log.Error("Problem loading Template files.  Exiting.");
                return false;
            }
            return true;
        }

        private bool OutputFoldersExist()
        {
            // Base folder
            String baseOutput = Settings.Settings.BaseOutputFolder;

            String svcOutput = Settings.Settings.ServiceTemplateFolder;
            String repoOutput = Settings.Settings.RepositoryTemplateFolder;
            String svcExt = Settings.Settings.ServiceExtensionTemplateFolder;

            Boolean bCreate = Settings.Settings.CreateOutputFolders;

            VerifyTrailingSlash(ref baseOutput);
            VerifyTrailingSlash(ref svcOutput);
            VerifyTrailingSlash(ref repoOutput);
            VerifyTrailingSlash(ref svcExt);

            try
            {
                if (!Directory.Exists(baseOutput))
                {
                    if(bCreate)
                    {
                        _log.Info("Base Output Folder does not exist.  Attempting to create...");
                        Directory.CreateDirectory(baseOutput);
                        _log.Info("Created " + baseOutput);
                    }
                    else
                    {
                        _log.Info("Base Output Folder does not exist.  Option for creating set to false.");
                        return false;
                    }
                }

                if (!Directory.Exists(baseOutput + svcOutput))
                {
                    if (bCreate)
                    {
                        _log.Info("Service Output Folder does not exist.  Attempting to create...");
                        Directory.CreateDirectory(baseOutput + svcOutput);
                        _log.Info("Created " + baseOutput + svcOutput);
                    }
                    else
                    {
                        _log.Info("Service Output Folder does not exist.  Option for creating set to false.");
                        return false;
                    }
                }

                if (!Directory.Exists(baseOutput + repoOutput))
                {
                    if (bCreate)
                    {
                        _log.Info("Repository Output Folder does not exist.  Attempting to create...");
                        Directory.CreateDirectory(baseOutput + repoOutput);
                        _log.Info("Created " + baseOutput + repoOutput);
                    }
                    else
                    {
                        _log.Info("Repositry Output Folder does not exist.  Option for creating set to false.");
                        return false;
                    }
                }

                Boolean bExt = Settings.Settings.AddServiceExtensions;
                if (bExt && !Directory.Exists(baseOutput + svcOutput + svcExt))
                {
                    if (bCreate)
                    {
                        _log.Info("Service Extension Output Folder does not exist.  Attempting to create...");
                        Directory.CreateDirectory(baseOutput + svcOutput + svcExt);
                        _log.Info("Created " + baseOutput + svcOutput + svcExt);
                    }
                    else
                    {
                        _log.Info("Service Extention Output Folder does not exist.  Option for creating set to false.");
                        return false;
                    }
                }

            }
            catch (IOException ioex)
            {
                _log.Error("IO Exception: " + ioex.Message);
                return false;
            }
            catch (UnauthorizedAccessException oa)
            {
                _log.Error("Unauthorized Exception: " + oa.Message);
                return false;
            }
            catch (Exception ex)
            {
                _log.Error("General Exception: " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Loads all the template files into local strings.
        /// </summary>
        /// <returns></returns>
        private bool LoadAllTemplates()
        {
            String repoClassTemplateFile = Settings.GetRepoClassTemplateFQFileName();
            String repoIFaceTemplateFile = Settings.GetRepoInterfaceTemplateFQFileName();
            String svcClassTempateFile = Settings.GetServiceClassTemplateFQFileName();
            String svcIFaceTemplateFile = Settings.GetServiceInterfaceTemplateFQFileName();

            String svcExtTemplateFile = Settings.GetServiceClassExtensionFQFileName();
            
            try
            {
                // Read the contents of the template files 
                this.RepoClassTemplate = File.ReadAllText(repoClassTemplateFile);
                this.RepoInterfaceTemplate = File.ReadAllText(repoIFaceTemplateFile);
                this.ServiceClassTemplate = File.ReadAllText(svcClassTempateFile);
                this.ServiceInterfaceTemplate = File.ReadAllText(svcIFaceTemplateFile);

                this.ServiceExtClassTemplate = File.ReadAllText(svcExtTemplateFile);

            }
            catch (FileNotFoundException fnf)
            {
                _log.Error("File Not found. " + fnf.Message);
                return false;
            }
            catch (FileLoadException fe)
            {
                _log.Error("File load error. " + fe.Message);
                return false;
            }
            catch (Exception ex)
            {
                _log.Error("General error while loading template files. " + ex.Message);
                return false;
            }

            return true;
        }



        /// <summary>
        /// Gets the list of tables from the database. These will be used to
        /// create the service and repository interfaces and classes.
        /// </summary>
        /// <returns></returns>
        private List<String> GetTableNames()
        {
            List<String> theList = new List<String>();
            SqlDataReader rs = null;
            try
            {
                String connStr = Settings.Settings.ConnectionString;

                // TODO: Check if special value APP;NAME.  This would mean to 
                // look in the application's connection string propery of NAME
                if (connStr.StartsWith("APP;"))
                {
                    String name = connStr.Substring(4);
                    ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];

                    if (settings != null)
                    {
                        connStr = settings.ConnectionString;
                        _log.Info("Using Application database connection: " + name );
                    }
                    else
                    {
                        _log.Info("Could NOT find connection string named " + name + ". Will try to use " + connStr);
                    }
                }
                
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand command = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME NOT LIKE '__%' ESCAPE '_'", conn);
                    conn.Open();
                    rs = command.ExecuteReader();
                    while (rs.Read())
                    {
                        String tbl = rs["TABLE_NAME"].ToString();
                        theList.Add(tbl);
                    }
                }
            }
            catch (SqlException se)
            {
                _log.Error("Database exception while trying to get table names. " + se.Message);
            }
            finally
            {
                if (rs != null)
                {
                    rs.Close();
                }
            }

            return theList;
        }

        /// <summary>
        /// Generates all the interface files in the IEntityService form
        /// </summary>
        private void GenerateServiceInterface(String tableName)
        {
            _log.Info("Generating Service Interface file for " + tableName);

            try
            {
                StringBuilder sb = new StringBuilder(this.ServiceInterfaceTemplate);
                ReplaceAllText(tableName, sb);

                String baseOutput = Settings.Settings.BaseOutputFolder;
                String svcOutput = Settings.Settings.ServiceTemplateFolder;

                // Make sure path(s) end in trailing slash before appending filename.
                VerifyTrailingSlash(ref svcOutput);
                VerifyTrailingSlash(ref baseOutput);

                // TODO: parameterize the file extension?
                String svcIFaceFileName = "I" + tableName + "Service.cs";
                String svcIFaceFQP = baseOutput + svcOutput + svcIFaceFileName;

                _log.Info("Full Path: " + svcIFaceFQP);

                File.WriteAllText(svcIFaceFQP, sb.ToString());
            }
            catch (Exception ex)
            {
                _log.Error("Error generating " + tableName + " Service Interface: " + ex.Message);
            }
        }

        /// <summary>
        /// Generates all the classes in the EntityService : IEntityService form
        /// </summary>
        private void GenerateServiceClass(String tableName)
        {
            _log.Info("Generating Service class file for " + tableName);

            try
            {
                // Builder with the template contents
                StringBuilder sb = new StringBuilder(this.ServiceClassTemplate);
                ReplaceAllText(tableName, sb);

                // TODO: create the output file and write the contents
                // of the StringBuilder to the file

                String baseOutput = Settings.Settings.BaseOutputFolder;
                String svcOutput = Settings.Settings.ServiceTemplateFolder;

                // Make sure path(s) end in trailing slash before appending filename.
                VerifyTrailingSlash(ref svcOutput);
                VerifyTrailingSlash(ref baseOutput);

                // TODO: parameterize the file extension?
                String svcFileName = tableName + "Service.cs";
                String svcFQP = baseOutput + svcOutput + svcFileName;

                _log.Info("Full Path: " + svcFQP);

                File.WriteAllText(svcFQP, sb.ToString());

                // if selected, generate the extension class too
                if (Settings.Settings.AddServiceExtensions)
                {
                    GenerateServiceClassExtension(tableName, baseOutput + svcOutput);
                }
            }
            catch (Exception e)
            {
                _log.Error("Error generating " + tableName + " Service Class: " + e.Message);
            }
        }

        private void GenerateServiceClassExtension(string tableName, string svcFolder)
        {
            _log.Info("Generating Service extension class file for " + tableName);

            try
            {
                // Builder with the template contents
                StringBuilder sb = new StringBuilder(this.ServiceExtClassTemplate);
                ReplaceAllText(tableName, sb);

                // put the extension files in a folder with the same name as where
                // the template was
                String svcExtOutput = Settings.Settings.ServiceExtensionTemplateFolder;

                // Make sure path(s) end in trailing slash before appending filename.
                VerifyTrailingSlash(ref svcExtOutput);

                // TODO: parameterize the file extension?
                String svcFileName = tableName + "Service.cs";
                String svcFQP = svcFolder + svcExtOutput + svcFileName;

                _log.Info("Full Path: " + svcFQP);

                File.WriteAllText(svcFQP, sb.ToString());
            }
            catch (Exception e)
            {
                _log.Error("Error generating " + tableName + " Service Extension class: " + e.Message);
            }
        }

        

        /// <summary>
        /// Modifies the IRepository Instance and generates new file
        /// </summary>
        private void GenerateRepositoryInterface()
        {
            _log.Info("Generating Repository Interface file.");

            try
            {
                // Builder with the template contents
                StringBuilder sb = new StringBuilder(this.RepoInterfaceTemplate);
                InsertGeneratedTime(sb);
                ReplaceNameSpaces(sb);
                ReplaceContextName(sb);
                

                String baseOutput = Settings.Settings.BaseOutputFolder;
                String repoOutput = Settings.Settings.RepositoryTemplateFolder;

                // Make sure path(s) end in trailing slash before appending filename.
                VerifyTrailingSlash(ref repoOutput);
                VerifyTrailingSlash(ref baseOutput);

                // TODO: parameterize the file extension?
                String repoFileName = "IRepository.cs";
                String repoFQP = baseOutput + repoOutput + repoFileName;

                File.WriteAllText(repoFQP, sb.ToString());
            }
            catch (Exception e)
            {
                _log.Error("Error generating IRepository file: " + e.Message);
            }
        }

        /// <summary>
        /// Generates the EntityRepository : IRepository classes
        /// </summary>
        private void GenerateRepositoryClass(String tableName)
        {
            _log.Info("Generating Repository class file for " + tableName);
            try
            {
                // Builder with the template contents
                StringBuilder sb = new StringBuilder(this.RepoClassTemplate);
                ReplaceAllText(tableName, sb);

                // TODO: create the output file and write the contents
                // of the StringBuilder to the file

                String baseOutput = Settings.Settings.BaseOutputFolder;
                String repoOutput = Settings.Settings.RepositoryTemplateFolder;

                // Make sure path(s) end in trailing slash before appending filename.
                VerifyTrailingSlash(ref repoOutput);
                VerifyTrailingSlash(ref baseOutput);
                
                // TODO: parameterize the file extension?
                String repoFileName = tableName + "Repository.cs";
                String repoFQP = baseOutput + repoOutput + repoFileName;


                _log.Info("Full Path: " + repoFQP);

                File.WriteAllText(repoFQP, sb.ToString());

            }
            catch (Exception ex)
            {
                _log.Error("Error generating " + tableName + " Repository class: " + ex.Message);
            }            
        }

        /// <summary>
        /// Helper method to centralize the string replacement calls
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sb"></param>
        private void ReplaceAllText(String tableName, StringBuilder sb)
        {
            InsertGeneratedTime(sb);
            ReplaceNameSpaces(sb);
            ReplaceEntityNames(sb, tableName);
            ReplaceContextName(sb);
        }

        /// <summary>
        /// Replaces all the generated time tags in the incoming
        /// String and returns a new string.
        /// 
        /// Generated Time reference is:
        /// {{generatedTime}}
        /// </summary>
        /// <param name="incoming"></param>
        /// <returns></returns>
        public void InsertGeneratedTime(StringBuilder incoming)
        {
            String timeNow = String.Format("{0:g}", DateTime.Now);
            incoming.Replace(TIME_TAG, timeNow);
        }


        /// <summary>
        /// Replaces all the namespace instances in the incoming
        /// String and returns a new string.
        /// 
        /// NameSpace instances are:
        /// {{repositoryNameSpace}}
        /// {{modelNameSpace}}
        /// {{serviceNameSpace}}
        /// </summary>
        /// <param name="incoming"></param>
        /// <returns></returns>
        public void ReplaceNameSpaces(StringBuilder incoming)
        {
            String repoNS = Settings.Settings.RepositoryNameSpace;
            String modelNS = Settings.Settings.ModelNameSpace;
            String svcNS = Settings.Settings.ServiceNameSpace;

            incoming.Replace("{{repositoryNameSpace}}", repoNS);
            incoming.Replace("{{modelNameSpace}}", modelNS);
            incoming.Replace("{{serviceNameSpace}}", svcNS);
        }

        /// <summary>
        /// Replaces all the entity instances in the incoming
        /// string and returns a new string.
        /// 
        /// Entity instances are:
        /// {{entityNameRP}} (for removing plurals)
        /// {{entityNameP}}  (for plural collection instances)
        /// {{entityName}}
        /// </summary>
        /// <param name="incoming"></param>
        /// <returns></returns>
        public void ReplaceEntityNames(StringBuilder incoming, String entity)
        {
            // Special identifier = entityNameRP.  if the template has GetAll{{entityName}}s
            // and the entity is plural (i.e. AllCountries) then the method will have double
            // ss on the end.  Use this special tag (RP = Remove Plural) to correct.
            string entityNonPlural = "";

            if (entity.EndsWith("s"))
            {
                entityNonPlural = entity.Remove(entity.Length - 1);
                incoming.Replace(ENTITY_REMOVE_PLURALS, entityNonPlural);
            }
            else if ( entity.EndsWith("y"))
            {
                String copy = entity;
                // Check for "y" on end, replace with "ie"
                if (entity.EndsWith("y"))
                {
                    int n = copy.LastIndexOf('y');
                    copy = copy.Remove(n);
                    copy += "ie";
                }
                incoming.Replace(ENTITY_REMOVE_PLURALS, copy);
            }
            else
            {
                incoming.Replace(ENTITY_REMOVE_PLURALS, entity);
            }


            if (Settings.Settings.PluralizeCollections)
            {
                String copy = entity;
                // Check for "y" on end, replace with "ie"
                if (entity.EndsWith("y"))
                {
                    int n = copy.LastIndexOf('y');
                    copy = copy.Remove(n);
                    copy += "ie";
                }
                incoming.Replace(ENTITY_PLURAL_COLS, (copy + "s"));
            }
            else
            {
                incoming.Replace(ENTITY_PLURAL_COLS, entity);
            }
            incoming.Replace(ENTITY_NAME, entity);
        }

        /// <summary>
        /// Replaces the context name in the incoming string and
        /// returns a new string.
        /// 
        /// Context instances are:
        /// {{contextName}}
        /// </summary>
        /// <param name="incoming"></param>
        /// <returns></returns>
        public void ReplaceContextName(StringBuilder incoming)
        {
            String ctx = Settings.Settings.ContextName;

            incoming.Replace(CONTEXT_TAG, ctx);
        }

        /// <summary>
        /// Simple helper to verify/add trailing slash to passed-in path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private void VerifyTrailingSlash(ref String path)
        {
            if (!path.EndsWith(@"\"))
            {
                path += @"\";
            }
        }

    }    
}
