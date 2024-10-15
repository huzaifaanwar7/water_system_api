using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmenityController(IAmenityService _amenity) : ControllerBase
    {
        
    }
}