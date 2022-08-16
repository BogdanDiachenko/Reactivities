namespace Reactivities.Core.Models.Base;

public interface IBaseEntity<T>
{
    T Id { get; set; }
}