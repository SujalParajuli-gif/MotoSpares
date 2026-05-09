import { useState, useEffect } from 'react';
import { toast } from 'react-hot-toast';
import { getAllVendors, createVendor, updateVendor, deleteVendor } from '../../services/vendorService';
import DashboardLayout from '../../components/DashboardLayout';

const emptyForm = { vendorName: '', vendorEmail: '', vendorPhone: '', vendorAddress: '' };

export default function ManageVendors() {
  const [vendors, setVendors] = useState([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingVendor, setEditingVendor] = useState(null);
  const [form, setForm] = useState(emptyForm);

  useEffect(() => {
    fetchVendors();
  }, []);

  const fetchVendors = async () => {
    try {
      setLoading(true);
      const res = await getAllVendors();
      if (res.isSuccess) {
        setVendors(res.data);
      }
    } catch (error) {
      toast.error('Failed to fetch vendors');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const openModal = (vendor = null) => {
    if (vendor) {
      setEditingVendor(vendor);
      setForm({
        vendorName: vendor.vendorName,
        vendorEmail: vendor.vendorEmail || '',
        vendorPhone: vendor.vendorPhone || '',
        vendorAddress: vendor.vendorAddress || '',
      });
    } else {
      setEditingVendor(null);
      setForm(emptyForm);
    }
    setIsModalOpen(true);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      let res;
      if (editingVendor) {
        res = await updateVendor(editingVendor.vendorId, form);
      } else {
        res = await createVendor(form);
      }

      if (res.isSuccess) {
        toast.success(editingVendor ? 'Vendor updated' : 'Vendor added');
        setIsModalOpen(false);
        fetchVendors();
      } else {
        toast.error(res.message || 'Operation failed');
      }
    } catch (error) {
      toast.error(error.response?.data?.message || 'An error occurred');
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this vendor?')) return;
    try {
      const res = await deleteVendor(id);
      if (res.isSuccess) {
        toast.success('Vendor deleted');
        fetchVendors();
      } else {
        toast.error(res.message || 'Failed to delete');
      }
    } catch (error) {
      toast.error('Error deleting vendor');
    }
  };

  return (
    <DashboardLayout>
      <div className="mb-8 flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold text-gray-800">Manage Vendors</h1>
          <p className="text-gray-500 mt-1">Add and manage your parts suppliers.</p>
        </div>
        <button
          onClick={() => openModal()}
          className="bg-primary text-white px-4 py-2 rounded-lg hover:bg-primary-dark transition-colors shadow-md"
        >
          + Add Vendor
        </button>
      </div>

      <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
        {loading ? (
          <div className="p-8 text-center text-gray-500">Loading vendors...</div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full text-left">
              <thead className="bg-gray-50 border-b border-gray-100">
                <tr>
                  <th className="px-6 py-4 text-sm font-semibold text-gray-600">Vendor Name</th>
                  <th className="px-6 py-4 text-sm font-semibold text-gray-600">Email</th>
                  <th className="px-6 py-4 text-sm font-semibold text-gray-600">Phone</th>
                  <th className="px-6 py-4 text-sm font-semibold text-gray-600">Address</th>
                  <th className="px-6 py-4 text-sm font-semibold text-gray-600 text-right">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-50">
                {vendors.map((vendor) => (
                  <tr key={vendor.vendorId} className="hover:bg-gray-50/50 transition-colors">
                    <td className="px-6 py-4 font-medium text-gray-800">{vendor.vendorName}</td>
                    <td className="px-6 py-4 text-gray-600">{vendor.vendorEmail || '-'}</td>
                    <td className="px-6 py-4 text-gray-600">{vendor.vendorPhone || '-'}</td>
                    <td className="px-6 py-4 text-gray-600">{vendor.vendorAddress || '-'}</td>
                    <td className="px-6 py-4 text-right space-x-3">
                      <button
                        onClick={() => openModal(vendor)}
                        className="text-blue-600 hover:text-blue-800 font-medium text-sm"
                      >
                        Edit
                      </button>
                      <button
                        onClick={() => handleDelete(vendor.vendorId)}
                        className="text-red-600 hover:text-red-800 font-medium text-sm"
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                ))}
                {vendors.length === 0 && (
                  <tr>
                    <td colSpan="5" className="px-6 py-8 text-center text-gray-400">
                      No vendors found. Add one to get started.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {/* Modal */}
      {isModalOpen && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-xl shadow-xl w-full max-w-md overflow-hidden">
            <div className="p-6 border-b border-gray-100">
              <h2 className="text-xl font-bold text-gray-800">
                {editingVendor ? 'Edit Vendor' : 'Add New Vendor'}
              </h2>
            </div>
            <form onSubmit={handleSubmit} className="p-6 space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Vendor Name *</label>
                <input
                  required
                  type="text"
                  name="vendorName"
                  value={form.vendorName}
                  onChange={handleChange}
                  className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
                <input
                  type="email"
                  name="vendorEmail"
                  value={form.vendorEmail}
                  onChange={handleChange}
                  className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Phone</label>
                <input
                  type="text"
                  name="vendorPhone"
                  value={form.vendorPhone}
                  onChange={handleChange}
                  className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Address</label>
                <textarea
                  name="vendorAddress"
                  value={form.vendorAddress}
                  onChange={handleChange}
                  rows="3"
                  className="w-full p-2 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none"
                ></textarea>
              </div>
              <div className="flex justify-end space-x-3 pt-2">
                <button
                  type="button"
                  onClick={() => setIsModalOpen(false)}
                  className="px-4 py-2 text-gray-600 hover:bg-gray-100 rounded-lg transition-colors"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="px-4 py-2 bg-primary text-white rounded-lg hover:bg-primary-dark transition-colors shadow-md"
                >
                  {editingVendor ? 'Update Vendor' : 'Add Vendor'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </DashboardLayout>
  );
}
