// SPDX-License-Identifier: AGPL-3.0-or-later
using Content.Medical.Shared.Body;
using Content.Shared.StatusEffect;
using Robust.Shared.Timing;
using Robust.Shared.Random;

namespace Content.Medical.Server.Body;

public sealed class RandomStatusActivationSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly StatusEffectsSystem _effects = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RandomStatusActivationComponent, MapInitEvent>(OnInit);
    }

    private void OnInit(EntityUid uid, RandomStatusActivationComponent component, MapInitEvent args) => GetRandomTime(component);

    private void GetRandomTime(RandomStatusActivationComponent component)
    {
        var randomSeconds = _random.NextDouble(component.MinActivationTime.TotalSeconds, component.MaxActivationTime.TotalSeconds);
        var randomSpan = TimeSpan.FromSeconds(randomSeconds);
        component.NextUpdate = _timing.CurTime + randomSpan;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<RandomStatusActivationComponent>();
        var now = _timing.CurTime;
        while (query.MoveNext(out var uid, out var comp))
        {
            if (now < comp.NextUpdate)
                continue;

            if (!TryComp<StatusEffectsComponent>(uid, out var effects))
                continue;

            foreach (var (key, component) in comp.StatusEffects)
                _effects.TryAddStatusEffect(uid, key, comp.Duration ?? TimeSpan.FromSeconds(1), refresh: true, component, effects);

            GetRandomTime(comp);
        }
    }
}
