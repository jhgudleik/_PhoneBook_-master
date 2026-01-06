using System;

namespace PhoneBook.Models;

public record class PhoneBookItem
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public string Name { get; init; }
    public string PhoneNumber { get; init; }
}