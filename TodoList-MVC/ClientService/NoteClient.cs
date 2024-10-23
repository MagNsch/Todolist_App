using Microsoft.AspNetCore.Http.HttpResults;
using RestSharp;
using System.Collections.ObjectModel;
using System.Net;
using TodoList_MVC.Models;

namespace TodoList_MVC.ClientService
{
    public class NoteClient : INoteClient
    {
        private readonly string url = "https://localhost:7083";

        public RestClient client;

        public NoteClient()
        {
            client = new RestClient(url);
        }

        public async Task<string?> CreateNoteAsync(Note note)
        {
            var request = new RestRequest("createnote", Method.Post)
                            .AddJsonBody(note);

            var response = await client.ExecuteAsync<string>(request);

            return response.StatusCode switch
            {
                HttpStatusCode.NotFound => null,
                HttpStatusCode.OK => response.Data,
                _ => string.Empty
            };
        }



        public async Task<bool> DeleteNoteAsync(string bsonString)
        {
            var request = new RestRequest($"deletenote/{bsonString}", Method.Delete);
            var response = await client.ExecuteAsync(request);

            if(response != null) { return true; }
            
            return false;
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            var request = new RestRequest("/getallnotes", Method.Get);
            var response = await client.ExecuteAsync<IEnumerable<Note>>(request);
            return response.Data;
        }

        public async Task<Note> GetByIdAsync(string id)
        {
            var request = new RestRequest($"getnote/{id}", Method.Get);
            var response = await client.ExecuteAsync<Note>(request);
            return response.Data;
        }
    }
}
