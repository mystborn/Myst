using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Myst.Extensions
{
    public static class ProcessExt
    {
        public static async Task<int> WaitForProcessAsync(this Process process)
        {
            var tcs = new TaskCompletionSource<int>();
            process.Exited += (s, e) =>
            {
                tcs.SetResult(((Process)s).ExitCode);
            };

            return await tcs.Task;
        }
    }
}
