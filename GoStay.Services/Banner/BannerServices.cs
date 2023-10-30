
using AutoMapper;
using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.Banner;


namespace GoStay.Service
{
    public class BannerServices : IBannerServices
    {
        private readonly ICommonRepository<Banner> _BannerRepository;
        private readonly ICommonUoW _commonUoW;
        private readonly IMapper _mapper;
        public BannerServices(ICommonRepository<Banner> BannerRepository, ICommonUoW commonUoW, IMapper mapper)
        {
            _BannerRepository = BannerRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
        }
       


        public string? AddNewBanner(Banner data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _BannerRepository.Insert(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.AddFail.Value}";
            }
        }

        public string? EditBanner(Banner data)
        {
            try
            {
                _commonUoW.BeginTransaction();
                _BannerRepository.Update(data);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.EditFail.Value}";
            }
        }

        public string? DeleteBanner(int id)
        {
            try
            {
                var entity = _BannerRepository.GetById(id);
                _commonUoW.BeginTransaction();
                _BannerRepository.SoftDelete(entity);
                _commonUoW.Commit();
                return $"{ErrorCodeMessage.Success.Value}";
            }
            catch
            {
                _commonUoW.RollBack();
                return $"{ErrorCodeMessage.DeleteFail.Value}";
            }
        }
    }
}

