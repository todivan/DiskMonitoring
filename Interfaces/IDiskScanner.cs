
using System.Collections.Specialized;

namespace Interfaces;

public interface IDiskScanner
{
    IEnumerable<string> GetDisks();
}

