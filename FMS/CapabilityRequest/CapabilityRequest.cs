// <copyright file="CapabilityRequest.cs" company="Flexera Software LLC">
//     Copyright (c) 2011-2016 Flexera Software LLC.
//     All Rights Reserved.
//     This software has been provided pursuant to a License Agreement
//     containing restrictions on its use.  This software contains
//     valuable trade secrets and proprietary information of
//     Flexera Software LLC and is protected by law.
//     It may not be copied or distributed in any form or medium, disclosed
//     to third parties, reverse engineered or used in any manner not
//     provided for in said License Agreement except with the prior
//     written authorization from Flexera Software LLC.
// </copyright>

using System;
using System.IO;
using System.Text;
using DemoUtilities;
using FlxDotNetClient;
using IdentityData;

/****************************************************************************
	CapabilityRequest.cs

	This example program allows you to:
	1. Send a capability request via http to the server and process response,
	   saving data into trusted storage.
	2. Write capability request to a file. This request can be fed into server
	   to generate response.
	3. Read capability response from a file and process accordingly.
*****************************************************************************/

namespace CapabilityRequest
{
	public static class CapabilityRequest
	{
		private const string ProcessingCapabilityResponse = "Processing capability response";
		private const string AcquiringLicense = "Acquiring license";

		private const string AttemptingAcquire = "Attempting to acquire license for feature '{0}' version '{1}'";

		private const string LicenseAcquired = "License acquisition for feature '{0}' version '{1}' successful";

		private const string AttemptingReturn = "Attempting to return license for feature '{0}' version '{1}'";
		private const string LicenseReturned = "License for feature '{0}' version '{1}' successfully returned";
		private const string Feature = "VirtualStation";
		private const string Version = "1.0";

		private static readonly string EmptyIdentity =
			@"License-enabled code requires client identity data,
which you create with pubidutil and printbin -CS.
See the User Guide for more information.";

		//private static readonly string dictionaryKey1 = "StringKey";
		//private static readonly string dictionaryKey2 = "Integer Key";

		private static ILicensing _licensing;
		private static RequestType _requestType;
		private static string _fileName = string.Empty;
		private static string _serverUrl = string.Empty;

		private static readonly string CurrentFeature = string.Empty;

		private static bool ValidateCommandLineArgs(string[] args)
		{
			var validCommand = false;
			var invalidSpec = false;
			if (args.Length >= 1)
				switch (args[0].ToLowerInvariant())
				{
					case "-h":
					case "-help":
						break;
					case "-generate":
						if (!(invalidSpec = args.Length != 2))
						{
							_fileName = args[1];
							_requestType = RequestType.GenerateCapabilityRequest;
							validCommand = true;
						}
						break;
					case "-process":
						if (!(invalidSpec = args.Length != 2))
						{
							_fileName = args[1];
							_requestType = RequestType.ProcessCapabilityResponse;
							validCommand = true;
						}
						break;
					case "-server":
						if (!(invalidSpec = args.Length != 2))
						{
							_serverUrl = args[1];
							_requestType = RequestType.SendCapabilityRequest;
							validCommand = true;
						}
						break;
					default:
						Util.DisplayErrorMessage($"unknown option: {args[0]}");
						break;
				}
			if (!validCommand && invalidSpec)
				Util.DisplayErrorMessage($"invalid specification for option: {args[0]}");
			return validCommand;
		}

		public static void Main(string[] args)
		{
			if (!ValidateCommandLineArgs(args))
			{
				Usage(Path.GetFileName(Environment.GetCommandLineArgs()[0]));
				return;
			}

			if ((IdentityClient.IdentityData == null) || (IdentityClient.IdentityData.Length == 0))
			{
				Console.WriteLine(EmptyIdentity);
				return;
			}

			try
			{
				var strPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) +
							  Path.DirectorySeparatorChar + "FMS_test_client_TS" + Path.DirectorySeparatorChar;
				// Initialize ILicensing interface with identity data using the Windows common document 
				// respository as the trusted storage location and the hard-coded string hostid "1234567890".
				using (_licensing = LicensingFactory.GetLicensing(
					IdentityClient.IdentityData,
					strPath,
                    "FmsTestController"))
				{
					// The optional host name is typically set by a user as a friendly name for the host.  
					// The host name is not used for license enforcement.                  
					//_licensing.LicenseManager.HostName = "Test Sample Device";
					// The host type is typically a name set by the implementer, and is not modifiable by the user.
					// While optional, the host type may be used in certain scenarios by some back-office systems such as FlexNet Operations.
					//_licensing.LicenseManager.HostType = "Test Sample Device Type";
                    // How to include Host UUID ??
				    foreach (var host in _licensing.LicenseManager.HostIds)
				    {
                        Console.WriteLine($"Key:  {host.Key}");
				        host.Value.ForEach((v)=> Console.WriteLine($"HostID: {v}"));
				    }
                    _licensing.LicenseManager.SetHostId(HostIdEnum.FLX_HOSTID_TYPE_ETHERNET, "4439C45ABDB0");  
                    ShowTsFeatures();
                    switch (_requestType)
                    {
                        case RequestType.GenerateCapabilityRequest:
                            GenerateCapabilityRequest();
                            break;
                        case RequestType.ProcessCapabilityResponse:
                            ProcessCapabilityResponse();
                            break;
                        case RequestType.SendCapabilityRequest:
                            SendCapabilityRequest();
                            break;
                    }
                }
			}
			catch (Exception exc)
			{
			    
                HandleException(exc);
			    
            }
		    //Console.ReadLine();

        }

		private static void GenerateCapabilityRequest()
		{
			// saving the capablity request to a file
			Util.DisplayInfoMessage("Creating the capability request");
			var capabilityRequestData = _licensing.LicenseManager.CreateCapabilityRequest(GenerateRequestOptions());
            if (Util.WriteData(_fileName, capabilityRequestData.ToArray()))
				Util.DisplayInfoMessage($"Capability request data written to: {_fileName}");
		}

		private static void ProcessCapabilityResponse()
		{
			// read the capability response from a file and process it
			Util.DisplayInfoMessage($"Reading capability response data from: {_fileName}");
			var binCapResponse = Util.ReadData(_fileName);
			if (binCapResponse == null)
				return;
			ProcessCapabilityResponse(binCapResponse);
		}

		private static void SendCapabilityRequest()
		{
			Util.DisplayInfoMessage("Creating the capability request");

			// create the capability request
			var options = GenerateRequestOptions();
			var capabilityRequestData = _licensing.LicenseManager.CreateCapabilityRequest(options);
			Util.DisplayInfoMessage($"Sending the capability request to: {_serverUrl}");
			byte[] binCapResponse = null;


			// send the capability request to the server and receive the server response
			CommFactory.Create(_serverUrl).SendBinaryMessage(capabilityRequestData.ToArray(), out binCapResponse);
			if ((binCapResponse != null) && (binCapResponse.Length > 0))
				Util.DisplayInfoMessage("Response received");
			if (options.Operation != CapabilityRequestOperation.Preview)
				ProcessCapabilityResponse(binCapResponse);
			else
				ShowPreviewResponse(binCapResponse);
		}

		private static void ProcessCapabilityResponse(byte[] binCapResponse)
		{
			Util.DisplayInfoMessage("Processing capability response");
			var response = _licensing.LicenseManager.ProcessCapabilityResponse(binCapResponse);
			Util.DisplayInfoMessage("Capability response processed");
			ShowCapabilityResponseDetails(response);
			ShowTsFeatures();
			AcquireReturn(Feature, Version);
		}

		private static void ShowPreviewResponse(byte[] binCapResponse)
		{
			Util.DisplayInfoMessage("Examining preview capability response");
			var response = _licensing.LicenseManager.GetResponseDetails(binCapResponse);
			ShowCapabilityResponseDetails(response);
			ShowCapabilityResponseFeatures(response);
		}

		private static void ShowCapabilityResponseDetails(ICapabilityResponse response)
		{
			Util.DisplayInfoMessage("Obtaining capability response details");

			// get machine type from response */
			switch (response.VirtualMachineType)
			{
				case MachineTypeEnum.FLX_MACHINE_TYPE_PHYSICAL:
					Util.DisplayInfoMessage("Machine type: PHYSICAL");
					break;
				case MachineTypeEnum.FLX_MACHINE_TYPE_VIRTUAL:
					Util.DisplayInfoMessage("Machine type: VIRTUAL");
					// get virtual machine dictionary from response
					ShowDictionary(response.VirtualMachineInfo, "virtual machine");
					break;
				case MachineTypeEnum.FLX_MACHINE_TYPE_UNKNOWN:
				default:
					Util.DisplayInfoMessage("Machine type: UNKNOWN");
					break;
			}

			// get vendor dictionary from response
			ShowDictionary(response.VendorDictionary, "vendor");

			// get status information
			Util.DisplayInfoMessage(
				$"Capability response contains {response.Status.Count} status item{(response.Status.Count == 1 ? string.Empty : "s")}");
			if (response.Status.Count > 0)
				foreach (var statusItem in response.Status)
					Util.DisplayInfoMessage(
						$"Status - category: {statusItem.TypeDescription}, code: {(int) statusItem.Code}{(string.IsNullOrEmpty(statusItem.CodeDescription) ? string.Empty : " (" + statusItem.CodeDescription + ")")}, details: {statusItem.Details}");

			// determine whether or not a confirmation request is needed
			Util.DisplayInfoMessage(
				$"Confirmation request is {(response.ConfirmationRequestNeeded ? string.Empty : "not ")}needed");
		}

		private static void ShowDictionary(ReadOnlyDictionary dictionary, string dictionaryType)
		{
			Util.DisplayInfoMessage(
				$"Capability response contains {dictionary.Count} {dictionaryType} dictionary item{(dictionary.Count == 1 ? string.Empty : "s")}");
			foreach (var item in dictionary)
			{
				string itemType;
				if (item.Value is string)
					itemType = "string";
				else if (item.Value is int)
					itemType = "integer";
				else
					itemType = "unknown";
				Util.DisplayInfoMessage(
					$"{dictionaryType.Substring(0, 1).ToUpperInvariant() + dictionaryType.Substring(1)} dictionary {itemType} item type: {item.Key}={item.Value}");
			}
		}

		private static void ShowTsFeatures()
		{
			// display the features found in the trusted storage

			var collection = _licensing.LicenseManager.GetFeatureCollection(LicenseSourceOption.TrustedStorage);
			var builder = new StringBuilder();
			builder.Append($"Features loaded from trusted storage: {collection.Count}");

			foreach (var feature in collection)
			{
				builder.AppendLine(string.Empty);
				builder.Append(feature);
			}

			Util.DisplayInfoMessage(builder.ToString());
		}

		private static void ShowCapabilityResponseFeatures(ICapabilityResponse response)
		{
			// display the features found in the capability response
			Util.DisplayInfoMessage("==============================================");
			Util.DisplayInfoMessage(
				$"Features found in {(response.IsPreview ? "preview " : "")}capability response:" +
				Environment.NewLine);

			var collection = response.FeatureCollection;
			var index = 1;
			foreach (var feature in collection)
			{
				var builder = new StringBuilder();
				builder.Append($"{index}: {feature.Name} {feature.Version}");
				builder.Append(
					feature.IsPreview
						? $" TYPE=preview COUNT={(feature.IsUncounted ? "uncounted" : feature.Count.ToString())} MAXCOUNT={(feature.MaxCount == feature.UncountedValue ? "uncounted" : feature.MaxCount.ToString())}"
						: $" COUNT={(feature.IsUncounted ? "uncounted" : feature.Count.ToString())}");
				if (feature.Expiration.HasValue)
					builder.Append($" EXPIRATION={(feature.IsPerpetual ? "permanent" : feature.Expiration.ToString())}");
				if (feature.IsMetered)
					builder.Append(
						$" MODEL=metered{(feature.IsMeteredReusable ? " REUSABLE" : "")}{(feature.MeteredUndoInterval.HasValue ? " UNDO_INTERVAL=" + feature.MeteredUndoInterval : "")}");
				if (!string.IsNullOrEmpty(feature.VendorString))
					builder.Append(" VENDOR_STRING=\"" + feature.VendorString + "\"");
				if (!string.IsNullOrEmpty(feature.Issuer))
					builder.Append(" ISSUER=\"" + feature.Issuer + "\"");
				if (feature.Issued.HasValue)
					builder.Append(" ISSUED=" + feature.Issued);
				if (!string.IsNullOrEmpty(feature.Notice))
					builder.Append(" NOTICE=\"" + feature.Notice + "\"");
				if (!string.IsNullOrEmpty(feature.SerialNumber))
					builder.Append(" SN=\"" + feature.SerialNumber + "\"");
				if (feature.StartDate.HasValue)
					builder.Append(" START=" + feature.StartDate);
				Util.DisplayInfoMessage(builder.ToString());
				index++;
			}
		}

		private static ICapabilityRequestOptions GenerateRequestOptions()
		{
			// create the capability request options object
			var options = _licensing.LicenseManager.CreateCapabilityRequestOptions();

            // Requesting licenses.
            //
            // Add requested license information here, such as desired features or rights IDs
            //
            //options.AddRightsId("ACT-TEST", 1);
            //options.AddRightsId("354E-8694-CAB4-FA13", 0);

            //options.AddDesiredFeature(new FeatureData(Feature, Version, 1));
            // To return the Feature
            //options.AddDesiredFeature(new FeatureData(Feature, Version, 0));
            //options.ForceIncludeUuid = true;

            //
            //
            // Previewing available licenses.
            //
            // If not generating a capability request to be serviced by a back-office license server.
            // you may uncomment the following code to create a preview capability request. The license
            // server will return details for the specified features or, if options.RequestAllFeatures is
            // set to true, return details for all features that could potentially be served to this client.
            //
            // Caution: You may not specify desired features and also set 'request all features' to true
            // on the same preview capability request.
            //
            options.Operation = CapabilityRequestOperation.Preview;
            //
            // Add specific features to preview...
            //options.AddDesiredFeature(new FeatureData(Feature, Version, 3));
            //
            // ... or alternatively preview all possible features.
            options.RequestAllFeatures = true;
            //

            // Optionally add capability requeest vendor dictionary items.
            //options.AddVendorDictionaryItem(dictionaryKey1, "Some string value");
            //options.AddVendorDictionaryItem(dictionaryKey2, 123);

            // force capability response from server even if nothing has changed
            options.ForceResponse = true;

			return options;
		}

		private static void AcquireReturn(string requestedFeature, string requestedVersion)
		{
			// acquire license
			var currentFeature = requestedFeature;
			Util.DisplayInfoMessage(string.Format(AttemptingAcquire, requestedFeature, requestedVersion));
			try
			{
				var acquiredLicense = _licensing.LicenseManager.Acquire(requestedFeature, requestedVersion);
				try
				{
					currentFeature = acquiredLicense.Name;
					Util.DisplayInfoMessage(string.Format(LicenseAcquired, currentFeature, requestedVersion));
					//// application logic here
				}
				finally
				{
					// return license 
					Util.DisplayInfoMessage(string.Format(AttemptingReturn, currentFeature, requestedVersion));
					acquiredLicense.ReturnLicense();
					Util.DisplayInfoMessage(string.Format(LicenseReturned, currentFeature, requestedVersion));
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private static void HandleException(Exception exc)
		{
			var builder = new StringBuilder();
			builder.AppendLine($"{exc.GetType()} encountered: ");
			var flxException = exc as PublicLicensingException;
			if (flxException != null)
				switch (flxException.ErrorCode)
				{
					case ErrorCode.FLXERR_RESPONSE_STALE:
					case ErrorCode.FLXERR_RESPONSE_EXPIRED:
					case ErrorCode.FLXERR_CAPABILITY_RESPONSE_DATA_MISSING:
					case ErrorCode.FLXERR_PREVIEW_RESPONSE_NOT_PROCESSED:
						builder.Append($"{ProcessingCapabilityResponse}: {flxException}");
						break;
					case ErrorCode.FLXERR_FEATURE_NOT_FOUND:
						builder.Append($"{AcquiringLicense} {CurrentFeature}: {flxException}");
						break;
					default:
						builder.Append(flxException);
						break;
				}
			else
				builder.Append(exc.Message);
			Util.DisplayErrorMessage(builder.ToString());
		}

		private static void Usage(string applicationName)
		{
			var builder = new StringBuilder();
			builder.AppendLine(string.Empty);
			builder.AppendLine($"{applicationName} [-generate outputfile]");
			builder.AppendLine($"{applicationName} [-process inputfile]");
			builder.AppendLine($"{applicationName} [-server url]");
			builder.AppendLine(string.Empty);
			builder.AppendLine("where:");
			builder.AppendLine("-generate Generates capability request into a file.");
			builder.AppendLine("-process  Processes capability response from a file.");
			builder.AppendLine("-server   Sends request to a server and processes the response.");
			builder.AppendLine("          For the test back-office server, use");
			builder.AppendLine("          http://hostname:8080/request.");
			builder.AppendLine("          For FlexNet Operations, use");
			builder.AppendLine("          http://hostname:8888/flexnet/deviceservices.");
			builder.AppendLine("          For FlexNet Embedded License Server, use");
			builder.AppendLine("          http://hostname:7070/fne/bin/capability.");

			Util.DisplayMessage(builder.ToString(), "USAGE");
		}

		private enum RequestType
		{
			GenerateCapabilityRequest,
			ProcessCapabilityResponse,
			SendCapabilityRequest
		}
	}
}