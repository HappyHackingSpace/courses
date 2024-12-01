using Bogus;
using HHS_ORM;
using Microsoft.EntityFrameworkCore;

var context = new AppDbContext();
var faker = new Faker();

//Example 1: insert random unique categories
var categories = faker.Commerce.Categories(100).Distinct().Take(10);
foreach (var category in categories)
{
    context.Categories.Add(new Category { Name = category });
}
context.SaveChanges();

//Example 2: insert random products with category ids
for (int i = 0; i < 100; i++)
{
    context.Products.Add(new Product
    {
        Name = faker.Commerce.ProductName(),
        Price = faker.Random.Double(10, 20),
        CategoryId = faker.Random.Int(1, 10),
    });
}

context.SaveChanges();

//Example3: read data without tracking
var allProducts = context.Products.AsNoTracking().ToList();
var filteredProducts = context.Products.AsNoTracking().Where(p => p.CategoryId == 5).ToList();
var paginatedProducts = context.Products.AsNoTracking().Where(p => p.Name.EndsWith("z")).OrderBy(p => p.Id).Skip(3)
    .Take(5).ToList();

//Example4: read data with wrong way
var wrongWayProducts = context.Products.ToList().Where(p => p.Name.Contains("r")).ToList();

//Correct chain => Filtering (Where()), Sorting(OrderBy(), OrderByDescending()), Shaping(Select()), Execution(ToList(), ToArray() etc.)

//Example5: update using tracker api
var entity = context.Products.First(p => p.Id == 5);
entity.Name = "new product name";
context.SaveChanges();

//Example6: update without tracker api
context.Products.Where(p => p.Id == 6).ExecuteUpdate(u => u.SetProperty(p => p.Name, "new product name, no tracking api"));

//Example7: delete using tracker api
var product = context.Products.First(p => p.Id == 10);
context.Entry(product).State = EntityState.Deleted;
context.SaveChanges();

//Example8: delete without tracker api
context.Products.Where(p => p.Id == 11).ExecuteDelete();