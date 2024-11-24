namespace QuizApp.Models
{
    public class Question
    {
        public string QuestionText {  get; set; }   
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public char CorrectVersion { get; set; }
    }
}
