using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filetoolkits.application.Product
{
    public record ProductQuery(string name):IRequest<string>;
    
}
