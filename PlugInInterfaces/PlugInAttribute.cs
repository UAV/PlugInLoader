using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BoTechnologies.PlugIn
{
    public class LoadAssemblyAttributesProxy : MarshalByRefObject
    {
        public LoadAssemblyAttributesProxy ()
	    {

	    }
        public PlugInAttribute[] LoadAssemblyAttributes(string assFile)
        {
            Assembly asm = Assembly.LoadFrom(assFile);
            PlugInAttribute[] plugInAttribute = asm.GetCustomAttributes(typeof(PlugInAttribute), false) as PlugInAttribute[];
            return plugInAttribute;
        }
    }

    [Serializable]
    [AttributeUsageAttribute(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    sealed public class PlugInAttribute : Attribute
    {
        public string PlugInName { get; private set; }
        public string EntryType { get; private set; }
        
        public PlugInAttribute(string pluginName, string entryType)
        {
            PlugInName = pluginName;
            EntryType = entryType;
        }
    }
}
