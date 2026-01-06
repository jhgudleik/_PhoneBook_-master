using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive;
using PhoneBook.Models;
using ReactiveUI;

namespace PhoneBook.Client.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private static HttpClient HttpClient => App.HttpClient;

    public ObservableCollection<PhoneBookItem> PhoneBook { get; } = new();

    private PhoneBookItem? _selectedItem;
    public PhoneBookItem? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    private Guid? _id;
    public Guid? Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    private string? _name;
    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string? _phone;
    public string? Phone
    {
        get => _phone;
        set => this.RaiseAndSetIfChanged(ref _phone, value);
    }

    public ReactiveCommand<Unit, Unit> LoadCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }

    public MainWindowViewModel()
    {
        this.WhenAnyValue(x => x.SelectedItem)
            .Subscribe(x =>
            {
                Id = x?.Id;
                Name = x?.Name;
                Phone = x?.PhoneNumber;
            });

        LoadCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            const string url = "http://localhost:5120/phonebook";
            var items = HttpClient.GetFromJsonAsAsyncEnumerable<PhoneBookItem>(url);

            PhoneBook.Clear();
            await foreach (var item in items)
            {
                PhoneBook.Add(item);
            }
        });

        SaveCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var item = new PhoneBookItem
            {
                Name = Name,
                PhoneNumber = Phone
            };

            const string url = "http://localhost:5120/phonebook";
            await HttpClient.PostAsJsonAsync(url, item);
        });
    }
}
