namespace Hexalith.Module.Abstractions.Tests.Fixture
{
    using System.Threading.Tasks;

    using Hexalith.Infrastructure.Modules;
    using Hexalith.Infrastructure.Modules.Definitions;
    using Hexalith.Infrastructure.Modules.Exceptions;

    using Microsoft.Extensions.Configuration;

    public class FakeModuleActivator1 : IModuleActivator
    {
        private readonly IConfiguration _configuration;

        public FakeModuleActivator1(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<IModule> FindModule(ModuleDefinition definition)
        {
            switch (definition.TypeName)
            {
                case "Fake1":
                    return Task.FromResult<IModule>(new Fake1ServiceModule(_configuration));

                case "Fake2":
                    return Task.FromResult<IModule>(new Fake2ServiceModule(_configuration));

                case "Fake3":
                    return Task.FromResult<IModule>(new Fake3ServiceModule(_configuration));

                case "Fake4":
                    return Task.FromResult<IModule>(new Fake4ServiceModule(_configuration));

                case "Fake5":
                    return Task.FromResult<IModule>(new Fake5ServiceModule(_configuration));

                case "Fake6":
                    return Task.FromResult<IModule>(new Fake6ServiceModule(_configuration));

                default:
                    break;
            }
            return Task.FromResult<IModule>(null);
        }

        public async Task<IModule> GetRequiredModule(ModuleDefinition definition)
        {
            return await FindModule(definition).ConfigureAwait(false) ?? throw new ModuleNotFoundException(definition);
        }
    }

    public class FakeModuleActivator2 : IModuleActivator
    {
        private readonly IConfiguration _configuration;

        public FakeModuleActivator2(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<IModule> FindModule(ModuleDefinition definition)
        {
            switch (definition.TypeName)
            {
                case "Fake1":
                    return Task.FromResult<IModule>(new Fake1ServiceModule(_configuration));

                case "Fake2":
                    return Task.FromResult<IModule>(new Fake2ServiceModule(_configuration));

                case "Fake3":
                    return Task.FromResult<IModule>(new Fake3ServiceModule(_configuration));

                default:
                    break;
            }
            return Task.FromResult<IModule>(null);
        }

        public async Task<IModule> GetRequiredModule(ModuleDefinition definition)
        {
            return await FindModule(definition).ConfigureAwait(false) ?? throw new ModuleNotFoundException(definition);
        }
    }

    public class FakeModuleActivator3 : IModuleActivator
    {
        private readonly IConfiguration _configuration;

        public FakeModuleActivator3(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<IModule> FindModule(ModuleDefinition definition)
        {
            switch (definition.TypeName)
            {
                case "Fake4":
                    return Task.FromResult<IModule>(new Fake4ServiceModule(_configuration));

                case "Fake5":
                    return Task.FromResult<IModule>(new Fake5ServiceModule(_configuration));

                case "Fake6":
                    return Task.FromResult<IModule>(new Fake6ServiceModule(_configuration));

                default:
                    break;
            }
            return Task.FromResult<IModule>(null);
        }

        public async Task<IModule> GetRequiredModule(ModuleDefinition definition)
        {
            return await FindModule(definition).ConfigureAwait(false) ?? throw new ModuleNotFoundException(definition);
        }
    }
}