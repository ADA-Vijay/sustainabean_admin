using Microsoft.Extensions.Configuration;
using sustainbean_api.Repository;
using System.Collections.Generic;

namespace sustainbean_api
{
    public static class ServicesExtension
    {
        public static IServiceCollection BindingAppServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("Blogs");

            // Register IHttpContextAccessor as a singleton
            services.AddHttpContextAccessor();

            // Register the TagRepository with a scoped lifetime to ensure it has access to the HttpContext
            services.AddTransient<ITagRepository>(provider => new TagRepository(connectionString, provider.GetRequiredService<IHttpContextAccessor>()));
            services.AddTransient<IBlogRepository>(provider => new BlogRepository(connectionString, provider.GetRequiredService<IHttpContextAccessor>()));
            services.AddTransient<ICategoryRepository>(provider => new CategoryRepository(connectionString, provider.GetRequiredService<IHttpContextAccessor>()));
            services.AddTransient<IFeatureRepository>(provider => new FeatureRepository(connectionString, provider.GetRequiredService<IHttpContextAccessor>()));

            return services;
        }
    }
}
