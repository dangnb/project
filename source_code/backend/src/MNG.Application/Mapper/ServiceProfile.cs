using AutoMapper;
using MNG.Contract.Abstractions.Shared;
using MNG.Contract.Services.V1.Product;
using MNG.Domain.Entities;

namespace MNG.Application.Mapper;
public class ServiceProfile : Profile
{
    public ServiceProfile()
    {

        //V1
        CreateMap<Product, Response.ProductResponse>().ReverseMap();
        CreateMap<PagedResult<Product>, PagedResult<Response.ProductResponse>>().ReverseMap();

    }
}
