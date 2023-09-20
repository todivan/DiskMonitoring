namespace Interfaces;

public interface IVolumes
{
    Task ShowVolumes(IEnumerable<string> volumes);
}
