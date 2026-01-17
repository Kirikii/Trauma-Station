// SPDX-License-Identifier: AGPL-3.0-or-later
using Content.Medical.Shared.StatusEffects;
using Content.Shared.Trigger.Systems;

namespace Content.Medical.Server.StatusEffects;

// TODO SHITMED: kill this dogshit and apply entity effects
public sealed class ScrambleDnaEffectSystem : EntitySystem
{
    [Dependency] private readonly DnaScrambleOnTriggerSystem _scramble = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ScrambleDnaEffectComponent, ComponentInit>(OnInit);
    }

    private void OnInit(EntityUid uid, ScrambleDnaEffectComponent component, ComponentInit args)
    {
        _scramble.Scramble(uid);
    }
}
