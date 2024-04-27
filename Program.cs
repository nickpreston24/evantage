using System.Reflection;
using CodeMechanic.Airtable;
using CodeMechanic.Embeds;
using CodeMechanic.FileSystem;
using CodeMechanic.RazorHAT.Services;
using CodeMechanic.Todoist;
using evantage.Services;
using IMarkdownService = evantage.Services.IMarkdownService;
using MarkdownService = CodeMechanic.RazorHAT.Services.MarkdownService;

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
DotEnv.Load();

builder.Services.AddTransient<IEmbeddedResourceQuery, EmbeddedResourceQuery>();
builder.Services.AddSingleton<IJsonConfigService, JsonConfigService>();
builder.Services.AddSingleton<IMarkdownService, evantage.Services.MarkdownService>();
builder.Services.AddSingleton<IInMemoryGraphService, InMemoryGraphService>();
builder.Services.AddSingleton<IRazorRoutesService2, RazorRoutesService2>();
builder.Services.AddSingleton<IDownloadImages, ImageDownloader>();
builder.Services.AddSingleton<IAirtableServiceV2, AirtableServiceV2>();
builder.Services.AddSingleton<ITodoistService, TodoistService>();

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

var app = builder.Build();

// source: https://github.com/tutorialseu/sending-emails-in-asp/blob/main/Program.cs

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors(policyName);
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();