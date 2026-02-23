namespace ENTOS.Module.Services
{
    public partial class DataTypeService
    {
        private static System.Type ResolveDataType(DataType dataType)
        {
            return dataType.GetType();
        }
    }
}
