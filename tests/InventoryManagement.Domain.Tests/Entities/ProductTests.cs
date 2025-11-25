using FluentAssertions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Tests.Entities;

public class ProductTests
{
    #region AromaBombelProduct Tests

    [Fact]
    public void AromaBombel_Create_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var name = ProductName.Create("Lavender Bombel");
        var description = "Relaxing lavender aroma";
        var price = Money.Create(1500m);
        var photoUrl = "https://example.com/lavender.jpg";
        var taste = TasteType.Flower;

        // Act
        var product = AromaBombelProduct.Create(name, description, price, photoUrl, taste);

        // Assert
        product.Should().NotBeNull();
        product.Id.Should().NotBeEmpty();
        product.Name.Should().Be(name);
        product.Description.Should().Be(description);
        product.Price.Should().Be(price);
        product.PhotoUrl.Should().Be(photoUrl);
        product.Taste.Should().Be(taste);
        product.ProductType.Should().Be(ProductType.AromaBombel);
        product.StockQuantity.Should().Be(0);
        product.IsActive.Should().BeTrue();
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        product.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AromaBombel_Create_WithoutName_ShouldThrowException()
    {
        // Arrange
        var price = Money.Create(1500m);

        // Act
        Action act = () => AromaBombelProduct.Create(null!, "Description", price, null, TasteType.Flower);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*name*");
    }

    [Fact]
    public void AromaBombel_Create_WithoutPrice_ShouldThrowException()
    {
        // Arrange
        var name = ProductName.Create("Lavender Bombel");

        // Act
        Action act = () => AromaBombelProduct.Create(name, "Description", null!, null, TasteType.Flower);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*price*");
    }

    [Fact]
    public void AromaBombel_AddStock_ValidQuantity_ShouldIncreaseStock()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        var quantityToAdd = 50;

        // Act
        var newStock = product.AddStock(quantityToAdd);

        // Assert
        newStock.Should().Be(50);
        product.StockQuantity.Should().Be(50);
    }

    [Fact]
    public void AromaBombel_AddStock_ZeroQuantity_ShouldThrowException()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();

        // Act
        Action act = () => product.AddStock(0);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*greater than zero*");
    }

    [Fact]
    public void AromaBombel_RemoveStock_ValidQuantity_ShouldDecreaseStock()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        product.AddStock(100);

        // Act
        var newStock = product.RemoveStock(30);

        // Assert
        newStock.Should().Be(70);
        product.StockQuantity.Should().Be(70);
    }

    [Fact]
    public void AromaBombel_RemoveStock_InsufficientStock_ShouldThrowException()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        product.AddStock(20);

        // Act
        Action act = () => product.RemoveStock(50);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Insufficient stock*");
    }

    [Fact]
    public void AromaBombel_RemoveStock_NegativeQuantity_ShouldThrowException()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        product.AddStock(50);

        // Act
        Action act = () => product.RemoveStock(-10);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*greater than zero*");
    }

    [Fact]
    public void AromaBombel_IsLowStock_BelowThreshold_ShouldReturnTrue()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        product.AddStock(8);

        // Act
        var isLow = product.IsLowStock(10);

        // Assert
        isLow.Should().BeTrue();
    }

    [Fact]
    public void AromaBombel_IsLowStock_AboveThreshold_ShouldReturnFalse()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        product.AddStock(50);

        // Act
        var isLow = product.IsLowStock(10);

        // Assert
        isLow.Should().BeFalse();
    }

    [Fact]
    public void AromaBombel_UpdateSpecificInfo_ShouldUpdateTaste()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        var originalUpdatedAt = product.UpdatedAt;
        Thread.Sleep(10); // Ensure time difference

        // Act
        product.UpdateSpecificInfo(TasteType.Sweet);

        // Assert
        product.Taste.Should().Be(TasteType.Sweet);
        product.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public void AromaBombel_Activate_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        product.Deactivate();

        // Act
        product.Activate();

        // Assert
        product.IsActive.Should().BeTrue();
    }

    [Fact]
    public void AromaBombel_Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();

        // Act
        product.Deactivate();

        // Assert
        product.IsActive.Should().BeFalse();
    }

    [Fact]
    public void AromaBombel_UpdateBasicInfo_ShouldUpdateCommonProperties()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        var newName = ProductName.Create("New Product Name");
        var newDescription = "New description";
        var newPrice = Money.Create(2000m);
        var newPhotoUrl = "https://example.com/new.jpg";

        // Act
        product.UpdateBasicInfo(newName, newDescription, newPrice, newPhotoUrl);

        // Assert
        product.Name.Should().Be(newName);
        product.Description.Should().Be(newDescription);
        product.Price.Should().Be(newPrice);
        product.PhotoUrl.Should().Be(newPhotoUrl);
    }

    #endregion

    #region AromaDeviceProduct Tests

    [Fact]
    public void AromaDevice_Create_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var name = ProductName.Create("Smart Aroma Device");
        var description = "Advanced aroma diffuser";
        var price = Money.Create(5000m);
        var photoUrl = "https://example.com/device.jpg";
        var color = ColorType.White;
        var format = "Compact";
        var programs = "3 timer modes";
        var plugType = DevicePlugType.WithPlug;
        var squareMeter = 50m;

        // Act
        var product = AromaDeviceProduct.Create(
            name, description, price, photoUrl,
            color, format, programs, plugType, squareMeter);

        // Assert
        product.Should().NotBeNull();
        product.Id.Should().NotBeEmpty();
        product.Name.Should().Be(name);
        product.Color.Should().Be(color);
        product.Format.Should().Be(format);
        product.Programs.Should().Be(programs);
        product.PlugType.Should().Be(plugType);
        product.SquareMeter.Should().Be(squareMeter);
        product.ProductType.Should().Be(ProductType.AromaDevice);
    }

    [Fact]
    public void AromaDevice_Create_WithNegativeSquareMeter_ShouldThrowException()
    {
        // Arrange
        var name = ProductName.Create("Smart Aroma Device");
        var price = Money.Create(5000m);

        // Act
        Action act = () => AromaDeviceProduct.Create(
            name, "Description", price, null,
            ColorType.White, null, null, DevicePlugType.WithPlug, -10m);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*cannot be negative*");
    }

    [Fact]
    public void AromaDevice_Create_WithoutPlugType_ShouldNotThrowException()
    {
        // Arrange
        var name = ProductName.Create("Smart Aroma Device");
        var price = Money.Create(5000m);

        // Act - PlugType is required (enum, not nullable), so we just pass a valid value
        var product = AromaDeviceProduct.Create(
            name, "Description", price, null,
            null, null, null, DevicePlugType.WithPlug, null);

        // Assert
        product.Should().NotBeNull();
        product.PlugType.Should().Be(DevicePlugType.WithPlug);
    }

    [Fact]
    public void AromaDevice_UpdateSpecificInfo_ShouldUpdateAllFields()
    {
        // Arrange
        var product = CreateTestAromaDeviceProduct();
        var newColor = ColorType.Black;
        var newFormat = "Large";
        var newPrograms = "5 timer modes";
        var newPlugType = DevicePlugType.WithoutPlug;
        var newSquareMeter = 100m;

        // Act
        product.UpdateSpecificInfo(newColor, newFormat, newPrograms, newPlugType, newSquareMeter);

        // Assert
        product.Color.Should().Be(newColor);
        product.Format.Should().Be(newFormat);
        product.Programs.Should().Be(newPrograms);
        product.PlugType.Should().Be(newPlugType);
        product.SquareMeter.Should().Be(newSquareMeter);
    }

    [Fact]
    public void AromaDevice_UpdateSpecificInfo_WithNegativeSquareMeter_ShouldThrowException()
    {
        // Arrange
        var product = CreateTestAromaDeviceProduct();

        // Act
        Action act = () => product.UpdateSpecificInfo(
            ColorType.White, null, null, DevicePlugType.WithPlug, -50m);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*cannot be negative*");
    }

    #endregion

    #region BatteryProduct Tests

    [Fact]
    public void Battery_Create_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var name = ProductName.Create("Alkaline Battery");
        var description = "Long-lasting battery";
        var price = Money.Create(200m);
        var photoUrl = "https://example.com/battery.jpg";
        var type = "Alkaline";
        var size = BatterySize.LR6;
        var brand = "Duracell";

        // Act
        var product = BatteryProduct.Create(
            name, description, price, photoUrl,
            type, size, brand);

        // Assert
        product.Should().NotBeNull();
        product.Id.Should().NotBeEmpty();
        product.Name.Should().Be(name);
        product.Type.Should().Be(type);
        product.Size.Should().Be(size);
        product.Brand.Should().Be(brand);
        product.ProductType.Should().Be(ProductType.Battery);
    }

    [Fact]
    public void Battery_Create_WithNullOptionalFields_ShouldCreateProduct()
    {
        // Arrange
        var name = ProductName.Create("Generic Battery");
        var price = Money.Create(150m);

        // Act
        var product = BatteryProduct.Create(
            name, null, price, null,
            null, null, null);

        // Assert
        product.Should().NotBeNull();
        product.Type.Should().BeNull();
        product.Size.Should().BeNull();
        product.Brand.Should().BeNull();
    }

    [Fact]
    public void Battery_UpdateSpecificInfo_ShouldUpdateFields()
    {
        // Arrange
        var product = CreateTestBatteryProduct();
        var newType = "Lithium";
        var newSize = BatterySize.LR9;
        var newBrand = "Energizer";

        // Act
        product.UpdateSpecificInfo(newType, newSize, newBrand);

        // Assert
        product.Type.Should().Be(newType);
        product.Size.Should().Be(newSize);
        product.Brand.Should().Be(newBrand);
    }

    #endregion

    #region ValidateCanBeDeleted Tests

    [Fact]
    public void ValidateCanBeDeleted_WithStock_ShouldThrowException()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        product.AddStock(50);

        // Act
        Action act = () => product.ValidateCanBeDeleted();

        // Assert
        act.Should().Throw<CannotDeleteProductWithStockException>()
            .WithMessage("*Test Bombel*")
            .WithMessage("*50*")
            .And.ProductName.Should().Be("Test Bombel");
        
        act.Should().Throw<CannotDeleteProductWithStockException>()
            .And.StockQuantity.Should().Be(50);
    }

    [Fact]
    public void ValidateCanBeDeleted_WithoutStock_ShouldNotThrow()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        // StockQuantity is 0 by default

        // Act
        Action act = () => product.ValidateCanBeDeleted();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateCanBeDeleted_AfterRemovingAllStock_ShouldNotThrow()
    {
        // Arrange
        var product = CreateTestAromaBombelProduct();
        product.AddStock(100);
        product.RemoveStock(100);

        // Act
        Action act = () => product.ValidateCanBeDeleted();

        // Assert
        act.Should().NotThrow();
        product.StockQuantity.Should().Be(0);
    }

    #endregion

    #region Helper Methods

    private static AromaBombelProduct CreateTestAromaBombelProduct()
    {
        return AromaBombelProduct.Create(
            ProductName.Create("Test Bombel"),
            "Test Description",
            Money.Create(1000m),
            null,
            TasteType.Flower);
    }

    private static AromaDeviceProduct CreateTestAromaDeviceProduct()
    {
        return AromaDeviceProduct.Create(
            ProductName.Create("Test Device"),
            "Test Description",
            Money.Create(5000m),
            null,
            ColorType.White,
            "Compact",
            "3 modes",
            DevicePlugType.WithPlug,
            50m);
    }

    private static BatteryProduct CreateTestBatteryProduct()
    {
        return BatteryProduct.Create(
            ProductName.Create("Test Battery"),
            "Test Description",
            Money.Create(200m),
            null,
            "Alkaline",
            BatterySize.LR6,
            "TestBrand");
    }

    #endregion
}
