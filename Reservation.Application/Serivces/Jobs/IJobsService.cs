﻿using Reservation.Domain.Models.DTO.Auth;
using Reservation.Domain.Models.DTO.Common;
using Reservation.Domain.Models.DTO.Jobs;
using Reservation.Domain.Models.ViewModel.Jobs;

namespace Reservation.Application.Serivces.Jobs
{
    public interface IJobsService
    {
        Task<AppResponse<bool>> CreateJobAsync(JobDTO job);

        Task<AppResponse<JobsViewModel>> GetAll(PaginationModel model);

        Task<AppResponse<bool>> ApplyJobAsync();
    }
}
