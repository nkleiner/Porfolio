using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Portfolio.Backend.Services.Local;

namespace Portfolio.Backend.Services;

public class DungeonsAndDragonsService : DNDService.DNDServiceBase
{
  private readonly WildMagicService _wildMagicService;
  public DungeonsAndDragonsService(WildMagicService wildMagicService)
  {
    _wildMagicService = wildMagicService;
  }

  public override Task<WildMagicResponse> GetWildMagicRoll(Empty request, ServerCallContext context)
  {
    var wildMagicRoll = _wildMagicService.GetWildMagicEffect();
    
    var response = new WildMagicResponse() 
    { 
      WildMagicEffect = wildMagicRoll.effect, 
      Roll = wildMagicRoll.roll 
    
    };
    return Task.FromResult(response);
  }
}
