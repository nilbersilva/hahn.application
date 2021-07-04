using AutoMapper;
using Hahn.ApplicatonProcess.February2021.Domain.Enums.Business;
using Hahn.ApplicatonProcess.February2021.Domain.Models.API;
using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using Hahn.ApplicatonProcess.February2021.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Domain.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AssetDto, Asset>()
              .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.AssetName))
              .ForMember(dest => dest.Department, opt => opt.MapFrom(src => int.Parse(src.Department)))
              .ForMember(dest => dest.CountryOfDepartment, opt => opt.MapFrom(src => src.Country))
              .ForMember(dest => dest.EMailAdressOfDepartment, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.PurchaseDate))
              .ForMember(dest => dest.Broken, opt => opt.MapFrom(src => src.Broken));

            CreateMap<Asset, AssetDto>()
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.AssetName))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => Convert.ToInt32(src.Department)))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryOfDepartment))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMailAdressOfDepartment))
                .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.PurchaseDate))
                .ForMember(dest => dest.Broken, opt => opt.MapFrom(src => src.Broken));

            CreateMap<enDepartment, DepartmentDto>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => $"{src}"));

            CreateMap<PostAssetRequest, AssetDto>();
            CreateMap<PutAssetRequest, AssetDto>();
        }
    }
}
