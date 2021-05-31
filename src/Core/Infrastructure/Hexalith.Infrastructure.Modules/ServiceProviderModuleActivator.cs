namespace Hexalith.Infrastructure.Modules
{
    using System;
    using System.Threading.Tasks;

    using Hexalith.Infrastructure.Modules.Definitions;
    using Hexalith.Infrastructure.Modules.Exceptions;

    using Microsoft.Extensions.Logging;

    public class ServiceProviderModuleActivator : IModuleActivator
    {
        private readonly ILogger _log;
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderModuleActivator(IServiceProvider serviceProvider, ILogger<IModuleActivator> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _log = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<IModule?> FindModule(ModuleDefinition definition)
        {
            if (!string.IsNullOrWhiteSpace(definition.TypeName))
            {
                var type = Type.GetType(definition.TypeName);
                if (type != null)
                {
                    var service = _serviceProvider.GetService(type);
                    if (service is IModule module)
                    {
                        _log.LogTrace($"Found module {definition.Name}.");
                        return Task.FromResult<IModule?>(module);
                    }
                }
            }
            _log.LogTrace($"Module not found : {definition.Name}.");
            return Task.FromResult<IModule?>(null);
        }

        public Task<IModule> GetRequiredModule(ModuleDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(definition.TypeName))
            {
                return Task.FromException<IModule>(new InvalidModuleDefinitionException(definition, $"{nameof(ModuleDefinition.TypeName)} is mandatory to be loaded by {GetType().Name}."));
            }
            var type = Type.GetType(definition.TypeName);
            if (type == null)
            {
                return Task.FromException<IModule>(new InvalidModuleDefinitionException(definition, $"Type {nameof(ModuleDefinition.TypeName)} not found. Check dependencies on the package containing the type."));
            }
            var service = Task.FromResult(_serviceProvider.GetService(type));
            if (service == null)
            {
                return Task.FromException<IModule>(new InvalidModuleDefinitionException(definition, $"Type {nameof(ModuleDefinition.TypeName)} not found in service provider. Check if the module has been added to the service container."));
            }
            if (service is not IModule module)
            {
                return Task.FromException<IModule>(new InvalidModuleDefinitionException(definition, $"The type {nameof(ModuleDefinition.TypeName)} class does not implement the {nameof(IModule)} interface."));
            }
            return Task.FromResult(module);
        }
    }
}