using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Reflection;

namespace NotificationService.Insfrastructure.Utilities.HangFire
{
    public static class HangfireServiceExtension
    {
        public static void AddHangFire(this WebApplicationBuilder builder, Assembly[] assemblies)
        {
            AddHangfireJobsFromAssemblies(builder, assemblies);
            builder.Services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseMongoStorage(new MongoClient(builder.Configuration["Mongo_ConnectionString"]),
                    "jobs", new MongoStorageOptions
                    {
                        MigrationOptions = new MongoMigrationOptions
                        {
                            MigrationStrategy = new MigrateMongoMigrationStrategy(),
                            BackupStrategy = new CollectionMongoBackupStrategy(),
                        },
                        Prefix = "hangfire",
                        CheckConnection = true,
                        CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
                    }));
            builder.Services.AddHangfireServer(serverOptions =>
                serverOptions.ServerName = $"Hangfire.Mongo.{builder.Configuration["RegionName"]}");
        }
        private static void AddHangfireJobsFromAssemblies(WebApplicationBuilder builder, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var jobTypes = assembly.GetTypes()
                    .Where(type => typeof(IHangFireJob).IsAssignableFrom(type) && !type.IsAbstract);

                foreach (var jobType in jobTypes)
                {
                    builder.Services.AddScoped(jobType);
                }
            }
        }
    }
}
