using System.Collections.ObjectModel;
using System.IO;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CursorInstaller;

namespace CursorInsraller;

public sealed partial class MainViewModel : ObservableObject
{
    public ObservableCollection<CursorInfo> Cursors { get; } = [];
    [ObservableProperty]
    private CursorInfo? _current;

    public MainViewModel()
    {
        var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cursors");
        if (!Directory.Exists(dir))
            return;

        Cursors.Clear();
        foreach (var item in Directory.GetDirectories(dir)
            .Select(i => new CursorInfo(i))
            .OrderBy(i => i.Name))
            Cursors.Add(item);

        Current = Cursors.FirstOrDefault();
    }
}
