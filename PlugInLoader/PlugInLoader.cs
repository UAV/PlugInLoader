using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace BoTechnologies.PlugIn
{
   
    public class PlugInLoader
    {
        private Dictionary<PlugInAttribute, AppDomain> _appDomains = new Dictionary<PlugInAttribute, AppDomain>();
        private Dictionary<PlugInAttribute, IPlugIn> _plugIns = new Dictionary<PlugInAttribute, IPlugIn>();
        private List<PlugInAttribute> _plugInsInfo = new List<PlugInAttribute>();
        List<string> _assemblyNames = new List<string>();

        public List<IPlugIn> PlugIns { get { return _plugIns.Values.ToList();} }

        public PlugInLoader()
        {
        }

        public void LoadAllPlugIns()
        {
            GetAssemblyNames();
            _plugInsInfo.ForEach(CreateAppDomain);
            _plugInsInfo.ForEach(InstantiatePlugIn);
        }

        public void UnloadAllPlugins()
        {
            _plugInsInfo.ForEach(UnLoadAppDomain);
            
        }

        private void GetAssemblyNames()
        {
            string[] fileNames = Directory.GetFiles(Environment.CurrentDirectory, "*.dll", SearchOption.AllDirectories);
            {                
                AppDomainSetup domainSetup = new AppDomainSetup();
                domainSetup.ApplicationName = "TempDomain";
                
                var tempAppDomain = AppDomain.CreateDomain("TempDomain", AppDomain.CurrentDomain.Evidence, domainSetup);

                foreach (var file in fileNames)
                {
                    LoadAssemblyAttributesProxy proxy = tempAppDomain.CreateInstanceAndUnwrap("PlugInInterfaces", typeof(LoadAssemblyAttributesProxy).FullName) as LoadAssemblyAttributesProxy;
                    var plugInAttribute = proxy.LoadAssemblyAttributes(file);
                    if (plugInAttribute.Length > 0)
                    {
                        _plugInsInfo.Add(plugInAttribute[0]);
                    }
                }
                AppDomain.Unload(tempAppDomain);
            }
        }


        private void CreateAppDomain(PlugInAttribute plugInInfo)
        {
            AppDomainSetup domainSetup = new AppDomainSetup();
            domainSetup.ApplicationName = plugInInfo.PlugInName;
            domainSetup.ConfigurationFile = plugInInfo.PlugInName + ".dll.config";
            domainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            var appDomain = AppDomain.CreateDomain(domainSetup.ApplicationName, AppDomain.CurrentDomain.Evidence, domainSetup);

            _appDomains.Add(plugInInfo, appDomain);
        }

       
        private void InstantiatePlugIn(PlugInAttribute plugInInfo)
        {
            var appDomain = _appDomains[plugInInfo];
            IPlugIn plugIn = appDomain.CreateInstanceAndUnwrap(plugInInfo.PlugInName, plugInInfo.EntryType) as IPlugIn;
            _plugIns.Add(plugInInfo, plugIn);
        }

        private void UnLoadAppDomain(PlugInAttribute plugInInfo)
        {
            AppDomain.Unload(_appDomains[plugInInfo]);
            _appDomains.Remove(plugInInfo);
        }

        public void PrintLoadedAssemblies(string message)
        {
            Console.WriteLine("-----------------------------------------------------");
            Console.Write(message);
            List<Assembly> assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
            Console.WriteLine(" for Current AppDomain: ");
            Console.WriteLine("-----------------------------------------------------");
            foreach (var assembly in assemblies)
            {
                Console.WriteLine(assembly.FullName);
            }

            Console.WriteLine("-----------------------------------------------------");

           
        }
    }
}
