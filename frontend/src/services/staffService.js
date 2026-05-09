import api from './api';

export const getAllStaff = async () => {
  try {
    const response = await api.get('/staff');
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getStaffById = async (id) => {
  try {
    const response = await api.get(`/staff/${id}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const addStaff = async (staffData) => {
  try {
    const response = await api.post('/staff', staffData);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const updateStaff = async (id, staffData) => {
  try {
    const response = await api.put(`/staff/${id}`, staffData);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const deleteStaff = async (id) => {
  try {
    const response = await api.delete(`/staff/${id}`);
    return response.data;
  } catch (error) {
    throw error;
  }
};
