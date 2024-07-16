using Content.Shared.Abilities;

namespace Content.Client.Overlays;

public sealed partial class DefaultVisionSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DefaultVisionComponent, ComponentInit>(OnDefaultVisionInit);
    }

    private void OnDefaultVisionInit(EntityUid uid, DefaultVisionComponent component, ComponentInit args)
    {
        RemComp<UltraVisionComponent>(uid);
        RemComp<DogVisionComponent>(uid);
    }
}
