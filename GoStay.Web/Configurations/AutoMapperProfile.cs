using AutoMapper;
using GoStay.DataDto.TourDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto;
using GoStay.DataDto.Album;
using GoStay.DataDto.Banner;
using GoStay.DataDto.Hành_Chính;
using GoStay.DataDto.Service;
using GoStay.DataDto.HotelDto;

namespace GoStay.Web.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<HotelDto, Hotel>().ReverseMap();
            CreateMap<AlbumDto, Album>().ReverseMap();
            CreateMap<Album, AlbumDto>().ReverseMap();
            CreateMap<ServiceDto, DataAccess.Entities.Service>().ReverseMap();
            CreateMap<DataAccess.Entities.Service, ServiceDto>().ReverseMap();

            CreateMap<DataAccess.Entities.Service, ServiceSearchDto>().ReverseMap();
            CreateMap<Quan, QuanDto>().ReverseMap();

            CreateMap<Banner, BannerDetailDto>().ReverseMap();

            CreateMap<TinhThanh,TinhThanhBannerDto>().ReverseMap();

            CreateMap<TourDetail, TourDetailDto>().ReverseMap();
            CreateMap<TourDetailDto, TourDetail>().ReverseMap();

            CreateMap<TourAddDto, Tour>().ReverseMap();

            CreateMap<SearchTourDto, InfoTourInList>().ReverseMap();
            CreateMap<RoomEditDto, HotelRoom>().ReverseMap();
            CreateMap<HotelRoom, RoomEditDto>().ReverseMap();
            CreateMap<RoomAddDto, HotelRoom>().ReverseMap();
            CreateMap<HotelRoom, RoomAddDto>().ReverseMap();

            //CreateMap<RoomEditDto, RoomDto>().ReverseMap();
            //CreateMap<RoomDto, RoomEditDto>().ReverseMap();
            //CreateMap<RoomAddDto, RoomDto>().ReverseMap();
            //CreateMap<RoomDto, RoomAddDto>().ReverseMap();
        }
    }
}
