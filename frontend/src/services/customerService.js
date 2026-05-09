import api from './api';

export const registerCustomerWithVehicle = async (data) => {
  try {
    const response = await api.post('/customers/register-with-vehicle', data);
    return response.data;
  } catch (error) {
    throw error;
  }
};
