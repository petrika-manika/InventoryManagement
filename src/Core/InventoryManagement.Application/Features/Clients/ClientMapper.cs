using InventoryManagement.Application.Common.Models;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Features.Clients;

/// <summary>
/// Static mapper class for converting Client entities to DTOs.
/// Centralizes mapping logic to reduce code duplication.
/// </summary>
public static class ClientMapper
{
    /// <summary>
    /// Maps a Client entity to its appropriate DTO based on the concrete type.
    /// </summary>
    /// <param name="client">The client entity to map.</param>
    /// <returns>The appropriate ClientDto subtype (IndividualClientDto or BusinessClientDto).</returns>
    public static ClientDto MapToDto(Client client)
    {
        return client switch
        {
            IndividualClient individual => MapIndividual(individual),
            BusinessClient business => MapBusiness(business),
            _ => MapBase(client) // Fallback for unknown types (shouldn't happen)
        };
    }

    /// <summary>
    /// Maps an IndividualClient entity to IndividualClientDto.
    /// </summary>
    /// <param name="client">The individual client entity.</param>
    /// <returns>A populated IndividualClientDto.</returns>
    public static IndividualClientDto MapIndividual(IndividualClient client)
    {
        return new IndividualClientDto
        {
            // Common properties
            Id = client.Id,
            ClientType = client.ClientType.ToString(),
            ClientTypeId = (int)client.ClientType,
            Address = client.Address,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Notes = client.Notes,
            CreatedAt = client.CreatedAt,
            UpdatedAt = client.UpdatedAt,
            CreatedBy = client.CreatedBy,
            UpdatedBy = client.UpdatedBy,
            IsActive = client.IsActive,

            // Individual-specific properties
            FirstName = client.FirstName,
            LastName = client.LastName,
            FullName = client.FullName
        };
    }

    /// <summary>
    /// Maps a BusinessClient entity to BusinessClientDto.
    /// </summary>
    /// <param name="client">The business client entity.</param>
    /// <returns>A populated BusinessClientDto.</returns>
    public static BusinessClientDto MapBusiness(BusinessClient client)
    {
        return new BusinessClientDto
        {
            // Common properties
            Id = client.Id,
            ClientType = client.ClientType.ToString(),
            ClientTypeId = (int)client.ClientType,
            Address = client.Address,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Notes = client.Notes,
            CreatedAt = client.CreatedAt,
            UpdatedAt = client.UpdatedAt,
            CreatedBy = client.CreatedBy,
            UpdatedBy = client.UpdatedBy,
            IsActive = client.IsActive,

            // Business-specific properties
            NIPT = client.NIPT.Value,
            OwnerFirstName = client.OwnerFirstName,
            OwnerLastName = client.OwnerLastName,
            OwnerPhoneNumber = client.OwnerPhoneNumber,
            OwnerFullName = client.OwnerFullName,
            ContactPersonFirstName = client.ContactPersonFirstName,
            ContactPersonLastName = client.ContactPersonLastName,
            ContactPersonPhoneNumber = client.ContactPersonPhoneNumber,
            ContactPersonFullName = client.ContactPersonFullName
        };
    }

    /// <summary>
    /// Maps base Client properties to ClientDto (fallback for unknown types).
    /// </summary>
    /// <param name="client">The client entity.</param>
    /// <returns>A base ClientDto with common properties only.</returns>
    private static ClientDto MapBase(Client client)
    {
        return new ClientDto
        {
            Id = client.Id,
            ClientType = client.ClientType.ToString(),
            ClientTypeId = (int)client.ClientType,
            Address = client.Address,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Notes = client.Notes,
            CreatedAt = client.CreatedAt,
            UpdatedAt = client.UpdatedAt,
            CreatedBy = client.CreatedBy,
            UpdatedBy = client.UpdatedBy,
            IsActive = client.IsActive
        };
    }
}
