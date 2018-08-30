using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CommonMessages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlxDotNetClient;
using LicenseServerTool.Models;
using RestSharp;
using RestSharp.Deserializers;


namespace RecieverTool
{
    internal class AcquireLicenseCommandHandler: IConsumer<AcquireLicenseCommand>
    {
        private readonly Object thisLock = new Object();
        public Task Consume(ConsumeContext<AcquireLicenseCommand> context)
        {
            var reservationGroupID = 2;
            var reservation_for_device_ID = 2;


            Console.Out.WriteLine("\n===========================");
            Console.Out.WriteLine($"[Consumer] OrderSequence: {context.Message.OrderSequence}");
            var headers = context.Headers;
            //Console.Out.WriteLine($"Headers count: {headers.GetAll().Count()} :");
            foreach (var keyValuePair in headers.GetAll())
            {
            //    Console.Out.WriteLine($"{keyValuePair.Key} : {keyValuePair.Value}");
            }
            //Console.Out.WriteLine($"Context corelationID: {context.CorrelationId?.ToString()}");
            //Console.Out.WriteLine($"Message corelationID: <Not implemented> :");

            string _localLicenseServer = "http://localhost:7070";
            //var reservationManager = new ReservationManager(_localLicenseServer);
            //lock (thisLock)
            //{
            //MakeSomeReservations(reservationManager);
            //Thread.Sleep(4000); // GOAL is to remove this sleep.
            //}

            var _client = new RestClient(_localLicenseServer);
            string Url = $"/api/1.0/reservationgroups/{reservationGroupID}/reservations"; // GET reservationgroups/2/reservations?hostid=8436091056A8710001

            var request = new RestRequest(Url, Method.GET);
            request.AddQueryParameter("hostid", "8436091056A8710001");
            //request.RootElement = "LLS";
            var reservationsResponse = _client.Execute<List<FneLicensedDevice>>(request);


            List<FneLicensedDevice> reservations = reservationsResponse.Data;
            if (reservations.Count > 0)
            {
                reservations.ForEach(resv =>
                {
                    Console.WriteLine($"-- Reservations for host/device: {resv.HostId} -- ID {resv.Id}  ");
                    resv.ReservationEntries.ForEach(resvEnt =>
                    {
                        Console.WriteLine($"--- Feature Id: {resvEnt.Id}  Name: {resvEnt.FeatureName}  FeatureCount: ({resvEnt.FeatureCount})");
                        string Url1 = $"/api/1.0/reservationgroups/{reservationGroupID}/reservations/{reservation_for_device_ID}/entries/{resvEnt.Id}"; // DELETE reservationgroups/2/reservations/2/entries/6 Gone
                        var delRequest = new RestRequest(Url1, Method.DELETE);
                        var delResponse = _client.Execute(delRequest);
                        if (delResponse.StatusCode == HttpStatusCode.Gone)
                        {
                            Console.WriteLine("---- DELETED ");
                        }
                        else
                            Console.WriteLine("---- DELETE-FAIL ");
                    });

                });
                CreateReservationEntryStraitht(_client, reservationGroupID, reservation_for_device_ID);

            }
            else
            {
                Console.WriteLine("No reservation for host.");
            }

            return Task.FromResult(0);


        }

        public void CreateReservationEntryStraitht(RestClient _client, int RGid, int deviceId)
        {
            // REQUEST CREATE RESERVATION
            List<AddReservationEntry> featureEntry = new List<AddReservationEntry>
            {
                new AddReservationEntry()
                {
                    featureCount = 1,
                    featureVersion = "1.0",
                    featureName = "VirtualStation"
                },
                new AddReservationEntry()
                {
                    featureCount = 1,
                    featureVersion = "1.0",
                    featureName = "SoftPLC"
                },
                new AddReservationEntry()
                {
                    featureCount = 1,
                    featureVersion = "1.0",
                    featureName = "Yield"
                },
            };

            string Url = $"/api/1.0/reservationgroups/{RGid}/reservations/{deviceId}/entries"; // POST reservationgroups/2/reservations/2/entries Created

            var request = new RestRequest(Url, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(featureEntry);

            var currentTask = _client.Execute(request);
            if (currentTask.StatusCode == HttpStatusCode.Created)
            {
                Console.WriteLine("-- CREATED SOME RESERVATION ENTRIES");
            }
            else
            {
                Console.WriteLine($"-- CREATED RESERVATION FAIL {currentTask.Content}");
            }
        }




        private void MakeSomeReservations(ReservationManager reservationManager)
        {
            //IRestResponse resp = reservationManager.GetHealth();
            // REQUEST HEALTH
            reservationManager.GetHealth(response =>
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.WriteLine($"--- HEALTH CHECK FAIL {response.Content}");
                LLS _lls = response.Data;
                Console.WriteLine($"\nHealth OK, Build version: {_lls.BuildVersion}");
                //REQUEST FEATURES
                reservationManager.GetFeatures(featureResponse =>
                {
                    if (featureResponse.StatusCode != HttpStatusCode.OK)
                        Console.WriteLine($"--- REQUEST FEATURE-LIST FAIL {featureResponse.Content}");
                    List<FneFeature> featureList = featureResponse.Data;
                    Console.WriteLine($"Features Count: {featureList.Count}");
                        //REQUEST RG
                        reservationManager.GetReservationGroups(reservationGroupResponse =>
                        {
                            if (reservationGroupResponse.StatusCode != HttpStatusCode.OK)
                                Console.WriteLine($"--- REQUEST RESERVATION-GROUP FAIL {reservationGroupResponse.Content}");
                            List<ReservationGroup> reservationGroups = reservationGroupResponse.Data;
                            Console.WriteLine($"RG Count: {reservationGroups.Count}");
                            // REQUEST DEVICE-RESERVATION 
                            reservationManager.GetReservationsForHost(reservationGroups[0].Id, reservationsResponse =>
                            {
                                if (reservationsResponse.StatusCode != HttpStatusCode.OK)
                                    Console.WriteLine($"--- REQUEST DEVICE-RESERVATION FAIL {reservationsResponse.Content}");
                                Console.WriteLine($"- RG ID: {reservationGroups[0].Id}");
                                List<FneLicensedDevice> reservations = reservationsResponse.Data;
                                if (reservations.Count > 0)
                                {
                                    reservations.ForEach(resv =>
                                    {
                                        Console.WriteLine($"-- Reservations for host/device: {resv.HostId} -- ID {resv.Id}  ");
                                        resv.ReservationEntries.ForEach(resvEnt =>
                                        {
                                            Console.WriteLine($"--- Feature Id: {resvEnt.Id}  Name: {resvEnt.FeatureName}  FeatureCount: ({resvEnt.FeatureCount})");
                                        });
                                    });
                                }
                                else
                                {
                                    Console.WriteLine("No reservation for host.");
                                }

                                // REQUEST DELETE RESERVATIONS
                                reservationManager.DeleteReservationAllEntry(
                                    reservationGroups[0].Id,
                                    reservations[0].Id,
                                    reservations[0].ReservationEntries,
                                    reservationEntryDeleteResponse =>
                                    {
                                        Console.WriteLine(
                                            $"--- DELETED RESERVATION ENTRIES : {reservationEntryDeleteResponse}");

                                        // REQUEST CREATE RESERVATION
                                        List<AddReservationEntry> featureEntry = new List<AddReservationEntry>
                                        {
                                            new AddReservationEntry()
                                            {
                                                featureCount = 1,
                                                featureVersion = "1.0",
                                                featureName = "VirtualStation"
                                            },
                                            new AddReservationEntry()
                                            {
                                                featureCount = 1,
                                                featureVersion = "1.0",
                                                featureName = "SoftPLC"
                                            },
                                            new AddReservationEntry()
                                            {
                                                featureCount = 1,
                                                featureVersion = "1.0",
                                                featureName = "Yield"
                                            },
                                        };
                                        reservationManager.CreateReservationEntry(
                                            reservationGroups[0].Id,
                                            reservations[0].Id,
                                            featureEntry,
                                            reservationEntryCreateResponse =>
                                            {
                                                Console.WriteLine(
                                                    $"--- CREATED RESERVATION FAIL {reservationEntryCreateResponse}");
                                            });
                                    });

                            });
                        });
                });
                });


        }
    }
}