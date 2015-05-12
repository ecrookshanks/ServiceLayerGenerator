using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using {{repositoryNameSpace}};
using {{modelNameSpace}};


namespace {{serviceNameSpace}}
{
    public partial class {{entityName}}Service : I{{entityName}}Service
    {
        public I{{entityName}}Repository Repo { get; set; }


        public {{entityName}}Service(I{{entityName}}Repository repo)
        {
            this.Repo = repo;
        }


        #region I{{entityName}}Service Members
		//
		// Simple pass-through to the repository.  Additional business
		// logic can be added here or in the other partial class file.
		//

        public List<{{entityName}}> GetAll{{entityName}}s()
        {
            return Repo.GetAllItems();
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
        }

        public bool Delete{{entityName}}({{entityName}} entity)
        {
            Repo.DeleteItem(entity);
        }

        public bool Delete{{entityName}}(int id)
        {
            Repo.DeleteItem(id);
        }

        #endregion
    }
}
