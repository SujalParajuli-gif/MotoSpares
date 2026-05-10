using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Appointment;

namespace MotoSpares.Application.Interfaces;

public interface IAppointmentService
{
    Task<ApiResponse<AppointmentResponseDto>> CreateAppointmentAsync(CreateAppointmentDto dto, Guid userId);
    Task<ApiResponse<List<AppointmentResponseDto>>> GetMyAppointmentsAsync(Guid userId);
    Task<ApiResponse<List<AppointmentResponseDto>>> GetAllAppointmentsAsync();
    Task<ApiResponse<AppointmentResponseDto>> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto dto, Guid userId);
    Task<ApiResponse<AppointmentResponseDto>> UpdateAppointmentStatusAsync(int appointmentId, UpdateAppointmentStatusDto dto);
    Task<ApiResponse<string>> DeleteAppointmentAsync(int appointmentId, Guid userId);
}
