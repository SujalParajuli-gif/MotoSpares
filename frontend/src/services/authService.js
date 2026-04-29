import api from './api';

/**
 * POST /api/auth/login
 * Backend expects: { email, password }
 * Backend returns: { isSuccess, message, data: { userId, fullName, email, role, token } }
 */
export const loginUser = async (email, password) => {
  const response = await api.post('/auth/login', { email, password });
  return response.data;
};

/**
 * POST /api/auth/register
 * Backend expects: { fullName, email, password, phone?, address? }
 * Backend returns: { isSuccess, message, data: { userId, fullName, email, role, token } }
 */
export const registerUser = async (payload) => {
  const response = await api.post('/auth/register', payload);
  return response.data;
};
