using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Interceptors.Autofac
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {

            // class attribute ile aspect olayı gerçekleşecek olan attributler
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();

            try
            {

                // metot attribute ile aspect olayı gerçekleşecek olan attributler
                var methodAttributes = type.GetMethod(method.Name).GetCustomAttributes<MethodInterceptionBaseAttribute>(true);                     
                // metod olanları da classdakilere ekleyelim (beraber çalıştırmak için )
                                                                                                                                                   // örneğin securty class ile verilmiştir ve o classdaki tüm metodlarda doğrulama vardır ama validate ise tek add metoduna metot Attribute ile eklenmişse ikisini de çalıştırmam gerekli.
                classAttributes.AddRange(methodAttributes);

            }
            catch (AmbiguousMatchException)
            {
            }


            // Priority int olarak değer alır ve metotda birden fazla aspect varsa hangisinin önce çalışacağını belirtmek için.
            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}
