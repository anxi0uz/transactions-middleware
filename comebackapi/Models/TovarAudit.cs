namespace comebackapi.Models;

public class TovarAudit
{
    public int Id { get; set; }
    
    public string Description { get; set; }
    
    public string UpdatedOn { get; set; }
    
    public int? TovarId { get; set; }
    
    public virtual Tovar Tovar { get; set; }
}