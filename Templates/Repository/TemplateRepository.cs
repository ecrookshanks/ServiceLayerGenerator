using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

using {{modelNameSpace}};


namespace {{repositoryNameSpace}}
{
	/// {{entityName}}Repository class.
	///
	/// Repository class for processing all requests dealing with
	/// {{entityName}} objects.  
	///
	/// Auto-generated file, any changes will be lost on regeneration
	///
	/// Generated: {{generatedTime}}
	///
    public class {{entityName}}Repository : IRepository<{{entityName}}>
    {
		// EF context name
        private {{contextName}} _context = null;

		// Expose the context as read-only
		public {{contextName}} Context 
		{ 
			get { return _context; } 
		}
		
		// No-arg constructor.  Assumes the connection string name is the same as the context.
        public {{entityName}}Repository()
        {
			// ConnectionString from the database support class
			String connString = DatabaseSupport.GetConnectionString();
            _context = new {{contextName}}(connString);
        }

		// Constructor that takes a context
        public {{entityName}}Repository({{contextName}} ctx)
        {
            _context = ctx;
        }


        #region IRepository<{{entityName}}> Members

        public List<{{entityName}}>  GetAllItems()
        {
            var list = _context.{{entityNameP}}.ToList();
            return list;
        }

        public {{entityName}} GetItem(int id)
        {
            {{entityName}} tb = _context.{{entityNameP}}.Find(id);
            return tb;
        }

        public void UpdateItem({{entityName}} item)
        {
            _context.Entry<{{entityName}}>(item).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void CreateItem({{entityName}} item)
        {
            _context.{{entityNameP}}.Add(item);
            _context.SaveChanges();
        }

        public void DeleteItem({{entityName}} item)
        {
            _context.{{entityNameP}}.Remove(item);
            _context.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            {{entityName}} tb = _context.{{entityNameP}}.First(t => t.ID == id);
            DeleteItem(tb);
        }

        #endregion
    }
}
