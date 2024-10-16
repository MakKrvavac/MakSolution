using System.Text;
using Newtonsoft.Json;

namespace MakTest.Helper;

internal class HttpHelper
{
    public static StringContent GetStringContent(object obj)
    {
        return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
    }

    internal static class Urls
    {
        public const string Register = "/api/auth/register";
        public const string Login = "/api/auth/login";

        public const string GetAllTasks = "/api/tasks";
        public const string GetTaskById = "/api/tasks/{0}";
        public const string CreateTask = "/api/tasks";
        public const string UpdateTask = "/api/tasks/{0}";
        public const string DeleteTask = "/api/tasks/{0}";

        public const string GetAllProjects = "/api/projects";
        public const string GetProjectById = "/api/projects/{0}";
        public const string CreateProject = "/api/projects";
        public const string UpdateProject = "/api/projects/{0}";
        public const string DeleteProject = "/api/projects/{0}";
    }

    internal static class Credentials
    {
        public const string Username = "testuser";
        public const string Password = "test@123";
    }
}