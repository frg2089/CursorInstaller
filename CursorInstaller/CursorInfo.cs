using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Win32;

namespace CursorInstaller;

public sealed partial class CursorInfo : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;
    [ObservableProperty]
    private Cursor? _wait;
    private readonly string? _waitName;
    [ObservableProperty]
    private Cursor? _upArrow;
    private readonly string? _upArrowName;
    [ObservableProperty]
    private Cursor? _sizeWE;
    private readonly string? _sizeWEName;
    [ObservableProperty]
    private Cursor? _sizeNWSE;
    private readonly string? _sizeNWSEName;
    [ObservableProperty]
    private Cursor? _sizeNS;
    private readonly string? _sizeNSName;
    [ObservableProperty]
    private Cursor? _sizeNESW;
    private readonly string? _sizeNESWName;
    [ObservableProperty]
    private Cursor? _sizeAll;
    private readonly string? _sizeAllName;
    [ObservableProperty]
    private Cursor? _pen;
    private readonly string? _penName;
    [ObservableProperty]
    private Cursor? _no;
    private readonly string? _noName;
    [ObservableProperty]
    private Cursor? _iBeam;
    private readonly string? _iBeamName;
    [ObservableProperty]
    private Cursor? _help;
    private readonly string? _helpName;
    [ObservableProperty]
    private Cursor? _hand;
    private readonly string? _handName;
    [ObservableProperty]
    private Cursor? _cross;
    private readonly string? _crossName;
    [ObservableProperty]
    private Cursor? _arrow;
    private readonly string? _arrowName;
    [ObservableProperty]
    private Cursor? _appStarting;
    private readonly string? _appStartingName;

    private readonly string _directory;

    private static readonly Regex Regex = new(@"\d+_(.+?)(マウス)?カーソル");
    public CursorInfo(string directory)
    {
        _directory = directory;
        var name = Path.GetFileName(directory);
        var match = Regex.Match(name);
        if (match.Success)
            name = match.Groups[1].Value;

        Name = name;
        foreach (var file in Directory.GetFiles(directory, "*.ani"))
        {
            using var fs = File.OpenRead(file);
            Cursor cursor = new(fs);
            switch (Path.GetFileNameWithoutExtension(file))
            {
                case "通常":
                case "通常の選択":
                    Arrow = cursor;
                    _arrowName = file;
                    break;
                case "ヘルプの選択":
                    Help = cursor;
                    _helpName = file;
                    break;
                case "バックグラウンドで作業中":
                    AppStarting = cursor;
                    _appStartingName = file;
                    break;
                case "待ち状態":
                    Wait = cursor;
                    _waitName = file;
                    break;
                case "領域選択":
                    Cross = cursor;
                    _crossName = file;
                    break;
                case "テキスト選択":
                    IBeam = cursor;
                    _iBeamName = file;
                    break;
                case "手書き":
                    Pen = cursor;
                    _penName = file;
                    break;
                case "利用不可":
                    No = cursor;
                    _noName = file;
                    break;
                case "上下に拡大縮小":
                    SizeNS = cursor;
                    _sizeNSName = file;
                    break;
                case "左右に拡大縮小":
                    SizeWE = cursor;
                    _sizeWEName = file;
                    break;
                case "斜めに拡大縮小1":
                    SizeNWSE = cursor;
                    _sizeNWSEName = file;
                    break;
                case "斜めに拡大縮小2":
                    SizeNESW = cursor;
                    _sizeNESWName = file;
                    break;
                case "移動":
                    SizeAll = cursor;
                    _sizeAllName = file;
                    break;
                case "代替選択":
                    UpArrow = cursor;
                    _upArrowName = file;
                    break;
                case "リンクの選択":
                    Hand = cursor;
                    _handName = file;
                    break;
                default:
                    break;
            }
        }
    }

    [RelayCommand]
    public void Install()
    {
        var target = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Cursors", Name);
        Directory.CreateDirectory(target);
        File.Copy(Path.Combine(_directory, _waitName), Path.Combine(target, _waitName), true);
        File.Copy(Path.Combine(_directory, _upArrowName), Path.Combine(target, _upArrowName), true);
        File.Copy(Path.Combine(_directory, _sizeWEName), Path.Combine(target, _sizeWEName), true);
        File.Copy(Path.Combine(_directory, _sizeNWSEName), Path.Combine(target, _sizeNWSEName), true);
        File.Copy(Path.Combine(_directory, _sizeNSName), Path.Combine(target, _sizeNSName), true);
        File.Copy(Path.Combine(_directory, _sizeNESWName), Path.Combine(target, _sizeNESWName), true);
        File.Copy(Path.Combine(_directory, _sizeAllName), Path.Combine(target, _sizeAllName), true);
        File.Copy(Path.Combine(_directory, _penName), Path.Combine(target, _penName), true);
        File.Copy(Path.Combine(_directory, _noName), Path.Combine(target, _noName), true);
        File.Copy(Path.Combine(_directory, _iBeamName), Path.Combine(target, _iBeamName), true);
        File.Copy(Path.Combine(_directory, _helpName), Path.Combine(target, _helpName), true);
        File.Copy(Path.Combine(_directory, _handName), Path.Combine(target, _handName), true);
        File.Copy(Path.Combine(_directory, _crossName), Path.Combine(target, _crossName), true);
        File.Copy(Path.Combine(_directory, _arrowName), Path.Combine(target, _arrowName), true);
        File.Copy(Path.Combine(_directory, _appStartingName), Path.Combine(target, _appStartingName), true);

        var value = string.Join(
            ",",
            Path.Combine(target, _arrowName),
            Path.Combine(target, _helpName),
            Path.Combine(target, _appStartingName),
            Path.Combine(target, _waitName),
            Path.Combine(target, _iBeamName),
            Path.Combine(target, _crossName),
            Path.Combine(target, _penName),
            Path.Combine(target, _noName),
            Path.Combine(target, _sizeNSName),
            Path.Combine(target, _sizeWEName),
            Path.Combine(target, _sizeNWSEName),
            Path.Combine(target, _sizeNESWName),
            Path.Combine(target, _sizeAllName),
            Path.Combine(target, _upArrowName),
            Path.Combine(target, _handName));

        using var basekey = Registry.CurrentUser.CreateSubKey(@"Control Panel\Cursors", true);

        basekey.SetValue(null, Name);
        basekey.SetValue("Arrow", Path.Combine(target, _arrowName));
        basekey.SetValue("Help", Path.Combine(target, _helpName));
        basekey.SetValue("AppStarting", Path.Combine(target, _appStartingName));
        basekey.SetValue("Wait", Path.Combine(target, _waitName));
        basekey.SetValue("Crosshair", Path.Combine(target, _iBeamName));
        basekey.SetValue("IBeam", Path.Combine(target, _crossName));
        basekey.SetValue("NWPen", Path.Combine(target, _penName));
        basekey.SetValue("No", Path.Combine(target, _noName));
        basekey.SetValue("SizeNS", Path.Combine(target, _sizeNSName));
        basekey.SetValue("SizeWE", Path.Combine(target, _sizeWEName));
        basekey.SetValue("SizeNWSE", Path.Combine(target, _sizeNWSEName));
        basekey.SetValue("SizeNESW", Path.Combine(target, _sizeNESWName));
        basekey.SetValue("SizeAll", Path.Combine(target, _sizeAllName));
        basekey.SetValue("UpArrow", Path.Combine(target, _upArrowName));
        basekey.SetValue("Hand", Path.Combine(target, _handName));

        using var schemes = basekey.CreateSubKey("Schemes", true);
        schemes.SetValue(Name, value);

        var proc = new Process();
        proc.StartInfo.FileName = "rundll32.exe";
        proc.StartInfo.Arguments = "shell32.dll,Control_RunDLL main.cpl @0";
        proc.Start();
    }
}
