using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.Product
{
    public class ProductHandler : IRequestHandler<ProductQuery , string>
    {
        public async Task<string> Handle(ProductQuery request, CancellationToken cancellationToken)
        {
            Console.WriteLine("working");
           
            return  "working";
        }
    }
}
