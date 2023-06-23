using CommunityToolkit.Mvvm.ComponentModel;

namespace PacManApp.Models;

public partial class Player : ObservableObject
{

    public readonly Guid Id = new();
    private uint lives=3;
    public uint Lives { get => lives; set => SetProperty(ref lives, value); }

    private uint score;
    public uint Score { get => score; set => SetProperty(ref score, value); }


}
