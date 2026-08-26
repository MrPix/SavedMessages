using System.Collections.ObjectModel;
using Briefcase.Maui.Models;

namespace Briefcase.Maui.ViewModels;

/// <summary>A titled group of messages for the grouped CollectionView.</summary>
public class MessageGroup : ObservableCollection<MockMessage>
{
    public string Name { get; }

    public MessageGroup(string name, IEnumerable<MockMessage> items) : base(items)
    {
        Name = name;
    }
}
