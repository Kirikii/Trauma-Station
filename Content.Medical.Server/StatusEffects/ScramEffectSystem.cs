using Content.Medical.Shared.StatusEffects;
using Content.Shared.Teleportation;
using Content.Goobstation.Shared.Teleportation.Systems;
using Content.Goobstation.Shared.Teleportation.Components;

namespace Content.Medical.Server.StatusEffects;

// TODO SHITMED: kill this dogshit and apply entity effects
public sealed class ScrambleLocationEffectSystem : EntitySystem
{
    [Dependency] private readonly SharedRandomTeleportSystem _teleportSys = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<ScrambleLocationEffectComponent, ComponentInit>(OnInit);
    }
    private void OnInit(EntityUid uid, ScrambleLocationEffectComponent component, ComponentInit args)
    {
        // TODO: Add the teleport component via onAdd:
        var teleport = EnsureComp<RandomTeleportComponent>(uid);
        _teleportSys.RandomTeleport(uid, teleport);
    }


}
