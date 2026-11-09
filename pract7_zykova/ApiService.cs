using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using pract7_zykova.Models;
using System.Net.Http.Json;

namespace pract7_zykova
{
    class ApiService
    {
        private readonly HttpClient _client = new HttpClient();
        public ApiService()
        {
            _client.BaseAddress = new Uri("https://localhost:7119/");
        }

        public async Task<List<Course>?> GetCourse()
        {
            return await _client.GetFromJsonAsync<List<Course>>("api/v1/Courses");
        }

        //создание
        public async Task CreateCourse(Course course)
        {
            var response = await _client.PostAsJsonAsync("api/v1/Course", course);
            response.EnsureSuccessStatusCode(); 
        }

        //филтрация спислк
        public async Task<List<Course>> GetCourses(string? teacher = null, int? minDuration = null)
        {
            string url = "api/v1/Course?";
            if (!string.IsNullOrWhiteSpace(teacher)) url += $"teacher={teacher}&";
            if (minDuration.HasValue) url += $"minDuration={minDuration}";

            return await _client.GetFromJsonAsync<List<Course>>(url) ?? new List<Course>();
        }

        public async Task UpdateCourse(Course course)
        {
            var response = await _client.PutAsJsonAsync($"api/v1/Course/{course.Id}", course);
            response.EnsureSuccessStatusCode();
        }

        public async Task PatchCourse(int id, object patchData)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(patchData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"api/v1/Course/{id}")
            {
                Content = content
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCourse(int id)
        {
            var response = await _client.DeleteAsync($"api/v1/Course/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
