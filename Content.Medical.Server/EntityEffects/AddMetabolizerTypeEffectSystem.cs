// SPDX-License-Identifier: AGPL-3.0-or-later
using Content.Medical.Shared.EntityEffects;
using Content.Server.Body.Components;
using Content.Shared.EntityEffects;

namespace Content.Medical.Server.EntityEffects;

public sealed class AddMetabolizerTypeEffectSystem : EntityEffectSystem<MetabolizerComponent, AddMetabolizerType>
{
    protected override void Effect(Entity<MetabolizerComponent> ent, ref EntityEffectEvent<AddMetabolizerType> args)
    {
        ent.Comp.MetabolizerTypes ??= new();
        ent.Comp.MetabolizerTypes.Add(args.Effect.Type);
    }
}
