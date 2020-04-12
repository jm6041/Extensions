using Jimlicat.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jimlicat.Services
{
    public interface IOrgService
    {
        void Add(OrgAddDto org);

        OrgDto[] Get();
    }
}
