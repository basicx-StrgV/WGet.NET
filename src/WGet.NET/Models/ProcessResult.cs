//--------------------------------------------------//
// Created by basicx-StrgV                          //
// https://github.com/basicx-StrgV/                 //
//--------------------------------------------------//
using System.Collections.Generic;

namespace WGetNET
{
    internal class ProcessResult
    {
        public int ExitCode { get; set; }
        public List<string> Output { get; set; }
    }
}
