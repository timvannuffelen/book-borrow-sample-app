using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Books;
internal sealed class Isbn : ValueObject
{
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
