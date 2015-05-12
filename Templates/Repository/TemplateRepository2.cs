using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using {{modelNameSpace}};


namespace {{repositoryNameSpace}}
{
	///  {{entityName}}Repository class.
	///
	///  Repository class for processing all requests dealing with
	///  {{entityName}} objects.  
	///
    public class {{entityName}}Repository : I{{entityName}}Repository
    {
		// EF context name
        private {{contextName}} _context = null;

		// No-arg constructor
        public {{entityName}}Repository()
        {
            _context = new {{contextName}}();
        }

		// Constructor that takes a context
        public {{entityName}}Repository({{contextName}} ctx)
        {
            _context = ctx;
        }


        #region IRepository<{{entityName}}> Members

        public List<{{entityName}}>  GetAllItems()
        {
            var list = _context.{{entityName}}.ToList();
            return list;
        }

        public {{entityName}} GetItem(int id)
        {
            {{entityName}} tb = _context.{{entityName}}.Find(id);
            return tb;
        }

        public void UpdateItem({{entityName}} item)
        {
            _context.Entry<{{entityName}}>(item).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void CreateItem({{entityName}} item)
        {
            _context.{{entityName}}.Add(item);
            _context.SaveChanges();
        }

        public void DeleteItem({{entityName}} item)
        {
            _context.{{entityName}}.Remove(item);
            _context.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            {{entityName}} tb = _context.{{entityName}}.First(t => t.{{entityName}}Id == id);
            DeleteItem(tb);
        }

        #endregion
    }
}
