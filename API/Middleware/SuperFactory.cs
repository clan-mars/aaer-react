using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application.Activities;
using Microsoft.AspNetCore.Http;

namespace API.Middleware
{
    public class SuperFactory
    {
        private readonly Assembly assembly;
        private readonly HttpContext context;

        public SuperFactory(HttpContext context, Assembly assembly)
        {
            this.context = context;
            this.assembly = assembly;
        }

        public Task<T> Process<T>(UseCaseRequest<T> request)
        {
            foreach (var x in assembly.GetTypes().Where(t => !t.IsAbstract && t.IsClass))
            {
                if (x.GenericTypeArguments.Contains(request.GetType()))
                {
                    var constructor = x.GetConstructors().First();
                    var parameters = constructor.GetParameters();
                    var objs = new System.Collections.Generic.List<object>();
                    foreach (var p in parameters)
                    {
                        objs.Add(context.RequestServices.GetService(p.ParameterType.BaseType));
                    }

                    var usecase = (UseCase<T>)constructor.Invoke(objs.ToArray());
                    return usecase.Perform(request);
                }
            }

            throw new System.Exception("Not good!");
        }
    }
}