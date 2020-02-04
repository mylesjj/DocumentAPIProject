using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.ResourceParameters;

namespace WebApi.Services
{
    public interface IDocumentService
    {
        Task<DocFileDto> CreateDocumentAsync(DocFileDto document);

        IEnumerable<DocFileSummaryDto> GetDocumentsSummary(SortingParams sortingParams);

        byte[] GetDocumentStream(int id);

        Task DeleteDocumentAsync(int id);

        DocFileDto GetDocument(int id);
    }
}