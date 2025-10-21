using System;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext context;

    public ProductsController(StoreContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await context.Products.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        context.Products.Add(product);
        await  context.SaveChangesAsync();
        return product;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        Console.WriteLine(id );
        Console.WriteLine(product.Id);
        Console.WriteLine(ProductExists(id));
        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ");

        if (product.Id != id || !ProductExists(id))
            return BadRequest("Connot update this product");

        context.Entry(product).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();

    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        Console.WriteLine((product));

        if (product == null) return NotFound();

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return NoContent();


    }

    private bool ProductExists(int id)
    {
        return context.Products.Any(x => x.Id == id);

    }
}
