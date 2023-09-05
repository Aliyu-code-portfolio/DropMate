using DropMate.Shared.Exceptions.Base;

namespace DropMate.Shared.Exceptions.Sub
{
    public class PackageNotAlterableException:NotAlterableException
    {
        public PackageNotAlterableException(object id):base($"The package with identity: {id} cannot be alter anymore in the database. The package has been booked")
        {
            
        }
        public PackageNotAlterableException(string message):base(message)
        {
            
        }
    }
}
