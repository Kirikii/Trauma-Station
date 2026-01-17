namespace Content.Shared.Body;

/// <summary>
/// Trauma - localization extensions
/// </summary>
public sealed partial class OrganCategoryPrototype
{
    public LocId NameLoc => $"organ-category-{ID}-name";

    /// <summary>
    /// Get the localized name for this category.
    /// </summary>
    public string Name => Loc.GetString(NameLoc);
}
