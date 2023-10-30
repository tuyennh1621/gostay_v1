using GoStay.Common;
using GoStay.DataDto.TourDto;
using GoStay.Service.Api.Base;

namespace GoStay.Service.Api.Tour
{
    public class TourApiServices : ApiServiceBase, ITourApiServices
    {
        public List<SearchTourDto> SearchTour(SearchTourRequest request)
        {
            var response = Post<SearchTourRequest, List<SearchTourDto>>("Tours/search", request);
            return response.Data;
        }

        public List<SuggestTourDto> SuggestTour(string searchText)
        {
            var response = Get<List<SuggestTourDto>>("Tours/suggest", new KeyValuePair<string, object>("searchText", searchText));
            return response.Data;
        }

        public TourContentDto GetTourContent(int Id)
        {
            var response = Get<TourContentDto>("Tours/tourcontent", new KeyValuePair<string, object>("Id", Id));
            return response.Data;
        }
        public ResponseBase<List<SearchTourDto>> GetTourHomePage()
        {
            var response = Get<List<SearchTourDto>>("Tours/tour-homepage");
            return response;
        }
        public ResponseBase<int> GetTourLocationTotal(int IdProvince)
        {
            var response = Get<int>("Tours/tour-location", new KeyValuePair<string, object>("IdProvince", IdProvince));
            return response;
        }

        public ResponseBase<string> UpdateTourToCompare(CompareTourParam param)
        {
            var response = Post<CompareTourParam, string>("Tours/update-tour-to-compare", param);
            return response;
        }
        public ResponseBase<List<TourInCompareDto>> GetListToursCompare(string listId)
        {
            var response = Get<List<TourInCompareDto>>("Tours/list-tour-compare", new KeyValuePair<string, object>("listId", listId));
            return response;
        }
    }
}