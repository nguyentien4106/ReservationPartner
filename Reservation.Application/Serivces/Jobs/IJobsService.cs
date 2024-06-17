﻿using Reservation.Domain.Models.DTO.Auth;
using Reservation.Domain.Models.DTO.Jobs;
using Reservation.Domain.Models.Request;
using Reservation.Domain.Models.ViewModel;
using Reservation.Domain.Models.ViewModel.Jobs;

namespace Reservation.Application.Serivces.Jobs
{
    public interface IJobsService
    {
        Task<AppResponse<bool>> CreateJobAsync(JobDTO job);

        Task<AppResponse<JobsViewModel>> GetAll(PagingRequest model);

        Task<AppResponse<bool>> ApplyJobAsync(ContractDTO contract);

        Task<AppResponse<JobsViewModel>> GetByUsers(PagingRequest model, string userId);

        Task<AppResponse<PagingViewModel<ContractDTO>>> GetByUserApplies(PagingRequest model, string userId);

    }
}
