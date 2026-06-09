#nullable disable
using System;

namespace Master.Entities.Models;

public partial class AuthRefreshTokens
{
    public int UserId { get; set; }
    public string ActiveRefreshTokenId { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsCompromised { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public virtual ErpUsers User { get; set; }
}


