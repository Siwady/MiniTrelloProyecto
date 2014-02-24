using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;
using AttributeRouting.Web.Http;
using AutoMapper;
using MiniTrello.Api.Models;
using MiniTrello.Domain.Entities;
using MiniTrello.Domain.Services;

namespace MiniTrello.Api.Controllers
{
    public class AccountController : ApiController
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteOnlyRepository _writeOnlyRepository;
        readonly IMappingEngine _mappingEngine;

        public AccountController(IReadOnlyRepository readOnlyRepository, IWriteOnlyRepository writeOnlyRepository,
            IMappingEngine mappingEngine)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeOnlyRepository = writeOnlyRepository;
            _mappingEngine = mappingEngine;
        }

        [POST("login")]
        public ReturnModel Login([FromBody] AccountLoginModel model)
        {
            var account =
                _readOnlyRepository.First<Account>(
                    account1 => account1.Email == model.Email && account1.Password == model.Password);

            ReturnModel remodel=new ReturnModel();
            if (account != null)
            {
                //account.TokenTime = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                account.TokenTime = DateTime.Now;
                account.Token = Guid.NewGuid().ToString();
                //account.Token = Convert.ToBase64String(account.TokenTime.Concat(account.TokenKey).ToArray()).Replace("/", "A").Replace("+", "B"); //.Replace(@"/", "").Replace(@"+","")
                
                var tokenCreated = _writeOnlyRepository.Update(account);
                if (tokenCreated != null) 
                    return new AuthenticationModel() { Token = account.Token };
                    
            }
            return remodel.ConfigureModel("Error", "NO se pudo acceder a su cuenta", remodel);
            
        }

        [POST("register")]
        public ReturnModel Register([FromBody] AccountRegisterModel model)
        {
            ReturnModel remodel=new ReturnModel();
            if (model.Password != model.ConfirmPassword)
            {
                return remodel.ConfigureModel("Error", "Las Claves no son iguales", remodel);
            }

            if (model.Password.Length <= 6 || Regex.IsMatch(model.Password, @"^[a-zA-Z]+$"))
            {
                return remodel.ConfigureModel("Error", "la clave debe ser mayor a 6 caracteres y debe contener almenos un numero", remodel);
            }

            var accountExist =
                _readOnlyRepository.First<Account>(
                    account1 => account1.Email == model.Email);
            if (accountExist == null)
            {
                Account account = _mappingEngine.Map<AccountRegisterModel, Account>(model);
                account.TokenTime = DateTime.Now;
                Account accountCreated = _writeOnlyRepository.Create(account);
                if (accountCreated != null)
                {
                    ReturnRegisterModel registermodel = _mappingEngine.Map<Account, ReturnRegisterModel>(account);
                    return remodel.ConfigureModel("Successfull", "Se Registro Correctamente", remodel);
                }
                return remodel.ConfigureModel("Error", "Error al Guardar el Usuario", remodel);
            }
            return remodel.ConfigureModel("Error", "Usuario ya existe", remodel);
        }

        /*[AcceptVerbs("PUT")]
        [PUT("resetPassword")]
        public HttpResponseMessage ResetPassword([FromBody]ResetPasswordModel model)
        {
            var account =_readOnlyRepository.First<Account>(account1 => account1.Email == model.Email);
            if (account != null)
            {
                account.Password = Guid.NewGuid().ToString().Substring(0,8);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            throw new BadRequestException("cuenta no existe");
        }*/

        [AcceptVerbs("PUT")]
        [PUT("{accessToken}")]
        public ReturnModel UpdateData([FromBody] UpdateDataModel model, string accessToken)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Token == accessToken);
            ReturnModel remodel=new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    account.FirstName = model.FirstName;
                    account.LastName = model.LastName;
                    var Updateaccount = _writeOnlyRepository.Update(account);
                    ReturnUpdateDataModel updatemodel = _mappingEngine.Map<Account, ReturnUpdateDataModel>(account);
                    return updatemodel.ConfigureModel("Successfull", "Se actualizo correctamente su informacion", remodel);
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
        }

        [AcceptVerbs("GET")]
        [GET("activities/{accessToken}")]
        public ReturnModel Activities( string accessToken)
        {
            var account = _readOnlyRepository.First<Account>(account1 => account1.Token == accessToken);
            ReturnModel remodel=new ReturnModel();
            if (account != null)
            {
                if (account.VerifyToken(account))
                {
                    ReturnActivitiesModel activitiesmodel = _mappingEngine.Map<Account, ReturnActivitiesModel>(account);
                    return activitiesmodel.ConfigureModel("Successfull", "", activitiesmodel);
                }
                return remodel.ConfigureModel("Error", "Su session ya expiro", remodel);
            }
            return remodel.ConfigureModel("Error", "No se pudo acceder a su cuenta", remodel);
        }
    }


    public class BadRequestException : HttpResponseException
    {
        public BadRequestException(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public BadRequestException(HttpResponseMessage response) : base(response)
        {
        }

        public BadRequestException(string errorMessage) : base(HttpStatusCode.BadRequest)
        {
            
            this.Response.ReasonPhrase = errorMessage;
        }
    }
}