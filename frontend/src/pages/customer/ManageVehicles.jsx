import { useState, useEffect } from 'react';
import { toast } from 'react-hot-toast';
import { getMyVehicles, addVehicle, updateVehicle, deleteVehicle } from '../../services/vehicleService';
import DashboardLayout from '../../components/DashboardLayout';

const emptyForm = { vehicleNumber: '', make: '', model: '', year: new Date().getFullYear() };

export default function ManageVehicles() {
  const [vehicles, setVehicles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [currentVehicle, setCurrentVehicle] = useState(null);
  const [form, setForm] = useState(emptyForm);

  useEffect(() => { fetchVehicles(); }, []);

  const fetchVehicles = async () => {
    try {
      setLoading(true);
      const res = await getMyVehicles();
      if (res.isSuccess) setVehicles(res.data);
    } catch {
      toast.error('Failed to load vehicles');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const openAdd = () => { setForm(emptyForm); setIsAddModalOpen(true); };
  const openEdit = (v) => {
    setCurrentVehicle(v);
    setForm({ vehicleNumber: v.vehicleNumber, make: v.make, model: v.model, year: v.year });
    setIsEditModalOpen(true);
  };

  const handleAdd = async (e) => {
    e.preventDefault();
    try {
      const res = await addVehicle({ ...form, year: parseInt(form.year) });
      if (res.isSuccess) {
        toast.success('Vehicle added!');
        setIsAddModalOpen(false);
        fetchVehicles();
      } else {
        toast.error(res.message || 'Failed to add vehicle');
      }
    } catch (err) {
      toast.error(err.response?.data?.message || 'Error adding vehicle');
    }
  };

  const handleEdit = async (e) => {
    e.preventDefault();
    try {
      const res = await updateVehicle(currentVehicle.vehicleId, { ...form, year: parseInt(form.year) });
      if (res.isSuccess) {
        toast.success('Vehicle updated!');
        setIsEditModalOpen(false);
        fetchVehicles();
      } else {
        toast.error(res.message || 'Failed to update vehicle');
      }
    } catch (err) {
      toast.error(err.response?.data?.message || 'Error updating vehicle');
    }
  };

  const handleDelete = async (vehicleId) => {
    if (!window.confirm('Remove this vehicle?')) return;
    try {
      const res = await deleteVehicle(vehicleId);
      if (res.isSuccess) {
        toast.success('Vehicle removed');
        fetchVehicles();
      } else {
        toast.error(res.message || 'Failed to remove vehicle');
      }
    } catch {
      toast.error('Error removing vehicle');
    }
  };

  const VehicleForm = ({ onSubmit, submitLabel }) => (
    <form onSubmit={onSubmit} className="space-y-4">
      <div>
        <label className="block text-sm font-medium text-gray-700 mb-1">License Plate / Vehicle Number *</label>
        <input required type="text" name="vehicleNumber" value={form.vehicleNumber} onChange={handleChange}
          className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
      </div>
      <div className="grid grid-cols-2 gap-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Make *</label>
          <input required type="text" name="make" value={form.make} onChange={handleChange}
            className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none"
            placeholder="e.g. Honda" />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Model *</label>
          <input required type="text" name="model" value={form.model} onChange={handleChange}
            className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none"
            placeholder="e.g. Civic" />
        </div>
      </div>
      <div>
        <label className="block text-sm font-medium text-gray-700 mb-1">Year *</label>
        <input required type="number" min="1900" max="2100" name="year" value={form.year} onChange={handleChange}
          className="w-full p-2.5 border rounded-lg focus:ring-2 focus:ring-primary focus:outline-none" />
      </div>
      <div className="flex justify-end space-x-3 pt-2">
        <button type="button"
          onClick={() => { setIsAddModalOpen(false); setIsEditModalOpen(false); }}
          className="px-4 py-2 text-gray-600 hover:bg-gray-100 rounded-lg">
          Cancel
        </button>
        <button type="submit" className="px-4 py-2 bg-primary text-white rounded-lg hover:bg-primary-dark">
          {submitLabel}
        </button>
      </div>
    </form>
  );

  return (
    <DashboardLayout>
      <div className="mb-8 flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold text-gray-800">My Vehicles</h1>
          <p className="text-gray-500 mt-1">Manage the vehicles registered under your account.</p>
        </div>
        <button onClick={openAdd}
          className="bg-primary text-white px-4 py-2 rounded-lg hover:bg-primary-dark transition-colors shadow-md">
          + Add Vehicle
        </button>
      </div>

      {/* Vehicle Cards Grid */}
      {loading ? (
        <p className="text-gray-500 text-center py-12">Loading vehicles...</p>
      ) : vehicles.length === 0 ? (
        <div className="text-center py-16 bg-white rounded-xl border border-gray-100 shadow-sm">
          <div className="text-5xl mb-4">🚗</div>
          <h3 className="text-xl font-semibold text-gray-700 mb-2">No vehicles yet</h3>
          <p className="text-gray-400 mb-6">Add your first vehicle to get started.</p>
          <button onClick={openAdd}
            className="bg-primary text-white px-6 py-2.5 rounded-lg hover:bg-primary-dark">
            + Add Vehicle
          </button>
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {vehicles.map((v) => (
            <div key={v.vehicleId}
              className="bg-white rounded-2xl border border-gray-100 shadow-sm hover:shadow-lg transition-shadow p-6 flex flex-col justify-between">
              <div>
                <div className="flex items-center justify-between mb-4">
                  <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-teal-50 text-teal-700 border border-teal-200">
                    {v.vehicleNumber}
                  </span>
                  <span className="text-gray-400 text-sm">{v.year}</span>
                </div>
                <h3 className="text-xl font-bold text-gray-800">{v.make}</h3>
                <p className="text-gray-500 text-base">{v.model}</p>
              </div>
              <div className="flex space-x-3 mt-6">
                <button onClick={() => openEdit(v)}
                  className="flex-1 py-2 text-sm border border-blue-200 text-blue-600 rounded-lg hover:bg-blue-50 transition-colors font-medium">
                  Edit
                </button>
                <button onClick={() => handleDelete(v.vehicleId)}
                  className="flex-1 py-2 text-sm border border-red-200 text-red-600 rounded-lg hover:bg-red-50 transition-colors font-medium">
                  Remove
                </button>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Add Modal */}
      {isAddModalOpen && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-xl shadow-xl w-full max-w-md p-6">
            <h2 className="text-2xl font-bold text-gray-800 mb-5">Add New Vehicle</h2>
            <VehicleForm onSubmit={handleAdd} submitLabel="Add Vehicle" />
          </div>
        </div>
      )}

      {/* Edit Modal */}
      {isEditModalOpen && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-xl shadow-xl w-full max-w-md p-6">
            <h2 className="text-2xl font-bold text-gray-800 mb-5">Edit Vehicle</h2>
            <VehicleForm onSubmit={handleEdit} submitLabel="Save Changes" />
          </div>
        </div>
      )}
    </DashboardLayout>
  );
}
