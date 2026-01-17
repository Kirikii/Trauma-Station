// SPDX-License-Identifier: AGPL-3.0-or-later
using Content.Medical.Shared.StatusEffects;
using Content.Shared.NPC.Components;
using Content.Shared.NPC.Systems;

namespace Content.Medical.Server.StatusEffects;

// TODO NUBODY: kill this dogshit
public sealed class SpawnEntityEffectSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _xformSys = default!;
    [Dependency] private readonly NpcFactionSystem _factionException = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<SpawnSpiderEggsComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SpawnSlimesComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SpawnEmpComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SpawnGravityWellComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SpawnFlashComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SpawnSmokeComponent, ComponentInit>(OnInit);
    }

    private void OnInit(EntityUid uid, SpawnEntityEffectComponent component, ComponentInit args)
    {
        EntityUid entity;

        if (component.AttachToParent)
        {
            entity = SpawnAttachedTo(component.EntityPrototype, Transform(uid).Coordinates);
            _xformSys.SetParent(entity, uid);
        }
        else
        {
            entity = Spawn(component.EntityPrototype, Transform(uid).Coordinates);
        }

        if (component.IsFriendly)
        {
            if (EnsureComp<FactionExceptionComponent>(entity, out var comp))
                return;

            _factionException.IgnoreEntities(entity, new[] { uid });
        }

    }


}
