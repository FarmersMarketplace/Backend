namespace FarmersMarketplace.Domain.Feedbacks
{
    public class FeedbackCollection
    {
        public Guid Id { get; set; }
        public int Count => Feedbacks.Count;
        public List<Feedback> Feedbacks { get; set; }
        public float AverageRating { get; set; }
        public int OneRatingCount { get; set; }
        public int TwoRatingCount { get; set; }
        public int ThreeRatingCount { get; set; }
        public int FourRatingCount { get; set; }
        public int FiveRatingCount { get; set; }

        public FeedbackCollection()
        {
            Feedbacks = new List<Feedback>();
        }

        public void Add(Feedback feedback)
        {
            Feedbacks.Add(feedback);
            UpdateRatingCounts(feedback, true);
            UpdateAverageRating();
        }

        public bool Remove(Feedback feedback)
        {
            bool removed = Feedbacks.Remove(feedback);
            if (removed)
            {
                UpdateRatingCounts(feedback, false);
                UpdateAverageRating();
            }
            return removed;
        }

        public Feedback this[int index]
        {
            get => Feedbacks[index];
            set
            {
                UpdateRatingCounts(Feedbacks[index], false);
                Feedbacks[index] = value;
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
            AverageRating = Feedbacks.Average(f => f.Rating);
        }
    }
}
