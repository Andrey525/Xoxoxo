using Grpc.Net.Client;
using Intellectual;
using TicTacToeLib;
using WebServer.Data;

var builder = WebApplication.CreateBuilder(args);

string? pathToIntellectService = Environment.GetEnvironmentVariable("INTELLECT_URLS");

if (pathToIntellectService == null)
{
    Console.WriteLine("Empty env variable");
    return;
}

using var channel = GrpcChannel.ForAddress(pathToIntellectService);

var grpcClient = new IntellectService.IntellectServiceClient(channel);

/*var helper = new RemoteHelper(grpcClient);*/

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddLogging();
/*builder.Services.AddSingleton((IHelper)helper);*/
builder.Services.AddSingleton<IHelper, RemoteHelper>();
builder.Services.AddSingleton<IHelper, LocalHelper>();
builder.Services.AddSingleton<FailoverBase, Failover>();
builder.Services.AddSingleton(grpcClient);
builder.Services.AddTransient<Game>();
builder.Services.AddTransient<Bot>();
/*builder.Services.AddTransient<Player>();*/

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();