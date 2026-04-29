import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import { registerUser } from '../../services/authService';
import { useAuth } from '../../context/AuthContext';

export default function SignupPage() {
  const navigate = useNavigate();
  const { login } = useAuth();

  const [form, setForm] = useState({
    fullName: '',
    email: '',
    password: '',
    confirmPassword: '',
    phone: '',
    address: '',
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const validate = () => {
    const errs = {};
    if (!form.fullName.trim()) errs.fullName = 'Full name is required';
    if (!form.email.trim()) errs.email = 'Email is required';
    else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email))
      errs.email = 'Enter a valid email address';
    if (!form.password) errs.password = 'Password is required';
    else if (form.password.length < 6)
      errs.password = 'Password must be at least 6 characters';
    if (form.password !== form.confirmPassword)
      errs.confirmPassword = 'Passwords do not match';
    return errs;
  };

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
    if (errors[e.target.name]) {
      setErrors({ ...errors, [e.target.name]: '' });
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const errs = validate();
    if (Object.keys(errs).length) {
      setErrors(errs);
      return;
    }

    setLoading(true);
    try {
      // Send only the fields expected by RegisterDto
      const payload = {
        fullName: form.fullName.trim(),
        email: form.email.trim(),
        password: form.password,
        phone: form.phone.trim() || null,
        address: form.address.trim() || null,
      };

      const result = await registerUser(payload);

      if (result.isSuccess) {
        const { token, ...userData } = result.data;
        login(userData, token);
        toast.success('Account created successfully!');
        navigate('/customer/dashboard');
      } else {
        toast.error(result.message || 'Registration failed');
      }
    } catch (err) {
      const msg =
        err.response?.data?.message ||
        err.response?.data?.Message ||
        'Registration failed. Please try again.';
      toast.error(msg);
    } finally {
      setLoading(false);
    }
  };

  const inputClass = (field) =>
    `w-full border-b-2 py-2.5 px-1 text-gray-800 font-medium placeholder:text-gray-300 focus:outline-none focus:border-primary bg-transparent ${
      errors[field] ? 'border-red-400' : 'border-gray-200'
    }`;

  return (
    <div className="min-h-screen flex">
      {/* ── Left panel: Illustration ── */}
      <div className="hidden lg:flex lg:w-1/2 relative items-center justify-center overflow-hidden bg-gradient-to-br from-teal-50 via-cyan-50 to-teal-100">
        <img
          src="/motospares-hero.svg"
          alt="MotoSpares motorcycle illustration"
          className="w-full h-full object-cover"
        />
        <div className="absolute right-0 top-0 h-full w-16">
          <svg viewBox="0 0 64 900" preserveAspectRatio="none" className="h-full w-full">
            <path d="M64,0 L64,900 L0,900 Q30,700 10,450 Q-10,200 64,0 Z" fill="white" />
          </svg>
        </div>
      </div>

      {/* ── Right panel: Signup Form ── */}
      <div className="flex-1 flex flex-col bg-white">
        {/* Navigation tabs */}
        <nav className="flex items-center justify-between px-8 pt-6 pb-4">
          <Link to="/" className="text-xl font-bold text-primary tracking-tight">
            MotoSpares
          </Link>
          <div className="flex gap-6">
            <Link
              to="/login"
              className="text-gray-400 font-medium hover:text-primary pb-1"
            >
              Login
            </Link>
            <Link
              to="/signup"
              className="text-primary font-semibold border-b-2 border-primary pb-1"
            >
              Sign up
            </Link>
          </div>
        </nav>

        {/* Form Area */}
        <div className="flex-1 flex items-center justify-center px-8 sm:px-16 lg:px-24 py-6">
          <div className="w-full max-w-md">
            <h1 className="text-3xl sm:text-4xl font-bold text-gray-800 mb-2">
              Create account
            </h1>
            <p className="text-gray-400 mb-6">Join the MotoSpares community</p>

            <form onSubmit={handleSubmit} className="space-y-4" noValidate>
              {/* Full Name */}
              <div>
                <label htmlFor="fullName" className="block text-xs text-gray-400 mb-1 uppercase tracking-wider">
                  Full Name
                </label>
                <input
                  id="fullName"
                  name="fullName"
                  type="text"
                  value={form.fullName}
                  onChange={handleChange}
                  placeholder="John Doe"
                  className={inputClass('fullName')}
                />
                {errors.fullName && (
                  <p className="text-red-500 text-xs mt-1 flex items-center gap-1">
                    <svg className="w-3.5 h-3.5" fill="currentColor" viewBox="0 0 20 20"><path fillRule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7 4a1 1 0 11-2 0 1 1 0 012 0zm-1-9a1 1 0 00-1 1v4a1 1 0 102 0V6a1 1 0 00-1-1z" clipRule="evenodd"/></svg>
                    {errors.fullName}
                  </p>
                )}
              </div>

              {/* Email */}
              <div>
                <label htmlFor="email" className="block text-xs text-gray-400 mb-1 uppercase tracking-wider">
                  Email
                </label>
                <input
                  id="email"
                  name="email"
                  type="email"
                  value={form.email}
                  onChange={handleChange}
                  placeholder="you@example.com"
                  className={inputClass('email')}
                />
                {errors.email && (
                  <p className="text-red-500 text-xs mt-1 flex items-center gap-1">
                    <svg className="w-3.5 h-3.5" fill="currentColor" viewBox="0 0 20 20"><path fillRule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7 4a1 1 0 11-2 0 1 1 0 012 0zm-1-9a1 1 0 00-1 1v4a1 1 0 102 0V6a1 1 0 00-1-1z" clipRule="evenodd"/></svg>
                    {errors.email}
                  </p>
                )}
              </div>

              {/* Phone & Address side by side */}
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label htmlFor="phone" className="block text-xs text-gray-400 mb-1 uppercase tracking-wider">
                    Phone <span className="text-gray-300">(optional)</span>
                  </label>
                  <input
                    id="phone"
                    name="phone"
                    type="tel"
                    value={form.phone}
                    onChange={handleChange}
                    placeholder="98XXXXXXXX"
                    className={inputClass('phone')}
                  />
                </div>
                <div>
                  <label htmlFor="address" className="block text-xs text-gray-400 mb-1 uppercase tracking-wider">
                    Address <span className="text-gray-300">(optional)</span>
                  </label>
                  <input
                    id="address"
                    name="address"
                    type="text"
                    value={form.address}
                    onChange={handleChange}
                    placeholder="Kathmandu"
                    className={inputClass('address')}
                  />
                </div>
              </div>

              {/* Password */}
              <div>
                <label htmlFor="password" className="block text-xs text-gray-400 mb-1 uppercase tracking-wider">
                  Password
                </label>
                <div className="relative">
                  <input
                    id="password"
                    name="password"
                    type={showPassword ? 'text' : 'password'}
                    value={form.password}
                    onChange={handleChange}
                    placeholder="Min 6 characters"
                    className={`${inputClass('password')} pr-10`}
                  />
                  <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute right-1 top-1/2 -translate-y-1/2 text-gray-400 hover:text-primary"
                  >
                    {showPassword ? (
                      <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21"/></svg>
                    ) : (
                      <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"/><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z"/></svg>
                    )}
                  </button>
                </div>
                {errors.password && (
                  <p className="text-red-500 text-xs mt-1 flex items-center gap-1">
                    <svg className="w-3.5 h-3.5" fill="currentColor" viewBox="0 0 20 20"><path fillRule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7 4a1 1 0 11-2 0 1 1 0 012 0zm-1-9a1 1 0 00-1 1v4a1 1 0 102 0V6a1 1 0 00-1-1z" clipRule="evenodd"/></svg>
                    {errors.password}
                  </p>
                )}
              </div>

              {/* Confirm Password */}
              <div>
                <label htmlFor="confirmPassword" className="block text-xs text-gray-400 mb-1 uppercase tracking-wider">
                  Confirm Password
                </label>
                <input
                  id="confirmPassword"
                  name="confirmPassword"
                  type="password"
                  value={form.confirmPassword}
                  onChange={handleChange}
                  placeholder="••••••••"
                  className={inputClass('confirmPassword')}
                />
                {errors.confirmPassword && (
                  <p className="text-red-500 text-xs mt-1 flex items-center gap-1">
                    <svg className="w-3.5 h-3.5" fill="currentColor" viewBox="0 0 20 20"><path fillRule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7 4a1 1 0 11-2 0 1 1 0 012 0zm-1-9a1 1 0 00-1 1v4a1 1 0 102 0V6a1 1 0 00-1-1z" clipRule="evenodd"/></svg>
                    {errors.confirmPassword}
                  </p>
                )}
              </div>

              {/* Submit */}
              <button
                type="submit"
                disabled={loading}
                className="w-full py-3 rounded-full bg-primary text-white font-semibold text-lg hover:bg-primary-dark active:scale-[0.98] disabled:opacity-60 disabled:cursor-not-allowed shadow-lg shadow-primary/25 cursor-pointer"
              >
                {loading ? (
                  <span className="flex items-center justify-center gap-2">
                    <svg className="w-5 h-5 animate-spin" viewBox="0 0 24 24" fill="none"><circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"/><path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z"/></svg>
                    Creating account…
                  </span>
                ) : (
                  'Sign up'
                )}
              </button>
            </form>

            <p className="text-center text-sm text-gray-400 mt-6">
              Already have an account?{' '}
              <Link to="/login" className="text-primary font-semibold hover:underline">
                Login
              </Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
