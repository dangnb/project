using System.Xml.Linq;
using MNG.Domain.Abstractions;
using MNG.Domain.Abstractions.Entities;

namespace MNG.Domain.Entities;
public class Product : EntityAuditBase<Guid>
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }

    public static Product CreateProduct(Guid id, string name, decimal price, string description)
    {
        return new Product(id,name, price, description);
    }

    public Product(Guid id, string name, decimal price, string description)
    {
        //if (!NameValid(name))
        //    throw new ArgumentNullException();
        Id= id;
        Name = name;
        Price = price;
        Description = description;
    }

    public void Update(string name, decimal price, string description)
    {
        //if (!NameValid(name))
        //    throw new ArgumentNullException();

        Name = name;
        Price = price;
        Description = description;
    }

    private bool NameValid(string name)
        => name.Contains("ABCD-");
}
