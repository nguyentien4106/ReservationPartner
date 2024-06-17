﻿using Microsoft.AspNetCore.Mvc;
using Reservation.Infrastructure.Data.Entities;
using Reservation.Domain.Models.DTO.Auth;
using Reservation.Domain.Models.DTO.Jobs;
using Reservation.Domain.Models.ViewModel.Jobs;
using Reservation.Application.Serivces.Jobs;
using Reservation.Domain.Models.Request;
using Reservation.Domain.Models.ViewModel;

namespace Reservation.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class JobsController(IJobsService service) : Controller
    {

        private readonly IJobsService _service = service;

        [HttpGet]
        public async Task<AppResponse<JobsViewModel>> GetAll([FromQuery] PagingRequest paging)
        {
            return await _service.GetAll(paging);
        }

        [HttpGet("[action]/{applicationUserId}")]
        public async Task<AppResponse<JobsViewModel>> Users(string applicationUserId, [FromQuery] PagingRequest paging)
        {
            return await _service.GetByUsers(paging, applicationUserId);
        }

        [HttpGet("[action]/{applicationUserId}")]
        public async Task<AppResponse<PagingViewModel<ContractDTO>>> UsersApplies(string applicationUserId, [FromQuery] PagingRequest paging)
        {
            return await _service.GetByUserApplies(paging, applicationUserId);
        }

        [HttpPost]
        public async Task<AppResponse<bool>> CreateJob(JobDTO job)
        {
            return await _service.CreateJobAsync(job);
        }

        [HttpPost("[action]")]
        public async Task<AppResponse<bool>> Apply(ContractDTO contract)
        {
            return await _service.ApplyJobAsync(contract);
        }
    }
}
