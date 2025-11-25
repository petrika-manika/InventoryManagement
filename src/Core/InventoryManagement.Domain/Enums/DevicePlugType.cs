namespace InventoryManagement.Domain.Enums;

/// <summary>
/// Represents plug format options for Aroma Device and Sanitizing Device products.
/// </summary>
public enum DevicePlugType
{
    /// <summary>
    /// Device comes with a plug
    /// </summary>
    WithPlug = 1,

    /// <summary>
    /// Device does not come with a plug
    /// </summary>
    WithoutPlug = 2
}
