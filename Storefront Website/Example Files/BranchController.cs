using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseStudyAPI.DAL;
using CaseStudyAPI.DAL.DAO;
using CaseStudyAPI.DAL.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudyAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase {
        AppDbContext _db;
        public BranchController(AppDbContext context) {
            _db = context;
        }

        [HttpGet("{lat}/{lng}")]
        public ActionResult<List<Branch>> Index(float lat, float lng) {
            BranchDAO dao = new BranchDAO(_db);
            return dao.GetThreeClosestBranches(lat, lng);
        }
    }
}