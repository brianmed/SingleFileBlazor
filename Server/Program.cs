using System.Reflection;

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

#if ISRELEASE
        // This was used because I had issues with files with lots of dots with 
        // EmbeddedFileProvider
        ManifestEmbeddedFileProvider manifestEmbeddedFileProvider = new(Assembly.GetEntryAssembly(), "clientWwwroot/wwwroot");
#else
        ManifestEmbeddedFileProvider manifestEmbeddedFileProvider = null;
#endif

        // Needed so that blazor files are served.
        FileExtensionContentTypeProvider provider = new();
        provider.Mappings[".cfg"] = "text/plain";
        provider.Mappings[".properties"] = "application/octect-stream";
        provider.Mappings[".wasm"] = "application/wasm";
        provider.Mappings[".blat"] = "application/octet-stream";
        provider.Mappings[".dll"] = "application/octet-stream";
        provider.Mappings[".dat"] = "application/octet-stream";
        provider.Mappings[".pdb"] = "application/octet-stream";
        provider.Mappings[".json"] = "application/json";
        provider.Mappings[".wasm"] = "application/wasm";
        provider.Mappings[".woff"] = "application/font-woff";
        provider.Mappings[".woff2"] = "application/font-woff";
        provider.Mappings[".js"] = "text/javascript";
        provider.Mappings[".css"] = "text/css";
        provider.Mappings[".html"] = "text/html";

#if ISRELEASE
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = manifestEmbeddedFileProvider,
            ContentTypeProvider = provider
        });
#else
        app.UseStaticFiles();
#endif

// This must be placed after the manifestEmbeddedFileProvider above.
app.UseBlazorFrameworkFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();

#if ISRELEASE
        // Strange, that the FileProvider was required (index.html is not servered without).
        app.MapFallbackToFile("index.html", new StaticFileOptions
        {
            FileProvider = manifestEmbeddedFileProvider,
            ContentTypeProvider = provider
        });
#else
        app.MapFallbackToFile("index.html");
#endif

app.Run();
