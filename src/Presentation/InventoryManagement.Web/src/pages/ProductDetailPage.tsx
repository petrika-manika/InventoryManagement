import { useParams, useNavigate } from "react-router-dom";
import Button from "../components/common/Button";
import { ArrowLeftIcon } from "@heroicons/react/24/outline";

export default function ProductDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  return (
    <div className="p-6">
      <Button
        variant="secondary"
        onClick={() => navigate("/products")}
        className="mb-6"
      >
        <ArrowLeftIcon className="h-5 w-5 mr-2" />
        Back to Products
      </Button>

      <div className="bg-white rounded-lg shadow p-8 text-center">
        <h1 className="text-2xl font-bold text-gray-900 mb-4">
          Product Detail Page
        </h1>
        <p className="text-gray-600 mb-2">Product ID: {id}</p>
        <p className="text-sm text-gray-500">
          This page will show full product details and stock history in a future
          update.
        </p>
      </div>
    </div>
  );
}
