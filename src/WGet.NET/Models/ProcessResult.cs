//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
namespace WGetNET
{
    internal class ProcessResult
    {
        /// <summary>
        /// Gets or sets the exit code of the process.
        /// </summary>
        public int ExitCode { get; set; }
        /// <summary>
        /// Gets or sets the output of the process.
        /// </summary>
        public string[] Output { get; set; }
        /// <summary>
        /// Gets if the process finished successfully.
        /// </summary>
        public bool Success
        {
            get
            {
                if (ExitCode == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
