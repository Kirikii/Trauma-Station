// SPDX-License-Identifier: AGPL-3.0-or-later
namespace Content.Medical.Common.Humanoid;

/// <summary>
/// Raised on a humanoid mob when their profile has finished being loaded
/// </summary>
[ByRefEvent]
public record struct ProfileLoadFinishedEvent;
