using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Entity;

using {{modelNameSpace}};


namespace {{serviceNameSpace}}
{
	/// I{{entityName}}Service class.
	///
	/// Service interface for handling all requests dealing with
	/// {{entityName}} objects.  Defined as an interface to allow
	/// for extensible testing via DI.
	///
	/// Auto-generated file, any changes will be lost on regeneration
	///
	/// Generated: {{generatedTime}}
	///
    public partial interface I{{entityName}}Service
    {
		// Expose the underlying DbSet.  Essentially a shortcut
		// for the GetAll() method, but doesn't trigger a query
		// until ToList() is called - more efficient in dynamic
		// LINQ query situations.
		DbSet<{{entityName}}> {{entityName}} { get; }
		
		// Standard GetAll() method - returns all objects
        List<{{entityName}}> GetAll{{entityNameRP}}s();

        // Standard GetAllxxxEager() method - returns all objects
        // and first-level related
        List<{{entityName}}> GetAll{{entityNameRP}}sEager();

		// Look up an entity given its ID
        {{entityName}} Get{{entityName}}(int id);

        // Look up an entity eagerly given its ID
        {{entityName}} Get{{entityName}}Eager(int id);

		// Add a new entity to the underlying collection
        void Create{{entityName}}({{entityName}} tb);
		
		// Update a given entity
        bool Update{{entityName}}({{entityName}} tb);

		// Remove an entity
        bool Delete{{entityName}}({{entityName}} tb);

		// Remove an entity
        bool Delete{{entityName}}(int id);

    }
}
