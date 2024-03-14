using Grpc.Net.Client;
using Intellectual;
using TicTacToeLib;
using WebServer.Data;

var builder = WebApplication.CreateBuilder(args);

using var channel = GrpcChannel.ForAddress("http://host.docker.internal:5291");

var grpcClient = new IntellectService.IntellectServiceClient(channel);

var helper = new RemoteHelper(grpcClient);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddLogging();
builder.Services.AddSingleton((IHelper)helper);
builder.Services.AddTransient<Game>();
builder.Services.AddTransient<Bot>();
builder.Services.AddTransient<Player>();

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