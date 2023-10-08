using System;
using System.Collections.Generic;
using TrainingProject.Models;
namespace TrainingProject.ViewModels
{
    public class GasStationVM
    {
        public List<string> GasStation { get; set; }
        public long GasStationId { get; set; }
        public string GasStationName { get; set; }
        public string Address { get; set; }
        public string OpeningTime { get; set; }
        public List<string> Types { get; set; }
        public Models.MDistrict District { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Rating { get; set; }
        public ICollection<GasStationFeedback> GasStationFeedback { get; set; }
        public DateTime FeedbackAt { get; set; }

    }
}
