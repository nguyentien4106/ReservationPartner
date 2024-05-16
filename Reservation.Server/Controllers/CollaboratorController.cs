﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using Reservation.Server.Data;
using Reservation.Server.Models.DTO.Auth;
using Reservation.Server.Models.DTO.Collaborator;
using Reservation.Server.Models.DTO.UserServicesRegister;
using Reservation.Server.Serivces.UserServiceRegister;

namespace Reservation.Server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CollaboratorController(ICollaboratorService collaboratorService) : ControllerBase
    {
        private readonly ICollaboratorService _collaboratorService = collaboratorService;

        [HttpGet]
        public async Task<AppResponse<string>> GetUser(string email)
        {
            return await _collaboratorService.GetUserIdAsync(email);
        }

        [HttpPost]
        public async Task<AppResponse<CollaboratorDTO>> Register(CollaboratorDTO dto)
        {
            return await _collaboratorService.RegisterAsync(dto);
        }

        [HttpGet]
        public async Task<AppResponse<CollaboratorDTO>> GetProfile(string id)
        {
            return await _collaboratorService.GetProfileAsync(id);
        }

        [HttpGet]
        public async Task<AppResponse<List<CollaboratorDTO>>> GetAll()
        {
            return await _collaboratorService.GetAllAsync();

        }
    }
}
