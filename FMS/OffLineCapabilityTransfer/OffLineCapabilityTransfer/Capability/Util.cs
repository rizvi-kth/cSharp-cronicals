// <copyright file="Util.cs" company="Flexera Software LLC">
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

// Uncomment to receive output in a message box.
//// #define UI_OUTPUT

using System; 
using System.IO;
#if UI_OUTPUT
using System.Windows;
#endif // UI_OUTPUT

namespace OffLineCapabilityTransfer.Capability
{
    /// <summary>
    /// The Util class is a simple wrapper around some common methods
    /// used by multiple demonstration projects.
    /// </summary>
    public class Util
    {
        private static readonly string invalidReadFile = "Unable to find file {0}";
        private static readonly string invalidWriteFile = "Unable to open file {0}";
        private static readonly string readIOError = "Unable to read data from file {0}";
        private static readonly string writeIOError = "Unable to write data to file {0}";
        private static readonly string accessError = "Unable to access file {0}";

        private static Util instance = new Util();

        private Util()
        {
        }

        public static Util Instance
        {
            get { return instance; }
        }


        public static void DisplayErrorMessage(string msg)
        {
            DisplayMessage(msg, "ERROR");
        }

        public static void DisplayInfoMessage(string msg)
        {
            DisplayMessage(msg, "INFO");
        }

        public static void DisplayMessage(string msg)
        {
            DisplayMessage(msg, null);
        }

        public static void DisplayMessage(string msg, string caption)
        {
#if UI_OUTPUT
                if (!String.IsNullOrEmpty(msg))
                {
                    MessageBox.Show(msg, caption, MessageBoxButton.OK);
                }
#else // !UI_OUTPUT
            if (String.IsNullOrEmpty(caption))
            {
                Console.Out.WriteLine(msg);
            }
            else
            {
                Console.Out.WriteLine("{0}: {1}", caption, msg);
            }
            Console.Out.Flush();
#endif // UI_OUTPUT
        }

        public static byte[] ReadData(string fileName)
        {
            try
            {
                return File.ReadAllBytes(fileName);
            }
            catch (Exception exc)
            {
                if (exc is ArgumentException ||
                    exc is ArgumentNullException ||
                    exc is PathTooLongException ||
                    exc is DirectoryNotFoundException ||
                    exc is FileNotFoundException ||
                    exc is NotSupportedException)
                {
                    DisplayErrorMessage(String.Format(invalidReadFile, fileName));
                }

                else if (exc is IOException)
                {
                    DisplayErrorMessage(String.Format(readIOError, fileName));
                }
                else
                {
                    DisplayErrorMessage(String.Format(accessError, fileName));
                }
                return null;
            }
        }

        public static string ReadText(string fileName)
        {
            try
            {
                return File.ReadAllText(fileName);
            }
            catch (Exception exc)
            {
                if (exc is ArgumentException ||
                    exc is ArgumentNullException ||
                    exc is PathTooLongException ||
                    exc is DirectoryNotFoundException ||
                    exc is FileNotFoundException ||
                    exc is NotSupportedException)
                {
                    DisplayErrorMessage(String.Format(invalidReadFile, fileName));
                }

                else if (exc is IOException)
                {
                    DisplayErrorMessage(String.Format(readIOError, fileName));
                }
                else
                {
                    DisplayErrorMessage(String.Format(accessError, fileName));
                }
                return null;
            }
        }

        public static bool WriteData(string fileName, byte[] data)
        {
            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        writer.Write(data);
                        writer.Close();
                    }
                }
                return true;
            }
            catch (Exception exc)
            {
                if (exc is ArgumentException ||
                    exc is ArgumentNullException ||
                    exc is PathTooLongException ||
                    exc is DirectoryNotFoundException ||
                    exc is FileNotFoundException ||
                    exc is NotSupportedException)
                {
                    DisplayErrorMessage(String.Format(invalidWriteFile, fileName));
                }

                else if (exc is IOException)
                {
                    DisplayErrorMessage(String.Format(writeIOError, fileName));
                }
                else
                {
                    DisplayErrorMessage(String.Format(accessError, fileName));
                }
                return false;
            }
        }
    }
}
