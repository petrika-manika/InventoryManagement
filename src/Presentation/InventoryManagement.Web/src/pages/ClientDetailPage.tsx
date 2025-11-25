/**
 * Client Detail Page
 * Placeholder page for viewing full client details
 */

import { useParams, useNavigate } from "react-router-dom";
import Button from "../components/common/Button";
import { ArrowLeftIcon } from "@heroicons/react/24/outline";

export default function ClientDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  return (
    <div className="p-6">
      <Button
        variant="secondary"
        onClick={() => navigate("/clients")}
        className="mb-6 inline-flex items-center"
      >
        <ArrowLeftIcon className="h-5 w-5 mr-2" />
        Back to Clients
      </Button>

      <div className="bg-white rounded-lg shadow p-8 text-center">
        <h1 className="text-2xl font-bold text-gray-900 mb-4">
          Client Detail Page
        </h1>
        <p className="text-gray-600 mb-2">Client ID: {id}</p>
        <p className="text-sm text-gray-500">
          This page will show full client details in a future update.
        </p>
      </div>
    </div>
  );
}
