using AcmeCorporation.Core.Models;

namespace AcmeCorporation.Core.Services;

public interface IFormService
{
    public Task<IEnumerable<FormSubmission>> GetAllParticipantsAsync();
    public Task<bool> SubmitFormAsync(FormSubmission submission);


}