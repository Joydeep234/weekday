namespace weekday.Models.ProjectManagerModel
{
    public class UpdatePlans
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int FeatureURL { get; set; }

        public string FeatureLogo { get; set; }

        public int planId { get; set; }
        public string PlanName { get; set; }
        public int Duration { get; set; }
        public int MaxMembers { get; set; } 
        public int Price { get; set; }

    }
}
