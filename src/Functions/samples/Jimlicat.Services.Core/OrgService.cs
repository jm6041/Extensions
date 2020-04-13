using Jimlicat.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jimlicat.Services
{
    public class OrgService : IOrgService
    {
        public Task Create(OrgAddDto org)
        {
            OrgData.Add(org);
            return Task.CompletedTask;
        }

        public Task<OrgDto[]> Get()
        {
            var data = OrgData.GetAll();
            return Task.FromResult(data);
        }

        public Task<OrgDto[]> Get2()
        {
            var data = OrgData.GetAll();
            return Task.FromResult(data);
        }
    }

    internal static class OrgData
    {
        private static readonly SortedDictionary<int, OrgDto> source = new SortedDictionary<int, OrgDto>()
        {
            {1, new OrgDto(){ Id = 1, Name = "T1", Address = "111" } },
            {2, new OrgDto(){ Id = 2, Name = "T2", Address = "222" } },
            {3, new OrgDto(){ Id = 3, Name = "T3", Address = "333" } },
            {4, new OrgDto(){ Id = 4, Name = "T4", Address = "444" } },
            {5, new OrgDto(){ Id = 5, Name = "T5", Address = "555" } },
            {6, new OrgDto(){ Id = 6, Name = "T6", Address = "666" } },
        };

        public static OrgDto[] GetAll()
        {
            return source.Values.ToArray();
        }

        public static void Add(OrgAddDto org)
        {
            int max = source.Keys.Max();
            int id = max + 1;
            source.Add(id, new OrgDto() { Id = id, Name = org.Name });
        }
    }
}
