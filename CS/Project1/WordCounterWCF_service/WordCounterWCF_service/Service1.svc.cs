using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WordCounterWCF_service
{
    public class Service1 : IService1
    {
        public Dictionary<string, int> GetData(string[] linesOfFile)
        {
            Type type;
            Type[] types;
            object sorterParallel = null;
            MethodInfo methodParallel = null;
            Dictionary<string, int> dictionary;
            try
            {
                types = Assembly.LoadWithPartialName("LibrarySorter").GetTypes();
                type = types.FirstOrDefault(x => x.Name == "CSorter");
                MethodInfo[] myArrayMethodInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                methodParallel = type.GetMethod(myArrayMethodInfo[0].Name, BindingFlags.Public | BindingFlags.Instance);
                sorterParallel = Activator.CreateInstance(type);
                if (types == null | type == null | myArrayMethodInfo == null | methodParallel == null | sorterParallel == null)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

            dictionary = (Dictionary<string, int>)methodParallel.Invoke(sorterParallel, new object[] { linesOfFile });

            return dictionary;
        }
    }
}
