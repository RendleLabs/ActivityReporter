using System;
using System.Diagnostics;

namespace RendleLabs.Diagnostics.DiagnosticSourceExtensions
{
    public static class DiagnosticSourceEx
    {
        public static DiagnosticSource IfEnabled(this DiagnosticSource source, string name)
            => source.IsEnabled(name) ? source : null;
    }
}