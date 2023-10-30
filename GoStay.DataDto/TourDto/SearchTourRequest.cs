
﻿using GoStay.DataAccess.Entities;
using System.Web.Mvc;

namespace GoStay.DataDto.TourDto

{
    public class SearchTourRequest
    {
        public int[]? IdTourTopic { get; set; }
        public int[]? IdTourStyle { get; set; }
        public int[]? IdDistrictFrom { get; set; }
        public int[]? IdDistrictTo { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }

        public int[]? Rating { get; set; }
        public int[]? ForeignTravel { get; set; }

        public int? NumMature { get; set; }

        public DateTime? StartDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class SearchTourParam
    {
        public List<SelectListItem>? ForeignTravel { get; set; }
        public List<SelectListItem>? IdTourTopic { get; set; }
        public List<SelectListItem>? IdTourStyle { get; set; }
        public List<SelectListItem>? IdProvinceFrom { get; set; }
        public List<SelectListItem>? IdProvinceTo { get; set; }
        public List<SelectListItem>? Rating { get; set; }
        public int? NumMature { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public string? CheckinDate { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
