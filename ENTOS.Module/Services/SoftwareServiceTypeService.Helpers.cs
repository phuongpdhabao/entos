namespace ENTOS.Module.Services
{
    public partial class SoftwareServiceTypeService
    {
        private static string JoinKeys(string[] keys)
        {
            return string.Join(", ", keys);
        }
    }
}
