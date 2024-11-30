using QuizApp.Models;
using System.Text.Json;

namespace QuizApp.Repository
{
    public class UserRepository
    {
        private readonly string _filePath;
        private List<User> _users;

        public UserRepository(string filepath) {
            _filePath = filepath;
            _users = LoadData();
        }

        public void Register(User user)
        {
            user.Id = _users.Any() ? _users.Max(a => a.Id) + 1 : 1;
            user.HighScore= 0;
            _users.Add(user);
            SaveData();
        }

        public void ShowTop10()
        {
            Console.WriteLine("Here is top 10 users of our application");
            var top10 = _users.OrderByDescending(user => user.HighScore).Take(10).ToList();

            int index =1;

            foreach (var user in top10) 
            {
                Console.WriteLine($" #{index}-{user.Username}, highscore:{user.HighScore}");
                index++;
            }
            Console.WriteLine();
        }


        public int Login(User user)
        {
            var index = _users.FindIndex(a => a.Username == user.Username);
            if (index >= 0)
            {
                return _users[index].Id;
            }
            return 0;
        }

        public void UpdateHighScore(int userId, int highScore) 
        {
            User user=_users.Find(a => a.Id==userId);
            user.HighScore=highScore;
            SaveData();            
        }

        public int GetHighScore(int userId)
        {
            User user = _users.Find(a => a.Id == userId);
            return user.HighScore;
        }

        public bool IsUsernameAvailable(string username)
        {
            if(username.Length == 0)
            {
                throw new Exception("Username must not be empty. Enter new one: ");
            }

            List<string> usernames = _users.Select(a => a.Username).ToList();
            foreach (var user in usernames)
            {
                if (user.Equals(username))
                {
                    throw new Exception("This username is busy. Enter new username: ");
                }
            }


            return true;

        }

        public bool IsUserRegistered(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Not with empty username. Try again: ");
            }

            List<string> RegisteredUsernames = _users.Select(user=>user.Username).ToList();


            //tu useri daregistrirebulia, daabruebs true
            foreach (var item in RegisteredUsernames)
            {
                if (item.Equals(username)) 
                {
                    return true;
                }
                
            }


            //tu ar ari daregistrirebuli, daabrunebs false da motxovs ro swori username sheiyvanos
            throw new Exception("This username is not registered. Enter corrrect one: ");    
            
        }

        private List<User> LoadData()
        {
            if (!File.Exists(_filePath))
                return new List<User>();

            using (StreamReader sr = new(_filePath))
            {
                return JsonSerializer.Deserialize<List<User>>(File.ReadAllText(_filePath));
            }
        }


        private void SaveData()
        {

            using (StreamWriter sw = new StreamWriter(_filePath, append: false))
            {
                var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions() { WriteIndented = true });
                sw.WriteLine(json);

            }

        }

    }
}
