using AcmeCorporation.Core.Models;
using AcmeCorporation.Data.Entities;
using AcmeCorporation.Data.Services;

namespace AcmeCorporation.Core.Services;

public class FormService : IFormService
{
    private readonly IParticipantRepository _repo;
    public FormService(IParticipantRepository repo)
    {
        _repo = repo;
    }
    
    public async Task<IEnumerable<FormSubmission>> GetAllParticipantsAsync()
    {
        var participants =  await _repo.GetAllAsync();
        return participants.Select(e => new FormSubmission(e.Id,
            e.FirstName,
            e.LastName,
            e.EmailAddress,
            e.ProductSerialNumber));
    }
    
    
    public async Task<bool> SubmitFormAsync(FormSubmission submission)
    {
        try
        {
            // Check for duplicates: same FirstName + LastName + Email
            var existingCount = await _repo.CountAsync(p =>
                p.FirstName == submission.FirstName &&
                p.LastName == submission.LastName &&
                p.EmailAddress == submission.EmailAddress);

            if (existingCount >= 2)
            {
                return false;
            }

            // Check if the serial number already exists in participants
            var serialExists = await _repo.ExistsAsync(p => p.ProductSerialNumber == submission.ProductSerialNumber);
            if (serialExists)
            {
                return false;
            }

            // Create and persist entity
            var entity = new Participant
            (
                0,
                submission.FirstName,
                submission.LastName,
                submission.EmailAddress,
                submission.ProductSerialNumber
            );

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return true;
        }
        catch (Exception exception)
        {
            // log exception
            return false;
        }
    }

}