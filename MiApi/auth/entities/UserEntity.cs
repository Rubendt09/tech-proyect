using MongoDB.Bson;

public class UserAuth
{
    public ObjectId Id { get; set; }
    public string Name { get; set; } = default!;
    public string Rol { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
