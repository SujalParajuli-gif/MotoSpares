import { useState, useEffect } from 'react';
import { toast } from 'react-hot-toast';
import { getAllStaff, addStaff, updateStaff, deleteStaff } from '../../services/staffService';
import DashboardLayout from '../../components/DashboardLayout';

export default function ManageStaff() {
  const [staffList, setStaffList] = useState([]);
  const [loading, setLoading] = useState(true);
  
  // Modals state
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [currentStaff, setCurrentStaff] = useState(null);

  // Form state
  const [form, setForm] = useState({
    fullName: '',
    email: '',
    password: '',
    phone: '',
    address: ''
  });

  useEffect(() => {
    fetchStaff();
  }, []);

  const fetchStaff = async () => {
    try {
      setLoading(true);
      const res = await getAllStaff();
      if (res.isSuccess) {
        setStaffList(res.data);
      }
    } catch (error) {
      toast.error('Failed to load staff list');
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const openAddModal = () => {
    setForm({ fullName: '', email: '', password: '', phone: '', address: '' });
    setIsAddModalOpen(true);
  };

  const openEditModal = (staff) => {
    setCurrentStaff(staff);
    setForm({
      fullName: staff.fullName,
      email: staff.email, // Read-only in edit
      phone: staff.phone || '',
      address: staff.address || ''
    });
    setIsEditModalOpen(true);
  };

  const handleAddSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await addStaff(form);
      if (res.isSuccess) {
        toast.success('Staff added successfully');
        setIsAddModalOpen(false);
        fetchStaff();
      } else {
        toast.error(res.message || 'Failed to add staff');
      }
    } catch (error) {
      toast.error(error.response?.data?.message || 'Error adding staff');
    }
  };

  const handleEditSubmit = async (e) => {
    e.preventDefault();
    try {
      const updateData = {
        fullName: form.fullName,
        phone: form.phone,
        address: form.address
      };
      const res = await updateStaff(currentStaff.id, updateData);
      if (res.isSuccess) {
        toast.success('Staff updated successfully');
        setIsEditModalOpen(false);
        fetchStaff();
      } else {
        toast.error(res.message || 'Failed to update staff');
      }
    } catch (error) {
      toast.error(error.response?.data?.message || 'Error updating staff');
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this staff member?')) {
      try {
        const res = await deleteStaff(id);
        if (res.isSuccess) {
          toast.success('Staff deleted successfully');
          fetchStaff();
        } else {
          toast.error(res.message || 'Failed to delete staff');
        }
      } catch (error) {
        toast.error('Error deleting staff');
      }
    }
  };

  return (
    <DashboardLayout>
      <div className="mb-8 flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold text-gray-800">Manage Staff</h1>
          <p className="text-gray-500 mt-1">Add, edit, or remove staff accounts</p>
        </div>
        <button 
          onClick={openAddModal}
          className="bg-primary text-white px-4 py-2 rounded-lg hover:bg-primary-dark transition-colors shadow-md"
        >
          + Add New Staff
        </button>
      </div>

      {/* Staff Table */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-gray-50 text-gray-500 text-sm uppercase tracking-wider">
                <th className="px-6 py-4 font-medium">Name</th>
                <th className="px-6 py-4 font-medium">Email</th>
                <th className="px-6 py-4 font-medium">Phone</th>
                <th className="px-6 py-4 font-medium">Address</th>
                <th className="px-6 py-4 font-medium text-right">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {loading ? (
                <tr>
                  <td colSpan="5" className="px-6 py-8 text-center text-gray-500">
                    Loading staff data...
                  </td>
                </tr>
              ) : staffList.length === 0 ? (
                <tr>
                  <td colSpan="5" className="px-6 py-8 text-center text-gray-500">
                    No staff accounts found.
                  </td>
                </tr>
              ) : (
                staffList.map((staff) => (
                  <tr key={staff.id} className="hover:bg-gray-50 transition-colors">
                    <td className="px-6 py-4 font-medium text-gray-800">{staff.fullName}</td>
                    <td className="px-6 py-4 text-gray-600">{staff.email}</td>
                    <td className="px-6 py-4 text-gray-600">{staff.phone || '-'}</td>
                    <td className="px-6 py-4 text-gray-600">{staff.address || '-'}</td>
                    <td className="px-6 py-4 text-right space-x-3">
                      <button 
                        onClick={() => openEditModal(staff)}
                        className="text-blue-600 hover:text-blue-800 font-medium"
                      >
                        Edit
                      </button>
                      <button 
                        onClick={() => handleDelete(staff.id)}
                        className="text-red-600 hover:text-red-800 font-medium"
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>

      {/* Add Staff Modal */}
      {isAddModalOpen && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-xl shadow-xl w-full max-w-md p-6">
            <h2 className="text-2xl font-bold text-gray-800 mb-4">Add New Staff</h2>
            <form onSubmit={handleAddSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Full Name</label>
                <input required type="text" name="fullName" value={form.fullName} onChange={handleInputChange} className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
                <input required type="email" name="email" value={form.email} onChange={handleInputChange} className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Password</label>
                <input required type="password" name="password" value={form.password} onChange={handleInputChange} className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Phone</label>
                <input type="text" name="phone" value={form.phone} onChange={handleInputChange} className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Address</label>
                <input type="text" name="address" value={form.address} onChange={handleInputChange} className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div className="flex justify-end space-x-3 mt-6">
                <button type="button" onClick={() => setIsAddModalOpen(false)} className="px-4 py-2 text-gray-600 hover:bg-gray-100 rounded-lg">Cancel</button>
                <button type="submit" className="px-4 py-2 bg-primary text-white rounded-lg hover:bg-primary-dark">Add Staff</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Edit Staff Modal */}
      {isEditModalOpen && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-xl shadow-xl w-full max-w-md p-6">
            <h2 className="text-2xl font-bold text-gray-800 mb-4">Edit Staff</h2>
            <form onSubmit={handleEditSubmit} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
                <input disabled type="email" value={form.email} className="w-full p-2 border rounded-lg bg-gray-100 text-gray-500 cursor-not-allowed" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Full Name</label>
                <input required type="text" name="fullName" value={form.fullName} onChange={handleInputChange} className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Phone</label>
                <input type="text" name="phone" value={form.phone} onChange={handleInputChange} className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Address</label>
                <input type="text" name="address" value={form.address} onChange={handleInputChange} className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
              </div>
              <div className="flex justify-end space-x-3 mt-6">
                <button type="button" onClick={() => setIsEditModalOpen(false)} className="px-4 py-2 text-gray-600 hover:bg-gray-100 rounded-lg">Cancel</button>
                <button type="submit" className="px-4 py-2 bg-primary text-white rounded-lg hover:bg-primary-dark">Save Changes</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </DashboardLayout>
  );
}

