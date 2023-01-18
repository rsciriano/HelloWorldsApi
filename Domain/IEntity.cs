using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface IEntity<T>
    {
        T Id { get; }
    }
}
