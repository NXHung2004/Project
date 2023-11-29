using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Modes;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public ProductController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IEnumerable<product>> Getproduct()
        {
            var pro = await _dbContext.Products.ToListAsync();
            return pro;
        }
        [HttpGet("{id}")]
        public async Task<product> Getpro(Guid id)
        {
            var pro = await _dbContext.Products.FindAsync(id);
            return pro;
        }
        [HttpPost]
        public async Task<product> Addproduct(product product)
        {
            product.Id = Guid.NewGuid();
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
        [HttpPut]
        public async Task<product> Updateproduct(product product)
        {
            var proExisted = await Getpro((Guid)product.Id);
            if (proExisted != null)
            {
                proExisted.Name = product.Name;


                _dbContext.Entry(proExisted).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return proExisted;
            }
            return null;
        }
        [HttpDelete("{id}")]
        public async Task<product> Deleteproduct(Guid id)
        {
            var employeeExisted = await Getpro(id);
            if (employeeExisted != null)
            {
                _dbContext.Products.Remove(employeeExisted);
                await _dbContext.SaveChangesAsync();
                return employeeExisted;
            }
            return null;
        }
    }
}

