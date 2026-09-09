using System.ComponentModel.DataAnnotations;

namespace MyReadingNookApp.Models;

public class FriendsModel
{
    public int Id { get; set; }

    public int RequesterId { get; set; }
    
    public int AddresseeId { get; set; }
    
    [AllowedValues("Pending", "Accepted", "Declined")]
    public string Status { get; set; } = "Pending";
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    // Navigation Properties
    public UserModel? Requester { get; set; }
    
    public UserModel? Addressee { get; set; }
}