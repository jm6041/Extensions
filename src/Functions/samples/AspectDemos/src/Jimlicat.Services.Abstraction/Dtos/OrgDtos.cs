using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jimlicat.Services.Dtos
{
    public class OrgDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class OrgAddDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class OrgAddDtoValidator : AbstractValidator<OrgAddDto>
    {
        public OrgAddDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Address).NotEmpty().Length(2, 100);
        }
    }
}
