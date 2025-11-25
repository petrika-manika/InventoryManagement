import type { ReactNode } from "react";

/**
 * Table component props
 */
export interface TableProps {
  children: ReactNode;
  className?: string;
}

/**
 * TableHeader component props
 */
export interface TableHeaderProps {
  children: ReactNode;
}

/**
 * TableBody component props
 */
export interface TableBodyProps {
  children: ReactNode;
}

/**
 * TableRow component props
 */
export interface TableRowProps {
  children: ReactNode;
  onClick?: () => void;
  className?: string;
}

/**
 * TableHead component props
 */
export interface TableHeadProps {
  children: ReactNode;
  className?: string;
}

/**
 * TableCell component props
 */
export interface TableCellProps {
  children: ReactNode;
  className?: string;
  colSpan?: number;
}

/**
 * Table Component
 * Main table wrapper with base styling matching ProductTable design
 *
 * @example
 * ```tsx
 * <Table>
 *   <TableHeader>
 *     <TableRow>
 *       <TableHead>Name</TableHead>
 *       <TableHead>Email</TableHead>
 *     </TableRow>
 *   </TableHeader>
 *   <TableBody>
 *     <TableRow>
 *       <TableCell>John Doe</TableCell>
 *       <TableCell>john@example.com</TableCell>
 *     </TableRow>
 *   </TableBody>
 * </Table>
 * ```
 */
export const Table = ({ children, className = "" }: TableProps) => {
  return (
    <div className="overflow-x-auto">
      <table className={`min-w-full divide-y divide-gray-200 ${className}`}>
        {children}
      </table>
    </div>
  );
};

/**
 * TableHeader Component
 * Wrapper for table header rows
 */
export const TableHeader = ({ children }: TableHeaderProps) => {
  return <thead className="bg-gray-50">{children}</thead>;
};

/**
 * TableBody Component
 * Wrapper for table body rows with divider styling
 */
export const TableBody = ({ children }: TableBodyProps) => {
  return (
    <tbody className="bg-white divide-y divide-gray-200">{children}</tbody>
  );
};

/**
 * TableRow Component
 * Table row with hover effect and optional click handler
 */
export const TableRow = ({
  children,
  onClick,
  className = "",
}: TableRowProps) => {
  const baseClasses = "hover:bg-gray-50 transition-colors";
  const cursorClass = onClick ? "cursor-pointer" : "";

  return (
    <tr
      onClick={onClick}
      className={`${baseClasses} ${cursorClass} ${className}`}
    >
      {children}
    </tr>
  );
};

/**
 * TableHead Component
 * Table header cell with styling matching ProductTable
 */
export const TableHead = ({ children, className = "" }: TableHeadProps) => {
  return (
    <th
      scope="col"
      className={`px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider ${className}`}
    >
      {children}
    </th>
  );
};

/**
 * TableCell Component
 * Table data cell with styling
 */
export const TableCell = ({
  children,
  className = "",
  colSpan,
}: TableCellProps) => {
  return (
    <td
      className={`px-6 py-4 whitespace-nowrap ${className}`}
      colSpan={colSpan}
    >
      {children}
    </td>
  );
};
