// <copyright file="BasicClient.cs" company="Flexera Software LLC">
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
using FlxDotNetClient;
using IdentityData;
using DemoUtilities;

/****************************************************************************
    BasicClient.cs

    This example program illustrates the general sequence of functions
    required to acquire "survey" and "highres" features from a buffer
    license source.  For compilation and usage instructions, see the
    FlexNet Embedded Getting Started Guide.

    Note that this example is intended ONLY as example code to verify that
    the basic system will compile and run in a particular environment.  For
    more comprehensive examples that illustrate additional license sources,
    cleanup during error conditions, etc., see the "Client" and other
    samples provided in the "examples" directory of the FlexNet Embedded
    toolkit.

*****************************************************************************/


namespace BasicClient
{
    public static class BasicClient
    {
        private static readonly string emptyIdentity = 
@"License-enabled code requires client identity data,
which you create with pubidutil and printbin -CS.
See the User Guide for more information.";

        private static readonly string surveyFeature = "Line_Manager";
        private static readonly string highresFeature = "Yield";
        private static readonly string version = "1.0";

        private static string inputFile = @".\Capab_resp_FromBackOffc_000EC6F975A6_short.bin";
        private static ILicensing licensing = null;

        private static bool ValidateCommandLineArgs(string[] args)
        {
            bool validCommand = false;
            if (args.Length == 0)
            {
                Util.DisplayInfoMessage(String.Format("Using default license file {0}", inputFile));
                validCommand = true;
            }
            else
            {
                string argument = args[0].ToLowerInvariant();
                if (argument != "-h" && argument != "-help")
                {
                    validCommand = true;
                    inputFile = args[0];
                }
            }
            return validCommand;
        }

        public static void Main(string[] args)
        {

            if (!ValidateCommandLineArgs(args))
            {
                Usage(Path.GetFileName(Environment.GetCommandLineArgs()[0]));
                return;
            }

            if (IdentityClient.IdentityData == null || IdentityClient.IdentityData.Length == 0)
            {
                Console.WriteLine(emptyIdentity);
                return;
            }

            try
            {
                Util.DisplayInfoMessage(String.Format("Reading data from {0}", inputFile));
                byte[] buffer = Util.ReadData(inputFile);
                if (buffer == null)
                {
                    // issue encountered accessing input file
                    return;
                }

                string path_my = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) +
                              Path.DirectorySeparatorChar + "Testing_trusted_store" + Path.DirectorySeparatorChar; 
                // Initialize ILicensing interface with identity data using memory-based trusted 
                // storage and the hard-coded string hostid "1234567890".
                using (licensing = LicensingFactory.GetLicensing(
                          IdentityClient.IdentityData,
                          path_my,
                          "1234567890"))
                {
                    Console.WriteLine($"Device Name: {licensing.LicenseManager.DeviceName}   Type: {licensing.LicenseManager.DeviceType}");
                    Console.WriteLine($"Host ID: {licensing.LicenseManager.HostId}   Type: {licensing.LicenseManager.HostIdType.ToString()}");
                    Console.WriteLine($"Feature Count: {licensing.LicenseManager.GetFeatureCollection().Count}   Licenses: {licensing.LicenseManager.Licenses().Count}");
                    // add a Buffer license source
                    licensing.LicenseManager.AddBufferLicenseSource(buffer, "BasicClientLicenseSource");

                    // Acquire and return 1 "survey" license
                    AcquireReturn(surveyFeature, version);

                    // Acquire and return 1 "highres" license
                    AcquireReturn(highresFeature, version);
                }
            }
            catch (DllNotFoundException)
            { 
                // Not a typical error. Make sure the native FlxCore library is available
                Util.DisplayErrorMessage(String.Format("Failed to load FlxCore{0}.dll. Check the PATH specification.",
                    IntPtr.Size == 4 ? String.Empty : "64"));
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            Console.ReadLine();
        }

        private static void AcquireReturn(string requestedFeature, string requestedVersion)
        {
            try
            {
                // acquire the license
                ILicense acquiredLicense = licensing.LicenseManager.Acquire(requestedFeature, requestedVersion);
                try
                {
                    Util.DisplayInfoMessage(String.Format("License acquisition for feature '{0}' version '{1}' successful", requestedFeature, acquiredLicense.Version));
                    //// application logic here
                }
                finally
                {
                    // return license. note: it is also possible to use a "using" statement to implicitly return the license
                    acquiredLicense.ReturnLicense();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
 
        private static void HandleException(Exception exc)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Format("{0} encountered: ", exc.GetType()));
            FlxException flxException = exc as FlxException;
            if (flxException != null)
            {
                builder.Append(flxException.ToString());
            }
            else
            {
                builder.Append(exc.Message);
            }
            Util.DisplayErrorMessage(builder.ToString());
        }

        private static void Usage(string applicationName)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Empty);
            builder.AppendLine(String.Format("{0} [binary_license_file]", applicationName));
            builder.AppendLine("Attempts to acquire 'survey' and 'highres' features from a signed");
            builder.AppendLine("binary license file.");
            builder.AppendLine(String.Empty);
            builder.AppendLine(String.Format("If unset, default binary_license_file is {0}.", inputFile));

            Util.DisplayMessage(builder.ToString(), "USAGE");
        }
    }
}