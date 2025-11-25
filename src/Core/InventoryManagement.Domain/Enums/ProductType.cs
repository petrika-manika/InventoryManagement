namespace InventoryManagement.Domain.Enums;

/// <summary>
/// Represents the fixed product categories for the aroma business.
/// These categories cannot be added or deleted by users.
/// </summary>
public enum ProductType
{
    /// <summary>
    /// Aroma bombs with taste options
    /// </summary>
    AromaBombel = 1,

    /// <summary>
    /// Aroma bottles with taste options
    /// </summary>
    AromaBottle = 2,

    /// <summary>
    /// Aroma devices with color, format, and programs
    /// </summary>
    AromaDevice = 3,

    /// <summary>
    /// Sanitizing devices with color, format, and programs
    /// </summary>
    SanitizingDevice = 4,

    /// <summary>
    /// Batteries with size and brand options
    /// </summary>
    Battery = 5
}
