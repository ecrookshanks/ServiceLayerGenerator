using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using {{modelNameSpace}};
using {{repositoryNameSpace}};

namespace {{serviceNameSpace}}
{
    //
    // Don't need to specify the interface in the partial class.
    //
    public partial class {{entityName}}Service
    {
		//
		// Example additional service methods.
		//
		
        public List<{{entityName}}> GetByExactName(String name)
        {
            List<{{entityName}}> theList = this.Repo.GetAllItems().Where(b => b.Name.Equals(name)).ToList();
            return theList;
        }


        public List<{{entityName}}> GetByContainsInName(String term)
        {
            List<{{entityName}}> theList = this.Repo.GetAllItems().Where(b => b.Name.Contains(term)).ToList();
            return theList;
        }


    }
}
