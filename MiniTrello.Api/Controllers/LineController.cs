using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using AutoMapper;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using MiniTrello.Domain.Services;

namespace MiniTrello.Api.Controllers
{

    public class LineController : ApiController
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        readonly IMappingEngine _mappingEngine;

        

        public LineController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository,
            IMappingEngine mappingEngine)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _mappingEngine = mappingEngine;
        }

        [POST("CreateLine/{idBoard}/{accesToken}")]
        public ReturnModel CreateLine([FromBody] LineModel model,long idBoard, string accesToken)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Token ==accesToken);

            ReturnModel remodel=new ReturnModel();

            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    Board board = _readOnlyRepository.GetById<Board>(idBoard);
                    Lines line=_mappingEngine.Map<LineModel, Lines>(model);
                    Lines lineCreated = _writeOnlyRepository.Create(line);
                    if (lineCreated != null)
                    {
                        board.AddLine(lineCreated);
                        var boardUpdate = _writeOnlyRepository.Update(board);
                        Activity activity = new Activity();
                        activity.Text = account.FirstName + " Creo una Line en "+board.Title;
                        account.AddActivities(activity);
                        var accountUpdate = _writeOnlyRepository.Update(account);
                        return remodel.ConfigureModel("Successfull", "Se creo exitosamente la line "+lineCreated.Title, remodel);
                    }
                    return remodel.ConfigureModel("Error", "No se pudo crear la line", remodel);
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
        }
    }
}