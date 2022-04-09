using TinyTODO.Core.DataModel;

namespace TinyTODO.Core.Contracts;

public interface IClipboardDataProvider
{
    ClipboardData? GetData();
}
