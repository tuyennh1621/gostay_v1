﻿using GoStay.DataDto.HotelDto;

namespace GoStay.Data.HotelDto
{
    public class HotelRoomDto
    {
        public int? Id { get; set; }
        public int? Idhotel { get; set; }
        public string? Name { get; set; }
        public decimal? RoomSize { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public byte? RemainNum { get; set; }
        public decimal? PriceValue { get; set; }
        public double? Discount { get; set; }
        public decimal? NewPrice { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        public byte? Palletbed { get; set; }
        public string ViewDirection { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public List<string> Pictures { get; set; } = new List<string>();
        //public List<string> AllPictures { get; set; } = new List<string>();

        public List<ServiceRoomDto> Services { get; set; }

    }
}
