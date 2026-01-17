// SPDX-License-Identifier: AGPL-3.0-or-later
using Content.Goobstation.Common.Weapons.Ranged;
using Content.Medical.Common.Wounds;
using Content.Medical.Shared.Targeting;
using Content.Medical.Shared.Wounds;
using Content.Shared.Mobs;

namespace Content.Medical.Server.Targeting;

public sealed class TargetingSystem : SharedTargetingSystem
{
    [Dependency] private readonly WoundSystem _wound = default!;

    private EntityQuery<TargetingComponent> _query;

    public override void Initialize()
    {
        base.Initialize();

        _query = GetEntityQuery<TargetingComponent>();

        SubscribeLocalEvent<TargetingComponent, MobStateChangedEvent>(OnMobStateChange);
        SubscribeNetworkEvent<ChangeTargetMessage>(OnChangeTarget);
    }

    private void OnChangeTarget(ChangeTargetMessage msg, EntitySessionEventArgs args)
    {
        if (args.SenderSession.AttachedEntity is not {} user ||
            !_query.TryComp(user, out var comp) ||
            comp.Target == msg.Part)
            return;

        comp.Target = msg.BodyPart;
        Dirty(user, comp);
    }

    private void OnMobStateChange(EntityUid uid, TargetingComponent component, MobStateChangedEvent args)
    {
        // Revival is handled by the server, so we're keeping all of this here.
        // TODO SHITMED: ??? what crack were you smoking mocho
        var changed = false;

        if (args.NewMobState == MobState.Dead)
        {
            foreach (var part in ValidParts)
            {
                component.BodyStatus[part] = WoundableSeverity.Severed;
                changed = true;
            }
        }
        else if (args.OldMobState == MobState.Dead)
        {
            component.BodyStatus = _wound.GetWoundableStatesOnBodyPainFeels(uid);
            changed = true;
        }

        if (!changed)
            return;

        Dirty(uid, component);
        RaiseNetworkEvent(new TargetIntegrityChangedMessage(), uid);
    }
}
