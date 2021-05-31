namespace Hexalith.WorkItems.Infrastructure.DevOps
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.TeamFoundation.Core.WebApi;

    /// <summary>
    /// Project class. It handles de DevOps project informations.
    /// </summary>
    internal class Project
    {
        private readonly string projectName;
        private TeamProject? project;

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        /// <param name="server">The DevOps server instance.</param>
        /// <param name="projectName">The DevOps project name.</param>
        public Project(DevOpsServer server, string projectName)
        {
            Server = server;
            this.projectName = projectName;
        }

        /// <summary>
        /// Gets the Server instance.
        /// </summary>
        public DevOpsServer Server { get; }

        /// <summary>
        /// Gets the DevOps project Id.
        /// </summary>
        public Guid Id => TeamProject.Id;

        private TeamProject TeamProject => project ??= GetProject().GetAwaiter().GetResult();

        /// <summary>
        /// Get the DevOps TeamProject.
        /// </summary>
        /// <returns>A <see cref="Task{TeamProject}"/> representing the asynchronous operation.</returns>
        public async Task<TeamProject> GetProject()
        {
            if (project == null)
            {
                ProjectHttpClient projectClient = await Server.Connection.GetClientAsync<ProjectHttpClient>();

                project = await projectClient.GetProject(projectName);
            }

            return project;
        }

        /// <summary>
        /// Get the project Id.
        /// </summary>
        /// <returns>A <see cref="Task{Guid}"/> representing the result of the asynchronous operation.</returns>
        public async Task<Guid> GetProjectId()
        {
            if (project == null)
            {
                return (await GetProject()).Id;
            }

            return project.Id;
        }
    }
}