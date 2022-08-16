using System.ComponentModel.DataAnnotations;

namespace Reactivities.Core.Models.Base;

public class BaseEntity : IBaseEntity<Guid>
{
    [Key]
    public Guid Id { get; set; }
}