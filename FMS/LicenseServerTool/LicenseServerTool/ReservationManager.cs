using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LicenseServerTool.Models;
using RestSharp;

namespace LicenseServerTool
{
    internal class ReservationManager
    {
        private string _localLicenseServer;
        private RestClient _client;
        public ReservationManager()
        {
        }

        public ReservationManager(string _localLicenseServer)
        {
            this._localLicenseServer = _localLicenseServer;
            this._client = new RestClient(_localLicenseServer);
        }

        public async Task<LLS> GetHealthAsync()
        {
            var request = new RestRequest("/api/1.0/health", Method.GET);
            request.RootElement = "LLS";
            var response = await _client.ExecuteTaskAsync<LLS>(request);
            return response.Data;
        }

        public void GetHealth(Action<IRestResponse<LLS>> responseHandler)
        {
            var request = new RestRequest("/api/1.0/health", Method.GET); // health OK
            request.RootElement = "LLS";
            _client.ExecuteAsync<LLS>(request, responseHandler);
            
        }

        public void GetFeatures(Action<IRestResponse<List<FneFeature>>> responseHandler)
        {
            var request = new RestRequest("/api/1.0/features", Method.GET); // GET features
            //request.RootElement = "LLS";
            _client.ExecuteAsync<List<FneFeature>>(request, responseHandler);

        }

        public void GetReservationGroups(Action<IRestResponse<List<ReservationGroup>>> responseHandler)
        {
            var request = new RestRequest("/api/1.0/reservationgroups", Method.GET); // GET reservationgroups?name = PF6000
            request.AddQueryParameter("name", "PF6000");
            //request.RootElement = "LLS";
            _client.ExecuteAsync<List<ReservationGroup>>(request, responseHandler);

        }

        public void GetReservationsForHost(int RGid, Action<IRestResponse<List<FneLicensedDevice>>> responseHandler)
        {
            string Url = $"/api/1.0/reservationgroups/{RGid}/reservations"; // GET reservationgroups/1/reservations?hostid=8436091056A8710001

            var request = new RestRequest(Url, Method.GET);
            request.AddQueryParameter("hostid", "8436091056A8710001");
            //request.RootElement = "LLS";
            _client.ExecuteAsync<List<FneLicensedDevice>>(request, responseHandler);

        }

        public void DeleteReservationEntry(int RGid, int deviceId ,int featureId, Action<IRestResponse> responseHandler)
        {
            string Url = $"/api/1.0/reservationgroups/{RGid}/reservations/{deviceId}/entries/{featureId}"; // DELETE reservationgroups/1/reservations/1/entries/6 Gone

            var request = new RestRequest(Url, Method.DELETE);
            
            _client.ExecuteAsync(request, responseHandler);
        }
        public void DeleteReservationAllEntry(int RGid, int deviceId, List<ReservationEntry> reservationEntries, Action<string> responseHandler)
        {
            string result = "";
            foreach (ReservationEntry reservationEntry in reservationEntries)
            {
                string Url = $"/api/1.0/reservationgroups/{RGid}/reservations/{deviceId}/entries/{reservationEntry.Id}"; // DELETE reservationgroups/1/reservations/1/entries/6 Gone
                var request = new RestRequest(Url, Method.DELETE);
                Task<IRestResponse> currentTask = _client.ExecuteTaskAsync(request);
                if (currentTask.Result.StatusCode == HttpStatusCode.Gone)
                    result += " DELETED ";
                else
                    result += " DELETE-FAIL ";
                
            }

            responseHandler(result);
        }

        public void DeleteReservationAllEntryParallel(int RGid, int deviceId, List<ReservationEntry> reservationEntries, Action<string> responseHandler)
        {
            string result = "";
            List<Task<IRestResponse>> tasks = new List<Task<IRestResponse>>();
            foreach (ReservationEntry reservationEntry in reservationEntries)
            {
                string Url = $"/api/1.0/reservationgroups/{RGid}/reservations/{deviceId}/entries/{reservationEntry.Id}"; // DELETE reservationgroups/1/reservations/1/entries/6 Gone
                var request = new RestRequest(Url, Method.DELETE);
                Task<IRestResponse> currentTask = _client.ExecuteTaskAsync(request);
                tasks.Add(currentTask);
                currentTask.ContinueWith((resp,resultObj) =>
                {
                    if (resp.Result.StatusCode == HttpStatusCode.Gone)
                        Console.WriteLine("---- DELETED ");
                    else
                        Console.WriteLine("---- DELETE-FAIL " + resp.Result.Content);

                }, result);
                
            }
            Task.WhenAll(tasks);
            responseHandler(result);
        }

        public void CreateReservationEntry(int RGid, int deviceId, List<AddReservationEntry> featureEntry, Action<IRestResponse> responseHandler)
        {
            string Url = $"/api/1.0/reservationgroups/{RGid}/reservations/{deviceId}/entries"; // POST reservationgroups/1/reservations/1/entries Created

            var request = new RestRequest(Url, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(featureEntry);

            _client.ExecuteAsync(request, responseHandler);
        }

        
    }
}