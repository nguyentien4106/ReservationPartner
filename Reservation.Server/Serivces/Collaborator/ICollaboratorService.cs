﻿using Reservation.Server.Data;
using Reservation.Server.Models.DTO.Auth;
using Reservation.Server.Models.DTO.Collaborator;

namespace Reservation.Server.Serivces.UserServiceRegister
{
    public interface ICollaboratorService
    {
        Task<AppResponse<string>> GetUserIdAsync(string email);

        Task<AppResponse<CollaboratorDTO>> GetProfileAsync(Guid? collaboratorId);

        Task<AppResponse<List<CollaboratorDTO>>> GetAllAsync();

        Task<AppResponse<string>> AddAsync(CollaboratorDTO dto);

        Task<AppResponse<string>> UpdateAsync(CollaboratorDTO dto);
    }
}
