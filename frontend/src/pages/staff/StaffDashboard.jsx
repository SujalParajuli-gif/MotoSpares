import { useAuth } from '../../context/AuthContext';
import DashboardLayout from '../../components/DashboardLayout';

export default function StaffDashboard() {
  const { user } = useAuth();

  const cards = [
    { title: 'Point of Sale', desc: 'Create new sale invoices', color: 'from-teal-500 to-emerald-600' },
    { title: 'Parts Lookup', desc: 'Search and check stock levels', color: 'from-amber-500 to-orange-600' },
    { title: 'Appointments', desc: 'View scheduled service appointments', color: 'from-blue-500 to-indigo-600' },
    { title: 'Part Requests', desc: 'Handle customer part requests', color: 'from-purple-500 to-violet-600' },
  ];

  return (
    <DashboardLayout>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-800">
          Staff Dashboard
        </h1>
        <p className="text-gray-500 mt-1">
          Welcome back, <span className="text-primary font-semibold">{user?.fullName}</span>
        </p>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
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
