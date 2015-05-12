using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

using {{repositoryNameSpace}};
using {{modelNameSpace}};


namespace {{serviceNameSpace}}
{
	/// {{entityName}}Service class.
	///
	/// Service class for handling all requests dealing with
	/// {{entityName}} objects.  Can be either pass-through to the
	/// {{entityName}}Repository class or can add business logic.
	///
	/// Auto-generated file, any changes will be lost on regeneration
	///
	/// Generated: {{generatedTime}}
	///
    public partial class {{entityName}}Service : I{{entityName}}Service
    {
		// Keep a reference to the associated repository.
        private {{entityName}}Repository Repo { get; set; }

		// Expose the underlying entity table as a read-only property.  
		// This allows for more efficient query writing.
		public DbSet<{{entityName}}> {{entityName}}  // can also be generic "EntityTable" 
		{
			get
			{
				return this.Repo.Context.{{entityName}};
			}
		}
		
		// Constructor accepting Repository interface (for ctor injection)
        public {{entityName}}Service(IRepository<{{entityName}}> repo)
        {
            this.Repo = repo as {{entityName}}Repository;
        }

		// Constructor taking a context.  Often used when other services have
		// to use this service and must share a context for LINQ queries.
		public {{entityName}}Service({{contextName}} ctx)
        {
            this.Repo = new {{entityName}}Repository(ctx);
        }
		
		// Default Constructor
		public {{entityName}}Service()
        {
            this.Repo = new {{entityName}}Repository();
        }

        #region I{{entityName}}Service Members
		//
		// Simple pass-throughs to the repository.  Additional business
		// logic should be added in the corresponding partial class file.
		//

        public List<{{entityName}}> GetAll{{entityNameRP}}s()
        {
            return Repo.GetAllItems();
        }

        //
        // Get all items eagerly
        public List<{{entityName}}> GetAll{{entityNameRP}}sEager()
        {
            List<{{entityName}}>> fullList = null;
            Repo.Context.Configuration.LazyLoadingEnabled=false;
            fullList = this.{{entityName}}.ToList();
            Repo.Context.Configuration.LazyLoadingEnabled=true;

            return fullList;
        }

        public {{entityName}} Get{{entityName}}(int id)
        {
            return Repo.GetItem(id);
        }

        public void Create{{entityName}}({{entityName}} entity)
        {
            Repo.CreateItem(entity);
        }

        public bool Update{{entityName}}({{entityName}} entity)
        {
            Repo.UpdateItem(entity);
			return true;
        }

        public bool Delete{{entityName}}({{entityName}} entity)
        {
            Repo.DeleteItem(entity);
			return true;
        }

        public bool Delete{{entityName}}(int id)
        {
            Repo.DeleteItem(id);
			return true;
        }

        #endregion
    }
}
