using System.Collections.Generic;
using MiniTrello.Domain.Entities;

namespace MiniTrello.Api.Models
{
    public class ReturnBoardsModel: ReturnModel
    {
        public IList<Board> Boards { get; set; }
    }
}