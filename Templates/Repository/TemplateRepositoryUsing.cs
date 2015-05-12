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
		private String _connString = null;

		// Expose the context as read-only
		public {{contextName}} Context 
		{ 
			get 
			{ 
				InitContext();
				return _context; 
			} 
		}
		
		// No-arg constructor.  Assumes the connection string name is the same as the context.
        public {{entityName}}Repository()
        {
			// ConnectionString from the database support class
			this._connString = DatabaseSupport.GetConnectionString();
            //_context = new {{contextName}}(connString);
        }

		// Constructor that takes a context
        public {{entityName}}Repository({{contextName}} ctx)
        {
            _context = ctx;
        }

		private void InitContext()
		{
			if(_context == null)
			{
				_context = new {{contextName}}(_connString);
			}
		}

        #region IRepository<{{entityName}}> Members

        public List<{{entityName}}>  GetAllItems()
        {
			List<{{entityName}}> theList = null;
			using( {{contextName}} ctx = new {{contextName}}(_connString))
			{
				theList = ctx.{{entityNameP}}.ToList();
			}
            return theList;
        }

        public {{entityName}} GetItem(int id)
        {
            {{entityName}} tb = null;
			
			using( {{contextName}} ctx = new {{contextName}}(_connString))
			{
				tb = ctx.{{entityNameP}}.Find(id);
			}
            return tb;
        }

        public void UpdateItem({{entityName}} item)
        {
			using( {{contextName}} ctx = new {{contextName}}(_connString))
			{
				ctx.Entry<{{entityName}}>(item).State = System.Data.Entity.EntityState.Modified;
				ctx.SaveChanges();
			}
        }

        public void CreateItem({{entityName}} item)
        {
            using( {{contextName}} ctx = new {{contextName}}(_connString))
			{
				ctx.{{entityNameP}}.Add(item);
				ctx.SaveChanges();
			}
        }

        public void DeleteItem({{entityName}} item)
        {
            using( {{contextName}} ctx = new {{contextName}}(_connString))
			{
				ctx.Entry<{{entityName}}>(item).State = System.Data.Entity.EntityState.Deleted;
				ctx.SaveChanges();
			}
        }

        public void DeleteItem(int id)
        {
			{{entityName}} tb = null;
            using( {{contextName}} ctx = new {{contextName}}(_connString))
			{
				tb = ctx.{{entityNameP}}.First(t => t.ID == id);
			}
			if ( tb != null )
			{
				DeleteItem(tb);
			}
        }

        #endregion
    }
}
