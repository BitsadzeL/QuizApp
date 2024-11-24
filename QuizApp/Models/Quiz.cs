namespace QuizApp.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public int OwnerId { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; set; }

    }
}
