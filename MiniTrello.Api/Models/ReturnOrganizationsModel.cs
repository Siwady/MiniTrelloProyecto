using System.Collections.Generic;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class ReturnOrganizationsModel:ReturnModel
    {
        public IList<Organization> Organizations { set; get; }
        public long Id { set; get; }
    }
}