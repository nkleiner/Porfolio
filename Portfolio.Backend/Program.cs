using Portfolio.Backend.Services;
using Portfolio.Backend.Services.Local;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<WildMagicService>();
builder.Services.AddGrpc();

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowSvelteFrontend", policy =>
  {
    policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
  });
});


var app = builder.Build();

app.UseRouting();
app.UseCors("AllowSvelteFrontend");
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapGrpcService<PortfolioServiceImpl>().EnableGrpcWeb();
app.MapGrpcService<DungeonsAndDragonsService>().EnableGrpcWeb();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();