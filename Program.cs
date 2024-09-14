using System.Reflection;
using CodeMechanic.Airtable;
using CodeMechanic.Diagnostics;
using CodeMechanic.Embeds;
using CodeMechanic.FileSystem;
using CodeMechanic.Scriptures;
using CodeMechanic.Sqlc;
using CodeMechanic.Todoist;
using evantage.Services;
using Lib.AspNetCore.ServerSentEvents;

var policyName = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Allow CORS: https://www.stackhawk.com/blog/net-cors-guide-what-it-is-and-how-to-enable-it/#enable-cors-and-fix-the-problem
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
        builder =>
        {
            builder
                // .WithOrigins("http://localhost:3000", "tpot-links-mkii")
                .AllowAnyOrigin()
                .WithMethods("GET")
                .AllowAnyHeader();
        });
});

// Load and inject .env files & values
DotEnv.Load(debug: true);

// add dependencies to services collection
builder.Services.AddHttpClient();


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// builder.Services.AddSingleton<IEmbeddedResourceQuery, EmbeddedResourceQuery>();
// var md_svc = new CodeMechanic.Markdown.MarkdownService();
// var readme_svc = new ReadmeService(md_svc);
// builder.Services.AddSingleton<IJsonConfigService, JsonConfigService>();
// builder.Services.AddSingleton<IPropertyCache, PropertyCache>();
// builder.Services.AddSingleton<ICsvService, CsvService>();
// builder.Services.AddSingleton<CodeMechanic.Markdown.IMarkdownService>(md_svc);
builder.Services.AddTransient<IGlobalLoggingService, GlobalLoggingService>();
builder.Services.AddSingleton<IInMemoryGraphService, InMemoryGraphService>();
// builder.Services.AddSingleton<IReadmeService>(readme_svc);
builder.Services.AddTransient<IScriptureService, ScriptureService>();
builder.Services.AddTransient<IRazorRoutesService2, RazorRoutesService2>();
builder.Services.AddTransient<IDownloadImages, ImageDownloader>();
builder.Services.AddTransient<IAirtableServiceV2, AirtableServiceV2>();
builder.Services.AddSingleton<ITodoistService, TodoistService>();
// builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IGenerateSQLTypes, SQLCService>();
builder.Services.AddTransient<INotesService, NotesService>();

builder.Services.AddTransient<IPartsRepository, PartsRepository>();
builder.Services.AddTransient<INugsService, NugsService>();
builder.Services.AddTransient<ICopyPastaService, CopyPastaService>();
builder.Services.AddControllers();

var main_assembly = Assembly.GetExecutingAssembly();
builder.Services.AddSingleton<IEmbeddedResourceQuery>(
    new EmbeddedResourceService(
            new Assembly[]
            {
                main_assembly
            },
            debugMode: false
        )
        .CacheAllEmbeddedFileContents());


// Add services to the container.
builder.Services.AddRazorPages();

// dependencies for server sent events
// credit: https://www.jetbrains.com/guide/dotnet/tutorials/htmx-aspnetcore/server-sent-events/
builder.Services.AddServerSentEvents();
builder.Services.AddHostedService<ServerEventsWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment().Dump("is dev?"))
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// source: https://github.com/tutorialseu/sending-emails-in-asp/blob/main/Program.cs

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors(policyName);
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


// the connection for server events
// credit: https://www.jetbrains.com/guide/dotnet/tutorials/htmx-aspnetcore/server-sent-events/
app.MapServerSentEvents("/rn-updates");

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();