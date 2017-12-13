using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.DependencyModel;
using Xunit;

namespace RendleLabs.Diagnostics.DiagnosticSourceExtensions.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ShouldNotCreateObjectIfNotEnabled()
        {
            object shouldBeNull = null;
            DiagnosticSource diagnosticSource = new DiagnosticListener(nameof(ShouldNotCreateObjectIfNotEnabled));
            diagnosticSource.IfEnabled("Nope")?.Write("Fail", shouldBeNull = new { failed = true});
            Assert.Null(shouldBeNull);

            var a = new StringReader("");
            var b = new StringWriter(new StringBuilder());
            
            (a,b).Dispose();
        }
    }

    public static class TupleAbuse
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dispose(this (IDisposable a, IDisposable b) disposeables)
        {
            SafeDispose(disposeables.a);
            SafeDispose(disposeables.b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SafeDispose(IDisposable d)
        {
            try
            {
                d?.Dispose();
            }
            catch
            {
                // Ignored
            }
        }
    }
}