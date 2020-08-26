using eCommerceSite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Data
{
    public class ProductDb
    {
        /// <summary>
        /// Return the total count of products
        /// </summary>
        /// <param name="_context">DB context to use</param>
        /// <returns></returns>
        public static async Task<int> GetTotalProductsAsync(ProductContext _context)
        {
            return await(from p in _context.Products
                         select p).CountAsync();
        }

        /// <summary>
        /// Get a page worth of products 
        /// </summary>
        /// <param name="_context">DB context</param>
        /// <param name="pageSize">number of products per page</param>
        /// <param name="pageNum">page of products to return</param>
        /// <returns></returns>
        public async static Task<List<Product>> GetProductsAsync(ProductContext _context, int pageSize, int pageNum)
        {
            return await (from p in _context.Products
                                            orderby p.Title ascending
                                            select p)
                                            .Skip(pageSize * (pageNum - 1)) //Skip must be before take
                                            .Take(pageSize)
                                            .ToListAsync();
        }

        public static async Task<Product> AddProductAsync(ProductContext _context, Product p)
        {
            //Add to DB
            _context.Products.Add(p); // Creates insert querey
            await _context.SaveChangesAsync(); // executes query

            return p;
        }

        public static async Task<Product> GetSingleProductAsync(ProductContext _context, int id)
        {
            // Get product with corresponding id
            return await (from prod in _context.Products
                       where prod.ProductId == id
                       select prod).SingleAsync();
        }
    }
}
