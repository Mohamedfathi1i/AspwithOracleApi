using AspwithOracleApi.Data;
using AspwithOracleApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace AspwithOracleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParameterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ProcessParameters([FromBody] UserParameters parameters)
        {
            // Create OracleParameter instances for the parameters
            var param = new OracleParameter("param", OracleDbType.Varchar2) { Value = parameters.Param1 };
            var param2 = new OracleParameter("param2", OracleDbType.Varchar2) { Value = parameters.Param2 };
            var param3 = new OracleParameter("param3", OracleDbType.Varchar2) { Value = parameters.Param3 };

            // Call the Oracle function and retrieve the result
            var resultParameter = new OracleParameter("result", OracleDbType.Varchar2, ParameterDirection.Output) { Size = 4000 };
            _context.Database.ExecuteSqlRaw("BEGIN :result := insert_and_concat_params(:param, :param2, :param3); END;",
                resultParameter,
                param,
                param2,
                param3);

            // Extract the result from the output parameter
            string oracleFunctionResult = resultParameter.Value.ToString();

            // Return the Oracle function result
            return Ok(oracleFunctionResult);
        }
    }
}