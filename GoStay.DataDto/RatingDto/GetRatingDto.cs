﻿namespace GoStay.DataDto.RatingDto
{
    public class GetRatingDto
    {
        public string Description { get; set; }
        public decimal LocationScore { get; set; }
        public decimal ValueScore { get; set; }
        public decimal ServiceScore { get; set; }
        public decimal CleanlinessScore { get; set; }
        public decimal RoomsScore { get; set; }
    }
    public class RatingAdminDto
    {
        public int Id { get; set; }
        public int IdHotel { get; set; }
        public string HotelName { get; set; }

        public decimal LocationScore { get; set; }
        public decimal ValueScore { get; set; }
        public decimal ServiceScore { get; set; }
        public decimal CleanlinessScore { get; set; }
        public decimal RoomsScore { get; set; }
        public string? Description { get; set; }
        public int IdUser { get; set; }
        public byte? Status { get; set; }
    }
}
