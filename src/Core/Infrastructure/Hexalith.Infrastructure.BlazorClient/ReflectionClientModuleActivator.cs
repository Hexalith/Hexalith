namespace Hexalith.Infrastructure.BlazorClient
{
    using System;
    using System.Threading.Tasks;

    using Hexalith.Infrastructure.Modules;
    using Hexalith.Infrastructure.Modules.Definitions;
    using Hexalith.Infrastructure.Modules.Exceptions;

    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

    public class ReflectionClientModuleActivator : IModuleActivator
    {
        private readonly string _clientName;
        private readonly IWebAssemblyHostEnvironment _hostEnvironment;
        private readonly string _serverApiName;

        public ReflectionClientModuleActivator(IWebAssemblyHostEnvironment hostEnvironment, string clientName, string serverApiName)
        {
            _hostEnvironment = hostEnvironment;
            _clientName = clientName;
            _serverApiName = serverApiName;
        }

        public Task<IModule?> FindModule(ModuleDefinition definition)
        {
            Type? moduleType = Type.GetType(definition.TypeName, false, false);
            if (moduleType == null)
            {
                return Task.FromResult<IModule?>(null);
            }
            return Task.FromResult((IModule?)(Activator.CreateInstance(moduleType, definition, _hostEnvironment, _clientName, _serverApiName) as IClientModule));
        }

        public Task<IModule> GetRequiredModule(ModuleDefinition definition)
        {
            Type? moduleType = Type.GetType(definition.TypeName, false, false);
            if (moduleType == null)
            {
                return Task.FromException<IModule>(new ModuleNotFoundException(definition, $"Type {definition.TypeName} not found."));
            }
            IModule? module = Activator.CreateInstance(moduleType, definition, _hostEnvironment, _clientName, _serverApiName) as IClientModule;
            if (module == null)
            {
                return Task.FromException<IModule>(new ModuleInstanceNotCreatedException(definition, $"Error while creating instance of {moduleType.FullName}."));
            }
            return Task.FromResult(module);
        }
    }
}