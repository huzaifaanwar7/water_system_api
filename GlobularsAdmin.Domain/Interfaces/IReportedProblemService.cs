using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobularsAdmin.Domain.DbModels;
using GlobularsAdmin.Domain;

namespace GlobularsAdmin.Domain
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