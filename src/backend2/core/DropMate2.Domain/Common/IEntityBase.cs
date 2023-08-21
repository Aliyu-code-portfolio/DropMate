namespace DropMate2.Domain.Common
{
    public interface IEntityBase
    {
        bool IsDeleted { get; set; }
    }
}