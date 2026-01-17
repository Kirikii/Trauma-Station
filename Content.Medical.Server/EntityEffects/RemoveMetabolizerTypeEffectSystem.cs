// SPDX-License-Identifier: AGPL-3.0-or-later
using Content.Medical.Shared.EntityEffects;
using Content.Server.Body.Components;
using Content.Shared.EntityEffects;

namespace Content.Medical.Server.EntityEffects;

public sealed class RemoveMetabolizerTypeEffectSystem : EntityEffectSystem<MetabolizerComponent, RemoveMetabolizerType>
{
    protected override void Effect(Entity<MetabolizerComponent> ent, ref EntityEffectEvent<RemoveMetabolizerType> args)
    {
        var type = args.Effect.Type;
        if (ent.Comp.MetabolizerTypes is {} types && types.Contains(type))
            types.Remove(type);
    }
}
