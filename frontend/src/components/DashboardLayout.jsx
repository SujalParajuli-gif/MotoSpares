import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import toast from 'react-hot-toast';

export default function DashboardLayout({ children }) {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    toast.success('Logged out successfully');
    navigate('/login');
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Top navbar */}
      <header className="bg-white border-b border-gray-100 shadow-sm">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 flex items-center justify-between h-16">
          <Link to="/" className="text-xl font-bold text-primary tracking-tight">
            MotoSpares
          </Link>

          <div className="flex items-center gap-4">
            {/* Role badge */}
            <span className="hidden sm:inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs font-semibold bg-primary/10 text-primary">
              {user?.role}
            </span>

            {/* User name */}
            <span className="text-sm font-medium text-gray-700">
              {user?.fullName}
            </span>

            {/* Logout */}
            <button
              onClick={handleLogout}
              className="px-4 py-1.5 rounded-full text-sm font-medium text-red-600 border border-red-200 hover:bg-red-50 cursor-pointer"
            >
              Logout
            </button>
          </div>
        </div>
      </header>

      {/* Main content */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {children}
      </main>
    </div>
  );
}
