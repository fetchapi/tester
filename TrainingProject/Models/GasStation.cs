using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TrainingProject.Models
{
    public partial class GasStation
    {
        public GasStation()
        {
            GasStationFeedback = new HashSet<GasStationFeedback>();
            GasStationGasType = new HashSet<GasStationGasType>();
        }
      
        public long GasStationId { get; set; }
        public string GasStationName { get; set; }
        public string Address { get; set; }
        public long? District { get; set; }
        public string OpeningTime { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Rating { get; set; }
        public DateTime InsertedAt { get; set; }
        public long InsertedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }

        public ICollection<GasStationFeedback> GasStationFeedback { get; set; }
        public ICollection<GasStationGasType> GasStationGasType { get; set; }
    }
}
