using System.Collections.Generic;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class ReturnLinesModel: ReturnModel
    {
        public IList<Lines> Lines { set; get; }
    }
}