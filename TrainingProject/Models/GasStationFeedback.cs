using System;
using System.Collections.Generic;

namespace TrainingProject.Models
{
    public partial class GasStationFeedback
    {
        public long GasStationFeedbackId { get; set; }
        public long GasStationId { get; set; }
        public string Feedback { get; set; }
        public DateTime FeedbackAt { get; set; }
        public long? FeedbackBy { get; set; }

        public GasStation GasStation { get; set; }
    }
}
