﻿using QuizApp.Models;
using System.Security.Principal;
using System.Text.Json;

namespace QuizApp.Repository
{
    public class QuizRepository
    {
        private readonly string _filePath;
        private List<Quiz> _quizes;
        public QuizRepository(string filepath)
        {
            _filePath = filepath;
            _quizes = LoadData();
        }

        public void CreateQuiz(Quiz quiz)
        {
            quiz.QuizId = _quizes.Any() ? _quizes.Max(a => a.QuizId) + 1 : 1;
            _quizes.Add(quiz);
            SaveData();

        }

        public List<Quiz> GetUsersQuizes(int UserId)
        {
            List<Quiz> UserQuizes = _quizes.Where(a=>a.OwnerId==UserId).ToList();
            return UserQuizes;

        }

        public List<Quiz> GetOtherUsersQuizes(int UserId)
        {
            List<Quiz> UserQuizes = _quizes.Where(a => a.OwnerId != UserId).ToList();
            return UserQuizes;

        }

        public void DeleteQuiz(int QuizIdToDelete)
        {
            var quiz = _quizes.FirstOrDefault(a => a.QuizId == QuizIdToDelete);
            if (quiz != null)
            {
                _quizes.Remove(quiz);
                SaveData();
            }

        }
        public void UpdateQuiz(Quiz quiz)
        {
            var index = _quizes.FindIndex(a => a.QuizId == quiz.QuizId);
            if (index >= 0)
            {
                _quizes[index] = quiz;
                SaveData();
            }
        }

        public Quiz GetQuizById(int QuizId)
        {
            return _quizes.FirstOrDefault(a => a.QuizId == QuizId);
        }
        
        public List<Question> GetQuestionsOfQuiz(int QuizId)
        {
            return _quizes.FirstOrDefault(a => a.QuizId == QuizId).Questions.ToList();
        }




        private List<Quiz> LoadData()
        {
            if (!File.Exists(_filePath))
                return new List<Quiz>();

            using (StreamReader sr = new(_filePath))
            {
                return JsonSerializer.Deserialize<List<Quiz>>(File.ReadAllText(_filePath));
            }
        }



        private void SaveData()
        {

            using (StreamWriter sw = new StreamWriter(_filePath, append: false))
            {
                var json = JsonSerializer.Serialize(_quizes, new JsonSerializerOptions() { WriteIndented = true });
                sw.WriteLine(json);

            }

        }
    }
}
