namespace QuizApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int HighScore { get; set; }
        public List<Quiz> Quizes { get; set; }
    }
}
