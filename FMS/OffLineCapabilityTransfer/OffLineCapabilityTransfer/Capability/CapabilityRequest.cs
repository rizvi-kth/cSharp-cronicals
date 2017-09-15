using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DemoUtilities;
using FlxDotNetClient;
//using IdentityData;


namespace OffLineCapabilityTransfer.Capability
{
    public enum CapabilityRequestMood
    {
        LicenseAquisationMood,
        LicensePreviewMood
    }

    public enum RequestType
    {   
        GenerateCapabilityRequest,
        ProcessCapabilityResponse,
        SendCapabilityRequest,
        ShowTrustedStorage,
        AcquireLicense,
        GenerateConformationCapabilityRequest
    }

    public class CapabilityRequest
    {
        private const string ProcessingCapabilityResponse = "Processing capability response";
        private const string AcquiringLicense = "Acquiring license";

        private const string AttemptingAcquire = "Attempting to acquire license for feature '{0}' version '{1}' count {2}";

        private const string LicenseAcquired = "License acquisition for feature '{0}' version '{1}' count {2} successful";

        private const string AttemptingReturn = "Attempting to return license for feature '{0}' version '{1}'  count {2}";
        private const string LicenseReturned = "License for feature '{0}' version '{1}' count {2} successfully returned";
        private const string Version = "1.0";

        private static ILicensing _licensing;
        public RequestType _requestType;
        public string _fileName = string.Empty;
        private static string _serverUrl = string.Empty;
        private string HostID = string.Empty;
        private string TrustedStoragePath = string.Empty;

        private static readonly string CurrentFeature = string.Empty;

        public string Feature;
        public string ActivationCode;
        public int FeatureCount;
        public CapabilityRequestMood capabilityRequestMood;

        public CapabilityRequest(string p_serverUrl, CapabilityRequestMood p_Capability_Request_Mood, string p_Feature, int p_FeatureCount, string p_HostId, string p_TrustedStoragePath)
        {

            _requestType = RequestType.SendCapabilityRequest;
            _serverUrl = p_serverUrl;
            capabilityRequestMood = p_Capability_Request_Mood;
            Feature = p_Feature;
            FeatureCount = p_FeatureCount;
            HostID = p_HostId;
            TrustedStoragePath = p_TrustedStoragePath;

        }

        public void ExecuteCommand()
        {
            try
            {
                // Initialize ILicensing interface with identity data using the Windows common document 
                // respository as the trusted storage location and the hard-coded string hostid "1234567890".
                // 8436091056A8710001
                // ToolsTalk
                using (_licensing = LicensingFactory.GetLicensing(
                    IdentityClient.IdentityData,
                    TrustedStoragePath,
                    HostID))
                {
                    // The optional host name is typically set by a user as a friendly name for the host.  
                    // The host name is not used for license enforcement.                  
                    //_licensing.LicenseManager.HostName = "Test Sample Device";
                    // The host type is typically a name set by the implementer, and is not modifiable by the user.
                    // While optional, the host type may be used in certain scenarios by some back-office systems such as FlexNet Operations.
                    //_licensing.LicenseManager.HostType = "Test Sample Device Type";
                    _licensing.LicenseManager.SetHostId(HostIdEnum.FLX_HOSTID_TYPE_ETHERNET, HostID);
                    switch (_requestType)
                    {
                        case RequestType.ShowTrustedStorage:
                            ShowTsFeatures();
                            break;
                        case RequestType.GenerateCapabilityRequest:
                            GenerateCapabilityRequest();
                            break;
                        case RequestType.GenerateConformationCapabilityRequest:
                            GenerateConformationCapabilityRequest();
                            break;
                        case RequestType.ProcessCapabilityResponse:
                            ProcessCapabilityResponse();
                            break;
                        case RequestType.AcquireLicense:
                            AcquireReturn(Feature,Version,FeatureCount);
                            return;
                        case RequestType.SendCapabilityRequest:
                            //SendCapabilityRequest();
                            break;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }

        }

        public List<string> ShowHosts(out string preferedHostId)
        {
            string prfHostID = string.Empty;
            List<string> hosts = new List<string>();
            using (_licensing = LicensingFactory.GetLicensing(
                IdentityClient.IdentityData,
                TrustedStoragePath,
                HostID))
            {
                foreach (var host in _licensing.LicenseManager.HostIds)
                {
                    hosts.Add($"Key:  {host.Key}");
                    host.Value.ForEach((v) =>
                    {
                        hosts.Add($"HostID: {v}");
                        if (prfHostID.Equals(string.Empty))
                        {
                            prfHostID = v;
                        }

                    });
                }
            }
            preferedHostId = prfHostID;
            return hosts;
        }

        private void ShowTsFeatures()
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

        private void GenerateCapabilityRequest()
        {
            // saving the capablity request to a file
            Util.DisplayInfoMessage("Creating the capability request");
            var capabilityRequestData = _licensing.LicenseManager.CreateCapabilityRequest(GenerateRequestOptionsByActivationID(ActivationCode, FeatureCount));
            if (Util.WriteData(_fileName, capabilityRequestData.ToArray()))
                Util.DisplayInfoMessage($"Capability request data written to: {_fileName}");
        }

        private void GenerateConformationCapabilityRequest()
        {
            // saving the capablity request to a file
            Util.DisplayInfoMessage("Creating the capability request");
            capabilityRequestMood = CapabilityRequestMood.LicensePreviewMood;
            var capabilityRequestData = _licensing.LicenseManager.CreateCapabilityRequest(GenerateRequestOptions());
            if (Util.WriteData(_fileName, capabilityRequestData.ToArray()))
                Util.DisplayInfoMessage($"Capability request data written to: {_fileName}");
        }


        private void ProcessCapabilityResponse()
        {
            // read the capability response from a file and process it
            Util.DisplayInfoMessage($"Reading capability response data from: {_fileName}");
            var binCapResponse = Util.ReadData(_fileName);
            if (binCapResponse == null)
                return;
            ProcessCapabilityResponse(binCapResponse);
        }

        private void SendCapabilityRequest()
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

        private void ShowPreviewResponse(byte[] binCapResponse)
        {
            Util.DisplayInfoMessage("Examining preview capability response");
            var response = _licensing.LicenseManager.GetResponseDetails(binCapResponse);
            ShowCapabilityResponseDetails(response);
            ShowCapabilityResponseFeatures(response);
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

        private void ProcessCapabilityResponse(byte[] binCapResponse)
        {
            Util.DisplayInfoMessage("Processing capability response");
            var response = _licensing.LicenseManager.ProcessCapabilityResponse(binCapResponse);
            Util.DisplayInfoMessage("Capability response processed");
            ShowCapabilityResponseDetails(response);
            ShowTsFeatures();
            //AcquireReturn(Feature, Version, FeatureCount);
        }

        private void AcquireReturn(string requestedFeature, string requestedVersion,int featureCount)
        {
            // acquire license
            var currentFeature = requestedFeature;
            Util.DisplayInfoMessage(string.Format(AttemptingAcquire, requestedFeature, requestedVersion, featureCount));
            try
            {
                // To get the Features from a trusted storage we need to add the TS as a License Source.
                _licensing.LicenseManager.AddTrustedStorageLicenseSource();

                // Now the License acquisitioin will be from the TS
                var acquiredLicense = _licensing.LicenseManager.Acquire(requestedFeature, requestedVersion, featureCount);
                try
                {
                    currentFeature = acquiredLicense.Name;
                    Util.DisplayInfoMessage(string.Format(LicenseAcquired, currentFeature, requestedVersion, featureCount));
                    //// application logic here
                }
                finally
                {
                    // return license 
                    Util.DisplayInfoMessage(string.Format(AttemptingReturn, currentFeature, requestedVersion, featureCount));
                    acquiredLicense.ReturnLicense();
                    Util.DisplayInfoMessage(string.Format(LicenseReturned, currentFeature, requestedVersion, featureCount));
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void ShowCapabilityResponseDetails(ICapabilityResponse response)
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
                        $"Status - category: {statusItem.TypeDescription}, code: {(int)statusItem.Code}{(string.IsNullOrEmpty(statusItem.CodeDescription) ? string.Empty : " (" + statusItem.CodeDescription + ")")}, details: {statusItem.Details}");

            // determine whether or not a confirmation request is needed
            Util.DisplayInfoMessage(
                $"Confirmation request is {(response.ConfirmationRequestNeeded ? string.Empty : "not ")}needed");
        }

        private void ShowDictionary(ReadOnlyDictionary dictionary, string dictionaryType)
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

        private ICapabilityRequestOptions GenerateRequestOptionsByActivationID(string ActivationCode, int count)
        {
            // create the capability request options object
            var options = _licensing.LicenseManager.CreateCapabilityRequestOptions();
            // Requesting licenses.
            //
            // Add requested license information here, such as desired features or rights IDs
            //
            options.AddRightsId(ActivationCode, count);

            // force capability response from server even if nothing has changed
            options.ForceResponse = true;

            return options;

        }
        private ICapabilityRequestOptions GenerateRequestOptions()
        {
            // create the capability request options object
            var options = _licensing.LicenseManager.CreateCapabilityRequestOptions();

            // Requesting licenses.
            //
            // Add requested license information here, such as desired features or rights IDs
            //
            //options.AddRightsId("ACT-TEST", 1);
            if (capabilityRequestMood == CapabilityRequestMood.LicenseAquisationMood)
                options.AddDesiredFeature(new FeatureData(Feature, Version, FeatureCount));
            // To return the Feature
            //options.AddDesiredFeature(new FeatureData(Feature, Version, 0));

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
            if (capabilityRequestMood == CapabilityRequestMood.LicensePreviewMood)
                options.Operation = CapabilityRequestOperation.Preview;
            //
            // Add specific features to preview...
            //options.AddDesiredFeature(new FeatureData(Feature, Version, 3));
            //
            // ... or alternatively preview all possible features.
            if (capabilityRequestMood == CapabilityRequestMood.LicensePreviewMood)
                options.RequestAllFeatures = true;
            //

            // Optionally add capability requeest vendor dictionary items.
            //options.AddVendorDictionaryItem(dictionaryKey1, "Some string value");
            //options.AddVendorDictionaryItem(dictionaryKey2, 123);

            // force capability response from server even if nothing has changed
            options.ForceResponse = true;

            return options;
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


    }
}
