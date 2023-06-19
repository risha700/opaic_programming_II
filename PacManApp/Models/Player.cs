using CommunityToolkit.Mvvm.ComponentModel;

namespace PacManApp.Models;

public partial class Player : ObservableObject
{

    public readonly Guid Id = new();
    private uint score;
    public uint Score { get => score; set => SetProperty(ref score, value); }


}
