import { Link } from 'react-router-dom';

export default function UnauthorizedPage() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-teal-50 via-white to-cyan-50">
      <div className="text-center p-10">
        <h1 className="text-4xl font-bold text-gray-800 mb-2">Access Denied</h1>
        <p className="text-gray-500 mb-6">You don't have permission to view this page.</p>
        <Link
          to="/login"
          className="inline-block px-6 py-3 rounded-full bg-primary text-white font-semibold hover:bg-primary-dark shadow-lg shadow-primary/25"
        >
          Back to Login
        </Link>
      </div>
    </div>
  );
}
