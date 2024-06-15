namespace FarmersMarketplace.Domain.Feedbacks
{
    public class FeedbackCollection
    {
        public int Count => Body.Count;
        public List<Feedback> Body { get; set; }
        public float AverageRating { get; set; }
        public int OneRatingCount { get; set; }
        public int TwoRatingCount { get; set; }
        public int ThreeRatingCount { get; set; }
        public int FourRatingCount { get; set; }
        public int FiveRatingCount { get; set; }

        public FeedbackCollection()
        {
            Body = new List<Feedback>();
        }

        public void Add(Feedback feedback)
        {
            Body.Add(feedback);
            UpdateRatingCounts(feedback, true);
            UpdateAverageRating();
        }

        public bool Remove(Feedback feedback)
        {
            bool removed = Body.Remove(feedback);
            if (removed)
            {
                UpdateRatingCounts(feedback, false);
                UpdateAverageRating();
            }
            return removed;
        }

        public Feedback this[int index]
        {
            get => Body[index];
            set
            {
                UpdateRatingCounts(Body[index], false);
                Body[index] = value;
                UpdateRatingCounts(value, true);
                UpdateAverageRating();
            }
        }

        private void UpdateRatingCounts(Feedback feedback, bool isAdding)
        {
            int change = isAdding ? 1 : -1;
            if (feedback.Rating >= 5)
            {
                FiveRatingCount += change;
            }
            else if (feedback.Rating >= 4)
            {
                FourRatingCount += change;
            }
            else if (feedback.Rating >= 3)
            {
                ThreeRatingCount += change;
            }
            else if (feedback.Rating >= 2)
            {
                TwoRatingCount += change;
            }
            else if (feedback.Rating >= 1)
            {
                OneRatingCount += change;
            }
        }

        private void UpdateAverageRating()
        {
            AverageRating = Body.Average(f => f.Rating);
        }
    }
}
