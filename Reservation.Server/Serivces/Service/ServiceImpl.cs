﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reservation.API.Data;
using Reservation.API.Data.Entities;
using Reservation.API.Models.DTO.Auth;
using Reservation.API.Models.DTO.Service;

namespace Reservation.API.Serivces.Service
{
    public class ServiceImpl : IService
    {
        public ServiceImpl(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public async Task<AppResponse<List<ServiceDTO>>> GetAllAsync()
        {   
            var services = await _context.Services.ToListAsync();

            return new AppResponse<List<ServiceDTO>>().SetSuccessResponse(_mapper.Map<List<ServiceDTO>>(services));
        }

        public async Task<AppResponse<bool>> AddAsync(ServiceDTO dto)
        {
            var service = _mapper.Map<Data.Entities.Service>(dto);

            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();

            return new AppResponse<bool>().SetSuccessResponse(true);
        }
    }
}
