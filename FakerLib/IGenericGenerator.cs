using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib
{
    public interface IGenericGenerator
    {
        public Type[] CollectionType { get; }

        object Create(Type type);
    }
}
