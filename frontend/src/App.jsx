import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { Toaster } from 'react-hot-toast';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';

// Auth pages
import LoginPage from './pages/auth/LoginPage';
import SignupPage from './pages/auth/SignupPage';

// Role dashboards
import AdminDashboard from './pages/admin/AdminDashboard';
import StaffDashboard from './pages/staff/StaffDashboard';
import CustomerDashboard from './pages/customer/CustomerDashboard';

// Misc pages
import UnauthorizedPage from './pages/UnauthorizedPage';

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        {/* Global toast notifications */}
        <Toaster
          position="top-right"
          toastOptions={{
            duration: 4000,
            style: {
              borderRadius: '12px',
              padding: '14px 20px',
              fontSize: '14px',
              fontWeight: 500,
            },
            success: { style: { background: '#0d9488', color: '#fff' } },
            error: { style: { background: '#ef4444', color: '#fff' } },
          }}
        />

        <Routes>
          {/* Public routes */}
          <Route path="/" element={<Navigate to="/login" replace />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/signup" element={<SignupPage />} />
          <Route path="/unauthorized" element={<UnauthorizedPage />} />

          {/* Admin routes */}
          <Route
            path="/admin/dashboard"
            element={
              <ProtectedRoute allowedRoles={['Admin']}>
                <AdminDashboard />
              </ProtectedRoute>
            }
          />

          {/* Staff routes */}
          <Route
            path="/staff/dashboard"
            element={
              <ProtectedRoute allowedRoles={['Staff']}>
                <StaffDashboard />
              </ProtectedRoute>
            }
          />

          {/* Customer routes */}
          <Route
            path="/customer/dashboard"
            element={
              <ProtectedRoute allowedRoles={['Customer']}>
                <CustomerDashboard />
              </ProtectedRoute>
            }
          />

          {/* Catch-all */}
          <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}
