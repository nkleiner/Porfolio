using Grpc.Core;

namespace Portfolio.Backend.Services
{
  public class PortfolioServiceImpl : PortfolioService.PortfolioServiceBase
  {
    private readonly ILogger<PortfolioServiceImpl> _logger;

    public PortfolioServiceImpl(ILogger<PortfolioServiceImpl> logger)
    {
      _logger = logger;
    }

    public override Task<GetProjectsResponse> GetProjects(GetProjectsRequest request, ServerCallContext context)
    {
      _logger.LogInformation("gRPC-Web client requested projects list.");

      var response = new GetProjectsResponse();

      response.Projects.Add(new Project
      {
        Title = "Enterprise C# Systems",
        Description = "A high-throughput architectural playground utilizing gRPC, Protobuf, and modern backend pipelines.",
        Url = "https://github.com/yourusername/portfolio"
      });

      response.Projects.Add(new Project
      {
        Title = "SvelteKit Static Sandbox",
        Description = "A lightning-fast frontend compiled down to surgical DOM updates with Svelte 5 runes.",
        Url = "https://github.com/yourusername/portfolio-frontend"
      });

      return Task.FromResult(response);
    }
  }
}