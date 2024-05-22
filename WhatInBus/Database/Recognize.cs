namespace WhatInBus.Database;

public partial class Recognize
{
    public int Id { get; set; }

    public byte[] Image { get; set; } = null!;

    public string? ModelName { get; set; }

    public DateOnly Date { get; set; }

    public int Result { get; set; }
}
