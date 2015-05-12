using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace {{repositoryNameSpace}}
{
    // Interface for all the repository classes.  All implementing
	// classes with Implement IRespository<entity> to provide a standard
	// repository class.
    //
    // Basically the CRUD operations, and exposing the Context
	//
	// This file was generated with the Service-Repository generator
	// tool.  Any modifications will be overwritten if the file is
	// regenerated.  
	// 
	// Generated: {{generatedTime}}
    //
    public interface IRepository<T>
    {
		// Expose the context from the repository as read-only
		{{contextName}} Context { get; }
		
		// Get a generic list of all items in this repository
        List<T> GetAllItems();

		// Get a single item by ID
        T GetItem(int id);

		// Update a passed-in item
        void UpdateItem(T item);
		
		// Add the passed-in item to the repository
        void CreateItem(T item);

		// Remove the passed in item from repository
        void DeleteItem(T item);

		// Remove the item from the repository given the ID
        void DeleteItem(int id);

    }
}
