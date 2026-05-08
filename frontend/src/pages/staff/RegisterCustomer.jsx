import { useState } from 'react';
import { toast } from 'react-hot-toast';
import { useNavigate } from 'react-router-dom';
import { registerCustomerWithVehicle } from '../../services/customerService';
import DashboardLayout from '../../components/DashboardLayout';

export default function RegisterCustomer() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [form, setForm] = useState({
    fullName: '',
    email: '',
    password: '',
    phone: '',
    address: '',
    vehicleNumber: '',
    make: '',
    model: '',
    year: new Date().getFullYear(),
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      const payload = {
        ...form,
        year: parseInt(form.year, 10)
      };
      const res = await registerCustomerWithVehicle(payload);
      if (res.isSuccess) {
        toast.success('Customer and Vehicle registered successfully!');
        navigate('/staff/dashboard');
      } else {
        toast.error(res.message || 'Registration failed');
      }
    } catch (error) {
      toast.error(error.response?.data?.message || 'Error registering customer');
    } finally {
      setLoading(false);
    }
  };

  return (
    <DashboardLayout>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-800">Register Customer</h1>
        <p className="text-gray-500 mt-1">Create a new customer account and add their vehicle details.</p>
      </div>

      <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-8 max-w-4xl">
        <form onSubmit={handleSubmit} className="space-y-8">
          
          {/* Customer Details Section */}
          <div>
            <h2 className="text-xl font-semibold text-gray-800 mb-4 border-b pb-2">Customer Details</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Full Name *</label>
                <input required type="text" name="fullName" value={form.fullName} onChange={handleChange} className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Email *</label>
                <input required type="email" name="email" value={form.email} onChange={handleChange} className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Temporary Password *</label>
                <input required type="password" name="password" value={form.password} onChange={handleChange} className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Phone</label>
                <input type="text" name="phone" value={form.phone} onChange={handleChange} className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div className="md:col-span-2">
                <label className="block text-sm font-medium text-gray-700 mb-1">Address</label>
                <input type="text" name="address" value={form.address} onChange={handleChange} className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
            </div>
          </div>

          {/* Vehicle Details Section */}
          <div>
            <h2 className="text-xl font-semibold text-gray-800 mb-4 border-b pb-2">Vehicle Details</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Vehicle Number (License Plate) *</label>
                <input required type="text" name="vehicleNumber" value={form.vehicleNumber} onChange={handleChange} className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Make (e.g., Honda) *</label>
                <input required type="text" name="make" value={form.make} onChange={handleChange} className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Model (e.g., Civic) *</label>
                <input required type="text" name="model" value={form.model} onChange={handleChange} className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Year *</label>
                <input required type="number" min="1900" max="2100" name="year" value={form.year} onChange={handleChange} className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
            </div>
          </div>

          <div className="flex justify-end pt-4">
            <button 
              type="submit" 
              disabled={loading}
              className="bg-primary text-white px-8 py-3 rounded-lg hover:bg-primary-dark transition-colors font-medium text-lg disabled:opacity-70 flex items-center shadow-lg shadow-primary/30"
            >
              {loading ? 'Registering...' : 'Register Customer'}
            </button>
          </div>
        </form>
      </div>
    </DashboardLayout>
  );
}
