using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrescottAppBackend.Domain.DbModels;
using PrescottAppBackend.Domain;

namespace PrescottAppBackend.Domain
{
    public interface IReportedProblemService
    {
        Task<List<ReportedProblemVM>> GetAllReportedProblemsAsync();
        Task<ReportedProblemVM> GetReportedProblemByIdAsync(int problemId);
        Task<ReportedProblemVM> AddUpdateReportedProblemAsync(ReportedProblemVM problemVM);
        Task DeleteReportedProblemAsync(int problemId);
        Task<List<ReportedProblemVM>> GetReportedProblemByUser(string userId);
    }
}