﻿using AutoMapper;
using Reservation.API.Models.DTO.Jobs;
using Reservation.API.Data.Entities;
using Reservation.API.Models.DTO.Collaborator;
using Reservation.API.Models.DTO.Customer;
using Reservation.API.Models.DTO.Home;
using Reservation.API.Models.DTO.Jobs;
using Reservation.API.Models.DTO.Service;

namespace Reservation.API.Profile
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CollaboratorService, CollaboratorServiceDTO>()
                .ForMember(item => item.Name, opt => opt.MapFrom(src => src.Service.Name));

            CreateMap<CollaboratorServiceDTO, CollaboratorService>();

            CreateMap<Collaborator, CollaboratorDTO>()
            .ForMember(dest => dest.CollaboratorServices, opt => opt.MapFrom(src => src.CollaboratorServices));

            CreateMap<Service, ServiceDTO>();

            CreateMap<ServiceDTO, Service>().ForMember(item => item.CollaboratorServices, opt => opt.Ignore());

            CreateMap<CollaboratorDTO, Collaborator>();

            CreateMap<View, ViewDTO>();

            CreateMap<Order, OrderDTO>()
                .ForMember(item => item.NickName, opt => opt.MapFrom(src => src.Collaborator.NickName));

            CreateMap<OrderDTO, Order>()
                .ForMember(item => item.CollaboratorId, opt => opt.MapFrom(src => src.CollaboratorId));

            CreateMap<Review, ReviewDTO>().ReverseMap();

            CreateMap<JobDTO, Job>().ReverseMap();

            CreateMap<JobServiceDTO, JobService>().ReverseMap();


        }
    }
}
