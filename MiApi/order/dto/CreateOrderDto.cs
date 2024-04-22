using System.ComponentModel.DataAnnotations;

public class CreateOrderDto
{
    [Required] public string Name { get; set; } = default!;
    [Required] public string Code { get; set; } = default!;
     public string TypeOrder { get; set; } = default!;
    [Required] public string Phase { get; set; } = default!;
    public bool Stopping { get; set; } = default!;
    public string TypeStopping {get; set;} = default!;
    public string OptionDowntime { get; set; } = default!;
    public DateTime StartDateStopping { get; set; } = default!;
    public DateTime CloseDateStopping { get; set; } = default!;
    public bool Reprogramate { get; set; } = default!;
    public string Reason { get; set; } = default!;
}