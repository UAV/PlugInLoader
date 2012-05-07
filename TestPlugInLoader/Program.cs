using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoTechnologies.PlugIn;

namespace TestPlugInLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            PlugInLoader loader = new PlugInLoader();
            loader.PrintLoadedAssemblies("***Loaded assemblies.");
            Console.WriteLine("$$$Loading PlugIns...");
            loader.LoadAllPlugIns();
            Console.WriteLine("$$$Loaded PlugIns.");
            loader.PrintLoadedAssemblies("***Loaded assemblies");

            Console.WriteLine("Enter to continue");
            Console.ReadLine();

            List<IPlugIn> plugIns = loader.PlugIns;
            plugIns.ForEach(plugin => plugin.PrintLoadedAssemblies("Loadded Assemblies "));
            Console.WriteLine("$$$Calling PlugIn methods.");
            plugIns.ForEach(plugin => Console.WriteLine("The name is: " + plugin.Name));
            plugIns.ForEach(plugin => plugin.Initialize());
            plugIns.ForEach(plugin => plugin.Execute());

            Console.WriteLine("Enter to continue");
            Console.ReadLine();

            Console.WriteLine("$$$Unloading PlugIns...");
            loader.UnloadAllPlugins();
            Console.WriteLine("$$$PlugIns unloaded.");

            loader.PrintLoadedAssemblies("***Loaded assemblies");

            Console.WriteLine("!!!Calling PlugIn methods after have been unloaded.");
            foreach (var plugin in plugIns)
            {
                try
                {
                    Console.WriteLine("!!!The name is: " + plugin.Name);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
                try
                {
                    plugin.Initialize();
                }
                catch (Exception ex )
                {

                    Console.WriteLine(ex.Message);
                }
                try
                {
                    plugin.Execute();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
          
            Console.WriteLine("Enter to continue");
            Console.ReadLine();


        }
    }
}
