import { ClockIcon } from "@heroicons/react/24/outline";
import type { StockHistoryDto } from "../../types/product.types";
import { formatDateTime } from "../../utils/formatters";
import {
  Table,
  TableHeader,
  TableBody,
  TableRow,
  TableHead,
  TableCell,
} from "../common/Table";
import Badge from "../common/Badge";
import LoadingSpinner from "../common/LoadingSpinner";

interface StockHistoryTableProps {
  history: StockHistoryDto[];
  loading?: boolean;
}

export default function StockHistoryTable({
  history,
  loading = false,
}: StockHistoryTableProps) {
  const getChangeTypeVariant = (
    changeType: string
  ): "success" | "danger" | "info" => {
    const variants: Record<string, "success" | "danger" | "info"> = {
      Added: "success",
      Removed: "danger",
      Adjusted: "info",
    };
    return variants[changeType] || "info";
  };

  const formatQuantityChange = (quantity: number) => {
    if (quantity > 0) {
      return <span className="text-green-600 font-medium">+{quantity}</span>;
    } else if (quantity < 0) {
      return <span className="text-red-600 font-medium">{quantity}</span>;
    }
    return <span className="text-gray-600">0</span>;
  };

  if (loading) {
    return (
      <div className="flex justify-center py-12">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  if (history.length === 0) {
    return (
      <div className="text-center py-12 bg-gray-50 rounded-lg">
        <ClockIcon className="mx-auto h-12 w-12 text-gray-400" />
        <h3 className="mt-2 text-sm font-medium text-gray-900">
          No stock history
        </h3>
        <p className="mt-1 text-sm text-gray-500">
          Stock changes will appear here once they are made.
        </p>
      </div>
    );
  }

  return (
    <div className="bg-white rounded-lg shadow">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Date & Time</TableHead>
            <TableHead>Product Name</TableHead>
            <TableHead>Change Type</TableHead>
            <TableHead>Quantity Change</TableHead>
            <TableHead>Quantity After</TableHead>
            <TableHead>Reason</TableHead>
            <TableHead>Changed By</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {history.map((item) => (
            <TableRow key={item.id}>
              <TableCell className="text-sm text-gray-900">
                {formatDateTime(item.changedAt)}
              </TableCell>
              <TableCell className="text-sm font-medium text-gray-900">
                {item.productName}
              </TableCell>
              <TableCell>
                <Badge
                  variant={getChangeTypeVariant(item.changeType)}
                  size="sm"
                >
                  {item.changeType}
                </Badge>
              </TableCell>
              <TableCell className="text-sm">
                {formatQuantityChange(item.quantityChanged)}
              </TableCell>
              <TableCell className="text-sm text-gray-900">
                {item.quantityAfter}
              </TableCell>
              <TableCell className="text-sm text-gray-600">
                {item.reason || "-"}
              </TableCell>
              <TableCell className="text-sm text-gray-600">
                {item.changedByName}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}
