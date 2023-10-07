﻿namespace CityInfo.API.DTOs
{
    public class PointsOfInterestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}