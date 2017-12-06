using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityReport
{
    public class FileReader
    {
        public IEnumerable<Task<string>> ReadAll(IEnumerable<string> paths)
        {
            return paths.Select(Read);
        }

        private static async Task<string> Read(string path)
        {
            using (var reader = File.OpenText(path))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}