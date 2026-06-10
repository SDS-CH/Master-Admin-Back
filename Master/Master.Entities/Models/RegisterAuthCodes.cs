#nullable disable
using System;

namespace Master.Entities.Models;

public partial class RegisterAuthCodes
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string CodeHash { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int Attempts { get; set; }
    public int MaxAttempts { get; set; }
    public int ResendCount { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

