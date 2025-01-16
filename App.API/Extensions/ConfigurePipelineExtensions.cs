using App.API.Extensons;

namespace App.API.Extensions
{
    public static class ConfigurePipelineExtensions
    {
        public static IApplicationBuilder ConfigurePipelineExt(this WebApplication app)
        {
            app.UseExceptionHandler(p => { });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerExt();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            return app;
        }
    }
}
