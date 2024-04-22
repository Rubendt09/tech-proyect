using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class OrderService
{
    private readonly IMongoCollection<OrderEntity> _orders;

    public OrderService(MongoDBContext dbContext)
    {
        _orders = dbContext.GetCollection<OrderEntity>("order");
    }

    public async Task<List<OrderEntity>> GetAllAsync()
    {
        return await _orders.Find(_ => true).ToListAsync();
    }

    public async Task<OrderEntity> GetByIdAsync(string id)
    {
        return await _orders.Find<OrderEntity>(order => order.Id == id).FirstOrDefaultAsync();
    }

    public async Task<OrderEntity> CreateAsync(CreateOrderDto orderDto)
    {
        var existingOrder = await _orders.Find(x => x.Code == orderDto.Code).FirstOrDefaultAsync();
        if (existingOrder != null || orderDto.Code.Length != 9)
            throw new Exception("Invalid order code or code already exists.");

        var order = new OrderEntity
        {
            Name = orderDto.Name,
            Code = orderDto.Code,
            TypeOrder = orderDto.TypeOrder,
            Phase = orderDto.Phase,
            Stopping = orderDto.Stopping,
            TypeStopping = orderDto.TypeStopping,
            OptionDowntime = orderDto.OptionDowntime,
            StartDateStopping = orderDto.StartDateStopping,
            CloseDateStopping = orderDto.CloseDateStopping,
            Reprogramate = orderDto.Reprogramate,
            Reason = orderDto.Reason
        };

        await _orders.InsertOneAsync(order);
        return order;
    }

    public async Task UpdateAsync(string id, UpdateOrderDto orderDto)
    {
        var updateDefinition = Builders<OrderEntity>.Update
            .Set(o => o.Name, orderDto.Name)
            .Set(o => o.TypeOrder, orderDto.TypeOrder)
            .Set(o => o.Phase, orderDto.Phase)
            .Set(o => o.Stopping, orderDto.Stopping)
            .Set(o => o.TypeStopping, orderDto.TypeStopping)
            .Set(o => o.OptionDowntime, orderDto.OptionDowntime)
            .Set(o => o.StartDateStopping, orderDto.StartDateStopping)
            .Set(o => o.CloseDateStopping, orderDto.CloseDateStopping)
            .Set(o => o.Reprogramate, orderDto.Reprogramate)
            .Set(o => o.Reason, orderDto.Reason);

        await _orders.UpdateOneAsync(o => o.Id == id, updateDefinition);
    }

    public async Task DeleteAsync(string id)
    {
        await _orders.DeleteOneAsync(o => o.Id == id);
    }
}
