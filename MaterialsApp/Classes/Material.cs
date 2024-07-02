using System.Linq;

namespace MaterialsApp.Models
{
    public partial class Material
    {
        public string SuppliesLine
        {
            get => 
                string.Join(", ", Suppliers.Select(x => x.Name).ToList());
        }
    }
}