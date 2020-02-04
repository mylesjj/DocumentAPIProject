using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApi.Database;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Models;
using WebApi.ResourceParameters;

namespace WebApi.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly FileDBContext _fileDBContext;

        public DocumentService(FileDBContext fileDBContext)
        {
            _fileDBContext = fileDBContext;
        }

        public IEnumerable<DocFileSummaryDto> GetDocumentsSummary(SortingParams sortingParams)
        {
            var documentSummaryList = _fileDBContext.DocFile.Select(f => new DocFileSummaryDto()
            {
                Filesize = f.Filesize,
                Name = f.Name,
                Id = f.Id
            }).OrderBy(sortingParams.SortBy);

            return documentSummaryList;
        }

        public byte[] GetDocumentStream(int id)
        {
            var document = _fileDBContext.DocFile.Where(d => d.Id == id).FirstOrDefault();

            //No time to implement ProblemDetails
            if (document == null)
                throw new KeyNotFoundException("Invalid id");

            return document.Content;

        }

        public DocFileDto GetDocument(int id)
        {
            var documentEntity = _fileDBContext.DocFile.Where(d => d.Id == id).FirstOrDefault();
            var document = new DocFileDto()
            {
                Id = documentEntity.Id,
                Location = documentEntity.Location,
                Filesize = documentEntity.Filesize,
                Name = documentEntity.Name,
                Content = documentEntity.Content
            };
            return document;
        }

        public async Task DeleteDocumentAsync(int id)
        {
            var documentEntity = _fileDBContext.DocFile.Where(d => d.Id == id).FirstOrDefault();
            _fileDBContext.DocFile.Remove(documentEntity);

            await _fileDBContext.SaveChangesAsync();

        }

        public async Task<DocFileDto> CreateDocumentAsync(DocFileDto document)
        {
            //Would use Automapper without time constraints
            var documentEntity = new DocFile()
            {
                Id = document.Id,
                Location = document.Location,
                Filesize = document.Filesize,
                Name = document.Name,
                Content = document.Content
            };

            _fileDBContext.DocFile.Add(documentEntity);

            var docKey = await _fileDBContext.SaveChangesAsync();

            document.Id = docKey;
            return document;
        }
    }
}
