namespace Hexalith.Infrastructure.QuartzScheduler.Helpers
{
    using Microsoft.Extensions.DependencyInjection;

    using Quartz;

    public static class QuartzHelper
    {
        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services, string name)
        {
            services.AddQuartz(q =>
            {
                // base quartz scheduler, job and trigger configuration handy when part of cluster
                // or you want to otherwise identify multiple schedulers
                q.SchedulerId = name;
                q.SchedulerName = "Scheduler " + name;

                // Scoped service support like EF Core DbContext
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                // these are the defaults
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp => tp.MaxConcurrency = 10);
            });

            // ASP.NET Core hosting
            services.AddQuartzServer(options =>
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true);
            return services;
        }
    }
}