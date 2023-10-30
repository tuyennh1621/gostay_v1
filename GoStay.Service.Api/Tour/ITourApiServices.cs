using GoStay.Common;
using GoStay.DataDto.TourDto;

namespace GoStay.Service.Api.Tour
{
    public interface ITourApiServices
    {

        public List<SearchTourDto> SearchTour(SearchTourRequest request);
        public List<SuggestTourDto> SuggestTour(string searchText);
        public TourContentDto GetTourContent(int Id);
        public ResponseBase<List<SearchTourDto>> GetTourHomePage();
        public ResponseBase<int> GetTourLocationTotal(int IdProvince);
        public ResponseBase<string> UpdateTourToCompare(CompareTourParam param);
        public ResponseBase<List<TourInCompareDto>> GetListToursCompare(string listId);


    }
}