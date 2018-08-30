using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OffLineCapabilityTransfer.Capability;
using System.IO;

namespace OffLineCapabilityTransfer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string HostID = string.Empty; // "ToolsTalk"; //string.Empty; // "TestStandAlone"
        string TS_Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + System.IO.Path.DirectorySeparatorChar + "FMS_test_client_TS" + System.IO.Path.DirectorySeparatorChar;
        // ToolsTalk Trusted Storage
        //string TS_Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + System.IO.Path.DirectorySeparatorChar ;
        string _serverUrl = "http://localhost:7070/fne/bin/capability"; // "http://localhost:7070/request"
        CapabilityRequestMood CapReqMood = CapabilityRequestMood.LicenseAquisationMood;
        
        // Doesn't matter what is set during LicensePreviewMood
        string Feature = string.Empty; //VirtualStation SoftPLC
        int FeatureCount = 0;

        public MainWindow()
        {
            InitializeComponent();
            TS_location.Content = $"Trusted Storage Location: {TS_Path}";
            CapabilityRequest capabilityRequest = new CapabilityRequest(_serverUrl, CapReqMood, Feature, FeatureCount, HostID, TS_Path);

            var hosts = capabilityRequest.ShowHosts(out HostID);
            hosts.ForEach((h)=> listHostIds.Items.Add(h));
            lblHostId.Content = $"Using Host ID: {HostID}";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CapabilityRequest capabilityRequest = new CapabilityRequest(_serverUrl, CapReqMood, Feature, FeatureCount, HostID, TS_Path);
            capabilityRequest._requestType = RequestType.ShowTrustedStorage;
            capabilityRequest.ExecuteCommand();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            CapabilityRequest capabilityRequest = new CapabilityRequest(_serverUrl, CapReqMood, Feature, FeatureCount, HostID, TS_Path);
            capabilityRequest._fileName = txtFileName.Text.Trim();
            capabilityRequest._requestType = RequestType.GenerateCapabilityRequest;
            capabilityRequest.ActivationCode = txtActivationId.Text;
            capabilityRequest.FeatureCount = Int32.Parse(txtFeatureCount.Text);
            capabilityRequest.ExecuteCommand();

        }

        private void BtnProcessResponse_OnClick(object sender, RoutedEventArgs e)
        {
            //Feature = txtAcureFeature.Text;
            CapabilityRequest capabilityRequest = new CapabilityRequest(_serverUrl, CapReqMood, Feature, FeatureCount, HostID, TS_Path);
            capabilityRequest._fileName = txtResponseFileName.Text.Trim();
            capabilityRequest._requestType = RequestType.ProcessCapabilityResponse;
            capabilityRequest.ExecuteCommand();

        }

        private void BtnAcquireFeature_OnClick(object sender, RoutedEventArgs e)
        {
            CapabilityRequest capabilityRequest = new CapabilityRequest(_serverUrl, CapReqMood, Feature, FeatureCount, HostID, TS_Path);
            capabilityRequest._fileName = txtResponseFileName.Text.Trim();
            capabilityRequest._requestType = RequestType.AcquireLicense;
            capabilityRequest.Feature = txtAcureFeature.Text;
            capabilityRequest.FeatureCount = Int32.Parse(txtAcquireFeatureCount.Text); 
            capabilityRequest.ExecuteCommand();

        }

        private void BtnCreateConformationRequest_OnClick(object sender, RoutedEventArgs e)
        {
            CapabilityRequest capabilityRequest = new CapabilityRequest(_serverUrl, CapReqMood, Feature, FeatureCount, HostID, TS_Path);
            capabilityRequest._fileName = txtCapReqConfFileName.Text.Trim();
            capabilityRequest._requestType = RequestType.GenerateConformationCapabilityRequest;
            //capabilityRequest.ActivationCode = txtActivationId.Text;
            //capabilityRequest.FeatureCount = Int32.Parse(txtFeatureCount.Text);
            capabilityRequest.ExecuteCommand();

        }
    }
}
