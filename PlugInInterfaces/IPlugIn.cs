using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BoTechnologies.PlugIn
{
    public interface IPlugIn
    {
        string Name { get; }

        void Initialize();

        void Shutdown();

        void Execute();

        void PrintLoadedAssemblies(string message);
    }

    public abstract class PlugIn : MarshalByRefObject, IPlugIn
    {

        public void PrintLoadedAssemblies(string message)
        {
            Console.WriteLine("-----------------------------------------------------");
            Console.Write(message);
            List<Assembly> assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
            Console.WriteLine(" for " + AppDomain.CurrentDomain.FriendlyName +": AppDomain.");
            Console.WriteLine("-----------------------------------------------------");
            foreach (var assembly in assemblies)
            {
                Console.WriteLine(assembly.FullName);
            }

            Console.WriteLine("-----------------------------------------------------");
        }

        #region IPlugIn Members

        public abstract string Name {get;}

        public abstract void Initialize();

        public abstract void Shutdown();

        public abstract void Execute();

        #endregion
    }
}
