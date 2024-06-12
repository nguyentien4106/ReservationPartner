﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reservation.API.Data;
using Reservation.API.Data.Entities;
using Reservation.API.Extensions;
using Reservation.API.Models.DTO.Auth;
using Reservation.API.Models.DTO.Collaborator;
using Reservation.API.Models.DTO.Email;
using Reservation.API.Models.DTO.Home;
using Reservation.API.Models.Enum;
using Reservation.API.Serivces.Email;
using System.Linq.Expressions;

namespace Reservation.API.Serivces.UserServiceRegister
{
    public class CollaboratorService(
        ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager, 
        IMapper mapper,
        IEmailService emailService
    ) 
        : ICollaboratorService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        private readonly IEmailService _emailService = emailService;
        private const string All = "All";

        public async Task<AppResponse<List<CollaboratorDTO>>> GetAllAsync(int type)
        {
            Expression<Func<Collaborator, bool>> predict = type switch
            {
                (int)CollaboratorGetType.All => (Collaborator c) => true,
                (int)CollaboratorGetType.ReadyAndReviewing => (Collaborator c) => c.IsReady == true && c.Status == (int)ProfileStatus.Reviewing,
                _ => (Collaborator c) => c.IsReady == false,
            };

            var collaborators = await _context.Collaborators
                .Include(item => item.CollaboratorServices)
                .ThenInclude(item => item.Service)
                .Where(predict)
                .ToListAsync();

            return new AppResponse<List<CollaboratorDTO>>().SetSuccessResponse(_mapper.Map<List<CollaboratorDTO>>(collaborators));
        }

        public async Task<AppResponse<CollaboratorDTO>> GetProfileAsync(Guid? collaboratorId)
        {
            if (collaboratorId == null || !collaboratorId.HasValue)
            {
                return new AppResponse<CollaboratorDTO>().SetErrorResponse("id", $"{collaboratorId} not found!");
            }

            var collaborator = await _context.Collaborators
                .Include(item => item.View)
                .Include(item => item.CollaboratorServices)
                .ThenInclude(item => item.Service)
                .SingleOrDefaultAsync(item => item.Id == collaboratorId);

            var orders = await _context.Orders.Include(item => item.Review).Where(item => item.CollaboratorId == collaboratorId).ToListAsync();

            collaborator.Orders = orders;


            if (collaborator == null)
            {
                return new AppResponse<CollaboratorDTO>().SetErrorResponse("id", "User not found!");
            }

            await UpdateView(collaborator);


            return new AppResponse<CollaboratorDTO>().SetSuccessResponse(_mapper.Map<CollaboratorDTO>(collaborator));
        }

        public async Task<AppResponse<string>> GetUserIdAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AppResponse<string>().SetErrorResponse("user", "User not found");
            }

            return new AppResponse<string>().SetSuccessResponse(user.Id);
        }

        public async Task<AppResponse<string>> AddAsync(CollaboratorDTO dto)
        {
            var newCollaborator = _mapper.Map<Collaborator>(dto);
            newCollaborator.Status = (int)ProfileStatus.Reviewing;
            newCollaborator.JoinedDate = DateTime.Now;

            await _context.Collaborators.AddAsync(newCollaborator);
            await _context.SaveChangesAsync();

            return new AppResponse<string>().SetSuccessResponse(newCollaborator.Id.ToString());
        }

        public async Task<AppResponse<string>> UpdateAsync(CollaboratorDTO dto)
        {
            if (!dto.Id.HasValue)
            {
                return new AppResponse<string>().SetErrorResponse("user", "User not found");
            }

            var collaborator = await _context.Collaborators.Include(item => item.CollaboratorServices).SingleOrDefaultAsync(item => item.Id == dto.Id);

            if (collaborator == null)
            {
                return new AppResponse<string>().SetErrorResponse("user", $"Collaborator is not existed with ${dto.Id}");

            }

            _context.CollaboratorServices.RemoveRange(collaborator.CollaboratorServices);
            var collaboratorServices = _mapper.Map<List<Data.Entities.CollaboratorService>>(dto.CollaboratorServices);
            _context.CollaboratorServices.AddRange(collaboratorServices);

            Update(collaborator, dto);
            await _context.SaveChangesAsync();
            return new AppResponse<string>().SetSuccessResponse("Update Successfully!");
        }

        private static void Update(Collaborator entity, CollaboratorDTO newValue)
        {
            entity.IsReady = newValue.IsReady;
            entity.NickName = newValue.NickName;
            entity.PhoneNumber = newValue.PhoneNumber;
            entity.Email = newValue.Email;
            entity.City = newValue.City;
            entity.District = newValue.District;
            entity.BirthDate = newValue.BirthDate;
            entity.PricePerHour = newValue.PricePerHour;
            entity.Introduction = newValue.Introduction;
            entity.Title = newValue.Title;
            entity.Weight = newValue.Weight;
            entity.Height = newValue.Height;
            entity.Job = newValue.Job;
            entity.Sex = newValue.Sex;
            entity.OtherServices = newValue.OtherServices;
        }

        public async Task<AppResponse<string>> ChangeStatusAsync(Guid? collaboratorId, int status)
        {
            if( collaboratorId == null || !collaboratorId.HasValue)
            {
                return new AppResponse<string>().SetErrorResponse("id", "The Collaborator went wrong!");
            }

            var collaborator = await _context.Collaborators.SingleOrDefaultAsync(item => item.Id == collaboratorId);
            
            if(collaborator == null)
            {
                return new AppResponse<string>().SetErrorResponse("id", "There is no collaborator with ID given");
            }

            collaborator.Status = status;

            await _context.SaveChangesAsync();

            return new AppResponse<string>().SetSuccessResponse("Approved Successfully!");

        }

        public async Task<AppResponse<CollaboratorDTO>> GetProfileByEmailAsync(string? email)
        {
            if (email == null || string.IsNullOrEmpty(email))
            {
                return new AppResponse<CollaboratorDTO>().SetSuccessResponse(new(), "id", $"{email} not found!");
            }

            var collaborator = await _context.Collaborators
                .Include(item => item.ApplicationUser)
                .Include(item => item.CollaboratorServices)
                .ThenInclude(cs => cs.Service)
                .FirstOrDefaultAsync(collaborator => collaborator.Email == email);

            if (collaborator == null)
            {
                return new AppResponse<CollaboratorDTO>().SetErrorResponse("id", "User not found!");
            }

            return new AppResponse<CollaboratorDTO>().SetSuccessResponse(_mapper.Map<CollaboratorDTO>(collaborator));
        }

        private async Task UpdateView(Collaborator collaborator)
        {
            var collaboratorView = await _context.Views.SingleOrDefaultAsync(item => item.CollaboratorId == collaborator.Id);
            if (collaboratorView == null)
            {
                var view = new View()
                {
                    CollaboratorId = collaborator.Id,
                    Count = 1
                };
                await _context.Views.AddAsync(view);
            }
            else
            {
                await _context.Database.ExecuteSqlRawAsync("UPDATE Views SET COUNT = COUNT + 1 WHERE CollaboratorId = {0}", collaborator.Id);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<AppResponse<List<CollaboratorDTO>>> GetAllAsync(string city, string district, string sex, int orderType)
        {
            var query = _context.Collaborators.Include(item => item.View).AsQueryable();

            query = query.Where(collaborator => city == All || collaborator.City == city);

            query = query.Where(collaborator => district == All || collaborator.District == district);

            query = query.Where(collaborator => sex == All || collaborator.Sex == sex);

            var result = await query.ToListAsync();

            if (result == null)
            {
                return new AppResponse<List<CollaboratorDTO>>().SetSuccessResponse([]);
            }
            var collaboratorDtos = _mapper.Map<List<CollaboratorDTO>>(result);

            switch (orderType)
            {
                case (int)OrderFilterType.PriceIncreasing:
                    collaboratorDtos = collaboratorDtos.OrderByPrice();
                    break;

                case (int)OrderFilterType.PriceDecreasing:
                    collaboratorDtos = collaboratorDtos.OrderByPriceDecreasing();
                    break;

                case (int)OrderFilterType.RateCountIncreasing:
                    collaboratorDtos = collaboratorDtos.OrderByRateCount();
                    break;

                case (int)OrderFilterType.RateCountDecreasing:
                    collaboratorDtos = collaboratorDtos.OrderByRateCountDecreasing();
                    break;

                case (int)OrderFilterType.AgeIncreasing:
                    collaboratorDtos = collaboratorDtos.OrderByAge();
                    break;

                case (int)OrderFilterType.AgeDecreasing:
                    collaboratorDtos = collaboratorDtos.OrderByAgeDecreasing();
                    break;
            }

            return new AppResponse<List<CollaboratorDTO>>().SetSuccessResponse(collaboratorDtos);
        }


        //public async Task<AppResponse<List<OrderDTO>>> GetRequestsAsync(Guid? collaboratorId)
        //{

        //    if (!collaboratorId.HasValue)
        //    {
        //        return new AppResponse<List<OrderDTO>>().SetErrorResponse("user", "Không tìm thấy User");
        //    }

        //    var requests = await _context.Orders.Where(item => item.CollaboratorId == collaboratorId).ToListAsync();

        //    return new AppResponse<List<OrderDTO>>().SetSuccessResponse(_mapper.Map<List<OrderDTO>>(requests));

        //}

        //public async Task<AppResponse<OrderDTO>> ComfirmRequestAsync(Guid? requestId, int status)
        //{
        //    if (!requestId.HasValue)
        //    {
        //        return new AppResponse<OrderDTO>().SetErrorResponse("bind", "Không tìm thấy request");
        //    }

        //    var request = await _context.Orders.SingleOrDefaultAsync(item => item.Id == requestId);

        //    if(request == null)
        //    {
        //        return new AppResponse<OrderDTO>().SetErrorResponse("request", "Không tìm thấy request trả về");
        //    }

        //    request.Status = status;
        //    request.ConfirmedDate = DateTime.Now;

        //    await _context.SaveChangesAsync();

        //    var collaborator = await _context.Collaborators.SingleOrDefaultAsync(item => item.Id == request.CollaboratorId);
        //    var email = new EmailContent()
        //    {
        //        ToEmail = request.Email,
        //        ToName = request.Name,
        //        Subject = "Thông báo mới về yêu cầu cho thuê",
        //        Content = $"{collaborator?.NickName ?? ""} đã xác nhận yêu cầu cho thuê của bạn đã tạo vào hồi {request.CreatedDate}. Xin hãy kiểm tra lại trang phản hồi để biết thêm thông tin chi tiết và số diện thoại của CTV. Nếu xảy ra bất cứ vấn đề xin hãy liên lạc lại chúng tôi để giải quyết yêu cầu."
        //    };

        //    var sent = _emailService.SendMail(email);

        //    if (!sent)
        //    {
        //        return new AppResponse<OrderDTO>().SetErrorResponse("mail", "Đã xác nhận thành công nhưng chưa thể gửi email cho người yêu cầu");
        //    }

        //    return new AppResponse<OrderDTO>().SetSuccessResponse(_mapper.Map<OrderDTO>(request));
        //}
    }


}
