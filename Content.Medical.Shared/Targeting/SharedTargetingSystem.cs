// SPDX-License-Identifier: AGPL-3.0-or-later
using Content.Medical.Common.Targeting;
using Content.Medical.Shared.Body;
using Content.Shared.Body;
using Content.Shared.Humanoid;

namespace Content.Medical.Shared.Targeting;

public abstract class SharedTargetingSystem : EntitySystem
{
    [Dependency] private readonly BodySystem _body = default!;
    [Dependency] private readonly BodyPartSystem _part = default!;

    /// <summary>
    /// Array of all valid targeting enums.
    /// </summary>
    public static readonly TargetBodyPart[] ValidParts =
    [
        TargetBodyPart.Head,
        TargetBodyPart.Chest,
        TargetBodyPart.Groin,
        TargetBodyPart.LeftArm,
        TargetBodyPart.LeftHand,
        TargetBodyPart.LeftLeg,
        TargetBodyPart.LeftFoot,
        TargetBodyPart.RightArm,
        TargetBodyPart.RightHand,
        TargetBodyPart.RightLeg,
        TargetBodyPart.RightFoot,
    ];

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TargetingComponent, GetTargetedPartEvent>(OnGetTargetedPart);
    }

    private void OnGetTargetedPart(Entity<TargetingComponent> ent, ref GetTargetedPartEvent args)
    {
        if (args.Part != null)
            return;

        var (partType, symmetry) = _body.ConvertTargetBodyPart(ent.Comp.Target);
        args.Part = _part.FindBodyPart(args.Target, partType, symmetry)?.Owner;
    }

    /* TODO NUBODY: kill?
    public static HumanoidVisualLayers ToVisualLayers(TargetBodyPart targetBodyPart)
    {
        switch (targetBodyPart)
        {
            case TargetBodyPart.Head:
                return HumanoidVisualLayers.Head;
            case TargetBodyPart.Chest:
                return HumanoidVisualLayers.Chest;
            case TargetBodyPart.Groin:
                return HumanoidVisualLayers.Groin;
            case TargetBodyPart.LeftArm:
                return HumanoidVisualLayers.LArm;
            case TargetBodyPart.LeftHand:
                return HumanoidVisualLayers.LHand;
            case TargetBodyPart.RightArm:
                return HumanoidVisualLayers.RArm;
            case TargetBodyPart.RightHand:
                return HumanoidVisualLayers.RHand;
            case TargetBodyPart.LeftLeg:
                return HumanoidVisualLayers.LLeg;
            case TargetBodyPart.LeftFoot:
                return HumanoidVisualLayers.LFoot;
            case TargetBodyPart.RightLeg:
                return HumanoidVisualLayers.RLeg;
            case TargetBodyPart.RightFoot:
                return HumanoidVisualLayers.RFoot;
            default:
                return HumanoidVisualLayers.Chest;
        }
    }*/
}
