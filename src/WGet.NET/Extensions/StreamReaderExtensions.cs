//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WGetNET.Extensions
{
    /// <summary>
    /// The <see langword="static"/> <see cref="StreamReaderExtensions"/> class,
    /// provieds extension methods for the <see cref="System.IO.StringReader"/>.
    /// </summary>
    internal static class StreamReaderExtensions
    {
        /// <summary>
        /// Reads the data from the process output to a string array.
        /// </summary>
        /// <param name="output">
        /// The <see cref="StreamReader"/> with the process output.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> array 
        /// containing the process output stream content by lines.
        /// </returns>
        public static string[] ReadSreamOutputByLine(this StreamReader output)
        {
            string[] outputArray = Array.Empty<string>();

            //Read output to list
            while (!output.EndOfStream)
            {
                string? outputLine = output.ReadLine();
                if (outputLine is null)
                {
                    continue;
                }

                outputArray = outputArray.Add(outputLine);
            }

            return outputArray;
        }

        /// <summary>
        /// Asynchronous reads the data from the process output to a string array.
        /// </summary>
        /// <param name="output">
        /// The <see cref="System.IO.StreamReader"/> with the process output.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="System.Threading.CancellationToken"/> for the <see cref="System.Threading.Tasks.Task"/>.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> array 
        /// containing the process output stream content by lines.
        /// </returns>
        public static async Task<string[]> ReadSreamOutputByLineAsync(this StreamReader output, CancellationToken cancellationToken = default)
        {
            string[] outputArray = Array.Empty<string>();

            //Read output to list
            while (!output.EndOfStream)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                string? outputLine = await output.ReadLineAsync();
                if (outputLine is null)
                {
                    continue;
                }

                outputArray = outputArray.Add(outputLine);
            }

            return outputArray;
        }
    }
}
