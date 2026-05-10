using MotoSpares.Application.DTOs;
using MotoSpares.Application.DTOs.Appointment;
using MotoSpares.Application.Interfaces;
using MotoSpares.Application.Interfaces.Repositories;
using MotoSpares.Domain.Entities;
using MotoSpares.Domain.Enums;

namespace MotoSpares.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<ApiResponse<AppointmentResponseDto>> CreateAppointmentAsync(CreateAppointmentDto dto, Guid userId)
    {
        var appointment = new Appointment
        {
            AppointmentDate = DateTime.SpecifyKind(dto.AppointmentDate, DateTimeKind.Utc),
            ServiceType = dto.ServiceType.Trim(),
            AppointmentNotes = dto.Notes?.Trim(),
            AppointmentStatus = AppointmentStatus.Pending
        };

        _appointmentRepository.Create(appointment);
        await _appointmentRepository.SaveChangesAsync();

        // Link the appointment to the customer via the join table
        var userAppointment = new UserAppointment
        {
            UserId = userId,
            AppointmentId = appointment.AppointmentId
        };
        await _appointmentRepository.AddUserAppointmentAsync(userAppointment);

        // Reload with user info
        var created = await _appointmentRepository.GetByIdWithUsersAsync(appointment.AppointmentId);

        return ApiResponse<AppointmentResponseDto>.Success(MapToDto(created!), "Appointment booked successfully.");
    }

    public async Task<ApiResponse<List<AppointmentResponseDto>>> GetMyAppointmentsAsync(Guid userId)
    {
        var appointments = await _appointmentRepository.GetByUserIdAsync(userId);
        var dtos = appointments.Select(MapToDto).ToList();
        return ApiResponse<List<AppointmentResponseDto>>.Success(dtos);
    }

    public async Task<ApiResponse<List<AppointmentResponseDto>>> GetAllAppointmentsAsync()
    {
        var appointments = await _appointmentRepository.GetAllWithUsersAsync();
        var dtos = appointments.Select(MapToDto).ToList();
        return ApiResponse<List<AppointmentResponseDto>>.Success(dtos);
    }

    public async Task<ApiResponse<AppointmentResponseDto>> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDto dto, Guid userId)
    {
        var appointment = await _appointmentRepository.GetByIdWithUsersAsync(appointmentId);

        if (appointment == null)
            return ApiResponse<AppointmentResponseDto>.Fail("Appointment not found.");

        var isOwner = appointment.UserAppointments.Any(ua => ua.UserId == userId);
        if (!isOwner)
            return ApiResponse<AppointmentResponseDto>.Fail("You can only update your own appointments.");

        if (appointment.AppointmentStatus == AppointmentStatus.Completed || appointment.AppointmentStatus == AppointmentStatus.Cancelled)
            return ApiResponse<AppointmentResponseDto>.Fail("Cannot update a completed or cancelled appointment.");

        if (dto.AppointmentDate.HasValue)
            appointment.AppointmentDate = DateTime.SpecifyKind(dto.AppointmentDate.Value, DateTimeKind.Utc);

        if (dto.Notes != null)
            appointment.AppointmentNotes = dto.Notes.Trim();

        _appointmentRepository.Update(appointment);
        await _appointmentRepository.SaveChangesAsync();

        return ApiResponse<AppointmentResponseDto>.Success(MapToDto(appointment), "Appointment updated successfully.");
    }

    public async Task<ApiResponse<AppointmentResponseDto>> UpdateAppointmentStatusAsync(int appointmentId, UpdateAppointmentStatusDto dto)
    {
        var appointment = await _appointmentRepository.GetByIdWithUsersAsync(appointmentId);

        if (appointment == null)
            return ApiResponse<AppointmentResponseDto>.Fail("Appointment not found.");

        if (!Enum.TryParse<AppointmentStatus>(dto.Status, true, out var newStatus))
            return ApiResponse<AppointmentResponseDto>.Fail($"Invalid status. Valid values: {string.Join(", ", Enum.GetNames<AppointmentStatus>())}");

        appointment.AppointmentStatus = newStatus;
        _appointmentRepository.Update(appointment);
        await _appointmentRepository.SaveChangesAsync();

        return ApiResponse<AppointmentResponseDto>.Success(MapToDto(appointment), "Appointment status updated.");
    }

    public async Task<ApiResponse<string>> DeleteAppointmentAsync(int appointmentId, Guid userId)
    {
        var appointment = await _appointmentRepository.GetByIdWithUsersAsync(appointmentId);

        if (appointment == null)
            return ApiResponse<string>.Fail("Appointment not found.");

        var isOwner = appointment.UserAppointments.Any(ua => ua.UserId == userId);
        if (!isOwner)
            return ApiResponse<string>.Fail("You can only cancel your own appointments.");

        if (appointment.AppointmentStatus == AppointmentStatus.Completed)
            return ApiResponse<string>.Fail("Cannot cancel a completed appointment.");

        appointment.AppointmentStatus = AppointmentStatus.Cancelled;
        _appointmentRepository.Update(appointment);
        await _appointmentRepository.SaveChangesAsync();

        return ApiResponse<string>.Success("Cancelled", "Appointment cancelled successfully.");
    }

    private static AppointmentResponseDto MapToDto(Appointment appointment)
    {
        var user = appointment.UserAppointments?.FirstOrDefault()?.User;
        return new AppointmentResponseDto
        {
            AppointmentId = appointment.AppointmentId,
            AppointmentDate = appointment.AppointmentDate,
            ServiceType = appointment.ServiceType,
            Status = appointment.AppointmentStatus.ToString(),
            Notes = appointment.AppointmentNotes,
            CustomerName = user?.FullName ?? string.Empty,
            CustomerId = user?.Id ?? Guid.Empty
        };
    }
}
