// SPDX-License-Identifier: AGPL-3.0-or-later
using Content.Client.Gameplay;
using Content.Client.UserInterface.Systems.Alerts.Widgets;
using Content.Medical.Client.Targeting;
using Content.Medical.Client.UserInterface.Systems.PartStatus.Widgets;
using Content.Medical.Common.Targeting;
using Content.Medical.Shared.PartStatus;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Client.UserInterface.Controllers;
using Robust.Shared.Utility;
using Robust.Shared.Timing;

namespace Content.Medical.Client.UserInterface.Systems.PartStatus;

public sealed class PartStatusUIController : UIController, IOnStateEntered<GameplayState>, IOnSystemChanged<TargetingSystem>
{
    [Dependency] private readonly IEntityManager _entMan = default!;
    [Dependency] private readonly IEntityNetworkManager _entNet = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    private SpriteSystem _sprite = default!;

    private TargetingComponent? _targetingComponent;
    private PartStatusControl? PartStatusControl => UIManager.GetActiveUIWidgetOrNull<PartStatusControl>();

    public void OnSystemLoaded(TargetingSystem system)
    {
        AlertsUI.OnAlertsUICreated += AddPartStatusToAlerts;

        system.PartStatusStartup += UpdatePartStatusControl;
        system.PartStatusShutdown += RemovePartStatusControl;
        system.PartStatusUpdate += UpdatePartStatusControl;
    }

    public void OnSystemUnloaded(TargetingSystem system)
    {
        AlertsUI.OnAlertsUICreated -= AddPartStatusToAlerts;

        system.PartStatusStartup -= UpdatePartStatusControl;
        system.PartStatusShutdown -= RemovePartStatusControl;
        system.PartStatusUpdate -= UpdatePartStatusControl;
    }

    private void AddPartStatusToAlerts(AlertsUI alerts)
    {
        alerts.AddChild(new PartStatusControl());
    }

    public void OnStateEntered(GameplayState state)
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (PartStatusControl is not {} control)
            return;

        control.SetVisible(_targetingComponent != null);
        if (_targetingComponent is {} comp)
            control.SetTextures(comp.BodyStatus);
    }

    public void RemovePartStatusControl()
    {
        _targetingComponent = null;
        UpdateVisibility();
    }

    public void UpdatePartStatusControl(TargetingComponent component)
    {
        _targetingComponent = component;
        UpdateVisibility();
    }

    public Texture GetTexture(SpriteSpecifier specifier)
    {
        if (_sprite == null)
            _sprite = _entMan.System<SpriteSystem>();

        return _sprite.Frame0(specifier);
    }

    public void GetPartStatusMessage()
    {
        if (_player.LocalEntity is not {} user
            || !_entMan.HasComponent<TargetingComponent>(user)
            || PartStatusControl == null
            || !_timing.IsFirstTimePredicted)
            return;

        _entNet.SendSystemNetworkMessage(new GetPartStatusEvent());
    }
}
