import { useState, useEffect } from "react";
import { toast } from "react-hot-toast";
import { PlusIcon, CubeIcon } from "@heroicons/react/24/outline";
import type { ProductDto } from "../types/product.types";
import type {
  AromaBombelProductDto,
  AromaBottleProductDto,
  AromaDeviceProductDto,
  SanitizingDeviceProductDto,
  BatteryProductDto,
} from "../types/product.types";
import productService from "../services/productService";
import Button from "../components/common/Button";
import ConfirmationDialog from "../components/common/ConfirmationDialog";
import ProductStats from "../components/products/ProductStats";
import ProductFilters from "../components/products/ProductFilters";
import ProductCard from "../components/products/ProductCard";
import ProductTable from "../components/products/ProductTable";
import ProductTypeModal from "../components/products/ProductTypeModal";
import AromaBombelForm from "../components/products/AromaBombelForm";
import AromaBottleForm from "../components/products/AromaBottleForm";
import AromaDeviceForm from "../components/products/AromaDeviceForm";
import SanitizingDeviceForm from "../components/products/SanitizingDeviceForm";
import BatteryForm from "../components/products/BatteryForm";
import StockManagementModal from "../components/products/StockManagementModal";

export default function ProductsPage() {
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [filteredProducts, setFilteredProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState("");
  const [filterType, setFilterType] = useState(0); // 0 = All
  const [viewMode, setViewMode] = useState<"grid" | "table">("grid");

  // Modal states
  const [showTypeModal, setShowTypeModal] = useState(false);
  const [showFormModal, setShowFormModal] = useState(false);
  const [showDeleteDialog, setShowDeleteDialog] = useState(false);
  const [showStockModal, setShowStockModal] = useState(false);

  // Selected items
  const [selectedProduct, setSelectedProduct] = useState<ProductDto | null>(
    null
  );
  const [selectedProductType, setSelectedProductType] = useState<number | null>(
    null
  );
  const [selectedProductForStock, setSelectedProductForStock] =
    useState<ProductDto | null>(null);

  // Load products on mount
  useEffect(() => {
    loadProducts();
  }, []);

  // Filter products when search/filter changes
  useEffect(() => {
    let filtered = products;

    // Filter by type
    if (filterType > 0) {
      filtered = filtered.filter((p) => p.productTypeId === filterType);
    }

    // Filter by search term
    if (searchTerm) {
      const term = searchTerm.toLowerCase();
      filtered = filtered.filter(
        (p) =>
          p.name.toLowerCase().includes(term) ||
          (p.description && p.description.toLowerCase().includes(term))
      );
    }

    setFilteredProducts(filtered);
  }, [products, filterType, searchTerm]);

  const loadProducts = async () => {
    setLoading(true);
    try {
      const data = await productService.getAllProducts();
      setProducts(data);
    } catch {
      toast.error("Failed to load products");
    } finally {
      setLoading(false);
    }
  };

  const handleAddClick = () => {
    setSelectedProduct(null);
    setSelectedProductType(null);
    setShowTypeModal(true);
  };

  const handleSelectType = (typeId: number) => {
    setSelectedProductType(typeId);
    setShowTypeModal(false);
    setShowFormModal(true);
  };

  const handleEdit = (product: ProductDto) => {
    setSelectedProduct(product);
    setSelectedProductType(product.productTypeId);
    setShowFormModal(true);
  };

  const handleDelete = (product: ProductDto) => {
    setSelectedProduct(product);
    setShowDeleteDialog(true);
  };

  const confirmDelete = async () => {
    if (!selectedProduct) return;

    try {
      await productService.deleteProduct(selectedProduct.id);
      toast.success("Product deleted successfully");
      loadProducts();
      setShowDeleteDialog(false);
    } catch (error: unknown) {
      // Check if the error has a message property (API error format)
      const errorMessage =
        error && typeof error === "object" && "message" in error
          ? String(error.message)
          : "Failed to delete product";

      toast.error(errorMessage);
    }
  };

  const handleFormSubmit = async (data: unknown) => {
    if (!selectedProductType) return;

    if (selectedProduct) {
      // Update
      await productService.updateProduct(
        selectedProductType,
        selectedProduct.id,
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        data as any
      );
    } else {
      // Create
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      await productService.createProduct(selectedProductType, data as any);
    }
    loadProducts();
    setShowFormModal(false);
  };

  const handleManageStock = (product: ProductDto) => {
    setSelectedProductForStock(product);
    setShowStockModal(true);
  };

  return (
    <div className="p-6">
      {/* Header */}
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Products</h1>
          <p className="text-gray-600 mt-1">Manage your inventory products</p>
        </div>
        <Button onClick={handleAddClick} className="flex items-center">
          <PlusIcon className="h-5 w-5 mr-2" />
          Add Product
        </Button>
      </div>

      {/* Stats */}
      <ProductStats products={products} />

      {/* Filters */}
      <ProductFilters
        searchTerm={searchTerm}
        onSearchChange={setSearchTerm}
        filterType={filterType}
        onTypeChange={setFilterType}
        viewMode={viewMode}
        onViewModeChange={setViewMode}
      />

      {/* Loading */}
      {loading && (
        <div className="flex justify-center py-12">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
        </div>
      )}

      {/* Empty State */}
      {!loading && filteredProducts.length === 0 && (
        <div className="text-center py-12">
          <CubeIcon className="mx-auto h-12 w-12 text-gray-400" />
          <h3 className="mt-2 text-sm font-medium text-gray-900">
            No products found
          </h3>
          <p className="mt-1 text-sm text-gray-500">
            Get started by creating a new product.
          </p>
          <div className="mt-6">
            <Button onClick={handleAddClick}>
              <PlusIcon className="h-5 w-5 mr-2" />
              Add Product
            </Button>
          </div>
        </div>
      )}

      {/* Grid View */}
      {!loading && viewMode === "grid" && filteredProducts.length > 0 && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6 mt-6">
          {filteredProducts.map((product) => (
            <ProductCard
              key={product.id}
              product={product}
              onEdit={handleEdit}
              onDelete={handleDelete}
              onManageStock={handleManageStock}
            />
          ))}
        </div>
      )}

      {/* Table View */}
      {!loading && viewMode === "table" && filteredProducts.length > 0 && (
        <div className="mt-6">
          <ProductTable
            products={filteredProducts}
            onEdit={handleEdit}
            onDelete={handleDelete}
            onManageStock={handleManageStock}
          />
        </div>
      )}

      {/* Modals */}
      <ProductTypeModal
        isOpen={showTypeModal}
        onClose={() => setShowTypeModal(false)}
        onSelectType={handleSelectType}
      />

      {/* Type-specific form modals */}
      {selectedProductType === 1 && (
        <AromaBombelForm
          isOpen={showFormModal}
          onClose={() => setShowFormModal(false)}
          product={selectedProduct as AromaBombelProductDto}
          onSubmit={handleFormSubmit}
        />
      )}
      {selectedProductType === 2 && (
        <AromaBottleForm
          isOpen={showFormModal}
          onClose={() => setShowFormModal(false)}
          product={selectedProduct as AromaBottleProductDto}
          onSubmit={handleFormSubmit}
        />
      )}
      {selectedProductType === 3 && (
        <AromaDeviceForm
          isOpen={showFormModal}
          onClose={() => setShowFormModal(false)}
          product={selectedProduct as AromaDeviceProductDto}
          onSubmit={handleFormSubmit}
        />
      )}
      {selectedProductType === 4 && (
        <SanitizingDeviceForm
          isOpen={showFormModal}
          onClose={() => setShowFormModal(false)}
          product={selectedProduct as SanitizingDeviceProductDto}
          onSubmit={handleFormSubmit}
        />
      )}
      {selectedProductType === 5 && (
        <BatteryForm
          isOpen={showFormModal}
          onClose={() => setShowFormModal(false)}
          product={selectedProduct as BatteryProductDto}
          onSubmit={handleFormSubmit}
        />
      )}

      {/* Delete Confirmation */}
      <ConfirmationDialog
        isOpen={showDeleteDialog}
        onClose={() => setShowDeleteDialog(false)}
        onConfirm={confirmDelete}
        title="Delete Product"
        message={`Are you sure you want to delete "${selectedProduct?.name}"? This action cannot be undone.`}
        confirmLabel="Delete"
        confirmVariant="danger"
      />

      {/* Stock Management Modal */}
      <StockManagementModal
        isOpen={showStockModal}
        onClose={() => setShowStockModal(false)}
        product={selectedProductForStock}
        onRefresh={loadProducts}
      />
    </div>
  );
}
