/**
 * Client Stats Component
 * Displays aggregated statistics about clients
 */

import {
  UsersIcon,
  UserIcon,
  BuildingOfficeIcon,
  CheckCircleIcon,
} from "@heroicons/react/24/outline";
import type { ClientDto } from "../../types/client.types";

interface ClientStatsProps {
  clients: ClientDto[];
}

interface StatCardProps {
  icon: React.ComponentType<{ className?: string }>;
  iconBgColor: string;
  iconColor: string;
  label: string;
  value: number;
}

function StatCard({
  icon: Icon,
  iconBgColor,
  iconColor,
  label,
  value,
}: StatCardProps) {
  return (
    <div className="bg-white rounded-lg shadow p-6 flex items-center gap-4">
      <div className={`p-3 rounded-full ${iconBgColor}`}>
        <Icon className={`h-6 w-6 ${iconColor}`} />
      </div>
      <div>
        <p className="text-sm font-medium text-gray-600">{label}</p>
        <p className="text-2xl font-bold text-gray-900">{value}</p>
      </div>
    </div>
  );
}

export default function ClientStats({ clients }: ClientStatsProps) {
  // Calculate statistics
  const totalClients = clients.length;
  const individualClients = clients.filter(
    (client) => client.clientTypeId === 1
  ).length;
  const businessClients = clients.filter(
    (client) => client.clientTypeId === 2
  ).length;
  const activeClients = clients.filter((client) => client.isActive).length;

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6">
      <StatCard
        icon={UsersIcon}
        iconBgColor="bg-purple-100"
        iconColor="text-purple-600"
        label="Total Clients"
        value={totalClients}
      />
      <StatCard
        icon={UserIcon}
        iconBgColor="bg-green-100"
        iconColor="text-green-600"
        label="Individual"
        value={individualClients}
      />
      <StatCard
        icon={BuildingOfficeIcon}
        iconBgColor="bg-blue-100"
        iconColor="text-blue-600"
        label="Business"
        value={businessClients}
      />
      <StatCard
        icon={CheckCircleIcon}
        iconBgColor="bg-emerald-100"
        iconColor="text-emerald-600"
        label="Active"
        value={activeClients}
      />
    </div>
  );
}
