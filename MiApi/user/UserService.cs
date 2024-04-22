
using MongoDB.Driver;

public class UserService
{
    private readonly MongoDBContext _dbContext;
    private readonly IConfiguration _configuration;

    public UserService(MongoDBContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public UserEntity Create(CreateUserDto createUserDto)
    {
        var user = new UserEntity
        {
            Name = createUserDto.Name,
            Email = createUserDto.Email,
            Rol = createUserDto.Rol,
            Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password)
        };
        
        _dbContext.GetCollection<UserEntity>("user").InsertOne(user);
        return user;
    }

    public UserEntity GetByEmail(string email)
    {
        return _dbContext.GetCollection<UserEntity>("user").Find(u => u.Email == email).FirstOrDefault();
    }


}
