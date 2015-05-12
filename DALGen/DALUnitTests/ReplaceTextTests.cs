using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

using NokelServices.DALGenLib.Configuration;
using NokelServices.DALGenLib;

namespace DALUnitTests
{
    [TestClass]
    public class ReplaceTextTests
    {
        StringBuilder sbContext = new StringBuilder();
        StringBuilder sbEntity = new StringBuilder();
        StringBuilder sbNameSpace = new StringBuilder();
        StringBuilder sbTime = new StringBuilder();
        String myTestEntity = "TestEntity";
        String pluralEntity = "TestEntities";
        String testNoY = "IntityTest";

        ProcessDAL proc = null;

        [TestInitialize]
        public void InitTestObjects()
        {
            // Init with all defaults
            proc = new ProcessDAL();

            sbTime.Append(String.Format("This is a string with the time tag {0} in it", ProcessDAL.TIME_TAG));

            // create an entity string will all variations of the entity tag
            //
            // {{entityNameRP}} (for removing plurals - entities becomes entitie b/c an s follows in the template)
            // {{entityNameP}}  (for plural collection instances) 
            // {{entityName}}
            sbEntity.Append("This string has " + ProcessDAL.ENTITY_NAME + " in it. ");
            sbEntity.Append("And also proper list as Get" + ProcessDAL.ENTITY_PLURAL_COLS + " entity plurals. ");
            sbEntity.Append("And also a list of " + ProcessDAL.ENTITY_REMOVE_PLURALS + "s that should be properly singular and pluralized in the template string.");

        }

        [TestMethod]
        public void TestTimeReplace()
        {
            String before = sbTime.ToString();
            Assert.IsTrue(before.Contains(ProcessDAL.TIME_TAG));

            proc.InsertGeneratedTime(sbTime);
            String after = sbTime.ToString();

            Assert.IsFalse(after.Contains(ProcessDAL.TIME_TAG));

        }

        [TestMethod]
        public void TestSingularEntityReplace()
        {
            String before = sbEntity.ToString();
            Assert.IsTrue(before.Contains(ProcessDAL.ENTITY_REMOVE_PLURALS), "Before - Contains Remove Plurals test");
            Assert.IsTrue(before.Contains(ProcessDAL.ENTITY_PLURAL_COLS), "Before - Contains Plurals test");
            Assert.IsTrue(before.Contains(ProcessDAL.ENTITY_NAME), "Before - Contains Plain Entity test");

            // Tell the processor that the collections should be made plural
            proc.Settings.Settings.PluralizeCollections = true;

            proc.ReplaceEntityNames(sbEntity, myTestEntity);
            String after = sbEntity.ToString();

            // Test no tags remain.
            Assert.IsFalse(after.Contains(ProcessDAL.ENTITY_REMOVE_PLURALS),"After - Contains Remove Plurals test");
            Assert.IsFalse(after.Contains(ProcessDAL.ENTITY_PLURAL_COLS), "After - Contains Plurals test");
            Assert.IsFalse(after.Contains(ProcessDAL.ENTITY_NAME), "After - Contains Plain Entity test");

            Assert.IsTrue(after.Contains(myTestEntity));
            Assert.IsTrue(after.Contains("Get" + myTestEntity.Substring(0,myTestEntity.Length-1) + "ies"));

        }

        [TestMethod]
        public void TestPluralEntityReplace()
        {
            String before = sbEntity.ToString();
            Assert.IsTrue(before.Contains(ProcessDAL.ENTITY_REMOVE_PLURALS), "Before - Contains Remove Plurals test");
            Assert.IsTrue(before.Contains(ProcessDAL.ENTITY_PLURAL_COLS), "Before - Contains Plurals test");
            Assert.IsTrue(before.Contains(ProcessDAL.ENTITY_NAME), "Before - Contains Plain Entity test");

            // Tell the processor that the collections are already plural
            proc.Settings.Settings.PluralizeCollections = false;

            proc.ReplaceEntityNames(sbEntity, pluralEntity);
            String after = sbEntity.ToString();

            // Test no tags remain.
            Assert.IsFalse(after.Contains(ProcessDAL.ENTITY_REMOVE_PLURALS), "After - Contains Remove Plurals test");
            Assert.IsFalse(after.Contains(ProcessDAL.ENTITY_PLURAL_COLS), "After - Contains Plurals test");
            Assert.IsFalse(after.Contains(ProcessDAL.ENTITY_NAME), "After - Contains Plain Entity test");
            
            // Test items properly replaced
            Assert.IsTrue(after.Contains(pluralEntity), "Plural entity contains test");
            Assert.IsFalse(after.Contains("ss"), "After - double s test");
        }

        [TestMethod]
        public void TestEntityNoY()
        {
            String before = sbEntity.ToString();
            Assert.IsTrue(before.Contains(ProcessDAL.ENTITY_REMOVE_PLURALS), "NoY Before - Contains Remove Plurals test");
            Assert.IsTrue(before.Contains(ProcessDAL.ENTITY_PLURAL_COLS), "NoY Before - Contains Plurals test");
            Assert.IsTrue(before.Contains(ProcessDAL.ENTITY_NAME), "NoY Before - Contains Plain Entity test");

            // Tell the processor that the collections should be made plural
            proc.Settings.Settings.PluralizeCollections = true;

            proc.ReplaceEntityNames(sbEntity, testNoY);
            String after = sbEntity.ToString();

            // Test no tags remain.
            Assert.IsFalse(after.Contains(ProcessDAL.ENTITY_REMOVE_PLURALS), "NoY After - Contains Remove Plurals test");
            Assert.IsFalse(after.Contains(ProcessDAL.ENTITY_PLURAL_COLS), "NoY After - Contains Plurals test");
            Assert.IsFalse(after.Contains(ProcessDAL.ENTITY_NAME), "NoY After - Contains Plain Entity test");
            
            // Test items properly replaced.
            Assert.IsTrue(after.Contains(testNoY), "NoY singular test");
            Assert.IsTrue(after.Contains("Get" + testNoY + "s"), "NoY Get plural test");
            Assert.IsTrue(after.Contains(" " + testNoY + "s"), "NoY standalone plural test");
        }

    }
}
