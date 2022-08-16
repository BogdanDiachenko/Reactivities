using Reactivities.Core.Models.Base;

namespace Reactivities.Core.Models;

public class Photo : BaseEntity
{
    public string PublicId  { get; set; }
    
    public string Url { get; set; }

    public bool IsMain { get; set; }    
}