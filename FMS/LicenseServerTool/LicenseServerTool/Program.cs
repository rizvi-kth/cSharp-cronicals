using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DemoUtilities;
using FlxDotNetClient;
using IdentityData;
using LicenseServerTool.Models;
using RestSharp;
using RestSharp.Deserializers;


namespace LicenseServerTool
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string HostID = "FmsTestController";
            string TS_Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar + "FMS_test_client_TS" + Path.DirectorySeparatorChar;
            string _serverUrl = "http://localhost:7070/fne/bin/capability"; // "http://localhost:7070/request"
            CapabilityRequestMood CapReqMood = CapabilityRequestMood.LicenseAquisationMood;
            // Doesn't matter what is set during LicensePreviewMood
            string Feature = "VirtualStation"; //VirtualStation SoftPLC
            int FeatureCount = 1;
            
            //CapabilityRequest capabilityRequest = new CapabilityRequest(_serverUrl, CapReqMood, Feature, FeatureCount, HostID, TS_Path);

            string _localLicenseServer = "http://localhost:7070";
            var reservationManager = new ReservationManager(_localLicenseServer);

            for (int i = 0; i < 10; i++) { 
                Console.WriteLine($"======================== Request {i}");
                MakeSomeReservations(reservationManager);
                Thread.Sleep(4000); // GOAL is to remove this sleep.
            }

            //Console.WriteLine(resp.Content);

            Console.WriteLine("### Finished ### ");
            Console.ReadLine();
        }

        private static void MakeSomeReservations(ReservationManager reservationManager)
        {
            //IRestResponse resp = reservationManager.GetHealth();
            // REQUEST HEALTH
            reservationManager.GetHealth(response =>
            {
                if(response.StatusCode != HttpStatusCode.OK)
                    Console.WriteLine($"--- HEALTH CHECK FAIL {response.Content}");
                LLS _lls = response.Data;
                Console.WriteLine($"\nHealth OK, Build version: {_lls.BuildVersion}");
                // REQUEST FEATURES
                reservationManager.GetFeatures(featureResponse =>
                {
                    if (featureResponse.StatusCode != HttpStatusCode.OK)
                        Console.WriteLine($"--- REQUEST FEATURE-LIST FAIL {featureResponse.Content}");
                    List<FneFeature> featureList = featureResponse.Data;
                    Console.WriteLine($"Features Count: {featureList.Count}");
                    // REQUEST RG
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

                            // REQUEST DELETE RESERVATIONS (DeleteReservationAllEntryParallel/DeleteReservationAllEntry)
                            reservationManager.DeleteReservationAllEntryParallel(
                                reservationGroups[0].Id,
                                reservations[0].Id,
                                reservations[0].ReservationEntries,
                                reservationEntryDeleteResponse =>
                                {
                                    Console.WriteLine($"--- DELETED RESERVATION ENTRIES : {reservationEntryDeleteResponse}");

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
                                            if (reservationEntryCreateResponse.StatusCode == HttpStatusCode.Created)
                                                Console.WriteLine("--- CREATED SOME RESERVATION ENTRIES");
                                            else
                                                Console.WriteLine($"--- CREATED RESERVATION FAIL {reservationEntryCreateResponse.Content}");
                                        });
                                });

                            





                        });
                    });
                });
            });


        }
    }
}
