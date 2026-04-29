import { useAuth } from '../../context/AuthContext';
import DashboardLayout from '../../components/DashboardLayout';

export default function AdminDashboard() {
  const { user } = useAuth();

  const cards = [
    { title: 'Manage Staff', desc: 'Add, edit or remove staff accounts', color: 'from-teal-500 to-emerald-600' },
    { title: 'Inventory', desc: 'View and manage spare parts stock', color: 'from-amber-500 to-orange-600' },
    { title: 'Purchase Orders', desc: 'Track vendor purchase invoices', color: 'from-blue-500 to-indigo-600' },
    { title: 'Sales Reports', desc: 'View sale analytics and revenue', color: 'from-purple-500 to-violet-600' },
    { title: 'Vendors', desc: 'Manage supplier relationships', color: 'from-rose-500 to-pink-600' },
    { title: 'Appointments', desc: 'View all service appointments', color: 'from-cyan-500 to-teal-600' },
  ];

  return (
    <DashboardLayout>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-800">
          Admin Dashboard
        </h1>
        <p className="text-gray-500 mt-1">
          Welcome back, <span className="text-primary font-semibold">{user?.fullName}</span>
        </p>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {cards.map((card) => (
          <div
            key={card.title}
            className="group relative overflow-hidden rounded-2xl bg-white border border-gray-100 shadow-sm hover:shadow-xl hover:-translate-y-1 cursor-pointer"
          >
            <div className={`absolute inset-0 bg-gradient-to-br ${card.color} opacity-0 group-hover:opacity-5`} />
            <div className="p-6">
              <h3 className="text-lg font-semibold text-gray-800 mt-3">{card.title}</h3>
              <p className="text-sm text-gray-400 mt-1">{card.desc}</p>
            </div>
          </div>
        ))}
      </div>
    </DashboardLayout>
  );
}
