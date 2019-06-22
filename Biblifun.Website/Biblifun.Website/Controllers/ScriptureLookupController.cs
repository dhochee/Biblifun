// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Biblifun.Website.ViewModels;
using AutoMapper;
using Biblifun.Data.Models;
using Biblifun.Data.Core.Interfaces;
using Biblifun.Website.Authorization;
using Biblifun.Website.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Biblifun.Data.Core;
using IdentityServer4.AccessTokenValidation;
using Biblifun.Data;
using Biblifun.Website.Managers;

namespace Biblifun.Website.Controllers
{
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ScriptureLookupController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IVerseParser _verseParser;
        readonly IScriptureLookupManager _scriptureLookupManager;

        public ScriptureLookupController(IUnitOfWork unitOfWork,
                                         IVerseParser verseParser,
                                         IScriptureLookupManager scriptureLookupManager)
        {
            _unitOfWork = unitOfWork;
            _verseParser = verseParser;
            _scriptureLookupManager = scriptureLookupManager;
        }


        [HttpGet("lookup/{language}/{verse}")]
        public async Task<IActionResult> LookupScripture(string language, string verse)
        {
            string html = null;

            switch(_verseParser.TryParseVerseString(verse, language, out VerseSetDescriptor verseSet))
            {
                case VerseParseResult.Success:

                    html = await _scriptureLookupManager.GetVerseHtmlFromSetCode(verseSet.Code, language);
                    break;

                case VerseParseResult.InvalidVerse:
                    html = "The verse could not be found.";
                    break;

                case VerseParseResult.InvalidSyntax:
                    html = "Invalid syntax. Try again.";
                    break;
            }

            return Json(html);
        }
    }
}
