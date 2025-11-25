namespace InventoryManagement.Domain.Exceptions;

/// <summary>
/// Exception thrown when attempting to create a business client with a NIPT that already exists.
/// </summary>
public sealed class DuplicateNIPTException : DomainException
{
    /// <summary>
    /// Gets the duplicate NIPT value.
    /// </summary>
    public string NIPT { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateNIPTException"/> class.
    /// </summary>
    /// <param name="nipt">The duplicate NIPT value.</param>
    public DuplicateNIPTException(string nipt)
        : base($"A business client with NIPT '{nipt}' already exists.")
    {
        NIPT = nipt;
    }
}
