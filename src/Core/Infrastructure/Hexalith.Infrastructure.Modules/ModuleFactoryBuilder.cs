namespace Hexalith.Infrastructure.Modules
{
    using System;
    using System.Collections.Generic;

    using Hexalith.Infrastructure.Modules.Definitions;

    public class ModuleFactoryBuilder
    {
        private readonly List<Func<IModuleDefinitionLoader>> _definitionLoaders = new();
        private readonly List<Func<IModuleActivator>> _moduleActivators = new();

        public ModuleFactoryBuilder AddDefinitionLoader(Func<IModuleDefinitionLoader> loader)
        {
            _definitionLoaders.Add(loader);
            return this;
        }

        public ModuleFactoryBuilder AddModuleActivator(Func<IModuleActivator> activator)
        {
            _moduleActivators.Add(activator);
            return this;
        }

        public IModuleFactory Build()
        {
            return new ModuleFactory(_definitionLoaders, _moduleActivators);
        }
    }
}