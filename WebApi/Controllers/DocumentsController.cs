using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.IO;
using System.Threading.Tasks;

using WebApi.Database;
using WebApi.Models;
using WebApi.ResourceParameters;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly FileDBContext _fileDBContext;
        private readonly IDocumentService _documentService;

        public DocumentsController(FileDBContext fileDBContext, IDocumentService documentService)
        {
            _fileDBContext = fileDBContext;
            _documentService = documentService;
        }

        [HttpPost("Upload", Name = nameof(Upload))]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (!(file == null))
            {
                //Neither the MIME type nor the file extension is reliable information to determine
                //the file type - both can be easily changed. 
                if (extension.ToLower() != ".pdf")
                {
                    return BadRequest("Non PDF file type");
                }

                if (file.Length > (5 * 1024 * 1024))
                {
                    return BadRequest("5MB file upload limit");
                }
            }
                       
            if (file.Length > 0)
                {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                if (stream.Length < 2097152)
                {
                    var docDto = new DocFileDto()
                    {
                        Name = file.FileName,
                        Content = stream.ToArray()
                    };
                    
                    await _documentService.CreateDocumentAsync(docDto);
            
                }
            }
  
            return Ok();
                
        }

        [HttpGet(Name = "GetDocuments")]
        public IActionResult GetDocuments([FromQuery] SortingParams sortParams)
        {
            var documents = _documentService.GetDocumentsSummary(sortParams);

            return Ok(documents);
        }

        [HttpGet("{id}", Name = nameof(GetDocument))]
        public IActionResult GetDocument(int id)
        {
            var fileContents = _documentService.GetDocumentStream(id);

            return File(fileContents, "application/pdf");
        }

        [HttpDelete("{id}", Name = nameof(DeleteDocumentAsync))]
        public async Task<ActionResult> DeleteDocumentAsync(int id)
        {
            var document = _documentService.GetDocument(id);

            if (document == null)
            {
                return NotFound();
            }

            await _documentService.DeleteDocumentAsync(document.Id);


            return NoContent();
        }

    }
}