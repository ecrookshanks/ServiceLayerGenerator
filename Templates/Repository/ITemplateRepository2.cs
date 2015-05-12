using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace {{repositoryNameSpace}}
{
    // Interface for all the repository classes
    //
    // Basically the CRUD operations
    //
    public interface I{{entityName}}Repository
    {
        List<{{entityName}}> GetAllItems();

        {{entityName}} GetItem(int id);

        void UpdateItem({{entityName}} item);

        void CreateItem({{entityName}} item);

        void DeleteItem({{entityName}} item);

        void DeleteItem(int id);

    }
}
