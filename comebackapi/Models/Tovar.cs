namespace comebackapi.Models;

public class Tovar
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public virtual ICollection<TovarAudit> Audits { get; set; } = new List<TovarAudit>();
}