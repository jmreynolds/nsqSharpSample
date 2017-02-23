using System;
using System.Threading;
using StructureMap;

namespace Core.IoC
{
    public static class ObjectFactory
    {
        private static readonly Lazy<Container> ContainerBuilder =
                    new Lazy<Container>(CreateDefaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container => ContainerBuilder.Value;

        private static Container CreateDefaultContainer()
        {
            return new Container(x => x.Scan(s =>
            {
                s.AssembliesFromApplicationBaseDirectory();
                s.LookForRegistries();
            }));
        }
    }
}