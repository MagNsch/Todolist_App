using Newtonsoft.Json;
using NuGet.Common;
using RestSharp;
using System.Net;
using TodoList_MVC.ClientService.Interface;
using TodoList_MVC.Models;

namespace TodoList_MVC.ClientService.Service
{
    public class NoteClient : INoteClient
    {
        private readonly string url = "https://localhost:7083";

        public RestClient _client;

        public NoteClient()
        {
            _client = new RestClient(url);
        }

        public async Task<string?> CreateNoteAsync(Note note, string token)
        {
            var request = new RestRequest("createnote", Method.Post)
                            .AddJsonBody(note).AddHeader("Authorization", $"Bearer {token}");

            var response = await _client.ExecuteAsync<string>(request);

            return response.StatusCode switch
            {
                HttpStatusCode.NotFound => null,
                HttpStatusCode.OK => response.Data,
                _ => string.Empty
            };
        }



        public async Task<bool> DeleteNoteAsync(string bsonString, string token)
        {
            var request = new RestRequest($"deletenote/{bsonString}", Method.Delete).AddHeader("Authorization", $"Bearer {token}");
            var response = await _client.ExecuteAsync(request);

            if (response != null) { return true; }

            return false;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync(string token)
        {
            var request = new RestRequest("/getallnotes", Method.Get).AddHeader("Authorization", $"Bearer {token}");
            var response = await _client.ExecuteAsync<IEnumerable<Note>>(request);
            return response.Data;
        }

        public async Task<Note> GetByIdAsync(string id, string token)
        {
            var request = new RestRequest($"getnote/{id}", Method.Get).AddHeader("Authorization", $"Bearer {token}");
            var response = await _client.ExecuteAsync<Note>(request);
            return response.Data;
        }


    }
}
